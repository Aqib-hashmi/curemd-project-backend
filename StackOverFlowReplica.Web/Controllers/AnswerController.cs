using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using StackOverFlowReplica.BLL.Services;
using StackOverFlowReplica.Models.payloadModel;
using System.Security.Claims;

namespace StackOverFlowReplica.StackOverFlowReplica.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnswerController :ControllerBase
    {
        private readonly AnswerService _service;

        public AnswerController(AnswerService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddAnswer([FromBody] AddAnswerDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var (success, message, data) = _service.AddAnswer(dto, userId);
                if (!success) return BadRequest(new { message });
                return Ok(new { message, data });
            }
            catch (SqlException ex) when (ex.Message.Contains("owner"))
            {
                return StatusCode(403, new { message = "Question owner answer nahi de sakta" });
            }
            catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
        }

        [HttpPut("{answerId}")]
        [Authorize]
        public IActionResult EditAnswer(int answerId, [FromBody] EditAnswerDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var RoleId = int.Parse(User.FindFirst(ClaimTypes.Role)?.Value ?? "0");

                var (success, message) = _service.EditAnswer(answerId, dto, userId, RoleId);
                if (!success) return StatusCode(403, new { message });
                return Ok(new { message });
            }
            catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
        }

        [HttpDelete("{answerId}")]
        [Authorize]
        public IActionResult DeleteAnswer(int answerId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var RoleId = int.Parse(User.FindFirst(ClaimTypes.Role)?.Value ?? "0");
                var (success, message) = _service.DeleteAnswer(answerId, userId, RoleId);
                if (!success) return StatusCode(403, new { message });
                return Ok(new { message });
            }
            catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
        }
    }
}
