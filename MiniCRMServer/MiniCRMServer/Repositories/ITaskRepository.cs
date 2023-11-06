using MiniCRMServer.Data;

namespace MiniCRMServer.Repositories
{
    public interface ITaskRepository
    {
        public Task<List<TaskModel>?> GetTasksByUsernameAsync(string username);
        public Task AddTaskAsync(TaskModel task, string username);
        public Task UpdateTaskAsync(TaskModel task);
        public Task DeleteTaskAsync(TaskModel task);
        public Task<List<TaskModel>?> GetTaskReportAsync();
        public Task<int> GetCompletedTaskCountByUsernameAsync(string username);
        public Task<int> GetTaskCountByUsernameAsync(string username);
        //public Task<int> GetCompetionRateByUsername(string username);
    }
}
