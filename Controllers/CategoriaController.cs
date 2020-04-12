using Microsoft.AspNetCore.Mvc;
using EmprestimoFerramentas.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using EmprestimoFerramentas.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace EmprestimoFerramentas.Controllers
{
    [Route("v1/categorias")]
    public class CategoriaController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 30)]
        // Para usar a linha abaixo, deve habilitar a parte de cache no startup.cs
        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<List<Categoria>>> Get([FromServices] DataContext context)
        {
            var categorias = await context.Categorias.AsNoTracking().ToListAsync();
            
            return Ok(categorias);
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Categoria>> GetById(int id, [FromServices] DataContext context)
        {
            var categoria = await context.Categorias.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (categoria == null)
                return NotFound(new { message = "Categoria não encontrada!"});

            return Ok(categoria);
        }

        [HttpPost]
        [Authorize(Roles = "funcionario")]
        public async Task<ActionResult<List<Categoria>>> Post([FromBody] Categoria model, [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Categorias.Add(model);
                await context.SaveChangesAsync();

                return Ok(model);
            }
            catch
            {
                return BadRequest(new { message = "Não foi possível cadastrar!" });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "funcionario")]
        public async Task<ActionResult<List<Categoria>>> Put(int id, [FromBody] Categoria model, [FromServices] DataContext context)
        {
            if (id != model.Id)
                return NotFound(new {message = "Categoria não encontrada!"});

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Entry<Categoria>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();

                return Ok(model);
            }
            catch
            {
                return BadRequest(new { message = "Não foi possível atualizar!"});
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "funcionario")]
        public async Task<ActionResult<List<Categoria>>> Delete(int id, [FromServices] DataContext context)
        {
            var categoria = await context.Categorias.FirstOrDefaultAsync(x => x.Id == id);
            if (categoria == null)
                return NotFound(new { message = "Categoria não encontrada!"});

            try
            {
                context.Categorias.Remove(categoria);
                await context.SaveChangesAsync();

                return Ok(categoria);
            }
            catch
            {
                return BadRequest(new { message = "Não foi possível excluir!"});
            }
        }
    }
}