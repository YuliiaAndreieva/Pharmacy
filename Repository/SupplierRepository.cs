using Microsoft.EntityFrameworkCore;
using WepPha2.Data;
using WepPha2.Interfaces;
using WepPha2.Models;

namespace WepPha2.Repository
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly ApplicationDbContext _context;

        public SupplierRepository(ApplicationDbContext context) 
        {
            _context = context;
        }
        public bool Add(Supplier supplier)
        {
            _context.Add(supplier);
            return Save();
        }
        public bool Delete(Supplier supplier)
        {
            _context.Remove(supplier);
            return Save();
        }
        public async Task<IEnumerable<Supplier>> GetAll()
        {
            return await _context.Suppliers.ToListAsync();
        }

        public async Task<Supplier> GetSupplierById(int id)
        {
            var sup=  await _context.Suppliers.FirstOrDefaultAsync(i => i.SupplierId == id);
            return sup;
        }
        public async Task<Supplier> GetSupplierByIdAsNoTracking(int id)
        {
            return await _context.Suppliers.AsNoTracking().FirstOrDefaultAsync(i => i.SupplierId == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Supplier supplier)
        {
            _context.Update(supplier);
            return Save();
        }


       
    }
}
