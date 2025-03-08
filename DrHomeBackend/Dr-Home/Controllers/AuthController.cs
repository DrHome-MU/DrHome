using Dr_Home.Data.Models;
using Dr_Home.DTOs.AuthDTOs;
using Dr_Home.DTOs.EmailSender;
using Dr_Home.Email_Sender;
using Dr_Home.Helpers.Interfaces;
using Dr_Home.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;

namespace Dr_Home.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthHelper _auth) : ControllerBase
    {
        //Register Endpoint
        [HttpPost("register")]
        public async Task<IActionResult> register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
            { return BadRequest(ModelState); }

            if (dto.ConfirmPassword != dto.Password)
            { return BadRequest("Make sure that the confirmed password is equal to the password"); }

            var response = await _auth.RegisterPatient(dto);

            if (!response.Success) return BadRequest(response);

            return Ok(
                new
                {
                    Success = response.Success,
                    message = response.Message,
                    userId = response.Data.Id,
                    role = response.Data.role,
                    email = response.Data.Email

                }
            );
        }

        //Login Endpoint
        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody] LogInDto dto)
        {
            var response = await _auth.LogIn(dto);

            return (!response.Success) ? Unauthorized(response) : Ok(new
            {
                Success = response.Success,
                message = response.Message,
                token = response.token , 
                role = response.Data.role,
                userId = response.Data.Id,
                email = response.Data.Email
            });
        }
        
        //Verify Endpoint
        [HttpGet("verify")]
        [Authorize]
        public async Task<IActionResult> VerifyAccount([FromQuery] string token)
        {
            bool IsVerified = await _auth.VerifyAccount(token);
            return (IsVerified) ? Ok("The Account Is Verified Successfully") : Unauthorized();
        }
        
        //Get All Users Endpoint
        [HttpGet("")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _auth.GetUsers();
            if (users == null) return BadRequest("There Is No Users");
            return Ok(users);
        }
        
        //Get User By Id Endpoint
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>GetUserById(Guid id)
        {
            User user = await _auth.GetUser(id);
            return (user == null) ? BadRequest("There Is No User With This Id"):Ok(user);
        }

        //Get User Profile
        [HttpGet("Profile")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetUserProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return Unauthorized();

            var response = await _auth.GetUserProfile(Guid.Parse(userId));

            return (!response.Success) ? BadRequest(response) : Ok(response);
        }

        //Update User Details 
        [HttpPut("UpdateProfile")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> UpdateProfile(UserProfileDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return Unauthorized();

            var response = await _auth.UpdateProfile(Guid.Parse(userId), dto);


            return (!response.Success) ? BadRequest(response) : Ok(response);
        }

        //Change The User`s Password 
        [HttpPut("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null) return Unauthorized();

            var response = await _auth.ChangePassword(Guid.Parse(userId), dto);

            return ( !response.Success ) ? BadRequest(response) : Ok(response) ;
        }
        
        //Delete User By Id Endpoint
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _auth.DeleteUser(id);
            return (user == null) ? BadRequest(new
            {
                Success = false,
                message = "There Is no user with this Id"
            }) : Ok(new
            {
                Success = true,
                Message = "User Deleted Successfully"
            });
        }
        
        
        [HttpGet("forgetpassword")]
        public async Task<IActionResult> ForgotPassword(forgotPasswordDto dto)
        {
            var response = await _auth.ForgetPassword(dto);
            return Ok("a7a");
        }
    }
}
