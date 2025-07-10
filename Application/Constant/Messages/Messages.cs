using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Constant.Messages
{
    public static class Messages
    {
        public static string SuccessToken = "Token başarıyla oluşturuldu.";

        public static string InvalidSmsCode = "Geçersiz SMS kodu.";
        public static string ExpireSmsCode = "Sms kodunun süresi dolmuş.";
        public static string PhoneClaimNotFound = "Geçersiz kimlik doğrulama.";


        public static string NotFoundTCKN ="TC Kimlik numarasına ait kullanıcı bulunamadı.";
    }
}