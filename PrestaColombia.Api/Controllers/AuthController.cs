using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Prestacol.Infrastructure.;
using Prestacol.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PrestaColombia.Api.Helpers;
using PrestaColombia.Api.DTOs;



namespace PrestaColombia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly PrestacolDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(PrestacolDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Email == email && u.Estado);

            if (usuario == null)
                return Unauthorized("Usuario no encontrado");

            // ⚠️ Por ahora comparación simple (luego hacemos hash real)
            if (!PasswordHasher.Verify(password, usuario.PasswordHash))
                return Unauthorized("Contraseña incorrecta");

            var jwtSettings = _configuration.GetSection("JwtSettings");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol.Nombre)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(
                    double.Parse(jwtSettings["DurationInMinutes"]!)
                ),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existe = await _context.Usuarios
                .AnyAsync(u => u.Email == request.Email);

            if (existe)
                return BadRequest("El usuario ya existe");

            var rol = await _context.Roles
                .FirstOrDefaultAsync(r => r.Nombre == "Admin");

            if (rol == null)
                return BadRequest("Rol no encontrado");

            var usuario = new Usuario
            {
                Id = Guid.NewGuid(),
                Nombre = request.Nombre,
                Email = request.Email,
                PasswordHash = PasswordHasher.Hash(request.Password),
                RolId = rol.Id,
                Estado = true
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok("Usuario registrado correctamente");
        }
    }
}