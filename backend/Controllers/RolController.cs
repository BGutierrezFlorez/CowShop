using CowShop.Data;
using CowShop.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CowShop.Controllers
{
    [RoutePrefix("api/rol")]
    public class RolController : ApiController
    {
        private RolData rolData = new RolData();

        // GET api/rol
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            var roles = rolData.ObtenerRol();
            return Ok(roles);
        }

        // GET api/rol/{id}
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var rol = RolData.ObtenerRolPorId(id);
            if (rol == null)
                return NotFound();
            return Ok(rol);
        }

        // POST api/rol
        [HttpPost]
        [Route("")]
        public IHttpActionResult Create([FromBody] Rol rol)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool creado = rolData.CrearRol(rol);
            if (creado)
                return Ok("Rol creado correctamente.");
            return BadRequest("No se pudo crear el rol.");
        }

        // PUT api/rol/{id}
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Update(int id, [FromBody] Rol rol)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            rol.ID_Rol = id;
            bool actualizado = rolData.ActualizarRol(rol);
            if (actualizado)
                return Ok("Rol actualizado correctamente.");
            return BadRequest("No se pudo actualizar el rol.");
        }

        // DELETE api/rol/{id}
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            bool eliminado = rolData.EliminarRol(id);
            if (eliminado)
                return Ok("Rol eliminado correctamente.");
            return BadRequest("No se pudo eliminar el rol.");
        }
    }
}
