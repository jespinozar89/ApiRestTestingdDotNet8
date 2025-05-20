using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApiRestTesting.Models.Entities
{
    [Table("CUSTOMERS")]
    public class Customer
    {
        [Key]
        [Column("CUSTOMER_ID", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }//PK
        [Column("EMAIL_ADDRESS", TypeName = "varchar(100)")]
        [Required(ErrorMessage = "Email address is required")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", 
            ErrorMessage = "Please enter a valid email address (e.g., user@example.com)")]
        public string EmailAdress { get; set; }
        [Column("FULL_NAME", TypeName = "varchar(255)")]
        [Required(ErrorMessage = "Full name is required")]
        public string FullName { get; set; }

        //Relacion: un Clinete puede tener muchos Pedidos
        public ICollection<Order> Orders { get; set; }

    }
}