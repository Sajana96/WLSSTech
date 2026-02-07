using API.Auth;
using API.Utility;
using Azure.Core;
using Common.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;

        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration config, ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _config = config;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = new ApplicationUser
                {
                    UserName = registerRequest.Name,
                    Email = registerRequest.Email
                };

                var result = await _userManager.CreateAsync(user, registerRequest.Password);

                if (!result.Succeeded)
                    return BadRequest(result.Errors.Select(e => e.Description));

                return Ok(new ApiResponse{ Message = "User registered successfully" , HttpStatusCode=(int)HttpStatusCode.OK, IsSuccess=true});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error Occured Registering User");
                return  StatusCode(500, new ApiResponse() { IsSuccess = false, Message = ex.Message, HttpStatusCode = (int)HttpStatusCode.InternalServerError});
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginUser ([FromBody] LoginRequest loginRequest)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = await _userManager.FindByEmailAsync(loginRequest.Email);
                if (user == null)
                    return Unauthorized(new ApiResponse { Message = "Invalid Credentials", IsSuccess = false, HttpStatusCode = (int)HttpStatusCode.Unauthorized });

                var valid = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
                if (!valid)
                    return Unauthorized(new ApiResponse { Message = "Invalid Credentials", IsSuccess = false, HttpStatusCode = (int)HttpStatusCode.Unauthorized });

                //Fetch user roles
                var roles = await _userManager.GetRolesAsync(user);

                var token = await JWTTokenUtil.GenerateJWTToken(user,_config,roles.ToList());
                return Ok(new { token });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Occured Login in User");
                return StatusCode(500, new ApiResponse() { IsSuccess = false, Message = ex.Message, HttpStatusCode = (int)HttpStatusCode.InternalServerError });
            }
        }
    }
}
