using AsyncAcademy.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsyncAcademy.Models;

public class Course
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [StringLength(4)]
    [Required]
    [Display(Name = "Course Number")]
    public required string CourseNumber { get; set; }

    [StringLength(10)]
    [Required]
    [Display(Name = "Department ID")]
    public required string Department { get; set; }

    [StringLength(60, MinimumLength = 1)]
    [Required]
    [Display(Name = "Class Title")]
    public required string Name { get; set; }

    [StringLength(500, MinimumLength = 1)]
    [Required]
    [Display(Name = "Class Description")]
    public required string Description { get; set; }

    [Required]
    [Display(Name = "Credit Hours")]
    public required int CreditHours { get; set; }

    [Required(ErrorMessage = "Instructor ID is required")]
    [Display(Name = "Instructor Id")]
    public required int InstructorId { get; set; }

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
    [DataType(DataType.Date)]
    [Display(Name = "Start Date")]
    public required DateTime StartDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "End Date")]
    public required DateTime EndDate { get; set; }
}

