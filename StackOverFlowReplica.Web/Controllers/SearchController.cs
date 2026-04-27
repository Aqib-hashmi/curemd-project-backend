using Microsoft.AspNetCore.Mvc;
using StackOverFlowReplica.BLL.Services;
using StackOverFlowReplica.Models.payloadModel;
using System.Security.Claims;

namespace StackOverFlowReplica.StackOverFlowReplica.Web.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly SearchService _service;

        public SearchController(SearchService service)
        {
            _service = service;
        }

        [HttpGet("suggestions")]
        public IActionResult GetSuggestions([FromQuery] string searchText)
        {
            try
            {
                // Logged in user ki history bhi consider karo
                //int? userId = null;
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                var result = _service.GetTagSuggestions(searchText, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // Recent search history
        [HttpGet("history")]
        public IActionResult GetSearchHistory([FromQuery] int userId)
       {
            try
            {
                if (userId == 0)
                    return BadRequest(new { message = "UserId required" });

                var result = _service.GetUserSearchHistory(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // Save search history
        [HttpPost("history")]
        public IActionResult SaveSearchHistory([FromBody] SaveSearchHistoryDto dto, [FromQuery] int userId)
        {
            try
            {
                if (userId == 0)
                    return BadRequest(new { message = "UserId required" });

                _service.SaveSearchHistory(userId, dto.TagId);
                return Ok(new { message = "Saved" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
