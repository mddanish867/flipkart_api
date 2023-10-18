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
    public class MailController : ControllerBase
    {
        Response responseobj = new Response();
        ResponseName responsename = new ResponseName();
        Dictionary<string, object> dict = new Dictionary<string, object>();

        private readonly IMailServices _mailServices;


        public MailController(IMailServices mailServices)
        {
            _mailServices = mailServices;
        }



        //[HttpPost("SendEmail")]        
        //public async Task<IActionResult> SendEmail()
        //{
        //    try
        //    {
        //        SendEmailNotification mailrequest = new SendEmailNotification();
        //        mailrequest.ToEmail = "mddanish867@gmail.com";
        //        mailrequest.Subject = "eCome an online purchase shop";
        //        mailrequest.Body = "You ordered Mens tshirts worth Rs.390";
        //        await _mailServices.send_email(mailrequest);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        ///<summary
        ///method to send mail notification
        ///</summary>
        //[HttpGet, Authorize]
        [HttpPost("SendEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailNotification sendEmail)
        {
            try
            {
                if (sendEmail == null)
                {
                    responseobj.message = "sendEmail json should not be null";
                    return BadRequest(responseobj);
                }
                else
                {
                    var serviceResult = await _mailServices.send_email(sendEmail);
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
