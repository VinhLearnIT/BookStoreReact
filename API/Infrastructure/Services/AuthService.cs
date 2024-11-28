using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using AutoMapper;
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
using ApplicationCore.Model;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;  

        public AuthService(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;  
        }

        public async Task<TokenResponseModel> LoginAsync(LoginModel loginModel)
        {
            try
            {
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Username == loginModel.Username);

                if (customer == null || !VerifyPassword(loginModel.Password, customer.Password))
                {
                    throw new UnauthorizedException("Tên đăng nhập hoặc mật khẩu không đúng.");
                }
                var accessToken = GenerateAccessToken(customer);
                var refreshToken = GenerateRefreshToken(customer);

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
            catch (UnauthorizedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình đăng nhập" + ex.Message, ex);
            }
        }

        public async Task<CustomerDTO> RegisterAsync(CustomerDTO customerDto)
        {
            try
            {
                var existingCustomer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Username == customerDto.Username ||
                                              c.Email == customerDto.Email ||
                                              c.Phone == customerDto.Phone ||
                                              c.CCCD == customerDto.CCCD);

                if (existingCustomer != null)
                {
                    if (existingCustomer.Username == customerDto.Username)
                        throw new BadRequestException("Tên đăng nhập đã tồn tại.");
                    if (existingCustomer.Email == customerDto.Email)
                        throw new BadRequestException("Email đã được sử dụng.");
                    if (existingCustomer.Phone == customerDto.Phone)
                        throw new BadRequestException("Số điện thoại đã được sử dụng.");
                    if (existingCustomer.CCCD == customerDto.CCCD)
                        throw new BadRequestException("CCCD đã được đăng ký.");
                }

                var customer = _mapper.Map<Customer>(customerDto);
                customer.Username = customerDto.Username;
                customer.Password = HashPassword(customerDto.Password);

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                return _mapper.Map<CustomerDTO>(customer);
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

        public async Task<object> SendCodeEmailAsync(SendMailModel sendMailModel)
        {
            try
            {
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Email == sendMailModel.Email ||
                                              c.CustomerID == sendMailModel.CustomerID);

                if (customer == null)
                {
                    throw new NotFoundException("Không tìm thấy tài khoản với thông tin đã cung cấp.");
                }

                var code = GenerateVerificationCode();
                await SendEmailAsync(customer.Email, "BookStore - Mã xác nhận của bạn", $"Mã xác nhận của bạn là: {code}");

                return new { verificationCode = code, customerID = customer.CustomerID};
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình gửi mã xác nhận qua email " + ex.Message, ex);
            }
        }

        public async Task<object> ForgotPasswordAsync(UpdatePasswordModel updatePasswordModel)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(updatePasswordModel.CustomerID)
                    ?? throw new NotFoundException("Không tìm thấy khách hàng");

                customer.Password = HashPassword(updatePasswordModel.NewPassword);
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

        public async Task<object> UpdatePasswordAsync(UpdatePasswordModel updatePasswordModel)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(updatePasswordModel.CustomerID)
                    ?? throw new NotFoundException("Không tìm thấy khách hàng");

                if (!VerifyPassword(updatePasswordModel.OldPassword, customer.Password))
                {
                    throw new UnauthorizedException("Mật khẩu cũ không chính xác.");
                }

                customer.Password = HashPassword(updatePasswordModel.NewPassword);
                await _context.SaveChangesAsync();

                return new { message = "Mật khẩu đã được thay đổi thành công!" };
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (UnauthorizedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình cập nhật mật khẩu " + ex.Message, ex);
            }
        }

        public async Task<TokenResponseModel> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                if (string.IsNullOrEmpty(refreshToken))
                {
                    throw new BadRequestException("Refresh Token không hợp lệ.");
                }
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.RefreshToken == refreshToken);

                if (customer == null || customer.RefreshTokenExpiry < DateTime.UtcNow)
                {
                    throw new UnauthorizedException("Refresh Token không hợp lệ hoặc đã hết hạn.");
                }

                var newAccessToken = GenerateAccessToken(customer);

                return new TokenResponseModel
                {
                    CustomerID = customer.CustomerID,
                    AccessToken = newAccessToken,
                    RefreshToken = refreshToken,
                    Role = customer.Role
                };
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (UnauthorizedException)
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

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("huuvinhhoctap0903@gmail.com", "jzoeqnlotjmjxpdo"),
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

        private string GenerateAccessToken(Customer customer)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, customer.CustomerID.ToString()),
                new Claim(ClaimTypes.Role, customer.Role),
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
