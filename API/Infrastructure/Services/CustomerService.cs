using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Model.Customer;
using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CustomerService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CustomerDTO>> GetAllCustomersAsync()
        {
            try
            {
                var customers = await _context.Customers.ToListAsync();
                return _mapper.Map<IEnumerable<CustomerDTO>>(customers);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy danh sách khách hàng" + ex.Message, ex);
            }
        }

        public async Task<CustomerDTO> GetCustomerByIdAsync(int id)
        {
            try
            {
                var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.CustomerID == id && c.IsDeleted == false) ??
                throw new UnauthorizedException("Tài khoản đã bị vô hiệu hóa. Vui lòng sử dụng tài khoản khác!");
                return _mapper.Map<CustomerDTO>(customer);
            }
            catch (UnauthorizedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy thông tin khách hàng" + ex.Message, ex);
            }
        }

    
        public async Task<object> UpdatePasswordAsync(UpdatePasswordModel updatePasswordModel)
        {
            try
            {
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.CustomerID == updatePasswordModel.CustomerID && c.IsDeleted == false)
                    ?? throw new NotFoundException("Không tìm thấy khách hàng");

                if (!VerifyPassword(updatePasswordModel.OldPassword, customer.Password))
                {
                    throw new BadRequestException("Mật khẩu cũ không chính xác.");
                }

                customer.Password = HashPassword(updatePasswordModel.NewPassword);
                await _context.SaveChangesAsync();

                return new { message = "Mật khẩu đã được thay đổi thành công!" };
            }
            catch (NotFoundException)
            {
                throw;
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


        public async Task<CustomerDTO> UpdateCustomerRoleAsync(int id, CustomerRoleModel customerRole)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(id)
                    ?? throw new NotFoundException("Không tìm thấy khách hàng");
                customer.Role = customerRole.Role;
                await _context.SaveChangesAsync();
                return _mapper.Map<CustomerDTO>(customer);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi cập nhật khách hàng" + ex.Message, ex);
            }
        }

        public async Task<CustomerDTO> UpdateCustomerAsync(int id, UpdateCustomerModel updateCustomer)
        {
            try
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerID == id)
                        ?? throw new NotFoundException("Không tìm thấy khách hàng");

                var existingCustomer = await _context.Customers
                    .FirstOrDefaultAsync(c => (c.Phone == updateCustomer.Phone || c.CCCD == updateCustomer.CCCD) && c.CustomerID != id);

                if (existingCustomer != null)
                {
                    if (existingCustomer.Phone == updateCustomer.Phone)
                        throw new BadRequestException("Số điện thoại đã được sử dụng!");
                    if (existingCustomer.CCCD == updateCustomer.CCCD)
                        throw new BadRequestException("CCCD đã được sử dụng!");
                }

                customer.FullName = updateCustomer.FullName;
                customer.Phone = updateCustomer.Phone;
                customer.CCCD = updateCustomer.CCCD;
                customer.Address = updateCustomer.Address;
                customer.FullInfo = true;

                await _context.SaveChangesAsync();
                return _mapper.Map<CustomerDTO>(customer);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi cập nhật khách hàng" + ex.Message, ex);
            }
        }
        public async Task<object> UpdateCustomerStatusAsync(int id, CustomerStatusModel customerStatus)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(id)
                    ?? throw new NotFoundException("Không tìm thấy khách hàng");


                if (customer.IsDeleted != customerStatus.IsDeleted)
                {
                    customer.IsDeleted = customerStatus.IsDeleted;
                    await _context.SaveChangesAsync();

                    var message = (bool)customerStatus.IsDeleted
                        ? "Vô hiệu hóa người dùng thành công!"
                        : "Kích hoạt người dùng thành công!";

                    return new { message };
                }
                else
                {
                    return new { message = "Trạng thái người dùng không thay đổi." };
                }
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi khi thay đổi trạng thái người dùng! " + ex.Message, ex);
            }
        }

        
    }
}