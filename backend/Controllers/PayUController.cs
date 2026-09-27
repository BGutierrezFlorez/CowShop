using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;

namespace CowShop.Controllers
{
    [RoutePrefix("api/payu")]
    public class PayUController : ApiController
    {
        private const string PAYU_API_URL = "https://sandbox.api.payulatam.com/payments-api/4.0/service.cgi";
        private const string API_LOGIN = "tu_apiLogin_sandbox";
        private const string API_KEY = "tu_apiKey_sandbox";
        private const string MERCHANT_ID = "tu_merchantId_sandbox";
        private const string ACCOUNT_ID = "tu_accountId_sandbox";

        public class PayURequest
        {
            public int UserId { get; set; }
            public decimal AmountInCents { get; set; }
            public string CustomerEmail { get; set; }
            public string Plan { get; set; }
            public string Currency { get; set; } = "COP";
            public string CustomerName { get; set; }
        }

        [HttpPost]
        [Route("createOrder")]
        public async Task<IHttpActionResult> CreateOrder([FromBody] PayURequest req)
        {
            try
            {
                // 1️⃣ Generar referencia única
                string referenceCode = $"CowShop-{DateTime.Now:yyyyMMddHHmmss}";

                // 2️⃣ Calcular firma (SHA256)
                string signature = GenerarFirma(API_KEY, MERCHANT_ID, referenceCode, req.AmountInCents.ToString("F2"), req.Currency);

                // 3️⃣ Crear objeto JSON según documentación PayU
                var body = new
                {
                    language = "es",
                    command = "SUBMIT_TRANSACTION",
                    merchant = new
                    {
                        apiLogin = API_LOGIN,
                        apiKey = API_KEY
                    },
                    transaction = new
                    {
                        order = new
                        {
                            accountId = ACCOUNT_ID,
                            referenceCode = referenceCode,
                            description = $"Pago plan {req.Plan}",
                            language = "es",
                            signature = signature,
                            notifyUrl = "https://tu-sitio.com/confirmacion.aspx",
                            additionalValues = new
                            {
                                TX_VALUE = new { value = req.AmountInCents / 100, currency = req.Currency }
                            },
                            buyer = new
                            {
                                emailAddress = req.CustomerEmail,
                                fullName = req.CustomerName
                            }
                        },
                        payer = new
                        {
                            emailAddress = req.CustomerEmail,
                            fullName = req.CustomerName
                        },
                        type = "AUTHORIZATION_AND_CAPTURE",
                        paymentMethod = "VISA",
                        paymentCountry = "CO",
                        ipAddress = "127.0.0.1"
                    },
                    test = true
                };

                var json = JsonConvert.SerializeObject(body);
                var httpClient = new HttpClient();
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(PAYU_API_URL, content);
                var respJson = await response.Content.ReadAsStringAsync();

                return Ok(JsonConvert.DeserializeObject(respJson));
            }
            catch (Exception ex)
            {
                return BadRequest("Error creando la orden: " + ex.Message);
            }
        }

        private string GenerarFirma(string apiKey, string merchantId, string referenceCode, string amount, string currency)
        {
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                string cadena = $"{apiKey}~{merchantId}~{referenceCode}~{amount}~{currency}";
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(cadena));
                StringBuilder sb = new StringBuilder();
                foreach (var b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}