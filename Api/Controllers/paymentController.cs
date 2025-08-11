    using System;
    using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Dtos.Payment;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Payments.HashHelper;
using Infrastructure.Payments.Interfaces;
using Infrastructure.Payments.Providers.Halkbank;
using Infrastructure.Persistence.Models.Payments;
using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    namespace Api.Controllers
    {
        //[Authorize(Policy = "PaymentOnly")]
        [ApiController]
        [Route("api/[controller]")]
        public class paymentController : ControllerBase
        {
            private readonly IYildatService _yildatService;
            private readonly IPaymentProvider _paymentProvider;
            private readonly IOdooAppService _odooAppService;
            private readonly IYildatPaymentResponseService _yildatPaymentResponseService;
            private readonly IHashService _hashService;
            private readonly IHashHelper _hashHelper;
            public paymentController(IYildatService yildatService, IPaymentProvider paymentProvider, 
            IOdooAppService odooAppService, IYildatPaymentResponseService yildatPaymentResponseService, IHashService hashService, IHashHelper hashHelper)
            {
                _yildatService = yildatService;
                _paymentProvider = paymentProvider;
                _odooAppService = odooAppService;
                _yildatPaymentResponseService = yildatPaymentResponseService;
                _hashService = hashService;
                _hashHelper = hashHelper;
            }
            [HttpGet]
            [Authorize(Policy = "PaymentOnly")]
            public async Task<IActionResult> Get()
            {
                var tckn = User.Claims.FirstOrDefault(p=> p.Type == "tckn").Value;
                
                var result = await _yildatService.GetYildatsByTcknAsync(tckn);
                if (!result.Success)
                {
                    return Ok(result);
                }
                return Ok(result);
            }


            [HttpPost("pay")]
            [Authorize(Policy = "PaymentOnly")]
            public async Task<IActionResult> pay([FromBody] PaymentRequest model)
            {
                var response = _paymentProvider.ProcessPaymentAsync(new PaymentRequest
                {
                    Amount = model.Amount,
                    key = model.key
                }).Result;
                
                return Content(response.postData, "text/html");
            }

            [HttpPost("fail")]
            public IActionResult fail([FromForm] List<KeyValuePair<string, string>> bankResponse)
            {
                try
                {
                    var logPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
                    if (!Directory.Exists(logPath))
                    {
                        Directory.CreateDirectory(logPath);
                    }

                    var filePath = Path.Combine(logPath, $"fail_log_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
                    using (var writer = new StreamWriter(filePath))
                    {
                        foreach (var kvp in bankResponse)
                        {
                            writer.WriteLine($"\"{kvp.Key}\":\"{kvp.Value}\"");
                        }
                    }

                    //var errorMessage = bankResponse.ContainsKey("ErrMsg") ? bankResponse["ErrMsg"] : "Ödeme reddedildi.";

                    return Ok(new
                    {
                        success = false,
                        message = "Ödeme işlemi başarısız.",
                        //error = errorMessage,
                        timestamp = DateTime.Now,
                        //transactionId = bankResponse.ContainsKey("oid") ? bankResponse["oid"] : null
                    });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Log yazma hatası: {ex.Message}");
                }
            }

            // [HttpPost("success")]
            // public IActionResult success([FromForm] Dictionary<string,string> bankResponse)
            // {
            //     try
            //     {
            //         var logPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            //         if (!Directory.Exists(logPath))
            //         {
            //             Directory.CreateDirectory(logPath);
            //         }

            //         var filePath = Path.Combine(logPath, $"success_log_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
            //         using (var writer = new StreamWriter(filePath))
            //         {
            //             foreach (var kvp in bankResponse)
            //             {
            //                 writer.WriteLine($"\"{kvp.Key}\":\"{kvp.Value}\"");
            //             }
            //         }

            //         var errorMessage = bankResponse.ContainsKey("ErrMsg") ? bankResponse["ErrMsg"] : "Ödeme reddedildi.";

            //         return Ok(new
            //         {
            //             success = false,
            //             message = "Ödeme işlemi başarısız.",
            //             error = errorMessage,
            //             timestamp = DateTime.Now,
            //             transactionId = bankResponse.ContainsKey("oid") ? bankResponse["oid"] : null
            //         });
            //     }
            //     catch (Exception ex)
            //     {
            //         return StatusCode(500, $"Log yazma hatası: {ex.Message}");
            //     }
            // }



            [HttpPost("success")]
            public async Task<IActionResult> success([FromForm] Dictionary<string, string> bankResponse)
            {
                try
                {

                    var orderHash = new Dictionary<string, string>(){
                        {"oid", bankResponse["oid"] }
                    };

                    var calcHash = _hashHelper.CalculateHashV3(orderHash);

                    if (calcHash != bankResponse["description"])
                    {
                        return Ok("Doğrulama başarısız.");
                    }

                    bankResponse.TryGetValue("oid", out var oid);
                    bankResponse.TryGetValue("mdStatus", out var mdStatus);
                    bankResponse.TryGetValue("Response", out var response);
                    bankResponse.TryGetValue("ProcReturnCode", out var procReturnCode);
                    bankResponse.TryGetValue("amount", out var amount);

                    var logPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
                    if (!Directory.Exists(logPath))
                    {
                        Directory.CreateDirectory(logPath);
                    }

                    var filePath = Path.Combine(logPath, $"callbackurl_log_{DateTime.Now:yyyyMMdd_HHmmss}.json");
                    var jsonString = JsonSerializer.Serialize(bankResponse, new JsonSerializerOptions { WriteIndented = true });
                    System.IO.File.WriteAllText(filePath, jsonString);

                    if (!string.IsNullOrEmpty(oid))
                        {
                            string[] oid_parameters = oid.Split('-');

                            if (oid_parameters.Length == 10)
                            {
                                int yildat_id = int.Parse(oid_parameters[0]);
                                string hizmet_turu = oid_parameters[1];
                                string yil = oid_parameters[2];
                                string odeme_turu = oid_parameters[3];
                                int odeme_id = int.Parse(oid_parameters[4]);

                                var isSuccess =
                                    response == "Approved" &&
                                    procReturnCode == "00" &&
                                    (mdStatus == "1" || mdStatus == "2" || mdStatus == "3" || mdStatus == "4");

                                if (isSuccess)
                                {
                                    if (hizmet_turu == "sayac")
                                    {
                                        await _odooAppService.CreateRecordSayacAsync(yildat_id, odeme_id, hizmet_turu, odeme_turu, Convert.ToDecimal(amount));
                                    }
                                    else
                                    {
                                        await _odooAppService.CreateRecordAsync(yildat_id, hizmet_turu, odeme_turu, Convert.ToDecimal(amount));
                                    }

                                    //Flutter webview kapatmak için func

                                    var html = @"
                                    <!DOCTYPE html>
                                    <html lang='tr'>
                                    <head>
                                    <meta charset='UTF-8'>
                                    <title>Ödeme Tamamlandı</title>
                                    <script>
                                    document.addEventListener('DOMContentLoaded', function() {
                                        try {
                                            window.close();
                                            setTimeout(function(){
                                                document.body.innerHTML = '<h2>Ödeme başarılı! Bu pencereyi kapatabilirsiniz.</h2>';
                                            }, 200);
                                        } catch (e) {
                                            document.body.innerHTML = '<h2>Ödeme başarılı! Bu pencereyi kapatabilirsiniz.</h2>';
                                        }
                                    });
                                    </script>
                                    </head>
                                    <body>
                                    <h2>Ödeme başarılı! Pencere kapanıyor...</h2>
                                    </body>
                                    </html>";

                                    return Content(html, "text/html");
                                }
                                
                                else
                                {
                                    return Ok("Callback URL işleme tamamlandı, ancak ödeme durumu kontrol edilemedi.");
                                }
                            }
                            else
                            {
                                return BadRequest("Oid parametresi beklenen formatta değil.");
                            }
                        }
                        else
                        {
                            return BadRequest("Oid parametresi boş olamaz.");
                        }
                        





                    
                    
                }
                catch (System.Exception)
                {
                    
                    return StatusCode(500, "Log yazma hatası");
                }
                    
            }

            

            // [HttpPost("callbackurl")]
            // public async Task<IActionResult> CallbackUrl([FromForm] Dictionary<string,string> bankResponse)
            // {
            //     try
            //     {
            //         bankResponse.TryGetValue("HASH", out var hash);
            //         bankResponse.TryGetValue("clientid", out var clientid);
            //         bankResponse.TryGetValue("oid", out var oid);
            //         bankResponse.TryGetValue("ErrMsg", out var errmsg);
            //         bankResponse.TryGetValue("amount", out var amount);
            //         bankResponse.TryGetValue("currency", out var currency);
            //         bankResponse.TryGetValue("ProcReturnCode", out var procReturnCode_);
            //         bankResponse.TryGetValue("AuthCode", out var authCode);
            //         bankResponse.TryGetValue("storetype", out var storeType);
            //         bankResponse.TryGetValue("mdStatus", out var mdStatus_);
            //         bankResponse.TryGetValue("Response", out var response_);
            //         bankResponse.TryGetValue("clientip", out var clientip);
            //         bankResponse.TryGetValue("hashAlgorithm", out var hashAlgorithm);
            //         bankResponse.TryGetValue("TransId", out var transid);
            //         bankResponse.TryGetValue("rnd", out var rnd);
                    
            //         var hashCheck = _hashService.VerifyPaymentResponse(bankResponse);

            //         var logPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            //         if (!Directory.Exists(logPath))
            //         {
            //             Directory.CreateDirectory(logPath);
            //         }

            //         var filePath = Path.Combine(logPath, $"callbackurl_log_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
            //         using (var writer = new StreamWriter(filePath))
            //         {
            //             foreach (var kvp in bankResponse)
            //             {
            //                 writer.WriteLine($"""{kvp.Key}: {kvp.Value}""");
            //             }
            //         }

            //         if (! await _yildatPaymentResponseService.IsExistsAsync(hash) && hash != null && hash.Length == 88)
            //         {                        
            //             //Mapper
            //             YildatPaymentResponse ypr = new YildatPaymentResponse();
            //             ypr.Hash = hash;
            //             ypr.Clientid = long.TryParse(clientid, out var parsedClientId) ? parsedClientId : (long?)null;
            //             ypr.Oid = oid;
            //             ypr.Errmsg = errmsg;
            //             ypr.Amount = Convert.ToDecimal(amount);
            //             ypr.Currency = Convert.ToInt16(currency);
            //             ypr.Procreturncode = procReturnCode_;
            //             ypr.Authcode = authCode;
            //             ypr.Storetype = storeType;
            //             ypr.Mdstatus = Convert.ToInt16(mdStatus_);
            //             ypr.Response = response_;
            //             ypr.Clientip = IPAddress.TryParse(clientip, out var parsedClientIp) ? parsedClientIp : null;
            //             ypr.Hashalgorithm = hashAlgorithm;
            //             ypr.Transid = transid;
                        
            //             if (!string.IsNullOrEmpty(oid))
            //             {
            //                 string[] oid_parameters = oid.Split('~');

            //                 if (oid_parameters.Length == 5)
            //                 {
            //                     int yildat_id = int.Parse(oid_parameters[0]);
            //                     string hizmet_turu = oid_parameters[1];
            //                     string yil = oid_parameters[2];
            //                     string odeme_turu = oid_parameters[3];

            //                     var isSuccess =
            //                         response_ == "Approved" &&
            //                         procReturnCode_ == "00" &&
            //                         (mdStatus_ == "1" || mdStatus_ == "2" || mdStatus_ == "3" || mdStatus_ == "4");

            //                     if (isSuccess)
            //                     {
            //                         await _yildatPaymentResponseService.CreatePaymentResponseAsync(ypr);
            //                         var result = await _odooAppService.CreateRecordAsync(yildat_id, hizmet_turu, odeme_turu, Convert.ToDecimal(amount));
                                    
            //                         return Ok("Callback URL işleme tamamlandı, ödeme başarılı. Odoo kaydı oluşturuldu.");
            //                     }
            //                     else
            //                     {
            //                         return Ok("Callback URL işleme tamamlandı, ancak ödeme durumu kontrol edilemedi.");
            //                     }
            //                 }
            //                 else
            //                 {
            //                     return BadRequest("Oid parametresi beklenen formatta değil.");
            //                 }
            //             }
            //             else
            //             {
            //                 return BadRequest("Oid parametresi boş olamaz.");
            //             }
            //         }
            //         else{
            //             return Ok("Callback URL işleme tamamlandı");
            //         }
            //     }
            //     catch (Exception ex)
            //     {
            //         return StatusCode(500, $"Log yazma hatası: {ex.Message}");
            //     }
            //}

            
            
        }
    }