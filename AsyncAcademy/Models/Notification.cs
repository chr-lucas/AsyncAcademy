using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AsyncAcademy.Models
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public int AssignmentId { get; set; } // Foreign key to link to Assignment

        [Required]
        public int UserId { get; set; } // Foreign key to link to User
    }
}
