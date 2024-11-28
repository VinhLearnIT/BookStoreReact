using ApplicationCore.DTOs;
using ApplicationCore.Model;

namespace ApplicationCore.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponseModel> LoginAsync(LoginModel loginModel);
        Task<CustomerDTO> RegisterAsync(CustomerDTO customerDto);
        Task<object> SendCodeEmailAsync(SendMailModel sendMailModel);
        Task<object> UpdatePasswordAsync(UpdatePasswordModel updatePasswordModel);
        Task<object> ForgotPasswordAsync(UpdatePasswordModel updatePasswordModel);
        Task<TokenResponseModel> RefreshTokenAsync(string refreshToken);
    }
}
