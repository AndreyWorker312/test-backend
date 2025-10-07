using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UsersApp.Application.Users;
using UsersApp.Application.Users.Dtos;

namespace UsersApp.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersApiController : ControllerBase
    {
        private readonly IUserService _service;
        public UsersApiController(IUserService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? query)
        {
            var list = await _service.ListAsync(query);
            return Ok(list);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _service.GetAsync(id);
            if (user is null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest model)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var (ok, error, user) = await _service.CreateAsync(model);
            if (!ok)
            {
                if (error?.Contains("already in use") == true)
                    return Conflict(new { message = error });
                return BadRequest(new { message = error });
            }
            return CreatedAtAction(nameof(GetById), new { id = user!.Id }, user);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest model)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var (ok, error, user) = await _service.UpdateAsync(id, model);
            if (!ok)
            {
                if (error == "User not found") return NotFound();
                if (error?.Contains("already in use") == true)
                    return Conflict(new { message = error });
                return BadRequest(new { message = error });
            }
            return Ok(user);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var (ok, error) = await _service.DeleteAsync(id);
            if (!ok && error == "User not found") return NotFound();
            return NoContent();
        }
    }
}
