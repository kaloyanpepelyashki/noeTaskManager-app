using System.ComponentModel.DataAnnotations;

namespace noeTaskManager_app.Models
{
    public class CreateTask
    {
        [Required]
        public string Summary { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Priority { get; set; }
        [Required]
        public string DueDate { get; set; }
    }
}
