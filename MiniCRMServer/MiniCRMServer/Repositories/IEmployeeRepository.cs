using MiniCRMServer.Data;

namespace MiniCRMServer.Repositories
{
    public interface IEmployeeRepository
    {
        public Task<string?> GetUsernameAsync(string firstName, string lastName, string patrnymic);
        public Task DeleteEmployeeAsync(string username);
        public Task UpdateEmployeeAsync(ApplicationUser employee);
        public Task AddEmployeeAsync(ApplicationUser employee);
    }
}
