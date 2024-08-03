using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P2Api.Data;
using P2Api.Models;
using P2Api.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security.Claims;
using System.Xml.Linq;

namespace P2Api.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        [Authorize(Roles = "Adm")]
        [HttpGet("user")]
        public async Task<IActionResult> GetUserAsync([FromServices] AppDbContext context)
        {
            try
            {
                var users = await context.Users
                                  .Select(x => new { x.id, x.username, x.role } )
                                  .OrderBy(x => x.role)
                                  .ToListAsync();
                return Ok(users);
            }
            catch
            {
                return StatusCode(500, new { message = "Erro Interno no Servidor." });
            }
        }

        [Authorize(Roles = "Adm")]
        [HttpDelete("user/{id:int}")]
        public async Task<IActionResult> DeleteUserAsync([FromServices] AppDbContext context, [FromRoute] int id)
        {
            try
            {
                var user = await context.Users.FindAsync(id);
                
                if (user == null)
                    return StatusCode(401, new { message = "Usuario Inexistente." });
                if (user.role == "Adm")
                    return StatusCode(401, new { message = "Insufficient Authorization." });

                context.Users.Remove(user);
                await context.SaveChangesAsync();

                return Ok();
            }
            catch
            {
                return StatusCode(500, new { message = "Erro Interno no Servidor." });
            }
        }

    }
}
