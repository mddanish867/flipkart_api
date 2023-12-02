using BookStore.API.Common;
using BookStore.API.Common.Settlements;
using BookStore.API.Data.Models;
using BookStore.API.Data.Repository.Implementation;
using BookStore.API.Data.Repository.Interface;
using BookStore.API.Services.Interface;
using Microsoft.AspNetCore.Identity;

namespace BookStore.API.Services.Implementation
{
    public class ProductsServices: IProductsServices
    {
        private readonly IProductsRepository _productsRepository;

        public ProductsServices(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        ///<summary>
        ///method is used to create/update Product 
        /// </summary>
        public async Task<ServiceResult<string>> add_products(Products jsonrequestobj)
        {
            ServiceResult<string> resultobj = await _productsRepository.add_products(jsonrequestobj);
            return resultobj;
        }

        ///<summary>
        ///method to get producs list details 
        /// </summary>
        public async Task<List<Products>> get_products(int ProductId, string ProductName , string ProductDescription , decimal ProductPrice , decimal ProductDiscount , int ProductQuanitity, string ProductImageurl , int Status, string Category , string SubCategory , decimal rating, string Color, string Size , double PercDiscount , string Type , string Brands , string Material , string Sleeve , string Fabrick , string NeckType , string Pattern )
        {
            List<Products> addProducts = await _productsRepository.get_products(ProductId,ProductName, ProductDescription,ProductPrice,ProductDiscount,ProductQuanitity,ProductImageurl,Status,Category,SubCategory,rating,Color,Size,PercDiscount,Type,Brands,Material,Sleeve, Fabrick, NeckType, Pattern);
            return addProducts;
        }

        ///<summary>
        ///method to get producs details 
        /// </summary>
        public async Task<List<ProductDetails>> products_details(int ProductId)
        {
            List<ProductDetails> addProductDetails = await _productsRepository.products_details(ProductId);
            return addProductDetails;
        }

        ///<summary>
        ///method is used to create/add products into wishlist
        /// </summary>
        public async Task<ServiceResult<string>> add_favourite(Faverites jsonrequestobj)
        {
            ServiceResult<string> resultobj = await _productsRepository.add_favourite(jsonrequestobj);
            return resultobj;
        }

        ///<summary>
        ///method to get wish list details 
        /// </summary>
        public async Task<List<Products>> get_favourite_products(string UserName)
        {
            List<Products> addProducts = await _productsRepository.get_favourite_products(UserName);
            return addProducts;
        }

        ///<summary>
        ///method to filter the products 
        /// </summary>      
        public async Task<List<Products>> filter_products(int MinPrice, int MaxPrice, string Brands, int Discount, string Size, string Color, string Sleeves)
        {
            List<Products> filterProducts = await _productsRepository.filter_products(MinPrice, MaxPrice, Brands, Discount, Size, Color, Sleeves);
            return filterProducts;
        }

        ///<summary>
        ///method to rating of the products 
        /// </summary>      
        public async Task<List<RatingReview>> product_ratings(int ProductId)
        {
            List<RatingReview> productRatings = await _productsRepository.product_ratings(ProductId);
            return productRatings;
        }

        ///<summary>
        ///method to retrieve rating details
        /// </summary>      
        public async Task<List<RatingReview>> product_ratings_details(int ProductId)
        {
            List<RatingReview> productRatingsDetails = await _productsRepository.product_ratings_details(ProductId);
            return productRatingsDetails;
        }

        ///<summary>
        ///method to retrieve products to provide feedback to purchased item  
        /// </summary>      
        public async Task<List<RatingReview>> purchased_product_ratings(int ProductId)
        {
            List<RatingReview> productRatings = await _productsRepository.purchased_product_ratings(ProductId);
            return productRatings;
        }

        ///<summary>
        ///method to filter the rating of the products 
        /// </summary>      
        public async Task<List<RatingReview>> product_ratings_filter(int ProductId, string Rating, string Recent)
        {
            List<RatingReview> productRatingsFilter = await _productsRepository.product_ratings_filter(ProductId, Rating, Recent);
            return productRatingsFilter;
        }

        ///<summary>
        ///method to retrieve Questinair details
        /// </summary>  
        public async Task<List<Questionair>> product_questionair_details(int ProductId, string search)
        {
            List<Questionair> productQuestionair = await _productsRepository.product_questionair_details(ProductId,search);
            return productQuestionair;
        }
    }
}
