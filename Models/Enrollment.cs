using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AsyncAcademy.Models
{
    public class Enrollment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // will need to figure out how to make these FKs
        [Required]
        public required int UserId { get; set; }

        [Required]
        public required int SectionId {  get; set; }
    }
}
