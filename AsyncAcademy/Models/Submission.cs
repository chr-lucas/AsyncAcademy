using System.ComponentModel.DataAnnotations;

namespace AsyncAcademy.Models
{
    public class Submission
    {
        public int Id { get; set; }
        
        [Required]
        public string Content { get; set; } = string.Empty;

        public int AssignmentId { get; set; } // Foreign key to link to Assignment

                // Add more properties as needed, e.g., UserId, SubmissionDate, etc.
    }
}
