using Microsoft.EntityFrameworkCore;
using WepPha2.Data;
using WepPha2.Interfaces;
using WepPha2.Models;

namespace WepPha2.Repository
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly ApplicationDbContext _context;

        public PurchaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Purchase purchase)
        {
            _context.Add(purchase);
            return Save();
        }

        public bool Delete(Purchase purchase)
        {
            _context.Remove(purchase);
            return Save();
        }

        public async Task<IEnumerable<Purchase>> GetAll()
        {
            return await _context.Purchases.Include(pd => pd.Employee).ToListAsync();
        }

        public async Task<Purchase> GetPurchaseById(int id)
        {
            return await _context.Purchases.FirstOrDefaultAsync(i => i.PurchaseId == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public async Task<Medicine> GetMedicineByIdForPurchase(int id)
        {
            return await _context.Medicines.FirstOrDefaultAsync(i => i.MedicineId == id);
        }
    }
}
