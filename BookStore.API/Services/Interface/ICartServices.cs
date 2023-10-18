using BookStore.API.Common;
using BookStore.API.Data.Models;

namespace BookStore.API.Services.Interface
{
    public interface ICartServices
    {
        //<summary>
        ///method is used to add product into cart 
        /// </summary>
        Task<ServiceResult<string>> addto_cart(Cart jsonrequestobj);

        //<summary>
        ///method to get cart details 
        /// </summary>      
        Task<List<Products>> cart_details(string UserName, int ProductId);

        //<summary>
        ///method is used to remove product from cart
        /// </summary>
        Task<ServiceResult<string>> remove_product(int ProductId);

    }
}
