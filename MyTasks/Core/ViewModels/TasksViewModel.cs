using MyTasks.Core.Models;
using MyTasks.Core.Models.Domains;

namespace MyTasks.Core.ViewModels

{
    public class TasksViewModel
    {
        public IEnumerable<MyTasks.Core.Models.Domains.Task> Tasks { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public FilterTasks FilterTasks { get; set; } 
    }
}
