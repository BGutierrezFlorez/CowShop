using CowShop.Data;
using CowShop.Helpers;
using CowShop.Models;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace CowShop.Controllers
{
    public class UsuarioController : ApiController
    {
        // Preflight CORS
        [HttpOptions]
        [Route("api/usuarios")]
        public IHttpActionResult OptionsUsuario() => Ok();

        [HttpOptions]
        [Route("api/usuarios/{id}")]
        public IHttpActionResult OptionsUsuarioPorId() => Ok();

        [HttpOptions]
        [Route("api/usuarios/login")]
        public IHttpActionResult OptionsLogin() => Ok();

        // GET api/usuarios
        [HttpGet]
        [Route("api/usuarios")]
        public IHttpActionResult Get()
        {
            var usuarios = UsuarioData.ListarUsuarios();
            return Ok(usuarios ?? new List<Usuario>());
        }

        // GET api/usuarios/{id}
        [HttpGet]
        [Route("api/usuarios/{id}", Name = "GetUsuarioById")]
        public IHttpActionResult Get(string id)
        {
            var usuario = UsuarioData.ObtenerUsuarioporId(id);
            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        // POST api/usuario
        [HttpPost]
        [Route("api/usuarios")]
        public IHttpActionResult Post([FromBody] Usuario usuario)
        {
            if (usuario == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            bool success = UsuarioData.RegistrarUsuario(usuario);
            if (!success)
                return BadRequest("No se pudo registrar el usuario.");

            return CreatedAtRoute("GetUsuarioById", new { id = usuario.ID_Usuario }, usuario);
        }

        // PUT api/usuario/{id}
        [HttpPut]
        [Route("api/usuario/{id}")]
        public IHttpActionResult Put(int id, [FromBody] Usuario usuario)
        {
            if (usuario == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            usuario.ID_Usuario = id;

            bool success = UsuarioData.ActualizarUsuario(id, usuario);
            if (!success)
                return NotFound();

            return Ok(usuario);
        }

        // DELETE api/usuario/{id}
        [HttpDelete]
        [Route("api/usuario/{id}")]
        public IHttpActionResult Delete(int id)
        {
            bool success = UsuarioData.EliminarUsuario(id.ToString());
            if (!success)
                return NotFound();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/usuarios/login
        [HttpPost]
        [Route("api/usuarios/login")]
        public IHttpActionResult Login([FromBody] UsuarioLoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Correo) || string.IsNullOrEmpty(request.Contrasena))
                return BadRequest("Correo y contraseña son obligatorios.");

            // Diagnóstico: usar VerificarLogin para obtener motivo del fallo (temporal)
            var ver = UsuarioData.VerificarLogin(request.Correo, request.Contrasena);
            if (ver == UsuarioData.VerificacionLoginResultado.NoExisteCorreo)
                return Content(HttpStatusCode.Unauthorized, "Correo no registrado");
            if (ver == UsuarioData.VerificacionLoginResultado.ContrasenaIncorrecta)
                return Content(HttpStatusCode.Unauthorized, "Contraseña incorrecta");

            // Si pasa verificación, obtener el usuario real
            var usuario = UsuarioData.Login(request.Correo, request.Contrasena);
            if (usuario == null)
                return Content(HttpStatusCode.InternalServerError, "Error al obtener usuario después de verificar");

            string token = JwtHelper.GenerarToken(usuario.ID_Usuario, usuario.Tipo_Usuario);

            return Ok(new
            {
                token,
                usuario = new
                {
                    usuario.ID_Usuario,
                    usuario.Nombre,
                    usuario.Tipo_Usuario
                }
            });


        }

        [HttpGet]
        [Route("api/usuarios/generar-hash")]
        public IHttpActionResult GenerarHash()
        {
            string password = "CowShop123!";
            string hash = BCrypt.Net.BCrypt.HashPassword(password);

            return Ok(new
            {
                password,
                hash
            });
        }
    }

    
    }