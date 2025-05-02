using System.Threading.Tasks;
using DRESystem.API.New.Models; // Adicione este namespace para o LoginModel
using Microsoft.AspNetCore.Mvc;

namespace DRESystem.API.New.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            // Implementação simplificada
            if (model.Username == "admin" && model.Password == "admin123")
            {
                return Ok(
                    new
                    {
                        Token = "token-simulado-para-teste",
                        User = new { Username = model.Username, Role = "Admin" },
                    }
                );
            }

            return Unauthorized(new { Message = "Credenciais inválidas" });
        }
    }
}
