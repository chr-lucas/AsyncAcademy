using AsyncAcademy.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsyncAcademy.Models;

public class Course
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CourseId { get; set; }

    [StringLength(60, MinimumLength = 3)]
    [Required]
    [Display(Name = "Course Name")]
    public required string CourseName { get; set; }

    [StringLength(60, MinimumLength = 1)]
    [Required]
    [Display(Name = "Class Name")]
    public required string ClassName { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "End Date")]
    public DateTime EndDate { get; set; }

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

}

