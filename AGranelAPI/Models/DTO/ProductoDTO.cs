namespace AGranelAPI.Models.DTO
{
    public class ProductoDTO
    {
        public int ProductID { get; set; }
        public string NombreProducto { get; set; }
        public decimal Precio { get; set; }
        public string Descripcion { get; set; }
        public int CantidadEnInventario { get; set; }
    }
}
