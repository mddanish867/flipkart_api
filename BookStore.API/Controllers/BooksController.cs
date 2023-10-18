using Microsoft.AspNetCore.Mvc;
using BookStore.API.Common;
using static BookStore.API.Common.Response;
using BookStore.API.Services.Interface;
using BookStore.API.Common.Settlements;

namespace BookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        Response responseobj = new Response();
        ResponseName responsename = new ResponseName();
        Dictionary<string, object> dict = new Dictionary<string, object>();

        private readonly IBookServices _bookServices;
        public BooksController(IBookServices bookServices)
        {
            _bookServices = bookServices;
        }

        ///<summary
        ///This method is used to get peanut specification exception list details
        ///</summary>
        [HttpGet]
        [Route("[action]/companies/{comp_id}/crop-years/{crop_year}/exceptions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(CustomError))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RetrievePeanutSpecificationExceptionDetails(string comp_id, int crop_year, string? buy_pt_id = "", string? pnut_variety_id = "", string? pnut_type_id = "", string? seed_ind = "", string? seg_type = "", string? edible_oil_ind = "")
        {
            try
            {
                if (string.IsNullOrEmpty(comp_id) || crop_year == 0)
                {
                    responseobj.message = "com_id and crop year values should not be null";
                    return BadRequest(responseobj);
                }
                List<Peanut_Specification> peanutspecexceptionlist = await _bookServices.peanut_spect_exception_list(comp_id, crop_year, buy_pt_id, pnut_variety_id, pnut_type_id, seed_ind, seg_type, edible_oil_ind);
                //List<Peanut_SpecException> peanutspecexceptionlist = await _bookServices.peanut_spect_exception_list(comp_id, crop_year, buy_pt_id, pnut_variety_id, pnut_type_id, seed_ind, seg_type, edible_oil_ind);
                if (peanutspecexceptionlist.Count == 0)
                {
                    responseobj.message = "Panut specification exception details not found";
                    return NotFound(responseobj);
                }
                else
                {
                    var peanutspecexceptionobj = peanutspecexceptionlist[0];
                    if (!string.IsNullOrEmpty(peanutspecexceptionobj.errormessage))
                    {
                        responseobj.message = peanutspecexceptionobj.errormessage;
                        return StatusCode(StatusCodes.Status500InternalServerError, responseobj);
                    }
                    else
                    {
                        dict.Add(responsename.result, peanutspecexceptionlist);
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
