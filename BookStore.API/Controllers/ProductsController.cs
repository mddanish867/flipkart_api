using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookStore.API.Common;
using BookStore.API.Common.Settlements;
using BookStore.API.Data.Models;
using BookStore.API.Services;
using BookStore.API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static BookStore.API.Common.Response;
using BookStore.API.Services.Implementation;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        Response responseobj = new Response();
        ResponseName responsename = new ResponseName();
        Dictionary<string, object> dict = new Dictionary<string, object>();

        private readonly IProductsServices _productsServices;
        public ProductsController(IProductsServices productsServices)
        {
            _productsServices = productsServices;
        }

        //<summary>
        ///method is used to create contract 
        /// </summary>
        [HttpPost]
        [Route("[action]/products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Addproducts([FromBody] Products jsonrequestobj)
        {
            try
            {
                if (jsonrequestobj == null)
                {
                    responseobj.message = "JsonObj request should not be null";
                    return BadRequest(responseobj);
                }
                else
                {
                    var serviceResult = await _productsServices.add_products(jsonrequestobj);
                    responseobj.result = serviceResult.Result;
                    responseobj.message = serviceResult.ErrorMessage;
                    if (serviceResult.Code == ServiceResultCode.Ok)
                        return Ok(responseobj);
                    //else if (serviceResult.Code == ServiceResultCode.NotFound)
                    //    return NotFound(responseobj);
                    else
                        return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
                }

            }
            catch (Exception ex)
            {
                responseobj.message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
            }
        }


        ///<summary
        ///method to get producs list details 
        ///</summary>
        //[HttpGet, Authorize]
        [HttpGet]
        [Route("[action]/products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RetrieveProductList(int? ProductId = null, string? ProductName="", string? ProductDescription="", decimal? ProductPrice=null, decimal? ProductDiscount=null, int? ProductQuanitity=null, string? ProductImageurl="", int? Status=null, string? Category="", string? SubCategory="", decimal? rating = null, string? Color="", string? Size = "", double? PercDiscount=null, string? Type="", string? Brands="", string? Material="", string? Sleeve="", string? Fabrick="", string? NeckType="", string? Pattern="")
        {
            try
            {

                List<Products> addProducts = await _productsServices.get_products(Convert.ToInt32(ProductId),ProductName, ProductDescription, Convert.ToDecimal(ProductPrice), Convert.ToDecimal(ProductDiscount), Convert.ToInt32(ProductQuanitity), ProductImageurl, Convert.ToInt32(Status), Category, SubCategory, Convert.ToDecimal(rating), Color,Size,Convert.ToDouble(PercDiscount), Type,Brands,Material,Sleeve,Fabrick,NeckType,Pattern);
                if (addProducts.Count == 0)
                {
                    responseobj.message = "Products details not found";
                    return NotFound(responseobj);
                }
                else
                {
                    var addProductsobj = addProducts[0];
                    if (!string.IsNullOrEmpty(addProductsobj.errormessage))
                    {
                        responseobj.message = addProductsobj.errormessage;
                        return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
                    }
                    else
                    {
                        dict.Add(responsename.result, addProducts);
                        return Ok(dict);
                    }
                }
            }
            catch (Exception ex)
            {
                responseobj.message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
            }
        }


        ///<summary
        ///method to get producs details 
        ///</summary>
        //[HttpGet, Authorize]
        [HttpGet]
        [Route("[action]/id/{ProductId}/products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RetrieveProductDetails(int ProductId )
        {
            try
            {

                List<ProductDetails> addProductDetails = await _productsServices.products_details(Convert.ToInt32(ProductId));
                if (addProductDetails.Count == 0)
                {
                    responseobj.message = "Products details not found";
                    return NotFound(responseobj);
                }
                else
                {
                    var addProductDetailsobj = addProductDetails[0];
                    if (!string.IsNullOrEmpty(addProductDetailsobj.errormessage))
                    {
                        responseobj.message = addProductDetailsobj.errormessage;
                        return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
                    }
                    else
                    {
                        dict.Add(responsename.result, addProductDetails);
                        return Ok(dict);
                    }
                }
            }
            catch (Exception ex)
            {
                responseobj.message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
            }
        }

        //<summary>
        ///method is used to create/add products into wishlist
        /// </summary>
        [HttpPost]
        [Route("[action]/favourites")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddProductstoFavourite([FromBody] Faverites jsonrequestobj)
        {
            try
            {
                if (jsonrequestobj == null)
                {
                    responseobj.message = "JsonObj request should not be null";
                    return BadRequest(responseobj);
                }
                else
                {
                    var serviceResult = await _productsServices.add_favourite(jsonrequestobj);
                    responseobj.result = serviceResult.Result;
                    responseobj.message = serviceResult.ErrorMessage;
                    if (serviceResult.Code == ServiceResultCode.Ok)
                        return Ok(responseobj);                    
                    else
                        return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
                }
            }
            catch (Exception ex)
            {
                responseobj.message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
            }
        }

        ///<summary
        ///method to get wish list details 
        ///</summary>
        //[HttpGet, Authorize]
        [HttpGet]
        [Route("[action]/favourites")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RetrieveProductList(string UserName)
        {
            try
            {

                List<Products> addProducts = await _productsServices.get_favourite_products(Convert.ToString(UserName));
                if (addProducts.Count == 0)
                {
                    responseobj.message = "Products details not found";
                    return NotFound(responseobj);
                }
                else
                {
                    var addProductsobj = addProducts[0];
                    if (!string.IsNullOrEmpty(addProductsobj.errormessage))
                    {
                        responseobj.message = addProductsobj.errormessage;
                        return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
                    }
                    else
                    {
                        dict.Add(responsename.result, addProducts);
                        return Ok(dict);
                    }
                }
            }
            catch (Exception ex)
            {
                responseobj.message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
            }
        }

        ///<summary
        ///method to get wish list details 
        ///</summary>
        //[HttpGet, Authorize]
        [HttpGet]
        [Route("[action]/filters")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FilterProducts(int? MinPrice = 0 , int? MaxPrice = 0, string? Brands = "", int? Discount = 0, string? Size = "", string? Color = "", string? Sleeves = "")
        {
            try
            {

                List<Products> filterProducts = await _productsServices.filter_products(Convert.ToInt32(MinPrice),Convert.ToInt32(MaxPrice),Brands,Convert.ToInt32(Discount), Size, Color, Sleeves);
                if (filterProducts.Count == 0)
                {
                    responseobj.message = "No products found based on given filtered criteria!";
                    return NotFound(responseobj);
                }
                else
                {
                    var filterProductsobj = filterProducts[0];
                    if (!string.IsNullOrEmpty(filterProductsobj.errormessage))
                    {
                        responseobj.message = filterProductsobj.errormessage;
                        return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
                    }
                    else
                    {
                        dict.Add(responsename.result, filterProducts);
                        return Ok(dict);
                    }
                }
            }
            catch (Exception ex)
            {
                responseobj.message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
            }
        }

        ///<summary>
        ///method to rating of the products 
        /// </summary> 
        [HttpGet]
        [Route("[action]/product-id/{ProductId}/rating-reviews")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ProductRatings(int ProductId)
        {
            try
            {

                List<RatingReview> productratings = await _productsServices.product_ratings(Convert.ToInt32(ProductId));
                if (productratings.Count == 0)
                {
                    responseobj.message = "No products found from ratings!";
                    return NotFound(responseobj);
                }
                else
                {
                    var productratingsobj = productratings[0];
                    if (!string.IsNullOrEmpty(productratingsobj.errormessage))
                    {
                        responseobj.message = productratingsobj.errormessage;
                        return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
                    }
                    else
                    {
                        dict.Add(responsename.result, productratings);
                        return Ok(dict);
                    }
                }
            }
            catch (Exception ex)
            {
                responseobj.message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
            }
        }

        ///<summary>
        ///method to retrieve rating details
        /// </summary> 
        [HttpGet]
        [Route("[action]/product-id/{ProductId}/rating-reviews-details")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ProductRatingDetails(int ProductId)
        {
            try
            {

                List<RatingReview> productratings = await _productsServices.product_ratings_details(Convert.ToInt32(ProductId));
                if (productratings.Count == 0)
                {
                    responseobj.message = "No ratings found!";
                    return NotFound(responseobj);
                }
                else
                {
                    var productratingsobj = productratings[0];
                    if (!string.IsNullOrEmpty(productratingsobj.errormessage))
                    {
                        responseobj.message = productratingsobj.errormessage;
                        return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
                    }
                    else
                    {
                        dict.Add(responsename.result, productratings);
                        return Ok(dict);
                    }
                }
            }
            catch (Exception ex)
            {
                responseobj.message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
            }
        }

        ///<summary>
        ///method to rating of the products 
        /// </summary> 
        [HttpGet]
        [Route("[action]/productId/{ProductId}/rating-reviews")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PurchasedProductRatings(int ProductId)
        {
            try
            {

                List<RatingReview> productratings = await _productsServices.purchased_product_ratings(Convert.ToInt32(ProductId));
                if (productratings.Count == 0)
                {
                    responseobj.message = "No products found from order to rate!";
                    return NotFound(responseobj);
                }
                else
                {
                    var productratingsobj = productratings[0];
                    if (!string.IsNullOrEmpty(productratingsobj.errormessage))
                    {
                        responseobj.message = productratingsobj.errormessage;
                        return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
                    }
                    else
                    {
                        dict.Add(responsename.result, productratings);
                        return Ok(dict);
                    }
                }
            }
            catch (Exception ex)
            {
                responseobj.message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
            }
        }

        ///<summary>
        ///method to retrieve rating details
        /// </summary> 
        [HttpGet]
        [Route("[action]/product-id/{ProductId}/rating-reviews-details")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ProductRatingFilter(int ProductId, string? Rating = "", string? Recent = "")
        {
            try
            {

                List<RatingReview> productratings = await _productsServices.product_ratings_filter(Convert.ToInt32(ProductId), Rating, Recent);
                if (productratings.Count == 0)
                {
                    responseobj.message = "No ratings found!";
                    return NotFound(responseobj);
                }
                else
                {
                    var productratingsobj = productratings[0];
                    if (!string.IsNullOrEmpty(productratingsobj.errormessage))
                    {
                        responseobj.message = productratingsobj.errormessage;
                        return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
                    }
                    else
                    {
                        dict.Add(responsename.result, productratings);
                        return Ok(dict);
                    }
                }
            }
            catch (Exception ex)
            {
                responseobj.message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
            }
        }


        ///<summary>
        ///method to retrieve product questionair details
        /// </summary> 
        [HttpGet]
        [Route("[action]/product-id/{ProductId}/product-questionair-details")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ProductQuestionair(int ProductId, string? search = "")
        {
            try
            {

                List<Questionair> productQuestionair = await _productsServices.product_questionair_details(Convert.ToInt32(ProductId),search);
                if (productQuestionair.Count == 0)
                {
                    responseobj.message = "No ratings found!";
                    return NotFound(responseobj);
                }
                else
                {
                    var productratingsobj = productQuestionair[0];
                    if (!string.IsNullOrEmpty(productratingsobj.errormessage))
                    {
                        responseobj.message = productratingsobj.errormessage;
                        return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
                    }
                    else
                    {
                        dict.Add(responsename.result, productQuestionair);
                        return Ok(dict);
                    }
                }
            }
            catch (Exception ex)
            {
                responseobj.message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
            }
        }
    }
}
