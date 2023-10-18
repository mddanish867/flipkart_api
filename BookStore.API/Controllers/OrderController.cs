using BookStore.API.Common;
using BookStore.API.Data.Models;
using BookStore.API.Services.Implementation;
using BookStore.API.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static BookStore.API.Common.Response;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        Response responseobj = new Response();
        ResponseName responsename = new ResponseName();
        Dictionary<string, object> dict = new Dictionary<string, object>();

        private readonly IOrdersService _ordersService;
        public OrderController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }
        //<summary>
        ///method is used to create orders 
        /// </summary>
        [HttpPost]
        [Route("[action]/products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PlacePreOrder([FromBody] PreOrders jsonrequestobj)
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
                    var serviceResult = await _ordersService.create_orders(jsonrequestobj);
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

        //<summary>
        ///method is used to create Order 
        /// </summary>
        [HttpPost]
        [Route("[action]/orders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PlaceOrder([FromBody] Orders jsonrequestobj)
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
                    var serviceResult = await _ordersService.add_orders(jsonrequestobj);
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
        [Route("[action]/orders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RetrieveOrderDetails(string? UserName = "", string? OrderTrackId = "")
        {
            try
            {

                List<Orders> getOrderDetails = await _ordersService.get_order_details(UserName, OrderTrackId);
                if (getOrderDetails.Count == 0)
                {
                    responseobj.message = "Orders not found";
                    return NotFound(responseobj);
                }
                else
                {
                    var getOrderDetailsobj = getOrderDetails[0];
                    if (!string.IsNullOrEmpty(getOrderDetailsobj.errormessage))
                    {
                        responseobj.message = getOrderDetailsobj.errormessage;
                        return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
                    }
                    else
                    {
                        dict.Add(responsename.result, getOrderDetails);
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
        ///method to get user address
        ///</summary>
        //[HttpGet, Authorize]
        [HttpGet]
        [Route("[action]/orders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FilterOrder(string Status)
        {
            try
            {

                List<Orders> getOrderDetails = await _ordersService.filter_orders(Status);
                if (getOrderDetails.Count == 0)
                {
                    responseobj.message = "Orders not found";
                    return NotFound(responseobj);
                }
                else
                {
                    var getOrderDetailsobj = getOrderDetails[0];
                    if (!string.IsNullOrEmpty(getOrderDetailsobj.errormessage))
                    {
                        responseobj.message = getOrderDetailsobj.errormessage;
                        return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
                    }
                    else
                    {
                        dict.Add(responsename.result, getOrderDetails);
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
