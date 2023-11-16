using AGranelAPI.Models;

namespace AGranelAPI.Repository.IRepository
{
    public interface IProductRepository : IRepository<Producto>
    {
        Task<Producto> Update(Producto entidad);
    }
}
