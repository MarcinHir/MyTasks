using MyTasks.Core.Models.Domains;

namespace MyTasks.Core.ViewModels
{
    public class TaskViewModel
    { 
        public string Heading { get; set; }
        public MyTasks.Core.Models.Domains.Task Task { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
