using System.ComponentModel.DataAnnotations.Schema;

namespace AGranelAPI.Models.DTO
{
    public class VentaDTO
    {
        public int SaleId { get; set; }
        public string FechaDeVenta { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public int VendedorId { get; set; }
        public List<DetalleVentaDTO> DetalleVentas { get; set; } = new List<DetalleVentaDTO>();
    }
}
