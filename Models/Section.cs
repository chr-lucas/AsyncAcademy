using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AsyncAcademy.Models
{
    public class Section
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Course Id")]
        public int CourseId { get; set; }

        // This needs to be an FK
        [Required(ErrorMessage = "Instructor ID is required")]
        public required int InstructorId;

        public static implicit operator string?(Section? v)
        {
            throw new NotImplementedException();
        }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        [Display(Name = "Course Name")]
        public required string CourseName{ get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }

        [StringLength(60, MinimumLength = 1)]
        [Required]
        [Display(Name = "Location")]
        public required string Location { get; set; }

        [Required]
        [Display(Name = "Student Capacity")]
        public required int StudentCapacity { get; set; }

        [Required]
        [Display(Name = "Students Enrolled")]
        public required int StudentsEnrolled { get; set; }

        [Required]
        [Display(Name = "Meeting Time")]
        [StringLength(60, MinimumLength = 1)]
        public required string MeetingTimeInfo { get; set; }
    }
}
