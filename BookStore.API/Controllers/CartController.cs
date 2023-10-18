using BookStore.API.Common;
using BookStore.API.Data.Models;
using BookStore.API.Services.Implementation;
using BookStore.API.Services.Interface;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static BookStore.API.Common.Response;
using static BookStore.API.Common.CustomError;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        Response responseobj = new Response();
        ResponseName responsename = new ResponseName();
        Dictionary<string, object> dict = new Dictionary<string, object>();

        private readonly ICartServices _cartServices;
        public CartController(ICartServices cartServices )
        {
            _cartServices = cartServices;
        }

        //<summary>
        ///method is used to add product into cart 
        /// </summary>
        [HttpPost]
        [Route("[action]/Cart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddProduct([FromBody] Cart jsonrequestobj)
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
                    var serviceResult = await _cartServices.addto_cart(jsonrequestobj);
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
        ///method to get cart details 
        ///</summary>
        //[HttpGet, Authorize]
        [HttpGet]
        [Route("[action]/cart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RetrieveCartDetails(string? UserName, int? ProductId)
        {
            try
            {

                List<Products> addCartDetails = await _cartServices.cart_details(Convert.ToString(UserName),Convert.ToInt32(ProductId));
                if (addCartDetails.Count == 0)
                {
                    responseobj.message = "cart details not found";
                    return NotFound(responseobj);
                }
                else
                {
                    var addCartDetailsobj = addCartDetails[0];
                    if (!string.IsNullOrEmpty(addCartDetailsobj.errormessage))
                    {
                        responseobj.message = addCartDetailsobj.errormessage;
                        return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
                    }
                    else
                    {
                        dict.Add(responsename.result, addCartDetails);
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
        ///method is used to remove product from cart
        /// </summary>
        [HttpDelete]
        [Route("[action]/product-id/{ProductId}/cart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveProduct(int ProductId)
        {
            try
            {
                if (ProductId == 0 )
                {
                    responseobj.message = "ProductId should not be null";
                    return BadRequest(responseobj);
                }
                else
                {
                    var serviceResult = await _cartServices.remove_product(ProductId);
                    responseobj.message = serviceResult.ErrorMessage;
                    if (serviceResult.Code == ServiceResultCode.Ok)
                        return Ok(responseobj);
                    else if (serviceResult.Code == ServiceResultCode.NotFound)
                        return NotFound(responseobj);
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

    }
}
