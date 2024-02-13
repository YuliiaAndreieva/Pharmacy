using WepPha2.Models;

namespace WepPha2.Interfaces
{
    public interface IPurchaseRepository
    {
        Task<IEnumerable<Purchase>> GetAll();
        Task<Purchase> GetPurchaseById(int id);
        Task<Medicine> GetMedicineByIdForPurchase(int id);
        bool Add(Purchase purchase);
        bool Delete(Purchase purchase);
        bool Save();
    }
}
