using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackOverFlowReplica.BLL.Services;
using StackOverFlowReplica.Models.payloadModel;
using StackOverFlowReplica.StackOverFlowReplica.Models;
using System.Security.Claims;

namespace StackOverFlowReplica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _service;

        public AuthController(AuthService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            try 
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var userId = _service.Register(user);

                if (userId == -1)
                {
                    return BadRequest("Email already exists");
                }
                return Ok(new
                {
                    message = "User registered successfully",
                    userId = userId
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message   // 👈 frontend ko ye milega
                });
            }
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string? errorMessage;
            var userData = _service.Login(request.Email, request.Password, out errorMessage);

            if (userData == null)
                return BadRequest(new { message = errorMessage });

            var token = _service.GenerateToken(userData);

            return Ok(new
            {
                message = "User logged in successfully",
                token = token,
                user = new
                {
                    userData.UserId,
                    userData.Name,
                    userData.Email,
                    userData.RoleId,
                    userData.Bio,
                    userData.isActive
                }
            });
        }

        [HttpGet("AllUsers")]
        public IActionResult GetAllUsers()
        {
            var users = _service.GetAllUsers();

            if (users.Count == 0)
                return NotFound("No users found");

            // Return list without passwords
            var result = users.Select(u => new
            {
                userId = u.UserId,
                name = u.Name,
                email = u.Email,
                roleId = u.RoleId,
                bio = u.Bio,
                isActive = u.isActive,
                createdDate = u.CreatedDate,
                updatedDate = u.UpdatedDate
            });

            return Ok(result);
        }
        [HttpGet("Profile/{userId}")]
        public IActionResult GetUserById(int userId)
        {
            var user = _service.GetUserById(userId);

            if (user == null)
                return NotFound("User not found");

            // Hide password in response
            return Ok(new
            {
                user.UserId,
                user.Name,
                user.Email,
                user.RoleId,
                user.Bio,
                user.isActive,
                user.CreatedDate,
                user.UpdatedDate
            });
        }

     
        [HttpPut("Update")]
        public IActionResult UpdateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = _service.UpdateUser(user);
            if (!updated)
                return NotFound("User not found or update failed.");

            return Ok(new
            {
                message = "User updated successfully",
                userId = user.UserId,
                 user.Name,
                user.Email,
                user.RoleId,
                user.Bio,
                user.isActive,
                user.isActiveBy
            });
        }


        // Controllers/AuthController.cs
        [HttpPut("change-status")]
        public IActionResult ChangeUserStatus([FromBody] ChangeUserStatusRequest request)
        {
            try
            {
                _service.ChangeUserStatus(request.AdminId, request.TargetUserId, request.IsActive);
                return Ok(new { Message = "User status updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}