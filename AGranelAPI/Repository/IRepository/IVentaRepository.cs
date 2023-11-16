using AGranelAPI.Models;
using System.Data.Entity.Infrastructure;

namespace AGranelAPI.Repository.IRepository
{
    public interface IVentaRepository : IRepository<Venta>
    {
        Task<Venta> Update(Venta entidad);
        Task<Venta?> ObtenerVentaPorId(int ventaId);
        Task<IEnumerable<Venta>> ObtenerVentas();
    }
}
