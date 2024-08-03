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
    public class TarefaController : ControllerBase
    {
        // PostTarefa - btnCreateTarefa() generate a new Tarefa and getAll Again to update.
        [Authorize(Roles = "User")]
        [HttpPost("tarefa")]
        public async Task<IActionResult> PostTarefaAsync([FromBody] TarefaCreateViewModel model,[FromServices] AppDbContext context)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId != null)
                {
                    var newTarefa = new Tarefa
                    {
                        title = model.title,
                        description = model.description,
                        createdByUser = int.Parse(userId),
                        status = "Em Andamento"
                    };

                    await context.Tarefas.AddAsync(newTarefa);
                    await context.SaveChangesAsync();

                    return Created($"tarefa/{newTarefa.id}", newTarefa);
                }
                else
                {
                    return NotFound(new { message = "Oops, something went wrong, Please try again later." });
                }
            }
            catch
            {
                return StatusCode(500, new { message = "Erro Interno no Servidor." });
            }  
        }
        // GetAll - Populate the Form with GroupBox from each Tarefa
        [Authorize(Roles = "User")]
        [HttpGet("tarefas")]
        public IActionResult GetAllTarefaFromLoggedAsync([FromServices] AppDbContext context)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId != null)
                {
                    var listTarefa = context.Tarefas
                                    .Where(x => x.createdByUser == int.Parse(userId))
                                    .Select(y => new { y.id, y.title, y.description, y.status });
                    return Ok(listTarefa);
                }
                else
                {
                    return NotFound(new { message = "Oops, something went wrong, Please try again later." });
                }
            }
            catch
            {
                return StatusCode(500, new { message = "Erro Interno no Servidor." });
            }
        }
        // GetByStatus - (Em Andamento, Urgente, Concluido)
        [Authorize(Roles = "User")]
        [HttpGet("tarefa/{status}")]
        public IActionResult GetByStatus([FromServices] AppDbContext context, [FromRoute] string status)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId != null)
                {
                    var listTarefa = context.Tarefas
                    .Where(x => x.status == status && x.createdByUser == int.Parse(userId))
                    .Select(y => new { y.id, y.title, y.description, y.status });

                    return Ok(listTarefa);
                }
                else
                {
                    return NotFound(new { message = "Oops, something went wrong, Please try again later." });
                }
            }
            catch
            {
                return StatusCode(500, new { message = "Erro Interno no Servidor." });
            }
        }
        // Put Status - Change the Status from an specific Tarefa
        [Authorize(Roles = "User")]
        [HttpPut("tarefa/status/{id:int}")]
        public async Task<IActionResult> PutTarefaStatusAsync([FromServices] AppDbContext context,
                                                              [FromRoute] int id,
                                                              [FromBody] TarefaStatusViewModel model)
        {
            try
            {
                var tarefa = await context.Tarefas.FindAsync(id);
                if(tarefa == null)
                    return NotFound(new { message = "Oops, something went wrong, Please try again later." });

                switch (model.status)
                {
                    case "Concluida":
                        tarefa.status = "Em Andamento";
                        break;
                    case "Urgente":
                        tarefa.status = "Concluida";
                        break;
                    case "Em Andamento":
                        tarefa.status = "Urgente";
                        break;
                }

                await context.SaveChangesAsync();
                return Ok();
            }
            catch
            {
                return StatusCode(500, new { message = "Erro Interno no Servidor." });
            }
        }
        // Delete - Delete Tarefa if smth did wrong and if user needed to.
        [Authorize(Roles = "User")]
        [HttpDelete("tarefa/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromServices] AppDbContext context, [FromRoute] int id)
        {
            try
            {
                var tarefa = await context.Tarefas.FindAsync(id);
                if (tarefa == null)
                    return NotFound(new { message = "Tarefa Inexistente." });

                context.Tarefas.Remove(tarefa);
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
