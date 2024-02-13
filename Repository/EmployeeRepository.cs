using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WepPha2.Data;
using WepPha2.Interfaces;
using WepPha2.Models;

namespace WepPha2.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Employee employee)
        {
            _context.Add(employee);
            return Save();
        }

        public bool Delete(Employee employee)
        {
            _context.Remove(employee);
            return Save();
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            return await _context.Employees.ToListAsync();
        }
        public async Task<IEnumerable<Pharmacy>> GetAllPharmacy()
        {
            return await _context.Pharmacies.ToListAsync();
        }
        public async Task<Employee> GetEmployeeByIdAsNoTracking(int id)
        {
            return await _context.Employees.AsNoTracking().FirstOrDefaultAsync(i => i.EmployeeId == id);
        }

        public async Task<Employee> GetEmployeeByEmail(string email)
        {
            return await _context.Employees.FirstOrDefaultAsync(i => i.Email == email);
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            return await _context.Employees.FirstOrDefaultAsync(i => i.EmployeeId == id);
        }
        
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Employee employee)
        {
            _context.Update(employee);
            return Save();
        }
        //public bool Task<AppUser>UpdateUserEmployee(AppUser appUser, AppUser appUser2)
        public async Task<bool> UpdateUserEmployee(AppUser appUser, string email)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(i => i.Email == email);

            if (employee != null)
            {
                employee.Phone = appUser.PhoneNumber;
                employee.Email = appUser.Email;

                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}
