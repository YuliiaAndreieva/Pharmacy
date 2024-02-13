using WepPha2.Models;

namespace WepPha2.Interfaces
{
    public interface ISupplierRepository
    {
        Task<IEnumerable<Supplier>> GetAll();
        Task<Supplier> GetSupplierById(int id);
        Task<Supplier> GetSupplierByIdAsNoTracking(int id);
        bool Add(Supplier supplier);
        bool Delete(Supplier supplier);
        bool Update(Supplier supplier);
        bool Save();
    }
}
