using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.Common;
using PetCare.Domain.DTOs;
using PetCare.Domain.Identity;

namespace PetCare.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var users = _userManager.Users.ToList();
            var result = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new
                {
                    id = user.Id,
                    email = user.Email,
                    fullName = $"{user.FirstName} {user.LastName}",
                    role = roles.FirstOrDefault() ?? "Sin rol"
                });
            }

            return Ok(ApiResponse<List<object>>.Success(result));
        }

        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] CreateUserDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                return BadRequest(ApiResponse<string>.Failure("El usuario ya existe"));

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(ApiResponse<string>.Failure(
                    string.Join(", ", result.Errors.Select(e => e.Description))
                ));

            await _userManager.AddToRoleAsync(user, dto.Role);

            return Ok(ApiResponse<string>.Success("Usuario creado correctamente"));
        }

        [HttpPut("CambiarRol/{id}")]
        public async Task<IActionResult> CambiarRol(string id, [FromBody] ChangeRoleDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound(ApiResponse<string>.Failure("Usuario no encontrado"));

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, dto.Role);

            return Ok(ApiResponse<string>.Success("Rol actualizado correctamente"));
        }

        [HttpDelete("Eliminar/{id}")]
        public async Task<IActionResult> Eliminar(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound(ApiResponse<string>.Failure("Usuario no encontrado"));

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return BadRequest(ApiResponse<string>.Failure("Error al eliminar el usuario"));

            return Ok(ApiResponse<string>.Success("Usuario eliminado correctamente"));
        }
    }  

   
}