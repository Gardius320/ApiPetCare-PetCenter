using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PetCare.Application.Auth;
using PetCare.Domain.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PetCare.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            // se valida si el usuario existee
            var usuarioExiste = await _userManager.FindByEmailAsync(model.Email);
            if (usuarioExiste != null)
                return BadRequest("El usuario ya existe");

            //  Crear el usuario
            var nuevoUsuario = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            //  Guardar en la base de datos
            var resultado = await _userManager.CreateAsync(nuevoUsuario, model.Password);
            if (!resultado.Succeeded)
                return BadRequest(resultado.Errors);

            // Crear el rol si no existe
            bool rolExiste = await _roleManager.RoleExistsAsync(model.Role);
            if (!rolExiste)
                await _roleManager.CreateAsync(new IdentityRole(model.Role));

            // Asignar el rol al usuario
            await _userManager.AddToRoleAsync(nuevoUsuario, model.Role);

            return Ok("Usuario registrado correctamente");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            // Buscar el usuario
            var usuario = await _userManager.FindByEmailAsync(model.Email);
            if (usuario == null)
                return Unauthorized("Email o contraseña incorrectos");

            // Verificar la contraseña
            bool contraseñaCorrecta = await _userManager.CheckPasswordAsync(usuario, model.Password);
            if (!contraseñaCorrecta)
                return Unauthorized("Email o contraseña incorrectos");

            //  Obtener los roles del usuario
            var roles = await _userManager.GetRolesAsync(usuario);

            // Generar el token
            var token = GenerarToken(usuario, roles);

            // Devolver la respuesta
            return Ok(new AuthResponseDto
            {
                Token = token,
                Email = usuario.Email,
                FullName = usuario.FullName,
                Role = roles.FirstOrDefault(),
                expires = DateTime.UtcNow.AddHours(24),
            });
        }

        private string GenerarToken(ApplicationUser usuario, IList<string> roles)
        {
            // Los datos que van dentro del token
            var claims = new List<Claim>
            {
                new Claim("id", usuario.Id),
                new Claim("email", usuario.Email),
                new Claim("nombre", usuario.FullName)
            };

            // Agregar roles al token
            foreach (var rol in roles)
                claims.Add(new Claim("role", rol));

            // Clave secreta para firmar el token
            var clave = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])); //esta linea va al appsettings.json y lo lee

            var firmado = new SigningCredentials(clave, SecurityAlgorithms.HmacSha256); //esta linea encripta conel .hamcSha en bytes.
             
            // Este segmento de codigo construye el token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: firmado
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
            //Esta linea convierte a un string que puedes enviar al frontend:
        }
    }
}