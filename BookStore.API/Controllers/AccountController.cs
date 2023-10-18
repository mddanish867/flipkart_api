using BookStore.API.Common;
using BookStore.API.Common.Settlements;
using BookStore.API.Data.Models;
using BookStore.API.Services;
using BookStore.API.Services.Implementation;
using BookStore.API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using NETCore.MailKit.Core;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static BookStore.API.Common.Response;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        Response responseobj = new Response();
        ResponseName responsename = new ResponseName();
        Dictionary<string, object> dict = new Dictionary<string, object>();

        private readonly IAccountServices _accountServices;

        
        public AccountController(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }


     

        //<summary>
        ///method is used to create user 
        /// </summary>
        [HttpPost]
        [Route("[action]/Users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUser([FromBody] AddUser jsonrequestobj)
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
                    var serviceResult = await _accountServices.create_user(jsonrequestobj);
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
        ///This method is used to get peanut specification exception list details
        ///</summary>
        //[HttpGet, Authorize]
        [HttpGet]
        [Route("[action]/email/{Email}/users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RetrieveUserDetails(string Email, string? Password,int? UserId)
        {
            try
            {
                if (UserId == 0)
                {
                    responseobj.message = "Email & Password values should not be null";
                    return BadRequest(responseobj);
                }
                List<AddUser> addUsers = await _accountServices.user_details(Email, Password,(Convert.ToInt32(UserId)));
                if (addUsers.Count == 0)
                {
                    responseobj.message = "User details not found";
                    return NotFound(responseobj);
                }
                else
                {
                    var addUsersobj = addUsers[0];
                    if (!string.IsNullOrEmpty(addUsersobj.errormessage))
                    {
                        responseobj.message = addUsersobj.errormessage;
                        return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
                    }
                    else
                    {
                        dict.Add(responsename.result, addUsers);
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


        [HttpPost("[action]/users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UserLogin([FromBody] LoginModel user)
        {
            try
            {
                if (user == null)
                {
                    responseobj.message = "user request should not be null";
                    return BadRequest(responseobj);
                }
                else
                {
                    var serviceResult = await _accountServices.users(user);
                    responseobj.result = serviceResult.Result;
                    responseobj.message = serviceResult.ErrorMessage;
                    if (serviceResult.Code == ServiceResultCode.Ok)
                        return Ok(responseobj);
                    else if (serviceResult.Code == ServiceResultCode.NotFound)
                        return NotFound(responseobj);
                    else
                        return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
                }
            }
            catch(Exception ex)
            {
                responseobj.message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
            }           
           
        }


        ///<summary
        ///This method is used to get category list details
        ///</summary>
        //[HttpGet, Authorize]
        [HttpGet]
        [Route("[action]/categories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RetrieveCategoryDetails()
        {
            try
            {
               
                List<Categories> addCategories = await _accountServices.category_details();
                if (addCategories.Count == 0)
                {
                    responseobj.message = "Category details not found";
                    return NotFound(responseobj);
                }
                else
                {
                    var addCategoriesobj = addCategories[0];
                    if (!string.IsNullOrEmpty(addCategoriesobj.errormessage))
                    {
                        responseobj.message = addCategoriesobj.errormessage;
                        return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
                    }
                    else
                    {
                        dict.Add(responsename.result, addCategories);
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
        ///This method is used to get subcategory list details
        ///</summary>
        //[HttpGet, Authorize]
        [HttpGet]
        [Route("[action]/subcategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RetrieveSubCategoryDetails(int? CategoryId, string? Category = "", string? SubCategory = "")
        {
            try
            {

                List<SubCategories> addSubCategories = await _accountServices.subcategory_details(Convert.ToInt32(CategoryId),Category, SubCategory);
                if (addSubCategories.Count == 0)
                {
                    responseobj.message = "SubCategory details not found";
                    return NotFound(responseobj);
                }
                else
                {
                    var addSubCategoriesobj = addSubCategories[0];
                    if (!string.IsNullOrEmpty(addSubCategoriesobj.errormessage))
                    {
                        responseobj.message = addSubCategoriesobj.errormessage;
                        return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
                    }
                    else
                    {
                        dict.Add(responsename.result, addSubCategories);
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
        ///method is used to create user 
        /// </summary>
        [HttpPost]
        [Route("[action]/delivery-address")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddDeleveryAddress([FromBody] DeliveryAddress jsonrequestobj)
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
                    var serviceResult = await _accountServices.add_delivery_address(jsonrequestobj);
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
        ///method to get user address
        ///</summary>
        //[HttpGet, Authorize]
        [HttpGet]
        [Route("[action]/user_name/{UserName}/delivery_address")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RetrieveDeliveryAddress( string UserName)
        {
            try
            {

                List<DeliveryAddress> getDeleiveryAddress = await _accountServices.get_user_address(UserName);
                if (getDeleiveryAddress.Count == 0)
                {
                    responseobj.message = "Delivery address not found";
                    return NotFound(responseobj);
                }
                else
                {
                    var getDeliveryAddressobj = getDeleiveryAddress[0];
                    if (!string.IsNullOrEmpty(getDeliveryAddressobj.errormessage))
                    {
                        responseobj.message = getDeliveryAddressobj.errormessage;
                        return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
                    }
                    else
                    {
                        dict.Add(responsename.result, getDeleiveryAddress);
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
        ///method is used to create Rating and Reviews 
        /// </summary>
        [HttpPost]
        [Route("[action]/rating_reviews")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RatingReviews([FromBody] RatingReview jsonrequestobj)
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
                    var serviceResult = await _accountServices.add_reviews(jsonrequestobj);
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

    }
}

