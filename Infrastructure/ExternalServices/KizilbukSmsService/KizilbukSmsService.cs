using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExternalServices.KizilbukSmsService
{
    public class KizilbukSmsService : IKizilbukSmsService
    {
        private readonly HttpClient _httpClient;
        public KizilbukSmsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public Task<bool> SendSmsAsync(string phoneNumber, string smsCode)
        {
            string xmlBody = $@"<?xml version=""1.0"" encoding=""utf-8"" ?>
                <MainmsgBody>
                <Command>0</Command>
                <PlatformID>1</PlatformID>
                <ChannelCode>608</ChannelCode>
                <UserName>otpkizilbuk</UserName>
                <PassWord>4xf%VRHr9</PassWord>
                <Mesgbody>Doğrulama kodunuz: {smsCode}. Bu kod 5 dakika geçerlidir.Lütfen başkalarıyla paylaşmayın.</Mesgbody>
                <Numbers>{phoneNumber}</Numbers>
                <Type>1</Type>
                <Originator>KIZILBUK</Originator>
                </MainmsgBody>";
            
            var content = new StringContent(xmlBody, Encoding.UTF8, "application/xml");
            var response = _httpClient.PostAsync("http://service2.turatel.com.tr/xml/process.aspx", content).Result;
            return response.IsSuccessStatusCode 
                ? Task.FromResult(true) 
                : Task.FromResult(false);       
        }
    }
}