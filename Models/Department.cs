using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AsyncAcademy.Models
{
    public class Department
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [StringLength(10)]
        public required string DepartmentId { get; set; }

        //[Required]
        //[StringLength(50)]
        //public required string Name { get; set; }
    }
}
