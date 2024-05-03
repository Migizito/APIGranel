using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AGranelAPI.Models
{
    public class DetalleVenta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DetalleID { get; set; }
        public int VentaID { get; set; }
        public int ProductoID { get; set; }
        public int CantidadVendida { get; set; }
        [ForeignKey("VentaID")]
        public Venta Venta { get; set; } = new Venta();
        [ForeignKey("ProductoID")]
        public Producto Producto { get; set; } = new Producto();
    }
}
