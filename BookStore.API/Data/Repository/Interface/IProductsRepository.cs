using BookStore.API.Common;
using BookStore.API.Data.Models;

namespace BookStore.API.Data.Repository.Interface
{
    public interface IProductsRepository
    {
        //<summary>
        ///method is used to create/add products 
        /// </summary>
        Task<ServiceResult<string>> add_products(Products jsonrequestobj);

        //<summary>
        ///method to get producs list details 
        /// </summary>      
        Task<List<Products>> get_products(int ProductId, string ProductName, string ProductDescription, decimal ProductPrice, decimal ProductDiscount, int ProductQuanitity, string ProductImageurl, int Status, string Category, string SubCategory, decimal rating, string Color, string Size, double PercDiscount, string Type, string Brands, string Material, string Sleeve, string Fabrick, string NeckType, string Pattern);


        //<summary>
        ///method to get producs details 
        /// </summary>      
        Task<List<ProductDetails>> products_details(int ProductId);

        //<summary>
        ///method is used to create/add products into wishlist
        /// </summary>
        Task<ServiceResult<string>> add_favourite(Faverites jsonrequestobj);

        //<summary>
        ///method to get wish list details 
        /// </summary>      
        Task<List<Products>> get_favourite_products(string UserName);

    }
}
