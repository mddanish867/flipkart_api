using BookStore.API.Common;
using BookStore.API.Data.Models;
using BookStore.API.Data.Repository.Implementation;
using BookStore.API.Data.Repository.Interface;
using BookStore.API.Services.Interface;

namespace BookStore.API.Services.Implementation
{
    public class CartServices:ICartServices
    {
        private readonly ICartRepository _cartRepository;

        public CartServices(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        //<summary>
        ///method is used to add product into cart 
        /// </summary>
        public async Task<ServiceResult<string>> addto_cart(Cart jsonrequestobj)
        {
            ServiceResult<string> resultobj = await _cartRepository.addto_cart(jsonrequestobj);
            return resultobj;
        }


        //<summary>
        ///method to get cart details 
        /// </summary>
        public async Task<List<Products>> cart_details(string UserName, int ProductId)
        {
            List<Products> addCartDetails = await _cartRepository.cart_details(UserName,ProductId);
            return addCartDetails;
        }

        //<summary>
        ///method is used to remove product from cart
        /// </summary>
        public async Task<ServiceResult<string>> remove_product(int ProductId)
        {
            ServiceResult<string> resultobj = await _cartRepository.remove_product(ProductId);
            return resultobj;
        }
    }
}
