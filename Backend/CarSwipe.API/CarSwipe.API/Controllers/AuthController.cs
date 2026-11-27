using CarSwipe.API.DTOS;
using CarSwipe.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSwipe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly CarSwipeContext adatbazis;

        public AuthController(CarSwipeContext db)
        {
            adatbazis = db;
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

                    if(await adatbazis.Users.AnyAsync(u => u.Email == registerDTO.Email))
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
                        Password = registerDTO.Password
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

                if (user == null || user.Password != loginDTO.Password)
                {
                    return Unauthorized(new
                    {
                        message = "Hibás email vagy jelszó"
                    });
                }


                return Ok(new
                {
                    Message = "Sikeres bejelentkezés",
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
    } 
}