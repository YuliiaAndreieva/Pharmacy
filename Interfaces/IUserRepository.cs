using WepPha2.Models;

namespace WepPha2.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetAllUsers();
        Task<IEnumerable<Employee>> GetAllEmployee();
        Task<Employee> GetEmployeeById(int id);
        Task<AppUser> GetUserById(string id);
        bool Add(AppUser user);
        bool Update(AppUser user);
        bool Delete(AppUser user);
        bool Save();
    }
}
