using WepPha2.Models;

namespace WepPha2.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAll();
        Task<Employee> GetEmployeeById(int id);
        Task<Employee> GetEmployeeByIdAsNoTracking(int id);
        Task<Employee>GetEmployeeByEmail(string email); 
        Task<IEnumerable<Pharmacy>> GetAllPharmacy();
        Task<bool> UpdateUserEmployee(AppUser appUser, string email);
        bool Add(Employee employee);
        bool Delete(Employee employee);
        bool Update(Employee employee);
        bool Save();
    }
}
