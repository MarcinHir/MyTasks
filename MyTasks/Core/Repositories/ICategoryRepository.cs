using MyTasks.Core.Models.Domains;

namespace MyTasks.Core.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetCategories(string userId);
        Category GetCategory(int id, string userId);
        void Update(Category category);
        void Add(Category category);
        void Delete(int id, string userId);
        bool IsUsed(int id);
        void AddDefaultCategory(string userId);
    }
}
