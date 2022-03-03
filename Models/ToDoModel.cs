using System.ComponentModel.DataAnnotations;

namespace ToDo_API.Models
{
    public class ToDoModel
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
    }
}