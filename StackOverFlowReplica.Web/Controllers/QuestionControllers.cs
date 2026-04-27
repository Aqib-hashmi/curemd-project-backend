using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackOverFlowReplica.BLL.Services;
using StackOverFlowReplica.Models.payloadModel;
using StackOverFlowReplica.StackOverFlowReplica.Models;
using System.Security.Claims;

namespace StackOverFlowReplica.StackOverFlowReplica.Web.Controllers
{
        [ApiController]
        [Route("api/[controller]")]
    public class QuestionControllers : ControllerBase
    {

        private readonly QuestionService _service;

        public QuestionControllers(QuestionService service)
        {
            _service = service;
        }


        [HttpPost("CreateQuestion")]
        [Authorize]
        public IActionResult CreateQuestion([FromBody] CreateQuestion dto)
        {
            try
            {
                if (dto.TagIds == null || !dto.TagIds.Any())
                    return BadRequest(new { message = "At least one tag is required" });

                 var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                //var userId = 20;

                var questionId = _service.CreateQuestion(dto, userId);

                return Ok(new { message = "Question created", questionId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("recommended")]
        [HttpGet("GetAllQuestions")]
        [AllowAnonymous]
        public IActionResult GetRecommendedQuestions([FromQuery] int pageNumber,[FromQuery] int pageSize, [FromQuery] string? tagName = null)
        {

            int? userId = null;

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                userId = int.Parse(userIdClaim.Value);
            }

            var result = _service.GetAllQuestions(userId, pageNumber, pageSize,tagName);
            if (!result.Any())
                return NotFound(new { message = "No questions found" });

            return Ok(result);
        }


        [HttpGet("QuestionDetail/{questionId}")]
        public IActionResult GetQuestionDetail(int questionId)
        {
            var result = _service.GetQuestionDetail(questionId);

            if (result == null)
                return NotFound(new { message = "Question not found" });

            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateQuestion(int id, [FromBody] UpdateQuestionDto dto)
        {
            try
            {
                int currentUserId = int.Parse( User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                string currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value ?? "";
                var (success, message) = _service.UpdateQuestion(id, dto, currentUserId, currentUserRole);

                if (!success)
                {
                    if (message == "Unauthorized")
                        return StatusCode(403, new { message });

                    return BadRequest(new { message });
                }

                return Ok(new { message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Server error", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteQuestion(int id)
        {
            try
            {
                int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                string currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value ?? "";
                var (success, message) = _service.deleteQuestion(id, currentUserId, currentUserRole);

                if (!success)
                {
                    if (message == "Unauthorized")
                        return StatusCode(403, new { message });

                    return BadRequest(new { message });
                }

                return Ok(new { message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Server error", error = ex.Message });
            }
        }

        [HttpPost("view")]
        [Authorize]
        public IActionResult AddQuestionView([FromBody] AddViewDto dto)
        {
            try
            {
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                var (success, status) = _service.AddQuestionView(dto.QuestionId, userId);

                return Ok(new { status });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        //[HttpGet("search")]
        //public IActionResult SearchQuestions([FromQuery] int tagId)
        //{
        //    var result = _service.SearchQuestionsByTag(tagId);
        //    return Ok(result);
        //}







        //[HttpGet("GetQuestionById/{QuestionId}")]
        //public IActionResult GetQuestionById(int QuestionId)
        //{
        //    try
        //    {
        //        var questions = _service.GetQuestionById(QuestionId);
        //        if (questions == null)
        //            return NotFound(new { message = "No questions found" });

        //        var response = new
        //        {
        //            questions.QuestionId,
        //            questions.Title,
        //            questions.Description,
        //            questions.UserId,
        //            questions.Views,
        //            questions.CreatedDate,
        //            questions.UpdatedDate
        //        };

        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { message = ex.Message });
        //    }
        //}

    }
}
