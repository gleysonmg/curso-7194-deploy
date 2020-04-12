using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using EmprestimoFerramentas.Data;
using EmprestimoFerramentas.Models;
using Microsoft.AspNetCore.Authorization;

namespace EmprestimoFerramentas.Controllers
{
    [Route("v1/produtos")]
    public class ProdutoController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<Produto>>> Get([FromServices] DataContext context)
        {
            var produtos = await context.Produtos.Include(x => x.Categoria).AsNoTracking().ToListAsync();
            
            return Ok(produtos);
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Produto>> GetById(int id, [FromServices] DataContext context)
        {
            var produto = await context.Produtos.Include(x => x.Categoria).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (produto == null)
                return NotFound(new { message = "Produto não encontrado!"});

            return Ok(produto);
        }

        [HttpGet]
        [Route("categorias/{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Produto>> GetByCategoria(int id, [FromServices] DataContext context)
        {
            var produtos = await context.Produtos.Include(x => x.Categoria).AsNoTracking().Where(x => x.CategoriaId == id).ToListAsync();
                
            return Ok(produtos);
        }

        [HttpPost]
        [Authorize(Roles = "funcionario")]
        public async Task<ActionResult<List<Produto>>> Post([FromBody] Produto model, [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Produtos.Add(model);
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
        public async Task<ActionResult<List<Produto>>> Put(int id, [FromBody] Produto model, [FromServices] DataContext context)
        {
            if (id != model.Id)
                return NotFound(new {message = "Produto não encontrado!"});

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Entry<Produto>(model).State = EntityState.Modified;
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
        public async Task<ActionResult<List<Produto>>> Delete(int id, [FromServices] DataContext context)
        {
            var produto = await context.Produtos.FirstOrDefaultAsync(x => x.Id == id);
            if (produto == null)
                return NotFound(new { message = "Produto não encontrado!"});

            try
            {
                context.Produtos.Remove(produto);
                await context.SaveChangesAsync();

                return Ok(produto);
            }
            catch
            {
                return BadRequest(new { message = "Não foi possível excluir!"});
            }
        }
    }
}