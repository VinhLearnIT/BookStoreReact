using ApplicationCore.DTOs;
using ApplicationCore.Model;

namespace ApplicationCore.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponseModel> LoginAsync(LoginModel loginModel);
        Task<CustomerDTO> RegisterAsync(CustomerDTO customerDto);
        Task<string> SendCodeEmailAsync(string username, string email);
        Task<bool> UpdatePasswordAsync(int customerID, string oldPassword, string newPassword);
        Task<TokenResponseModel> RefreshTokenAsync(string refreshToken);
    }
}
