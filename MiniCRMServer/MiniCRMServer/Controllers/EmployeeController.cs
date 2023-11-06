using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
    public class EmployeeController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITaskRepository _taskRepository;
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeController(UserManager<ApplicationUser> userManager, ITaskRepository taskRepository, IEmployeeRepository accountRepository)
        {
            _userManager = userManager;
            _taskRepository = taskRepository;
            _employeeRepository = accountRepository;
        }
        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetEmployeeList() 
        {
            var userList =  _userManager.Users.ToList();
            if (!userList.Any())
                return BadRequest("No Data Found");
            var employeeViewModelList = new List<EmployeeViewModel>();
            foreach (var user in userList)
            {
                var numberOfTasks = await _taskRepository.GetTaskCountByUsernameAsync(user.UserName!);
                if (numberOfTasks == 0)
                {
                    employeeViewModelList.Add(user.ToEmployeeViewModel(numberOfTasks, 0));
                    continue;
                }
                var competionRate = (await _taskRepository.GetCompletedTaskCountByUsernameAsync(user.UserName!) * 100) / numberOfTasks;
                employeeViewModelList.Add(user.ToEmployeeViewModel(numberOfTasks, competionRate));
            }
            return Ok(employeeViewModelList);
        }
        [HttpPost("[action]/{username}")]
        [Authorize]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeViewModel employeeViewModel, string username)
        {
            string password = "password";
            string[] fullNameSplit = employeeViewModel.GetSplitFullName();
            var newEmployee = new ApplicationUser
            {
                FirstName = fullNameSplit[0],
                LastName = fullNameSplit[1],
                Patronymic = fullNameSplit[2],
                Position = employeeViewModel.Position,
                UserName = username,
            };
            var hashedPassword = _userManager.PasswordHasher.HashPassword(newEmployee, password);
            newEmployee.PasswordHash = hashedPassword;
            await _employeeRepository.AddEmployeeAsync(newEmployee);
            return Ok();
        }
        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetUsername(string fullName)
        {
            string[] fullNameSplit = fullName.Split(' ');
            var username = await _employeeRepository.GetUsernameAsync(fullNameSplit[0], fullNameSplit[1], fullNameSplit[2]);
            if (username is null)
                return NotFound();
            return Ok(username);
        }
        [HttpPut("[action]/{username}")]
        [Authorize]
        public async Task<IActionResult> UpdateEmployee([FromBody]EmployeeViewModel employeeViewModel, string username)
        {
            var userToUpdate = await _userManager.FindByNameAsync(username);
            string[] fullNameSplit = employeeViewModel.GetSplitFullName();
        
            if (userToUpdate is null)
                return BadRequest("Employee Not Found");
            if (fullNameSplit.Length == 3)
            {
                userToUpdate.FirstName = fullNameSplit[0];
                userToUpdate.LastName = fullNameSplit[1];
                userToUpdate.Patronymic = fullNameSplit[2];
            }
            userToUpdate.Position = employeeViewModel.Position;
            await _employeeRepository.UpdateEmployeeAsync(userToUpdate);
            return Ok();
        }
        [HttpDelete("[action]")]
        [Authorize]
        public async Task<IActionResult> DeleteEmployee(string username)
        {
            try
            {
                await _employeeRepository.DeleteEmployeeAsync(username);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return Ok();
        }
    }
}
