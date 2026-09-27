using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using CowShop.Data;
using CowShop.Helpers;
using CowShop.Models;

namespace CowShop.Controllers
{
    public class VacaController : ApiController
    {
        [HttpGet]
        [Route("api/vaca")]
        public IHttpActionResult Get()
        {
            try
            {
                var vacas = VacaData.ListarVacas();
                if (vacas == null || vacas.Count == 0)
                    return NotFound();

                return Ok(vacas);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/vaca/{id}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var vaca = VacaData.ObtenerVaca(id);
                if (vaca == null)
                    return NotFound();

                return Ok(vaca);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("api/vaca")]
        public IHttpActionResult Post([FromBody] Vaca oVaca)
        {
            try
            {
                if (oVaca == null)
                    return BadRequest("La vaca no puede ser nula.");

                /* var authHeader = Request.Headers.Authorization;
                 if (authHeader == null || authHeader.Scheme != "Bearer")
                     return Unauthorized();

                 string token = authHeader.Parameter;
                 var claims = AuthHelper.ValidarToken(token);

                 if (claims == null || !claims.ContainsKey("rol") || !claims.ContainsKey("id"))
                     return Unauthorized();

                 string rol = claims["rol"].ToString();
                 string idUsuarioStr = claims["id"].ToString()

                 if (!int.TryParse(idUsuarioStr, out int idUsuario))
                     return BadRequest("ID de usuario inválido en el token.");

                 if (rol != "Vendedor" && rol != "Ambos")
                     return Unauthorized();;

                oVaca.ID_Vendedor = idUsuario; ; */
                bool success = VacaData.RegistrarVaca(oVaca);

                if (success)
                    return Created("api/vaca/" + oVaca.ID_Vaca, oVaca);
                else
                    return BadRequest("No se pudo registrar la vaca.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("api/vaca/{id}")]
        public IHttpActionResult Put(int id, [FromBody] Vaca oVaca)
        {
            try
            {
                if (oVaca == null)
                    return BadRequest("La vaca no puede ser nula.");

                if (id != oVaca.ID_Vaca)
                    return BadRequest("El ID de la ruta no coincide con el ID de la vaca.");

                bool success = VacaData.ActualizarVaca(id, oVaca);

                if (success)
                    return Ok(oVaca);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("api/vaca/{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                // CONVERSIÓN DE int A string
                bool success = VacaData.EliminarVaca(id.ToString());

                if (success)
                    return StatusCode(HttpStatusCode.NoContent);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}