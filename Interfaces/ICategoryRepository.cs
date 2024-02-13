using WepPha2.Models;

namespace WepPha2.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAll();
        Task<Category> GetCategoryById(int id);
        Task<Category> GetCategoryByIdAsNoTracking(int id);
        bool Add(Category category);
        bool Delete(Category category);
        bool Update(Category category);
        bool Save();

    }
}
