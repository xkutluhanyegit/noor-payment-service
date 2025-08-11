using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories.OdooYildatOdeme
{
    public class OdooService : IOdooService
    {
        private readonly HttpClient _httpClient;
        public OdooService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> CreateOdooSayacOdeme(int id, int odeme_id, string kurulumTuru, string odemeTipi, decimal tutar)
        {
            var payload = new
            {
                jsonrpc = "2.0",
                method = "call",
                @params = new
                {
                    service = "object",
                    method = "execute",
                    args = new object[]
                    {
                        "NOOR17",
                        2,
                        "Sn569632",
                        "yildat",
                        "write",
                        new[] { id },
                        new
                        {
                            odeme_ids = new[]
                            {
                                new object[]
                                {
                                    1,
                                    odeme_id,
                                    new
                                    {
                                        hizmet_turu = kurulumTuru,
                                        odeme_turu = odemeTipi,
                                        tutar = tutar,
                                        odeme_tarihi = DateTime.UtcNow.ToString("yyyy-MM-dd")
                                    }
                                }
                            }
                        }
                    }
                },
                id = 1
            };

            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(payload), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://portal.thenoorhotels.com/jsonrpc", content);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> CreateOdooYildatOdeme(int id, string kurulumTuru, string odemeTipi, decimal tutar)
        {
            var payload = new
            {
                jsonrpc = "2.0",
                method = "call",
                @params = new
                {
                    service = "object",
                    method = "execute",
                    args = new object[]
                    {
                        "NOOR17",
                        2,
                        "Sn569632",
                        "yildat",
                        "write",
                        new[] { id },
                        new
                        {
                            odeme_ids = new[]
                            {
                                new object[]
                                {
                                    0,
                                    0,
                                    new
                                    {
                                        hizmet_turu = kurulumTuru,
                                        odeme_turu = odemeTipi,
                                        tutar = tutar,
                                        odeme_tarihi = DateTime.UtcNow.ToString("yyyy-MM-dd")
                                    }
                                }
                            }
                        }
                    }
                },
                id = 1
            };

            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(payload), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://portal.thenoorhotels.com/jsonrpc", content);
            return await response.Content.ReadAsStringAsync();
        }
    }
}