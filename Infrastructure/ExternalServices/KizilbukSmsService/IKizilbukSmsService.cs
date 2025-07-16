using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.ExternalServices.KizilbukSmsService
{
    public interface IKizilbukSmsService
    {
        Task<bool> SendSmsAsync(string phoneNumber, string message);
    }
}