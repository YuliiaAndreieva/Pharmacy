using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WepPha2.Data;
using WepPha2.Interfaces;
using WepPha2.Models;

namespace WepPha2.Repository
{
    public class PurchaseDetailsRepository : IPurchaseDetailsRepository
    {
        private readonly ApplicationDbContext _context;

        public PurchaseDetailsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<PurchaseDetails>> GetMedicinesByPurchaseId(int id)
        {
               var result = await _context.PurchaseDetails            
                .Where(detail => detail.PurchaseId == id)
                .Include(pd => pd.PurchasedMedicine).ToListAsync();

               return result;
        }

        public bool Add(PurchaseDetails purchaseDetails)
        {
            _context.Add(purchaseDetails);
            return Save();
        }


        public async Task<IEnumerable<PurchaseDetails>> GetAll()
        {
            return await _context.PurchaseDetails.ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public async Task<IEnumerable<Medicine>> GetAllMedicines()
        {
            return await _context.Medicines.ToListAsync();
        }


    }
}
