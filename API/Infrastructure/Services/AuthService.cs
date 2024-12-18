using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using ApplicationCore.Model.Auth;
using System.Net.WebSockets;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<TokenResponseModel> LoginAsync(LoginModel loginModel)
        {
            try
            {
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Username == loginModel.Username && c.IsDeleted == false);

                if (customer == null || !VerifyPassword(loginModel.Password, customer.Password))
                {
                    throw new BadRequestException("Tên đăng nhập hoặc mật khẩu không đúng.");
                }
                var accessToken = GenerateAccessToken(customer);
                var refreshToken = GenerateRefreshToken(customer);

                Serilog.Log.Information("Đăng nhập thành công: " + customer.FullName);

                customer.RefreshToken = refreshToken;
                customer.RefreshTokenExpiry = DateTime.UtcNow.AddDays(1);

                await _context.SaveChangesAsync();

                return new TokenResponseModel
                {
                    CustomerID = customer.CustomerID,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    Role = customer.Role
                };
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình đăng nhập" + ex.Message, ex);
            }
        }

        public async Task<object> RegisterAsync(RegisterModel registerModel)
        {
            try
            {
                var existingCustomer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Username == registerModel.Username ||
                                              c.Email == registerModel.Email);

                if (existingCustomer != null)
                {
                    if (existingCustomer.Username == registerModel.Username)
                        throw new BadRequestException("Tên đăng nhập đã tồn tại.");
                    if (existingCustomer.Email == registerModel.Email)
                        throw new BadRequestException("Email đã được sử dụng.");
                }

                var customer = new Customer
                {
                    FullName = registerModel.FullName,
                    Email = registerModel.Email,
                    Username = registerModel.Username,
                    Password = HashPassword(registerModel.Password),
                    Role = registerModel.Role,
                    FullInfo = false,
                    IsDeleted = false
                };


                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                return new { message = "Đăng kí tài khoản thành công!"};
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình đăng ký " + ex.Message, ex);
            }
        }

        public async Task<object> ForgotPasswordAsync(ForgotPasswordModel forgotPasswordModel)
        {
            try
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == forgotPasswordModel.Email)
                    ?? throw new NotFoundException("Không tìm thấy khách hàng");

                customer.Password = HashPassword(forgotPasswordModel.NewPassword);
                await _context.SaveChangesAsync();

                return new { message = "Mật khẩu đã được thay đổi thành công!" };
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình cập nhật mật khẩu " + ex.Message, ex);
            }
        }

        public async Task<object> SendVerificationCodeAsync(SendMailModel sendMailModel)
        {
            try
            {
                var code = GenerateVerificationCode();
                var subject = "BookStore - Mã xác nhận của bạn";
                var body = $"<p>Mã xác nhận của bạn là: <strong>{code}</strong><br></p><p>Mã này chỉ tồn tại trong 3 phút.</p>";
                await SendEmailAsync(sendMailModel.Email, subject, body);
                return new { verificationCode = code };
            }
            catch (SmtpException smtpEx)
            {
                throw new Exception("Có lỗi xảy ra khi gửi email: " + smtpEx.Message, smtpEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi gửi email: " + ex.Message, ex);
            }
        }

        public async Task<object> SendMailContactAsync(SendMailModel sendMailModel)
        {
            try
            {                
                var subject = $"BookStore - Liên hệ từ {sendMailModel.FullName}";
                var body = $"<p>Email: <strong>{sendMailModel.Email}</strong><br></p>" +
                    $"<p>Số điện thoại: <strong>{sendMailModel.Phone}</strong><br></p>" +
                    $"<p>Nội dung: <strong>{sendMailModel.Message}</strong><br></p>";
                await SendEmailAsync("huuvinhhoctap0903@gmail.com", subject, body);
                return new { message = "Gửi thông tin liên hệ thành công!" };
            }
            catch (SmtpException smtpEx)
            {
                throw new Exception("Có lỗi xảy ra khi gửi email: " + smtpEx.Message, smtpEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi gửi email: " + ex.Message, ex);
            }
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("huuvinhhoctap0903@gmail.com", "mwtdthgfhurhlasq"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("huuvinhhoctap0903@gmail.com"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(toEmail);

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (SmtpException smtpEx)
            {
                throw new Exception("Có lỗi xảy ra khi gửi email: " + smtpEx.Message, smtpEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi gửi email: " + ex.Message, ex);
            }
        }

        public async Task<TokenResponseModel> RefreshTokenAsync(RefreshTokenModel refreshTokenModel)
        {
            try
            {
                if (string.IsNullOrEmpty(refreshTokenModel.RefreshToken))
                {
                    throw new BadRequestException("Refresh Token không hợp lệ.");
                }
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.RefreshToken == refreshTokenModel.RefreshToken && c.IsDeleted == false);

                if (customer == null || customer.RefreshTokenExpiry < DateTime.UtcNow)
                {
                    throw new BadRequestException("Refresh Token không hợp lệ hoặc đã hết hạn.");
                }

                var newAccessToken = GenerateAccessToken(customer);

                return new TokenResponseModel
                {
                    CustomerID = customer.CustomerID,
                    AccessToken = newAccessToken,
                    RefreshToken = refreshTokenModel.RefreshToken,
                    Role = customer.Role
                };
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình cập nhật mật khẩu " + ex.Message, ex);
            }
        }

        private string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) throw new ArgumentException("Mật khẩu không được để trống.");

            using (SHA256 sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = sha256.ComputeHash(bytes);

                return Convert.ToBase64String(hashBytes);
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            string hashedInputPassword = HashPassword(password);
            return hashedInputPassword == hashedPassword;
        }

        

        private string GenerateAccessToken(Customer customer)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, customer.CustomerID.ToString()),
                new Claim(ClaimTypes.Role, customer.Role),
                new Claim(ClaimTypes.DateOfBirth, DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"))
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken(Customer customer)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, customer.CustomerID.ToString()),
                new Claim(ClaimTypes.Role, customer.Role),
                 new Claim(ClaimTypes.DateOfBirth, DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"))
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateVerificationCode()
        {
            Random random = new Random();
            return random.Next(1000, 10000).ToString("D4");
        }
    }
}
