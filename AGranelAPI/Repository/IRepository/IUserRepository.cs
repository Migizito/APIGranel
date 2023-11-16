using AGranelAPI.Models;

namespace AGranelAPI.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> Update(User entidad);
        Task<IEnumerable<User>> ObtenerUsuarios();
    }
}
