using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AGranelAPI.Models
{
    public class Venta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SaleID { get; set; }
        public DateTime FechaDeVenta { get; set; }
        public decimal Monto { get; set; }
        public int VendedorID { get; set; }
        [ForeignKey("VendedorID")]
        public User Vendedor { get; set; } = new User();
        public List<DetalleVenta> DetallesVenta { get; set; } = new List<DetalleVenta>();
    }
}
