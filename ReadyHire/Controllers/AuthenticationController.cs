using ReadyHire.Models.Authentication;
using ReadyHire.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ReadyHire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }



        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model) 
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);  
            }
            var result=await _authService.RegisterAsync(model);
            if(!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult>LoginAsync([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.LoginAsync(model);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> AddrolesAsync([FromBody] AddRoleModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.AddrolesAsync(model);
            if (!string.IsNullOrEmpty(result))
            {
                return BadRequest(result);
            }
            return Ok(model);
        }


        [HttpPost("SendToken_ToMail")]
        public async Task<IActionResult> SendToken([FromBody] tokenrequest tokenrequest)
        {
            if (string.IsNullOrEmpty(tokenrequest.email))
                return BadRequest("Email is required");

            var result = await _authService.SendTokenToEmailAsync(tokenrequest.email);

            if (result == "Email is not registered")
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] resetpassword model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.ResetPasswordAsync(model);

            if (result.StartsWith("Error"))
                return BadRequest(result);

            return Ok(result);
        }



    }
}
