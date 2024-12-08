using ApplicationCore.DTOs;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoryService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _context.Categories.ToListAsync();
                return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi lấy danh sách thể loại" + ex.Message, ex);
            }
        }

        public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO categoryDto)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryDto);
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                categoryDto.CategoryID = category.CategoryID;

                return _mapper.Map<CategoryDTO>(category);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi tạo thể loại", ex);
            }
        }

        public async Task<CategoryDTO> UpdateCategoryAsync(int id, CategoryDTO categoryDto)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id)
                    ?? throw new NotFoundException("Không tìm thấy thể loại");

                category.CategoryName = categoryDto.CategoryName;

                await _context.SaveChangesAsync();

                return _mapper.Map<CategoryDTO>(category);
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi cập nhật thể loại" + ex.Message, ex);
            }
        }

        public async Task<object> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id)
                    ?? throw new NotFoundException("Không tìm thấy thể loại");

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return new { message = "Xóa thể loại thành công!" };
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra khi xóa thể loại" + ex.Message, ex);
            }
        }
    }
}
