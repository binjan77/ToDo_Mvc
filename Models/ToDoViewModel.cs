using System.ComponentModel.DataAnnotations;

namespace ToDo_Mvc_App.Models
{
    public class ToDoViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
    }
}