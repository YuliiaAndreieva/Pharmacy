using Microsoft.EntityFrameworkCore;
using WepPha2.Data;
using WepPha2.Interfaces;
using WepPha2.Models;

namespace WepPha2.Repository
{
    public class PharmacyRepository : IPharmacyRepository
    {
        private readonly ApplicationDbContext _context;
        public PharmacyRepository(ApplicationDbContext context) 
        { 
            _context = context;
        }

        public bool Add(Pharmacy pharmacy)
        {
            _context.Add(pharmacy);
            return Save();
        }

        public bool Delete(Pharmacy pharmacy)
        {
            _context.Remove(pharmacy);
            return Save();
        }

        public async Task<IEnumerable<Pharmacy>> GetAll()
        {
            return await _context.Pharmacies.ToListAsync();
        }

        public async Task<Pharmacy> GetPharmacyById(int id)
        {
            return await _context.Pharmacies.FirstOrDefaultAsync(i => i.PharmacyId == id);
        }

        public async Task<Pharmacy> GetPharmacyByIdAsNoTracking(int id)
        {
            return await _context.Pharmacies.AsNoTracking().FirstOrDefaultAsync(i => i.PharmacyId == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Pharmacy pharmacy)
        {
            _context.Update(pharmacy);
            return Save();
        }
    }
}
