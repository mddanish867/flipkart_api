using BookStore.API.Common;
using BookStore.API.Data.Models;
using BookStore.API.Data.Repository.Implementation;

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

        ///<summary>
        ///method to get wish list details 
        /// </summary>      
        Task<List<Products>> get_favourite_products(string UserName);

        ///<summary>
        ///method to filter the products 
        /// </summary>      
        Task<List<Products>> filter_products(int MinPrice, int MaxPrice, string Brands, int Discount, string Size, string Color, string Sleeves);

        ///<summary>
        ///method to rating of the products 
        /// </summary>      
       Task<List<RatingReview>> product_ratings(int ProductId);

        ///<summary>
        ///method to retrieve rating details
        /// </summary>      
        Task<List<RatingReview>> product_ratings_details(int ProductId);

        ///<summary>
        ///method to retrieve products to provide feedback to purchased item  
        /// </summary>      
        Task<List<RatingReview>> purchased_product_ratings(int ProductId);

        ///<summary>
        ///method to filter the rating of the products 
        /// </summary>
        Task<List<RatingReview>> product_ratings_filter(int ProductId, string Rating, string Recent);


        ///<summary>
        ///method to retrieve Questinair details
        /// </summary>  
        Task<List<Questionair>> product_questionair_details(int ProductId, string search);       
    }
}
