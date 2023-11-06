using MiniCRMServer.Data;
using MiniCRMServer.ViewModels;

namespace MiniCRMServer.Extensions
{
    public static class MappingExtensions
    {
        public static EmployeeViewModel ToEmployeeViewModel(this ApplicationUser applicationUser, int numberOfTasks, int competionRate)
        {
            return new EmployeeViewModel
            {
                Id = applicationUser.Id,
                FullName = applicationUser.GetFullName(),
                Position = applicationUser.Position,
                NumberOfTasks = numberOfTasks,
                CompetionRate = competionRate
            };
        }
        public static TaskViewModel ToTaskViewModel(this TaskModel taskModel)
        {
            return new TaskViewModel
            {
                Id = taskModel.Id,
                Title = taskModel.Title,
                Description = taskModel.Description,
                StartDate = taskModel.StartDate,
                DeadLine = taskModel.DeadLine,
                CompetionRate = taskModel.CompetionRate,
            };
        }
        public static OverdueTaskViewModel ToOverdueTaskViewModel(this TaskModel taskModel, int id, 
            string employeeFullName)
        {
            return new OverdueTaskViewModel
            {
                Id = id,
                EmployeeFullName = employeeFullName,
                Title = taskModel.Title,
                StartDate = taskModel.StartDate,
                DeadLine = taskModel.DeadLine,
                CompetionRate = taskModel.CompetionRate,
                DaysOverdue = Math.Abs((DateTime.Now - taskModel.DeadLine).Days),
            };
        }
    }
}
