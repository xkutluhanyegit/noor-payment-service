using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Dtos.Request.RequestCode
{
    public class RequestCodeDataDto
    {
        public string SmsToken { get; set; }
        public int ExpiresInSeconds { get; set; }
    }
}