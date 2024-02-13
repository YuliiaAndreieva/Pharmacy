using Microsoft.AspNetCore.Mvc;
using WepPha2.Models;

namespace WepPha2.Interfaces
{
    public interface IPurchaseDetailsRepository
    {
        Task<IEnumerable<PurchaseDetails>> GetAll();
        Task<IEnumerable<PurchaseDetails>> GetMedicinesByPurchaseId(int id);
        Task<IEnumerable<Medicine>> GetAllMedicines();
        bool Add(PurchaseDetails purchase);
        bool Save();
    }
}
