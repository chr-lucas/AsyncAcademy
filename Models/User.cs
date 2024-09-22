using AsyncAcademy.Utils;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

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
    [DataType(DataType.EmailAddress)]
    [Display(Name = "E-Mail")]
    public required string Mail { get; set; }
    
    [StringLength(255, MinimumLength = 8)]
    [Required]
    [Display(Name = "Password")]
    public required string Pass { get; set; }

    // Hashing the Pass field doesn't occur until post, so this comparison works on the raw pass.
    [Compare(nameof(Pass), ErrorMessage = "Passwords do not match.")] 
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    [NotMapped] // Used in model verification but not added as a database attribute
    public required string ConfirmPass { get; set; }

    [Required]
    [ValidiateBirthday] // custom validation
    [DataType(DataType.Date)]
    public DateTime Birthday { get; set; }

    // This is only a stopgap solution until a proper Professor model is implemented
    [Required]
    [Display(Name = "IsProfessor")]
    public required bool IsProfessor { get; set; }

    // Nullable path to profile picture
    [StringLength(255)]
    public string ProfilePath { get; set; } = "/images/default_pfp.png";

    [StringLength(255)]
    [Required]
    [Display(Name = "Street Address")]
    public required string Addr_Street { get; set; }


    [StringLength(255)]
    [Required]
    [Display(Name = "City")]
    public required string Addr_City { get; set; }

    [StringLength(2)]
    [Required]
    [Display(Name = "State")]
    public required string Addr_State { get; set; }

    [Required]
    [Display(Name = "Zip Code")]
    public required int Addr_Zip { get; set; }

    [Required]
    [Display(Name = "Phone")]
    public required int Phone { get; set; }


}