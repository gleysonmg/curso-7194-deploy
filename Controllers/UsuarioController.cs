using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmprestimoFerramentas.Data;
using EmprestimoFerramentas.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using EmprestimoFerramentas.Services;

namespace EmprestimoFerramentas.Controllers
{
    [Route("v1/usuarios")]
    public class UsuarioController : Controller
    {
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "administrador")]
        public async Task<ActionResult<List<Usuario>>> Get([FromServices] DataContext context)
        {
            var usuarios = await context.Usuarios.AsNoTracking().ToListAsync();

            return usuarios;
        }

        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        // [Authorize(Roles = "administrador")]
        public async Task<ActionResult<Usuario>> Post(
            [FromServices] DataContext context,
            [FromBody]Usuario model)
        {
            // Verifica se os dados são válidos
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Força o usuário a ser sempre "funcionário"
                model.Perfil = "funcionario";

                context.Usuarios.Add(model);
                await context.SaveChangesAsync();

                // Esconde a senha
                model.Senha = "";
                return model;
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível criar o usuário" });
            }
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate(
                    [FromServices] DataContext context,
                    [FromBody]Usuario model)
        {
            var usuario = await context.Usuarios
                .AsNoTracking()
                .Where(x => x.NomeUsuario == model.NomeUsuario && x.Senha == model.Senha)
                .FirstOrDefaultAsync();

            if (usuario == null)
                return NotFound(new { message = "Usuário ou senha inválidos!" });

            var token = TokenService.GenerateToken(usuario);
            // Esconde a senha
            usuario.Senha = "";
            return new
            {
                usuario = usuario,
                token = token
            };
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "administrator")]
        public async Task<ActionResult<Usuario>> Put(
            [FromServices] DataContext context,
            int id,
            [FromBody]Usuario model)
        {
            // Verifica se os dados são válidos
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verifica se o ID informado é o mesmo do modelo
            if (id != model.Id)
                return NotFound(new { message = "Usuário não encontrado!" });

            try
            {
                context.Entry(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return model;
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível criar o usuário!" });
            }
        }
    }
}