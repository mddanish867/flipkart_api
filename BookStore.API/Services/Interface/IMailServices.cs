using BookStore.API.Common;
using BookStore.API.Data.Models;

namespace BookStore.API.Services.Interface
{
    public interface IMailServices
    {
        ///<summary>
        ///method to send mail notification
        /// </summary>
        Task<ServiceResult<string>> send_email(SendEmailNotification sendEmail);
    }
}
