
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApiRestTesting.Models.Entities
{
    [Table("ORDERS")]
    public class Order
    {
        [Key]
        [Column("ORDER_ID", TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; } //PK
        [Column("ORDER_TMS", TypeName = "datetime")]
        public DateTime? OrderTimestamp { get; set; }
        [Column("CUSTOMER_ID", TypeName = "int")]
        [Required]
        public int CustomerId { get; set; } //FK
        [Column("ORDER_STATUS", TypeName = "varchar(50)")]
        [Required(ErrorMessage = "Order status is required")]
        [RegularExpression(@"^(Pending|Shipped|Delivered|Cancelled)$", 
            ErrorMessage = "Order status must be one of the following: Pending, Shipped, Delivered, Cancelled")]
        public string OrderStatus { get; set; }
        [Column("STORE_ID", TypeName = "int")]
        public int? StoreId { get; set; } //FK

        //Relacion: un pedido pertenece a un cliente
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        //Relacion:un pedido puede teenr muchos items
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}