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
    [Display(Name = "Course Number")]
    public required int CourseNumber { get; set; }

    [StringLength(10)]
    [Required]
    [Display(Name = "Department ID")]
    public required string DepartmentId { get; set; }

    [StringLength(60, MinimumLength = 1)]
    [Required]
    [Display(Name = "Class Title")]
    public required string CourseTitle { get; set; }

    [StringLength(500, MinimumLength = 1)]
    [Required]
    [Display(Name = "Class Description")]
    public required string Description { get; set; }

    [Required]
    [Display(Name = "Credit Hours")]
    public required int CreditHours { get; set; }


}

