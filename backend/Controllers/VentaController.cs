using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CowShop.Models;
using CowShop.Data;

namespace CowShop.Controllers
{
    public class VentaController : ApiController
    {

        [HttpGet]
        [Route("api/venta")]
        public IHttpActionResult ListarVentas()
        {
            var ventas = VentaData.ListarVentas();
            return Ok(ventas);
        }


        [HttpGet]
        [Route("api/venta/{id}")]
        public IHttpActionResult ObtenerVenta(int id)
        {
            var venta = VentaData.ObtenerVentaPorId(id);
            if (venta == null)
                return Content(HttpStatusCode.NotFound, "No se encontró la venta con el ID " + id);

            return Ok(venta);
        }

        // POST: api/venta/multiple
        [HttpPost]
        [Route("api/venta/multiple")]
        public IHttpActionResult RegistrarVentaMultiple([FromBody] VentaRequest request)
        {
            if (request == null || request.VacasDetalle == null || !request.VacasDetalle.Any())
            {
                return BadRequest("La información de la venta es inválida.");
            }

            try
            {
                int idVenta = VentaData.RegistrarVentaMultiple(request);
                return Ok(new { ID_Venta = idVenta });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("api/venta/{id}")]
        public IHttpActionResult EliminarVenta(int id)
        {
            bool exito = VentaData.EliminarVenta(id);
            if (exito)
                return Ok($"Venta {id} eliminada exitosamente.");
            else
                return BadRequest("No se pudo eliminar la venta.");
        }


    }
}
