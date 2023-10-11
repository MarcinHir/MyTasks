using Microsoft.EntityFrameworkCore;
using MyTasks.Core;
using MyTasks.Core.Models.Domains;
using MyTasks.Core.Services;
using System.Threading.Tasks;

namespace MyTasks.Persistance.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<Category> GetCategories(String userId)
        {
            return _unitOfWork.Category.GetCategories(userId);
        }
        public Category GetCategory(int id, string userId)
        {
            return _unitOfWork.Category.GetCategory(id, userId);
        }
        public void Update(Category category)
        {
            _unitOfWork.Category.Update(category);
            _unitOfWork.Complete();
        }
        public void Add(Category category)
        {
            _unitOfWork.Category.Add(category);
            _unitOfWork.Complete();
        }
        public void Delete(int id, string userId)
        {
            _unitOfWork.Category.Delete(id, userId);
            _unitOfWork.Complete();
        }
        public bool IsUsed(int id)
        {
            return _unitOfWork.Category.IsUsed(id);           
        }
        public void AddDefaultCategory(string userId)
        {
            _unitOfWork.Category.AddDefaultCategory(userId);
            _unitOfWork.Complete();
        }
    }
}
