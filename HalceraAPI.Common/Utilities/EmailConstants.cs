using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Common.Utilities
{
    public static class EmailConstants
    {
        public const string ForgotPasswordSubject = "Request to change password";

        public static string ForgotPasswordPlainTextMessage(string userPasswordResetToken)
        {
            return $"Request to change password {userPasswordResetToken}";
        }
        public const string ForgotPasswordHtmlMessage = "Request to change password";
    }
}
