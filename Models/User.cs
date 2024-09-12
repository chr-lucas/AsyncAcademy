using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AsyncAcademy.Utils;

namespace FirstLastApp.Models;

public class User
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [StringLength(60, MinimumLength = 3)]
    [Required(ErrorMessage = "Username is required.")]
    public required string Username { get; set; }

    [StringLength(60, MinimumLength = 1)]
    [Required(ErrorMessage = "First Name is required.")]
    [Display(Name = "First Name")]
    public required string FirstName { get; set; }

    [StringLength(60, MinimumLength = 1)]
    [Required(ErrorMessage = "Last Name is required.")]
    [Display(Name = "Last Name")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Email address is required.")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email")]
    public required string Mail { get; set; }
    
    [StringLength(60, MinimumLength = 8)]
    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public required string Pass { get; set; }

    [Compare(nameof(Pass), ErrorMessage = "Passwords do not match.")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    [NotMapped]
    public required string ConfirmPass { get; set; }

    [Required(ErrorMessage = "Birthday is required.")]
    [ValidiateBirthday]
    [DataType(DataType.Date)]
    public DateTime Birthday { get; set; }
}