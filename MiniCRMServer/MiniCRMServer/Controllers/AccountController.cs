using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniCRMServer.Data;
using MiniCRMServer.Repositories;

namespace MiniCRMServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public readonly IEmployeeRepository _accountRepository;
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(IEmployeeRepository accountRepository, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) 
        {
            _accountRepository = accountRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost]
        [Route("[action]/{username}/{password}")]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (User.Identity!.IsAuthenticated)
                return BadRequest("You already authorized");

            var foundUser = await _userManager.FindByNameAsync(username);
            if (foundUser is null) 
                return Unauthorized("Invalid username or password");
            bool checkPassword = await _userManager.CheckPasswordAsync(foundUser, password);
            bool isInRole = await _userManager.IsInRoleAsync(foundUser, "admin");
            if (isInRole)
            {
                if (checkPassword)
                {
                    await _signInManager.SignInAsync(foundUser, true);
                    return Ok("Authorized");
                }
            }
            return Unauthorized("No Access");
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity!.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
                return Ok("Logout");
            }
            return BadRequest("You are not authorized");


        }
    }
}
