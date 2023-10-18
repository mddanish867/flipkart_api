using BookStore.API.Common;
using BookStore.API.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BookStore.API.Data.Repository.Interface
{
    public interface IAccountRepository
    {
       

        //<summary>
        ///method is used to create contract 
        /// </summary>
        Task<ServiceResult<string>> create_user(AddUser jsonrequestobj);

        ///<summary>
        ///method for user list details
        /// </summary>
        Task<List<AddUser>> user_details(string Email, string Password,int UserId);

        ///<summary>
        ///method of login details
        /// </summary>
        Task<ServiceResult<string>> users(LoginModel user);

        ///<summary>
        ///method to get category list details
        /// </summary>
        Task<List<Categories>> category_details();

        ///<summary>
        ///method to get subcategory list details
        /// </summary>
        Task<List<SubCategories>> subcategory_details(int CategoryId, string Category, string SubCategory);

        //<summary>
        ///method is used to add Delivery Address 
        /// </summary>
        Task<ServiceResult<string>> add_delivery_address(DeliveryAddress jsonrequestobj);

        ///<summary>
        ///method to get user address
        /// </summary>
        Task<List<DeliveryAddress>> get_user_address(string UserName);

        //<summary>
        ///method is used to create rating and reviews 
        /// </summary>
        Task<ServiceResult<string>> add_reviews(RatingReview jsonrequestobj);
    }
}
