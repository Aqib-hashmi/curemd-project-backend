using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackOverFlowReplica.BLL.Services;
using StackOverFlowReplica.Models.payloadModel;
using System.Security.Claims;

namespace StackOverFlowReplica.StackOverFlowReplica.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : ControllerBase
    {
        private readonly TagService _service;

        public TagController(TagService service)
        {
            _service = service;
        }

        [HttpPost("Create")]
       // [AllowAnonymous] // 👈 important
        public IActionResult Create([FromBody] CreateTag dto)
        {

            try
            {
                var tag = _service.CreateTag(dto);

                return Ok(tag);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Tag/GetAllTags
        [HttpGet("GetAllTags")]
        public IActionResult GetAllTags()
        {
            try
            {
                var result = _service.GetAllTags();

                if (result == null || !result.Any())
                {
                    return NotFound(new { message = "No tags found" });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Something went wrong",
                    error = ex.Message
                });
            }
        }

        [HttpGet("suggest")]
        public IActionResult SearchTagSuggestions([FromQuery] string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return Ok(new List<TagSuggestionDto>());

            var result = _service.SearchTagSuggestions(search);
            return Ok(result);
        }
    }
}