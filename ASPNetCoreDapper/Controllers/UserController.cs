using Microsoft.AspNetCore.Mvc;
using ASPNetCoreDapper.Entities;

namespace ASPNetCoreDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                var messages = ModelState
                  .SelectMany(modelState => modelState.Value.Errors)
                  .Select(err => err.ErrorMessage)
                  .ToList();

                return BadRequest(messages);
            }

            // TODO: Add user creation logic here
            return Ok(user);
        }
    }
} 