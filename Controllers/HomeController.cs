using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EmprestimoFerramentas.Data;
using EmprestimoFerramentas.Models;

namespace EmprestimoFerramentas.Controllers
{
    [Route("v1")]
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<dynamic>> Get([FromServices] DataContext context)
        {
            var funcionario = new Usuario { Id = 1, NomeUsuario = "robin", Senha = "robin", Perfil = "funcionario" };
            var administrador = new Usuario { Id = 2, NomeUsuario = "batman", Senha = "batman", Perfil = "administrador" };
            var categoria = new Categoria { Id = 1, NomeCategoria = "Informática" };
            var produto = new Produto { Id = 1, Categoria = categoria, NomeProduto = "Mouse", Preco = 299, Descricao = "Mouse Gamer" };
            context.Usuarios.Add(funcionario);
            context.Usuarios.Add(administrador);
            context.Categorias.Add(categoria);
            context.Produtos.Add(produto);
            await context.SaveChangesAsync();

            return Ok(new
            {
                message = "Dados configurados"
            });
        }
    }
}