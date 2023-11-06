using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniCRMServer.Data;
using MiniCRMServer.Extensions;
using MiniCRMServer.Repositories;
using MiniCRMServer.ViewModels;

namespace MiniCRMServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public TaskController(ITaskRepository taskRepository, UserManager<ApplicationUser> userManager) 
        {
            _taskRepository = taskRepository;
            _userManager = userManager;
        }
        [HttpGet("[action]/{username}")]
        [Authorize]
        public async Task<IActionResult> GetTasksByEmployee(string username)
        {
            var tasksByUsername = await _taskRepository.GetTasksByUsernameAsync(username);
            if (!tasksByUsername.Any())
                return BadRequest("Tasks not found");
            var taskViewModelList = tasksByUsername!.Select(task => task.ToTaskViewModel()).ToList();
            return Ok(taskViewModelList);
        }
        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetTaskReport()
        {
            var tasksReport = await _taskRepository.GetTaskReportAsync();
            if (!tasksReport.Any())
                return BadRequest("Tasks not found");
            var overdueTaskViewModelList = tasksReport
                .Select(task => task.ToOverdueTaskViewModel(task.Id, task.User.GetFullName()));
            return Ok(overdueTaskViewModelList);
        }
        [HttpPost("[action]/{username}")]
        [Authorize]
        public async Task<IActionResult> AddTask([FromBody]TaskViewModel taskViewModel, string username)
        {
            var newTask = new TaskModel
            {
                Id = taskViewModel.Id,
                Title = taskViewModel.Title,
                Description = taskViewModel.Description,
                StartDate = taskViewModel.StartDate,
                DeadLine = taskViewModel.DeadLine,
            };
            try
            {
                await _taskRepository.AddTaskAsync(newTask, username);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return Ok();
        }
        [HttpPut("[action]")]
        [Authorize]
        public async Task<IActionResult> UpdateTask([FromBody] TaskViewModel taskViewModel)
        {
            var taskToUpdate = new TaskModel
            {
                Id = taskViewModel.Id,
                Title = taskViewModel.Title,
                Description = taskViewModel.Description,
                StartDate = taskViewModel.StartDate,
                DeadLine = taskViewModel.DeadLine,
            };
            try
            {
                await _taskRepository.UpdateTaskAsync(taskToUpdate);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return Ok();
        }
        [HttpDelete("[action]")]
        [Authorize]
        public async Task<IActionResult> DeleteTask([FromBody] int taskId, string title, string description, DateTime startDate, DateTime deadLine)
        {
            var taskToUpdate = new TaskModel
            {
                Id = taskId,
                Title = title,
                Description = description,
                StartDate = startDate,
                DeadLine = deadLine,
            };
            try
            {
                await _taskRepository.DeleteTaskAsync(taskToUpdate);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return Ok();
        }

    }
}
