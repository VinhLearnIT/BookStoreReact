using ApplicationCore.DTOs;
using ApplicationCore.Interfaces;
using ApplicationCore.Model;
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

        // Đăng nhập
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

        // Đăng ký
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] CustomerDTO customerDto)
        {
            if (customerDto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }
            var response = await _authService.RegisterAsync(customerDto);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> SendVerificationCode([FromBody] SendMailModel sendMail)
        {
            try
            {
                return Ok(await _authService.SendCodeEmailAsync(sendMail));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Cập nhật mật khẩu
        [HttpPut]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordModel updatePassword)
        {
            if (updatePassword == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            return Ok(await _authService.UpdatePasswordAsync(updatePassword));
        }

        [HttpPut]
        public async Task<IActionResult> ForgotPassword([FromBody] UpdatePasswordModel updatePassword)
        {
            if (updatePassword == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            return Ok(await _authService.ForgotPasswordAsync(updatePassword));
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            return Ok(await _authService.RefreshTokenAsync(refreshToken));
        }
    }
}
