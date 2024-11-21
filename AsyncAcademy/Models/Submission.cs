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

        // Not required. Fresh submissions have null for this attribute
        // A value here means the assignment has been graded
        public int? PointsGraded { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Timestamp { get; set; }

        // Bit for informing notifications at homepage
        // Set to TRUE when a submission is graded
        // Set to FALSE when shown in the notification bar
        public bool? IsNew { get; set; } = false;

        // Add more properties as needed, e.g., UserId, SubmissionDate, etc.
    }
}
