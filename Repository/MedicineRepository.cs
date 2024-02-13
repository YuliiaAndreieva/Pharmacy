using Microsoft.EntityFrameworkCore;
using WepPha2.Data;
using WepPha2.Interfaces;
using WepPha2.Models;

namespace WepPha2.Repository
{
    public class MedicineRepository : IMedicineRepository
    {
        private readonly ApplicationDbContext _context;

        public MedicineRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Medicine medicine)
        {
            _context.Add(medicine);
            return Save();
        }
        public bool Delete(Medicine medicine)
        {
            _context.Remove(medicine);
            return Save();
        }
        public async Task<IEnumerable<Medicine>> GetAll()
        {
            return await _context.Medicines.ToListAsync();
        }
        public async Task<Medicine> GetMedicineById(int id)
        {
            return await _context.Medicines.FirstOrDefaultAsync(i => i.MedicineId == id);
        }
        public async Task<Medicine> GetMedicineByIdAsNoTracking(int id)
        {
            return await _context.Medicines.AsNoTracking().FirstOrDefaultAsync(i => i.MedicineId == id);
        }
        public async Task<IEnumerable<Medicine>> GetMedicineBySupplier(int SupplierId)
        {
            return await _context.Medicines.Where(c => c.Supplier.SupplierId.Equals(SupplierId)).ToListAsync();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Medicine medicine)
        {
            _context.Update(medicine);
            return Save();
        }

        public async Task<IEnumerable<Supplier>> GetAllSuppliers()
        {
            var suppliers = await _context.Suppliers.ToListAsync();
            return suppliers;
        }
        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return categories;
        }
        public async Task<IEnumerable<Medicine>> GetMedicineByCategoryAsync(Supplier category)
        {
            return await _context.Medicines.Where(c => c.Category == category).ToListAsync();
        }
        public async Task<IEnumerable<Medicine>> GetMedicineByCategory(int? categoryId)
        {
            return await _context.Medicines.Where(predicate => predicate.CategoryId == categoryId).ToListAsync();
        }
        public async Task<IEnumerable<Medicine>> SearchMedicineByName(string searchName)
        {
            return await _context.Medicines
             .Where(predicate => predicate.MedicineName.ToUpper().Contains(searchName.ToUpper()))
             .ToListAsync();
        }
        public async Task<IEnumerable<dynamic>> GetSupplierMedicinesCount()
        {
            var supplierMedicinesCount = await _context.Medicines
                .GroupBy(medicine => medicine.SupplierId)
                .Select(group => new
                {
                    SupplierId = group.Key,
                    MedicineCount = group.Count(),
                    SupplierName = _context.Suppliers.FirstOrDefault(s => s.SupplierId == group.Key) != null ? _context.Suppliers.FirstOrDefault(s => s.SupplierId == group.Key).CompanyName : null
                }).ToListAsync();

            return supplierMedicinesCount.Cast<dynamic>();
        }
        public async Task<List<(string Name, double BasePrice, double TotalPrice)>> GetMedicineDetails(Dictionary<int, int> medicines)
        {
            List<(string Name, double BasePrice, double TotalPrice)> medicineDetailsList = new List<(string, double, double)>();

            foreach (var item in medicines)
            {
                var medicine = await GetMedicineById(item.Key);

                if (medicine != null)
                {
                    double basePrice = medicine.UnitPrice;
                    double totalPrice = basePrice * item.Value;

                    medicineDetailsList.Add((medicine.MedicineName, basePrice, totalPrice));
                }
            }
            return medicineDetailsList;
        }
    }
}
