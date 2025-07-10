using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Dtos.Response.TokenResponse
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public string ExpireInMinutes { get; set; }
        public string TokenType { get; set; } = "Bearer";
        public string Description { get; set; }
    }
}