using ApplicationCore.DTOs;
using ApplicationCore.Model.Customer;

namespace ApplicationCore.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDTO>> GetAllCustomersAsync();
        Task<CustomerDTO> GetCustomerByIdAsync(int id);
        Task<CustomerDTO> CreateCustomerAsync(CustomerDTO customerDto);
        Task<CustomerDTO> UpdateCustomerAsync(int id, CustomerDTO customerDto);
        Task<CustomerDTO> UpdateCustomerRoleAsync(int id, CustomerRoleModel customerRole);
        //Task<object> DeleteCustomerAsync(int id);
        Task<object> ToggleCustomerStatusAsync(int id, CustomerStatusModel customerStatus);
    }
}
