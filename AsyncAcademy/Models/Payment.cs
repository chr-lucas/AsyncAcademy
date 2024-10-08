using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsyncAcademy.Models
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; } // Foreign key to link to User

        [Required]
        public decimal AmountPaid { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Timestamp { get; set; }
    }
}
