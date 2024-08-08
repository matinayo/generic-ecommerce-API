using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Services.Contract
{
    public interface IEmailSenderOperation
    {
        Task SendEmailAsync(string receiverEmail, string subject, string plainTextMessage, string htmlMessage);
    }
}
