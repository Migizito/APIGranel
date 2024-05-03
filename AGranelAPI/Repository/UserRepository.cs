using AGranelAPI.DataContext;
using AGranelAPI.Models;
using AGranelAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
namespace AGranelAPI.Repository
{
    public class UserRepository : Repository<Usuario>, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Usuario> Update(Usuario entidad)
        {
            _context.Usuarios.Update(entidad);
            await _context.SaveChangesAsync();
            return entidad;
        }
        public async Task<IEnumerable<Usuario>> ObtenerUsuarios()
        {
            return await _context.Usuarios.Include(r => r.Roles).Include(r => r.Roles).ToListAsync();
        }
    }
}
