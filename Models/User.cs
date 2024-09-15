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
    
    [DataType(DataType.Date)]
    public DateTime Birthday { get; set; }

}