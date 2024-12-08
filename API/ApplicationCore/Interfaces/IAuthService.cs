using ApplicationCore.Model.Auth;

namespace ApplicationCore.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponseModel> LoginAsync(LoginModel loginModel);
        Task<object> RegisterAsync(RegisterModel registerModel);
        Task<object> SendVerificationCodeAsync(SendMailModel sendMailModel);
        Task<object> ForgotPasswordAsync(ForgotPasswordModel forgotPasswordModel);
        Task<TokenResponseModel> RefreshTokenAsync(RefreshTokenModel refreshTokenModel);
        Task<object> SendMailContactAsync(SendMailModel sendMailModel);
    }
}
