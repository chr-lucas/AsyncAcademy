using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AsyncAcademy.Models
{
    public class Section
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Course name is required.")]
        public required string CourseName { get; set; }

        // This needs to be an FK
        [Required(ErrorMessage = "Instructor ID is required")]
        public required int InstructorId;
    }
}
