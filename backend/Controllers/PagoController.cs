using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json.Linq;

namespace CowShop.Controllers
{
    public class PagoController : ApiController
    {
        // Preflight CORS
        [HttpOptions]
        [Route("api/pago/create")]
        public IHttpActionResult OptionsCreate() => Ok();

    [HttpOptions]
    [Route("api/pago")]
    public IHttpActionResult OptionsPago() => Ok();

        [HttpOptions]
        [Route("api/pago/webhook")]
        public IHttpActionResult OptionsWebhook() => Ok();

        private readonly string WOMPI_API = "https://sandbox.wompi.co/v1/transactions";
        private readonly string WOMPI_PRIVATE_KEY = ConfigurationManager.AppSettings["WOMPI_PRIVATE_KEY"] ?? "PLACEHOLDER_WOMPI_PRIVATE_KEY";

        [HttpPost]
        [Route("api/pago/create")]
        public async Task<IHttpActionResult> CreatePago([FromBody] PagoRequest req)
        {
            if (req == null || req.AmountInCents <= 0)
                return BadRequest("Petición inválida");

            var reference = $"US{req.UserId}-{DateTime.UtcNow.Ticks}";

            var payload = new JObject
            {
                ["amount_in_cents"] = req.AmountInCents,
                ["currency"] = "COP",
                ["customer_email"] = req.CustomerEmail ?? "no-reply@cowshop.local",
                ["reference"] = reference,
                ["redirect_url"] = req.RedirectUrl ?? "http://localhost:5500/pago.html",
            };

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", WOMPI_PRIVATE_KEY);
                var content = new StringContent(payload.ToString(), Encoding.UTF8, "application/json");
                var resp = await client.PostAsync(WOMPI_API, content);
                var body = await resp.Content.ReadAsStringAsync();

                if (!resp.IsSuccessStatusCode)
                {
                    var c = Content(resp.StatusCode, body);
                    return c;
                }

                var parsed = JObject.Parse(body);

                // Intentar extraer referencia y transaction id
                var tx = parsed["data"]?["transaction"];
                var transactionId = tx?["id"]?.ToString();
                var referenceResp = tx?["reference"]?.ToString() ?? reference;

                // Guardar en tabla Pagos (pendiente)
                try
                {
                    var sql = $"INSERT INTO Pagos (Reference, TransactionId, UserId, AmountInCents, Currency, Estado, RawResponse) VALUES ('{referenceResp}', '{transactionId}', {req.UserId}, {req.AmountInCents}, 'COP', 'PENDING', '{parsed.ToString().Replace("'","''")}')";
                    var db = new ConexionBD();
                    db.EjecutarSentencia(sql, false);
                }
                catch (Exception ex)
                {
                    // No detener el flujo de creación en Wompi por fallo en DB
                    System.Diagnostics.Debug.WriteLine("No se pudo guardar Pago: " + ex.Message);
                }

                // Retornar la respuesta de Wompi al frontend (sandbox)
                var okRes = Ok(parsed);
                return okRes;
            }
        }

        // Convención POST api/pago (soporta llamadas sin route 'create')
        [HttpPost]
        [Route("api/pago")]
        public Task<IHttpActionResult> Post([FromBody] PagoRequest req)
        {
            return CreatePago(req);
        }

        // Webhook endpoint (simple) para recibir notificaciones de Wompi en pruebas
        [HttpPost]
        [Route("api/pago/webhook")]
        public IHttpActionResult Webhook([FromBody] JObject payload)
        {
            // En pruebas, sólo guardamos/mostramos la carga. En producción valida firma y procesa.
            try
            {
                var tx = payload["data"]?["transaction"];
                if (tx != null)
                {
                    var reference = tx["reference"]?.ToString();
                    var transactionId = tx["id"]?.ToString();
                    var status = tx["status"]?.ToString();

                    if (!string.IsNullOrEmpty(reference))
                    {
                        var sql = $"UPDATE Pagos SET TransactionId = '{transactionId}', Estado = '{status}', UpdatedAt = GETDATE(), RawResponse = '{payload.ToString().Replace("'","''")}' WHERE Reference = '{reference}'";
                        var db = new ConexionBD();
                        db.EjecutarSentencia(sql, false);
                    }
                }

                var okWebhook = Ok(new { ok = true, received = payload });
                return okWebhook;
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public class PagoRequest
        {
            public int UserId { get; set; }
            public int AmountInCents { get; set; }
            public string CustomerEmail { get; set; }
            public string Plan { get; set; }
            public string RedirectUrl { get; set; }
        }
    }
}
