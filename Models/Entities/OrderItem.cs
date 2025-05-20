using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApiRestTesting.Models.Entities
{

    [Table("ORDERS_ITEMS")]
    public class OrderItem
    {
        [Column("ORDER_ID", TypeName = "int")]
        [Required(ErrorMessage = "Order ID is required")]
        public int OrderId { get; set; } //FK
        [Column("LINE_ITEM_ID", TypeName = "int")]
        [Required(ErrorMessage = "Line item ID is required")]
        public int LineItemId { get; set; } //PK
        [Column("PRODUCT_ID", TypeName = "int")]
        [Required(ErrorMessage = "Product ID is required")]
        public int ProductId { get; set; } //FK
        [Column("UNIT_PRICE", TypeName = "int")]
        [Required(ErrorMessage = "Unit price is required")]
        public int UnitPrice { get; set; }
        [Column("QUANTITY", TypeName = "int")]
        [Required(ErrorMessage = "Quantity is required")]
        [Range(1000, int.MaxValue, ErrorMessage = "Quantity must be greater than 999")]
        public int Quantity { get; set; }
        [Column("SHIPMENT_ID", TypeName = "int")]
        public int? ShipmentId { get; set; } //FK

        //Relacion: Un item pertenece a un pedido
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
    }

}