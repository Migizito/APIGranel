using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AGranelAPI.Models
{
    public class DetalleVenta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DetailID { get; set; }
        public int SaleID { get; set; }
        public int ProductID { get; set; }
        public int CantidadVendida { get; set; }
        [ForeignKey("SaleID")]
        public Venta Venta { get; set; } = new Venta();
        [ForeignKey("ProductID")]
        public Producto Producto { get; set; } = new Producto();
    }
}
