using BookStore.API.Common;
using BookStore.API.Common.Settlements;
using BookStore.API.Data.Models;
using BookStore.API.Data.Repository.Implementation;
using BookStore.API.Data.Repository.Interface;
using BookStore.API.Services.Interface;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BookStore.API.Services.Implementation
{
    public class AccountServices:IAccountServices
    {
        private readonly IAccountRepository _accountRepository ;

        public AccountServices(IAccountRepository accountRepository )
        {
            _accountRepository = accountRepository;
        }

      
        //<summary>
        ///method is used to create user 
        /// </summary>
        public async Task<ServiceResult<string>> create_user(AddUser jsonrequestobj)
        {
            ServiceResult<string> resultobj = await _accountRepository.create_user(jsonrequestobj);
            return resultobj;
        }
        ///<summary>
        ///method of peanut specification list details
        /// </summary>
        public async Task<List<AddUser>> user_details(string Email, string Password,int UserId)
        {
            List<AddUser> addUsers = await _accountRepository.user_details(Email,Password,UserId);
            return addUsers;
        }

        ///<summary>
        ///method of login details
        /// </summary>
        public async Task<ServiceResult<string>> users(LoginModel user)
        {
            ServiceResult<string> resultobj = await _accountRepository.users(user);
            return resultobj;
        }

        ///<summary>
        ///method to get category list details
        /// </summary>
        public async Task<List<Categories>> category_details()
        {
            List<Categories> addCategories = await _accountRepository.category_details();
            return addCategories;
        }
        ///<summary>
        ///method to get sub category list details
        /// </summary>
        public async Task<List<SubCategories>> subcategory_details(int CategoryId, string Category, string SubCategory)
        {
            List<SubCategories> addSubCategories = await _accountRepository.subcategory_details(CategoryId,Category, SubCategory);
            return addSubCategories;
        }

        //<summary>
        ///method is used to add Delivery Address 
        /// </summary>
        public async Task<ServiceResult<string>> add_delivery_address(DeliveryAddress jsonrequestobj)
        {
            ServiceResult<string> resultobj = await _accountRepository.add_delivery_address(jsonrequestobj);
            return resultobj;
        }
        
        ///<summary>
        ///method to get user address
        /// </summary>
        public async Task<List<DeliveryAddress>> get_user_address(string UserName)
        {
            List<DeliveryAddress> getUserAddress = await _accountRepository.get_user_address(UserName);
            return getUserAddress;
        }
        //<summary>
        ///method is used to create Rating and Reviews
        /// </summary>
        public async Task<ServiceResult<string>> add_reviews(RatingReview jsonrequestobj)
        {
            ServiceResult<string> resultobj = await _accountRepository.add_reviews(jsonrequestobj);
            return resultobj;
        }

    }
}
