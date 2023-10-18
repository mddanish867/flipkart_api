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

        //<summary>
        ///method is used to create/update Product 
        /// </summary>
        public async Task<ServiceResult<string>> add_products(Products jsonrequestobj)
        {
            ServiceResult<string> resultobj = await _productsRepository.add_products(jsonrequestobj);
            return resultobj;
        }

        //<summary>
        ///method to get producs list details 
        /// </summary>
        public async Task<List<Products>> get_products(int ProductId, string ProductName , string ProductDescription , decimal ProductPrice , decimal ProductDiscount , int ProductQuanitity, string ProductImageurl , int Status, string Category , string SubCategory , decimal rating, string Color, string Size , double PercDiscount , string Type , string Brands , string Material , string Sleeve , string Fabrick , string NeckType , string Pattern )
        {
            List<Products> addProducts = await _productsRepository.get_products(ProductId,ProductName, ProductDescription,ProductPrice,ProductDiscount,ProductQuanitity,ProductImageurl,Status,Category,SubCategory,rating,Color,Size,PercDiscount,Type,Brands,Material,Sleeve, Fabrick, NeckType, Pattern);
            return addProducts;
        }

        //<summary>
        ///method to get producs details 
        /// </summary>
        public async Task<List<ProductDetails>> products_details(int ProductId)
        {
            List<ProductDetails> addProductDetails = await _productsRepository.products_details(ProductId);
            return addProductDetails;
        }

        //<summary>
        ///method is used to create/add products into wishlist
        /// </summary>
        public async Task<ServiceResult<string>> add_favourite(Faverites jsonrequestobj)
        {
            ServiceResult<string> resultobj = await _productsRepository.add_favourite(jsonrequestobj);
            return resultobj;
        }

        //<summary>
        ///method to get wish list details 
        /// </summary>
        public async Task<List<Products>> get_favourite_products(string UserName)
        {
            List<Products> addProducts = await _productsRepository.get_favourite_products(UserName);
            return addProducts;
        }
    }
}
