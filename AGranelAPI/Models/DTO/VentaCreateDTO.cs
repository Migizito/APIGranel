namespace AGranelAPI.Models.DTO
{
    public class VentaCreateDTO
    {
        public DateTime FechaDeVenta { get; set; }
        public List<DetalleVentaDTO> DetallesVenta { get; set; }
        public int VendedorId { get; set; }
    }
}
