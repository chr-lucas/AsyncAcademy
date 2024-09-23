using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AsyncAcademy.Models
{
    public class Section
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Section Id")]
        public int SectionId { get; set; }

        // This needs to be an FK
        [Required(ErrorMessage = "Instructor ID is required")]
        [Display(Name = "Instructor Id")]
        public required int InstructorId {  get; set; }

        [Required(ErrorMessage = "Course ID is required")]
        [Display(Name = "Course Id")]
        public required int CourseId { get; set; }

        public static implicit operator string?(Section? v)
        {
            throw new NotImplementedException();
        }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Start Time")]
        public required DateTime StartTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "End Time")]
        public required DateTime EndTime { get; set; }

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

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Start Date")]
        public required DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "End Date")]
        public required DateTime EndDate { get; set; }
    }
}
