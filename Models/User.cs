using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsyncAcademy.Models;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [StringLength(60, MinimumLength = 3)]
    [Required]
    public required string Username { get; set; }

    [StringLength(60, MinimumLength = 1)]
    [Required]
    [Display(Name = "First Name")]
    public required string FirstName { get; set; }

    [StringLength(60, MinimumLength = 1)]
    [Required]
    [Display(Name = "Last Name")]
    public required string LastName { get; set; }

    [Required]
    [Display(Name = "E-Mail")]
    public required string Mail { get; set; }
    
    [StringLength(255, MinimumLength = 8)]
    [Required]
    [Display(Name = "Password")]
    public required string Pass { get; set; }

    // Hashing the Pass field doesn't occur until post, so this comparison.
    [Compare(nameof(Pass), ErrorMessage = "Passwords do not match.")] 
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    [NotMapped] // Used in model verification but not added as a database attribute
    public required string ConfirmPass { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime Birthday { get; set; }

    // This is only a stopgap solution until a proper Professor model is implemented
    [Required]
    [Display(Name = "IsProfessor")]
    public required bool IsProfessor { get; set; }

}