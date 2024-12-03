using ApplicationCore.DTOs;
using ApplicationCore.Model.Auth;

namespace ApplicationCore.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponseModel> LoginAsync(LoginModel loginModel);
        Task<object> RegisterAsync(RegisterModel registerModel);
        Task<object> SendEmailAsync(SendMailModel sendMailModel);
        Task<object> UpdatePasswordAsync(UpdatePasswordModel updatePasswordModel);
        Task<object> ForgotPasswordAsync(ForgotPasswordModel forgotPasswordModel);
        Task<TokenResponseModel> RefreshTokenAsync(RefreshTokenModel refreshTokenModel);
    }
}
