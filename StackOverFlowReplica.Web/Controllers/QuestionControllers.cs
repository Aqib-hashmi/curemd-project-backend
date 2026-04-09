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


        [HttpPost("Create")]
        public IActionResult Create([FromBody] CreateQuestion dto)
        {
            try
            {
                // Get the logged-in user ID
                //var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userId = 20;
                var newQuestion = new Question
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    UserId = userId // optional, assign the user ID
                };

                var id = _service.CreateQuestion(newQuestion);

                return Ok(new { message = "Question created successfully", QuestionId = id });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpGet("GetAllQuestions")]
        public IActionResult GetAllQuestions()
        {
            var result = _service.GetAllQuestions();

            if (!result.Any())
                return NotFound(new { message = "No questions found" });

            return Ok(result);
        }


        [HttpGet("GetQuestionById/{QuestionId}")]
        public IActionResult GetQuestionById(int QuestionId)
        {
            try
            {
                var questions = _service.GetQuestionById(QuestionId);
                if (questions == null)
                    return NotFound(new { message = "No questions found" });

                var response = new
                {
                    questions.QuestionId,
                    questions.Title,
                    questions.Description,
                    questions.UserId,
                    questions.Views,
                    questions.CreatedDate,
                    questions.UpdatedDate
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}
