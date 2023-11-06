using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MiniCRMServer.Data;

namespace MiniCRMServer.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public EmployeeRepository(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<string?> GetUsernameAsync(string firstName, string lastName, string patronymic)
        {
            var query = _db.Users
                .Where(user => user.FirstName == firstName && user.LastName == lastName && user.Patronymic == patronymic).Select(user => user.UserName);
            var username = await query.FirstOrDefaultAsync();
            if (username is null)
                return null;
            else
                return username;
        }
        public async Task DeleteEmployeeAsync(string username)
        {
            var employee = await _userManager.FindByNameAsync(username);
            if (employee is null)
                throw new InvalidOperationException("User is not found");
            var tasks = await _db.Tasks
                .Include(task => task.User.Id)
                .Where(task => task.UserId == employee.Id)
                .ToListAsync();
            if (!tasks.Any())
                return;
            _db.RemoveRange(tasks);
            await _userManager.DeleteAsync(employee);
        }
        public async Task UpdateEmployeeAsync(ApplicationUser employee)
        {
            await _userManager.UpdateAsync(employee);
        }
        public async Task AddEmployeeAsync(ApplicationUser employee)
        {
            await _userManager.CreateAsync(employee);
        }
    }
}
