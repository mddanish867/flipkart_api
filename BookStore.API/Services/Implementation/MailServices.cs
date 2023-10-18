using BookStore.API.Common;
using BookStore.API.Data.Models;
using BookStore.API.Data.Repository.Interface;
using BookStore.API.Services.Interface;
using MailKit;

namespace BookStore.API.Services.Implementation
{
    public class MailServices: IMailServices
    {

        private readonly IMailRepository _mailRepository;

        public MailServices(IMailRepository mailRepository )
        {
            _mailRepository = mailRepository;
        }
        ///<summary>
        ///method to send mail notification
        /// </summary>
        public async Task<ServiceResult<string>> send_email(SendEmailNotification sendEmail)
        {
            ServiceResult<string> resultobj = await _mailRepository.send_email(sendEmail);
            return resultobj;
        }
    }
}
