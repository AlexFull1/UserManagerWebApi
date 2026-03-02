using Microsoft.AspNetCore.Mvc;
using WebAPIProject.Services;
using WebAPIProject.DTOs.UsersDTO;

namespace WebAPIProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserServices userServices;

        // Constructor injection to satisfy non-nullable field initialization (fixes CS8618)
        public UsersController(IUserServices userServices)
        {
            this.userServices = userServices ?? throw new ArgumentNullException(nameof(userServices));
        }

        [HttpGet]
        public async Task<ActionResult<List<GetUserDTO>>> GetAll()
        {
            return await userServices.GetAllUsers();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserDTO>> Get(int id)
        {
            try
            {
                var user = await userServices.GetUserById(id);
                return Ok(user);
            }
            catch (Exception ex)
            { 
                return NotFound(ex.Message);
            }
        }

            [HttpPost]
            public async Task<ActionResult<GetUserDTO>> Create(AddUserDTO user)
            {
                if (user is null)
                    return BadRequest("User payload is required.");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdUser = await userServices.AddUser(user);

                if (createdUser is null)
                    return StatusCode(500, "Failed to create user.");

                // Ensure the route value matches the route parameter name ("id")
                return CreatedAtAction(nameof(Get), new { id = createdUser.Id }, createdUser);
            }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateUserDTO user)
        {
            try
            {
                await userServices.GetUserById(id);
                await userServices.UpdateUser(user, id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) 
        {
            var user = await userServices.GetUserById(id);
            
            if(user is null)
                return NotFound();

            await userServices.DeleteUser(id);
            return NoContent();
        }
    }
}
