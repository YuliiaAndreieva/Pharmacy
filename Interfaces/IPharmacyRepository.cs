using WepPha2.Models;

namespace WepPha2.Interfaces
{
    public interface IPharmacyRepository
    {
        Task<IEnumerable<Pharmacy>> GetAll();
        Task<Pharmacy> GetPharmacyById(int id);
        Task<Pharmacy> GetPharmacyByIdAsNoTracking(int id);
        bool Add(Pharmacy pharmacy);
        bool Delete(Pharmacy pharmacy);
        bool Update(Pharmacy pharmacy);
        bool Save();
    }
}
