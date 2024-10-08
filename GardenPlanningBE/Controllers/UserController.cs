using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using GardenPlanningBE.Models;
using System.Diagnostics.Eventing.Reader;
using System.Text.RegularExpressions;

namespace GardenPlanningBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly RegisterDbContext _context;
        public UserController(RegisterDbContext _context)
        {
            this._context = _context;
        }
        [HttpPost]
        [Route("Registration")]
        public IActionResult Registration(UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var objUser = _context.Users.FirstOrDefault(x => x.Email == userDTO.Email);
            if (objUser == null)
            {
                _context.Users.Add(new User
                {
                    FirstName = userDTO.FirstName,
                    LastName = userDTO.LastName,
                    Email = userDTO.Email,
                    Password = userDTO.Password,

                });
                _context.SaveChanges();
                return Ok("User registered succesfully");
            }else
                    {
                         return BadRequest("User already exists with the same email address.");
                    }
        }
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginDTO loginDTO)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == loginDTO.Email && x.Password == loginDTO.Password);
            if(user != null) {
                return Ok("Login succesfull.");
            }
            return BadRequest("Invalid user.");
        }

        [HttpGet]
        [Route("GetUsers")]
        public IActionResult GetUsers()
        { 
        return Ok(_context.Users.ToList());
        }

        [HttpGet]
        [Route("GetUser")]
        public IActionResult GetUser(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.UserId == id);
            if (user != null)
            {
                return Ok(user);
            }
            else
                return NoContent();
        }
    }
}
