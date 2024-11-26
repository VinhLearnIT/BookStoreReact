using ApplicationCore.DTOs;
using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
        Task<CategoryDTO> GetCategoryByIdAsync(int id);
        Task<CategoryDTO> CreateCategoryAsync(CategoryDTO categoryDto);
        Task<CategoryDTO> UpdateCategoryAsync(int id, CategoryDTO categoryDto);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
