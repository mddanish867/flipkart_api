using BookStore.API.Common;
using BookStore.API.Data.Models;

namespace BookStore.API.Data.Repository.Interface
{
    public interface IMailRepository 
    {

        ///<summary>
        ///method to send mail notification
        /// </summary>
        Task<ServiceResult<string>> send_email(SendEmailNotification sendEmail);
    }
}
