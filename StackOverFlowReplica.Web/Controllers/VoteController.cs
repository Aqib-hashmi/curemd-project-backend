using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using StackOverFlowReplica.BLL.Services;
using StackOverFlowReplica.Context;
using StackOverFlowReplica.Models.payloadModel;
using System.Security.Claims;

namespace StackOverFlowReplica.StackOverFlowReplica.Web.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class VoteController : ControllerBase
    {
        private readonly VoteService _service;

        public VoteController(VoteService service)
        {
            _service = service;
        }


        [HttpPost("questionVote")]
        [Authorize]
        public IActionResult VoteQuestion([FromBody] VoteQuestionDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                var (success, message) = _service.VoteQuestion(userId, dto);

                if (!success)
                    return BadRequest(new { message });

                return Ok(new { message });
            }
            catch (SqlException ex) when (ex.Message.Contains("owner"))
            {
                return StatusCode(403, new { message = "Question owner vote nahi kar sakta" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("answer")]
        [Authorize]
        public IActionResult VoteAnswer([FromBody] VoteAnswerDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                var (success, message) = _service.VoteAnswer(userId, dto);

                if (!success)
                    return BadRequest(new { message });

                return Ok(new { message });
            }
            catch (SqlException ex) when (ex.Message.Contains("owner"))
            {
                return StatusCode(403, new { message = "Answer owner vote nahi kar sakta" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
