namespace AGranelAPI.Models.DTO
{
    public class ProductoUpdateDTO
    {
        public decimal Precio { get; set; }
        public string Descripcion { get; set; }
        public int CantidadEnInventario { get; set; }
    }
}
