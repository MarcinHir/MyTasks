using Microsoft.EntityFrameworkCore;
using MyTasks.Core;
using MyTasks.Core.Models.Domains;
using MyTasks.Core.Repositories;
using System.Threading.Tasks;

namespace MyTasks.Persistance.Repositories
{

    public class CategoryRepository : ICategoryRepository
    {
        private IApplicationDbContext _context;
        public CategoryRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Category category)
        {
            _context.Categories.Add(category);
        }

        public void Delete(int id, string userId)
        {
            
                var categoryToDelete = _context.Categories.Single(x => x.Id == id && x.UserId == userId);
                _context.Categories.Remove(categoryToDelete);
            
        }
        public bool IsUsed(int id)
        {
            if (_context.Tasks.Any(x => x.CategoryId == id))
            {
                return true;
            }           
            else
                return false;
        }

        public IEnumerable<Category> GetCategories(string userId)
        {
            var categories = _context.Categories.Where(x => x.UserId == userId).OrderBy(x => x.Id).ToList();
            return categories;
        }
        public Category GetCategory(int id, string userId)
        {
            var category = _context.Categories.Single(x => x.Id == id && x.UserId == userId);
            return category;
        }

        public void Update(Category category)
        {
            var categoryToUpdate = _context.Categories.Single(x => x.Id == category.Id && x.UserId == category.UserId);
            categoryToUpdate.Name = category.Name;
        }

        public void AddDefaultCategory(string userId)
        {
            Category categoryToAdd = new Category { Name = "Ogólne", UserId = userId };
            _context.Categories.Add(categoryToAdd);
        }
    }
}
