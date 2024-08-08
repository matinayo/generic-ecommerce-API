
using HalceraAPI.Common.AppsettingsOptions;
using HalceraAPI.Services.Contract;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace HalceraAPI.Services.Operations
{
    public class EmailSenderOperation : IEmailSenderOperation
    {
        private readonly EmailSenderOptions _emailOptions;

        public EmailSenderOperation(IOptions<EmailSenderOptions> options)
        {
            _emailOptions = options.Value;
        }

        public async Task SendEmailAsync(string receiverEmail, string subject, string plainTextMessage, string htmlMessage)
        {
            await Execute(receiverEmail, subject, plainTextMessage, htmlMessage);
        }

        private async Task Execute(string receiverEmail, string subject, string plainTextMessage, string htmlMessage)
        {
            var client = new SendGridClient(_emailOptions.SendGridKey);
            var from = new EmailAddress(_emailOptions.SendGridEmail, _emailOptions.SendGridUser);
            var to = new EmailAddress(receiverEmail);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextMessage, htmlMessage);
            await client.SendEmailAsync(msg);
        }
    }
}
