using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Domain.Utilities.Results.Implementations;
using Infrastructure.Payments.HashHelper;
using Infrastructure.Payments.Interfaces;
using Infrastructure.Persistence.Models.Payments;
using Microsoft.Extensions.Options;

namespace Infrastructure.Payments.Providers.Halkbank
{
    public class HalkbankPaymentProvider : IPaymentProvider
    {
        private readonly IHashHelper _hashHelper;
        private readonly string PaymentUrl;
        private readonly string StoreKey;
        private readonly string ClientId;
        private readonly string BaseUrl;

        public HalkbankPaymentProvider(IOptions<HalkbankPaymentProviderOptions> options, IHashHelper hashHelper)
        {
            PaymentUrl = options.Value.PaymentGatewayUrl;
            StoreKey = options.Value.StoreKey;
            ClientId = options.Value.ClientId;
            BaseUrl = options.Value.BaseUrl;
            _hashHelper = hashHelper;
        }

        public async Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request)
        {
            var rndHash = new Dictionary<string, string>
            {
                {"rndHash", request.key}
            };

            var rndHashValue = _hashHelper.CalculateHashV3(rndHash);
           
            var randomValue = DateTime.Now.Ticks.ToString();
            var parameters = new Dictionary<string, string>
            {
                {"clientid", ClientId},
                {"storetype", "3d_pay_hosting"},
                {"hashAlgorithm", "ver3"},
                {"islemtipi", "Auth"},
                {"amount", request.Amount.ToString("0.00")},
                {"currency", "949"}, 
                {"oid", request.key},
                {"okUrl", "https://payment.thenoorhotels.com/api/payment/success"},
                {"failUrl", "https://payment.thenoorhotels.com/api/payment/fail"},
                {"callbackURL","https://payment.thenoorhotels.com/api/payment/callbackurl"},
                {"lang", "tr"},
                {"rnd", randomValue},
                {"refreshtime", "5"},
                {"BillToName","name"},
                {"BillToCompany","billToCompany"},
                {"description",rndHashValue}
            };
            parameters["hash"] = _hashHelper.CalculateHashV3(parameters);
            

            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("<title></title>");
            sb.AppendLine("<script src=\"https://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js\"></script>");
            sb.AppendLine("<script>");
            sb.AppendLine("$(document).ready(function () {");
            sb.AppendLine("document.getElementById('frm').submit();");
            sb.AppendLine("});");
            sb.AppendLine("</script>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("<center>");
            sb.AppendLine("<form id=\"frm\" method=\"post\" action=\"https://sanalpos.halkbank.com.tr/fim/est3Dgate\">");

            foreach (var pair in parameters)
            {
                sb.AppendLine($"<input type=\"hidden\" name=\"{pair.Key}\" value=\"{pair.Value}\" />");
            }

            sb.AppendLine("</form>");
            sb.AppendLine("</center>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
        
            PaymentResponse response = new PaymentResponse();
            response.postData = sb.ToString();
            response.postUrl = PaymentUrl;

            return response;

        }

    }
}