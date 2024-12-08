using ApplicationCore.DTOs;
using ApplicationCore.Model.Customer;

namespace ApplicationCore.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDTO>> GetAllCustomersAsync();
        Task<CustomerDTO> GetCustomerByIdAsync(int id);
        Task<object> UpdatePasswordAsync(UpdatePasswordModel updatePasswordModel);
        Task<CustomerDTO> UpdateCustomerAsync(int id, UpdateCustomerModel updateCustomer);
        Task<CustomerDTO> UpdateCustomerRoleAsync(int id, CustomerRoleModel customerRole);
        Task<object> UpdateCustomerStatusAsync(int id, CustomerStatusModel customerStatus);
    }
}
