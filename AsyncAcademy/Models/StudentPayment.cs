using System.ComponentModel.DataAnnotations;

namespace AsyncAcademy.Models
{
    public class StudentPayment
    {
        [Key]
        public int StudentPaymentId { get; set; }  

        [Required]
        public int UserId { get; set; }     

        [Required]
        public decimal TotalOwed { get; set; }  

        [Required]
        public decimal Outstanding { get; set; }  

        public decimal TotalPaid { get; set; }  

        public DateTime LastUpdated { get; set; } 
    }
}
