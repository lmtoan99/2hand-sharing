using Application.DTOs.Email;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Service;
using Domain.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<MailSettings> mailSettings, ILogger<EmailService> logger)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
            logger.LogInformation("Create SendMailService");
        }

        public async Task SendAsync(EmailRequest mailContent)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.EmailFrom);
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.EmailFrom));
            email.To.Add(MailboxAddress.Parse(mailContent.To));
            email.Subject = mailContent.Subject;


            var builder = new BodyBuilder();
            builder.HtmlBody = mailContent.Body;
            email.Body = builder.ToMessageBody();

            // dùng SmtpClient của MailKit
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                smtp.Connect(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.EmailFrom, _mailSettings.Password);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
                System.IO.Directory.CreateDirectory("mailssave");
                var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                await email.WriteToAsync(emailsavefile);

                _logger.LogInformation("Lỗi gửi mail, lưu tại - " + emailsavefile);
                _logger.LogError(ex.Message);
            }

            smtp.Disconnect(true);

            _logger.LogInformation("send mail to " + mailContent.To);
        }

        //public async Task SendAsync(string email, string subject, string htmlMessage)
        //{
        //    await SendMail(new EmailRequest()
        //    {
        //        To = email,
        //        Subject=subject,
        //        Body=htmlMessage
        //    });

        //}

        //public async Task SendAsync(EmailRequest request)
        //{
        //    try
        //    {
        //        // create message
        //        var email = new MimeMessage();
        //        email.Sender = MailboxAddress.Parse(request.From ?? _mailSettings.EmailFrom);
        //        email.To.Add(MailboxAddress.Parse(request.To));
        //        email.Subject = request.Subject;
        //        var builder = new BodyBuilder();
        //        builder.HtmlBody = request.Body;
        //        email.Body = builder.ToMessageBody();
        //        using var smtp = new SmtpClient();
        //        smtp.Connect(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
        //        smtp.Authenticate(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
        //        await smtp.SendAsync(email);
        //        smtp.Disconnect(true);

        //    }
        //    catch (System.Exception ex)
        //    {
        //        _logger.LogError(ex.Message, ex);
        //        throw new ApiException(ex.Message);
        //    }
        //}
    }
}
