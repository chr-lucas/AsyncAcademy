using AsyncAcademy.Utils;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace AsyncAcademy.Models
{
    public class Assignment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int CourseId { get; set; }

        [StringLength(120)]
        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        [Display(Name = "Maximum Available Points")]
        public required int MaxPoints { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Due Date")]
        public required DateTime Due { get; set; }

        [Required]
        [Display(Name = "Submission Type")]
        public required string Type { get; set; }
        
    }
}
