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
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel authDto)
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
        public async Task<IActionResult> RegisterAsync([FromBody] CustomerDTO customerDto)
        {
            if (customerDto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }
            var response = await _authService.RegisterAsync(customerDto);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> SendVerificationCodeAsync([FromBody] SendMailModel sendMail)
        {
            if (string.IsNullOrEmpty(sendMail.Username) || string.IsNullOrEmpty(sendMail.Email))
            {
                return BadRequest("Tên đăng nhập hoặc email không hợp lệ.");
            }

            try
            {
                var code = await _authService.SendCodeEmailAsync(sendMail.Username, sendMail.Email);
                return Ok(new { VerificationCode = code });
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
        public async Task<IActionResult> UpdatePasswordAsync([FromBody] UpdatePasswordModel updatePassword)
        {
            if (updatePassword == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }


            bool result = await _authService.UpdatePasswordAsync(updatePassword.CustomerID, updatePassword.OldPassword, updatePassword.NewPassword);
            if (result)
            {
                return Ok("Mật khẩu đã được cập nhật thành công.");
            }
            return BadRequest("Cập nhật mật khẩu thất bại.");

        }

        [HttpPost]
        public async Task<IActionResult> RefreshTokenAsync(string refreshToken)
        {
            return Ok(await _authService.RefreshTokenAsync(refreshToken));
        }
    }
}
