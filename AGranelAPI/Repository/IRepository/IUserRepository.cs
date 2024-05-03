using AGranelAPI.Models;

namespace AGranelAPI.Repository.IRepository
{
    public interface IUserRepository : IRepository<Usuario>
    {
        Task<Usuario> Update(Usuario entidad);
        Task<IEnumerable<Usuario>> ObtenerUsuarios();
    }
}
