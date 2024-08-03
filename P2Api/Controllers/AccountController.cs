using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P2Api.Data;
using P2Api.Models;
using P2Api.Services;
using P2Api.ViewModels;

namespace P2Api.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost("account/login")]
        public IActionResult Login([FromBody] UserViewModel model,
                                   [FromServices] AppDbContext context,
                                   [FromServices] TokenService tokenService)
        {
            try
            {
                var user = context.Users.FirstOrDefault(x => x.username == model.username);
                if (user == null)
                    return NotFound(new { message = "Username or Password Incorrect" });
                if (Settings.GenerateHash(model.password) != user.password)
                    return NotFound(new { message = "Username or Password Incorrect" });

                var token = tokenService.CreateToken(user);
                return Ok(new { token = token });
            }
            catch
            {
                return StatusCode(500, new { message = "Erro Interno no Servidor." });
            }
        }


        [HttpPost("account/signup")]
        public IActionResult SignUp([FromBody] UserViewModel model, [FromServices] AppDbContext context)
        {
            var user = context.Users.FirstOrDefault(x => x.username == model.username);
            if (user != null)
            {
                return BadRequest(new { message = "Usuario já Cadastrado." });
            }
            try
            {
                var newPasswordCrypt = Settings.GenerateHash(model.password); // Criptografia da Senha

                var newUser = new User
                {
                    username = model.username,
                    password = newPasswordCrypt,
                    role = "User"
                };
                context.Users.Add(newUser);
                context.SaveChanges();

                return Ok(new { message = "Cadastro Realizado com Sucesso." });
            }
            catch
            {
                return StatusCode(500, new { message = "Erro Interno no Servidor." });
            }
        }
    }
}
