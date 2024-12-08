using ApplicationCore.DTOs;
using ApplicationCore.Interfaces;
using ApplicationCore.Model.Auth;
using ApplicationCore.Model.Customer;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel authDto)
        {
            if (authDto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var response = await _authService.LoginAsync(authDto);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            if (registerModel == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }
            var response = await _authService.RegisterAsync(registerModel);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> SendVerificationCode([FromBody] SendMailModel sendMailModel)
        {
            if (sendMailModel == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }
           
                return Ok(await _authService.SendVerificationCodeAsync(sendMailModel));
            
        }
        [HttpPost]
        public async Task<IActionResult> SendMailContact([FromBody] SendMailModel sendMailModel)
        {
            if (sendMailModel == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }
            
                return Ok(await _authService.SendMailContactAsync(sendMailModel));            
        }


        [HttpPut]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel forgotPasswordModel)
        {
            if (forgotPasswordModel == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            return Ok(await _authService.ForgotPasswordAsync(forgotPasswordModel));
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken(RefreshTokenModel refreshTokenModel)
        {
            if (refreshTokenModel == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }
            return Ok(await _authService.RefreshTokenAsync(refreshTokenModel));
        }
    }
}
