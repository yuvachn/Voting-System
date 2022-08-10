using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using React.Models;

namespace React.Controllers
{
    //[EnableCors("corspolicy")]
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserContext db;

        public UserController(UserContext dbcontext)
        {
            db = dbcontext;
        }

        //GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> getUsers()
        {
            if(db.Users == null)
            {
                return NotFound();

            }
            return await db.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> getUsers(int id)
        {
            if (db.Users == null)
            {
                return NotFound();

            }
            var users = await db.Users.FindAsync(id);
            if(users == null)
            {
                return NotFound();
            }
            return users;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult<User>> Login([FromBody]User user)
        {
            var obj = await db.Users.Where(m=>m.Email == user.Email).ToListAsync();
            if (obj != null)
            {
                //logged in

                return Ok(obj);
            }



            return Ok(new { message = "Invalid credentials" });
        }

        [Route("[action]")]
        [HttpPost]
        
        public async Task<ActionResult<User>> PostUser([FromBody] User user)
        {
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return CreatedAtAction(nameof(getUsers), new { id = user.Id }, user);
        }



    }
}
