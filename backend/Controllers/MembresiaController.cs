using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using CowShop.Data;
using CowShop.Models;

namespace CowShop.Controllers
{
    [EnableCors(origins: "http://127.0.0.1:5503,http://localhost:5000",
                headers: "*", methods: "GET,POST,PUT,DELETE,OPTIONS")]
    [RoutePrefix("api/membresia")]
    public class MembresiaController : ApiController
    {
        // GET api/membresia
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            var membresias = MembresiaData.ListarMembresia() ?? new List<Membresia>();
            return Ok(membresias);
        }

        // GET api/membresia/{id}
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            var membresias = MembresiaData.ObtenerMembresia(id);
            if (membresias == null || membresias.Count == 0)
                return NotFound();

            return Ok(membresias);
        }

        // POST api/membresia
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] Membresia membresia)
        {
            if (membresia == null)
                return BadRequest("La membresía no puede ser nula.");

            bool success = MembresiaData.RegistrarMembresia(membresia);
            if (!success)
                return BadRequest("No se pudo registrar la membresía.");

            return Created($"api/membresia/{membresia.ID_Membresia}", membresia);
        }

        // PUT api/membresia/{id}
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Put(string id, [FromBody] Membresia membresia)
        {
            if (membresia == null)
                return BadRequest("La membresía no puede ser nula.");

            bool success = MembresiaData.ActualizarMembresia(id, membresia);
            if (!success)
                return NotFound();

            return Ok(membresia);
        }

        // DELETE api/membresia/{id}
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(string id)
        {
            bool success = MembresiaData.EliminarMembresia(id);
            if (!success)
                return NotFound();

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}