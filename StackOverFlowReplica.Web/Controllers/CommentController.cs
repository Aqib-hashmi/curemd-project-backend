using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackOverFlowReplica.BLL.Services;
using StackOverFlowReplica.Models.payloadModel;
using System.Security.Claims;

namespace StackOverFlowReplica.StackOverFlowReplica.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly CommentService _service;

        public CommentController(CommentService service)
        {
            _service = service;
        }

        [HttpPost("question")]
        [Authorize]
        public IActionResult AddQuestionComment([FromBody] AddQuestionCommentDto dto)
        {
            try
            {
                //var userId = 27;
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var (success, message, data) = _service.AddQuestionComment(dto, userId);
                if (!success) return BadRequest(new { message });
                return Ok(new { message, data });
            }
            catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
        }

        [HttpPut("question/{commentId}")]
        [Authorize]
        public IActionResult EditQuestionComment(int commentId, [FromBody] EditCommentDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var RoleId = int.Parse(User.FindFirst(ClaimTypes.Role)?.Value ?? "0");
                var (success, message) = _service.EditQuestionComment(commentId, dto, userId, RoleId);
                if (!success) return StatusCode(403, new { message });
                return Ok(new { message });
            }
            catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
        }
        [HttpDelete("question/{commentId}")]
        [Authorize]
        public IActionResult DeleteQuestionComment(int commentId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var RoleId = int.Parse(User.FindFirst(ClaimTypes.Role)?.Value ?? "0");
                var (success, message) = _service.DeleteQuestionComment(commentId, userId, RoleId);
                if (!success) return StatusCode(403, new { message });
                return Ok(new { message });
            }
            catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
        }

        [HttpPost("answer")]
        [Authorize]
        public IActionResult AddAnswerComment([FromBody] AddAnswerCommentDto dto)
        {
            try
            {
                //var userId = 26;
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var (success, message, data) = _service.AddAnswerComment(dto, userId);
                if (!success) return BadRequest(new { message });
                return Ok(new { message, data });
            }
            catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
        }
        [HttpPut("answer/{commentId}")]
        [Authorize]
        public IActionResult EditAnswerComment(int commentId, [FromBody] EditCommentDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var RoleId = int.Parse(User.FindFirst(ClaimTypes.Role)?.Value ?? "0");
                var (success, message) = _service.EditAnswerComment(commentId, dto, userId, RoleId);
                if (!success) return StatusCode(403, new { message });
                return Ok(new { message });
            }
            catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
        }

        [HttpDelete("answer/{commentId}")]
        [Authorize]
        public IActionResult DeleteAnswerComment(int commentId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var RoleId = int.Parse(User.FindFirst(ClaimTypes.Role)?.Value ?? "0");
                var (success, message) = _service.DeleteAnswerComment(commentId, userId, RoleId);
                if (!success) return StatusCode(403, new { message });
                return Ok(new { message });
            }
            catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
        }

    }
}
