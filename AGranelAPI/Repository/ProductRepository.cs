using AGranelAPI.DataContext;
using AGranelAPI.Models;


namespace AGranelAPI.Repository.IRepository
{
    public class ProductRepository : Repository<Producto>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Producto> Update(Producto entidad)
        {
            _context.Productos.Update(entidad);
            await _context.SaveChangesAsync();
            return entidad;
        }
    }
}
