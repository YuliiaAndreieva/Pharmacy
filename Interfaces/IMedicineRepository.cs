using Microsoft.EntityFrameworkCore.Query;
using WepPha2.Models;

namespace WepPha2.Interfaces
{
    public interface IMedicineRepository
    {
        Task<IEnumerable<Medicine>> GetAll();
        Task<Medicine> GetMedicineById(int id);
        Task<Medicine> GetMedicineByIdAsNoTracking(int id);

        Task<Medicine> GetSupplierDataByMed(
            Medicine medicine);
        Task<IEnumerable<Medicine>> GetMedicineBySupplier(int SupplierId);
        Task<IEnumerable<Medicine>> GetMedicineByCategory(int? CategoryId);
        //Task<Dictionary<string, double>> GetMedicineNameAndCount(int id);
        //Task<Dictionary<string, double>> GetMedicineNameAndCount(int id);
        Task<List<(string Name, double BasePrice, double TotalPrice)>> GetMedicineDetails(Dictionary<int, int> medicines);
        Task<IEnumerable<Medicine>> SearchMedicineByName(string searchName);
        Task<IEnumerable<dynamic>> GetSupplierMedicinesCount();
        Task<IEnumerable<Supplier>> GetAllSuppliers();
        //List<Supplier> GetAllSuppliers();
        Task<IEnumerable<Category>> GetAllCategories();
        bool Add(Medicine medicine);
        bool Delete(Medicine medicine);
        bool Update(Medicine medicine);
        bool Save();


    }
}
