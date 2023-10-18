using BookStore.API.Common;
using BookStore.API.Data.Models;
using BookStore.API.Data.Repository.Interface;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using System.Threading.Channels;

namespace BookStore.API.Data.Repository.Implementation
{
    public class MailRepository :IMailRepository
    {
        private readonly Models.MailSettings _mailSettings;

        public MailRepository(IOptions<Models.MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;           
        }

        ///<summary>
        ///method to send mail notification
        /// </summary>
        public async Task<ServiceResult<string>> send_email(SendEmailNotification sendEmail)
        {
            ServiceResult<string> resultobj = new ServiceResult<string>();
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(sendEmail.ToEmail));
            email.Subject = sendEmail.Subject;
            var builder = new BodyBuilder();
            
            //if (sendEmail.Attachments != null)
            //{
            //    byte[] fileBytes;
            //    foreach (var file in sendEmail.Attachments)
            //    {
            //        if (file.Length > 0)
            //        {
            //            using (var ms = new MemoryStream())
            //            {
            //                file.CopyTo(ms);
            //                fileBytes = ms.ToArray();
            //            }
            //            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
            //        }
            //    }
            //}
            builder.HtmlBody = sendEmail.Body;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
            resultobj.Code = ServiceResultCode.Ok;
            resultobj.ErrorMessage = "email sent sucessfully";
            return resultobj;
        }

        
        
    }
}
