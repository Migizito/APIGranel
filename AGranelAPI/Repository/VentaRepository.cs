using AGranelAPI.DataContext;
using AGranelAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AGranelAPI.Repository.IRepository
{
    public class VentaRepository : Repository<Venta>, IVentaRepository
    {
        private readonly ApplicationDbContext _context;
        public VentaRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Venta?> ObtenerVentaPorId(int ventaId)
        {   
            var venta = await _context.Ventas.Include(v => v.Vendedor).Include(v => v.DetallesVenta).FirstOrDefaultAsync(v => v.SaleID == ventaId);
            if(venta != null) 
            {
                return venta;
            }
            return null;
        }
        public async Task<IEnumerable<Venta>> ObtenerVentas()
        {
            return await _context.Ventas.Include(v => v.Vendedor).Include(v => v.DetallesVenta).ToListAsync();
        }



        public async Task<Venta> Update(Venta entidad)
        {
            _context.Ventas.Update(entidad);
            await _context.SaveChangesAsync();
            return entidad;
        }
    }
}
