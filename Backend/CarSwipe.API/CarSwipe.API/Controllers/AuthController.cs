using CarSwipe.API.DTOS;
using CarSwipe.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace CarSwipe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly CarSwipeContext adatbazis;
        private readonly IConfiguration config;

        public AuthController(CarSwipeContext db, IConfiguration configuration)
        {
            adatbazis = db;
            config = configuration;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDTO registerDTO)
        {
            try
            {
                if (registerDTO.Name == null || registerDTO.Email == null || registerDTO.Password == null)
                {
                    return StatusCode(400, new
                    {
                        message = "Töltse ki az összes mezőt"
                    });
                }
                else
                {

                    if (await adatbazis.Users.AnyAsync(u => u.Email == registerDTO.Email))
                    {
                        return Conflict(new
                        {
                            message = "Az email cím már foglalt"
                        });
                    }

                    var registeruser = new User()
                    {
                        Name = registerDTO.Name,
                        Email = registerDTO.Email,
                        Password = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password)
                    };

                    await adatbazis.Users.AddAsync(registeruser);
                    await adatbazis.SaveChangesAsync();
                    return StatusCode(201, new
                    {
                        message = "Sikeres regisztráció!"
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    message = "Szerverhiba történt"
                });
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var user = await adatbazis.Users.FirstOrDefaultAsync(u => u.Email == loginDTO.Email);

                if (user == null || !BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password))
                {
                    return Unauthorized(new
                    {
                        message = "Hibás email vagy jelszó"
                    });
                }


                return Ok(new
                {
                    Message = "Sikeres bejelentkezés",
                    Token = CreateToken(user),
                    UserId = user.UserId,
                    Name = user.Name,
                    Email = user.Email,
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    message = "Szerverhiba történt"
                });
            }
        }
        private string CreateToken(User user)
        {
          
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email)
            };

          
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));

           
            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddYears(1),   // 1 év
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}