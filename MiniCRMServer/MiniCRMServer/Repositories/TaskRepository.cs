using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using MiniCRMServer.Data;

namespace MiniCRMServer.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _db;
        public TaskRepository(ApplicationDbContext db) 
        {
            _db = db;
        }
        public async Task<List<TaskModel>?> GetTasksByUsernameAsync(string username)
        {
            var query = _db.Tasks
                .Include(task => task.User)
                .Where(task => task.User.UserName == username);
            List<TaskModel>? tasksByUsername = await query.ToListAsync();
            return tasksByUsername;
        }
        public async Task AddTaskAsync(TaskModel task, string username)
        {
            var user = await _db.Users
                .Where(user => user.UserName == username)
                .FirstOrDefaultAsync() ?? throw new InvalidOperationException("User not found");
            task.UserId = user.Id;
            await _db.Tasks.AddAsync(task);
            await _db.SaveChangesAsync();
        }
        public async Task UpdateTaskAsync(TaskModel task)
        {
            var foundTask = await _db.Tasks
                .FindAsync(task.Id) ?? throw new InvalidOperationException("Task not found");

            var currentValues = _db.Entry(foundTask).CurrentValues.Clone();

            foundTask.Title = task.Title;
            foundTask.Description = task.Description;
            foundTask.StartDate = task.StartDate;
            foundTask.DeadLine = task.DeadLine;
            foundTask.CompetionRate = task.CompetionRate;
            _db.Entry(foundTask).CurrentValues.SetValues(currentValues);
            await _db.SaveChangesAsync();

            await _db.SaveChangesAsync();
        }
        public async Task DeleteTaskAsync(TaskModel task)
        {
            var foundTask = await _db.Tasks
                .FindAsync(task.Id) ?? throw new InvalidOperationException("Task not found");
            _db.Tasks.Remove(task);
            await _db.SaveChangesAsync();
        }
        public async Task<List<TaskModel>?> GetTaskReportAsync()
        {
            var query = _db.Tasks
                .Include(task => task.User)
                .Where(task => task.DeadLine.CompareTo(DateTime.Now) < 0 && task.CompetionRate < 100);
            var tasksForReport = await query.ToListAsync();
            return tasksForReport;
        }
        public async Task<int> GetTaskCountByUsernameAsync(string username)
        {
            var query = _db.Tasks
                .Include(task => task.User)
                .Where(task => task.User.UserName == username)
                .CountAsync();
            var taskCount = await query;
            return taskCount;    
        }
        public async Task<int> GetCompletedTaskCountByUsernameAsync(string username)
        {
            var query = _db.Tasks
                .Include(task => task.User)
                .Where(task => task.User.UserName == username)
                .CountAsync(task => task.DeadLine.CompareTo(DateTime.Now) <= 0 &&
                task.CompetionRate >= 100);
            var taskCount = await query;
            return taskCount;
        }
    }
}
