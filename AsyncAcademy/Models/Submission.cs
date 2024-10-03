using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsyncAcademy.Models
{
    public class Submission
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

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Timestamp { get; set; }

        // Add more properties as needed, e.g., UserId, SubmissionDate, etc.
    }
}