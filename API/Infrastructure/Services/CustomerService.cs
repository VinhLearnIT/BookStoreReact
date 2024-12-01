using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

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
                var customer = await _context.Customers.FindAsync(id)
                    ?? throw new NotFoundException("Không tìm thấy khách hàng");
                return _mapper.Map<CustomerDTO>(customer);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy thông tin khách hàng" + ex.Message, ex);
            }
        }

        public async Task<CustomerDTO> CreateCustomerAsync(CustomerDTO customerDto)
        {
            try
            {
                var existingCustomer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Email == customerDto.Email ||
                                              c.Phone == customerDto.Phone ||
                                              c.CCCD == customerDto.CCCD);

                if (existingCustomer != null)
                {
                    if (existingCustomer.Email == customerDto.Email)
                        throw new BadRequestException("Email đã được sử dụng.");
                    if (existingCustomer.Phone == customerDto.Phone)
                        throw new BadRequestException("Số điện thoại đã được sử dụng.");
                    if (existingCustomer.CCCD == customerDto.CCCD)
                        throw new BadRequestException("CCCD đã được đăng ký.");
                }
                var customer = _mapper.Map<Customer>(customerDto);
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                customerDto.CustomerID = customer.CustomerID;
                return _mapper.Map<CustomerDTO>(customer);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi tạo khách hàng mới" + ex.Message, ex);
            }
        }

        public async Task<CustomerDTO> UpdateCustomerAsync(int id, CustomerDTO customerDto)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(id)
                    ?? throw new NotFoundException("Không tìm thấy khách hàng");

                var existingCustomer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Email == customerDto.Email ||
                                              c.Phone == customerDto.Phone ||
                                              c.CCCD == customerDto.CCCD);

                if (existingCustomer != null)
                {
                    if (existingCustomer.Email == customerDto.Email)
                        throw new BadRequestException("Email đã được sử dụng.");
                    if (existingCustomer.Phone == customerDto.Phone)
                        throw new BadRequestException("Số điện thoại đã được sử dụng.");
                    if (existingCustomer.CCCD == customerDto.CCCD)
                        throw new BadRequestException("CCCD đã được đăng ký.");
                }

                customer.FullName = customerDto.FullName;
                customer.Email = customerDto.Email;
                customer.Phone = customerDto.Phone;
                customer.CCCD = customerDto.CCCD;
                customer.Address = customerDto.Address;

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

        public async Task<object> DeleteCustomerAsync(int id)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(id)
                    ?? throw new NotFoundException("Không tìm thấy khách hàng");

                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
                return new { message = "Xóa khách hàng thành công!" };

            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi xóa khách hàng" + ex.Message, ex);
            }
        }
    }
}