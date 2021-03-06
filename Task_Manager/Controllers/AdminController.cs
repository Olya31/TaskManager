using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Task_Manager.Controllers
{
    [Route("api/admin")] 
    [ApiController]
    [System.Web.Http.Authorize(Roles = "Administrator")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public AdminController(
            UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("getAll")]
        public IActionResult GetAll()
        {
            var users =  _userManager.Users.ToList();

            return Ok(users);
        }
    }
}
