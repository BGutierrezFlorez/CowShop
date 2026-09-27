// CowShop/App_Start/WebApiConfig.cs
using System.Web.Http;
using System.Web.Http.Cors;

namespace CowShop
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // 🔓 CORS (Cross-Origin Resource Sharing)
            // Permite llamadas desde cualquier origen mientras estás en desarrollo.
            // ⚠️ En producción, reemplaza "*" por tus dominios permitidos, ej. "https://cowshop.com"
            var cors = new EnableCorsAttribute(
                origins: "*",
                headers: "*",
                methods: "*"
            );
            config.EnableCors(cors);

            // 📦 Configuración de formato de respuesta (solo JSON)
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // 🛣️ Habilitar rutas por atributos
            // Permite usar [Route] y [RoutePrefix] directamente en los controladores
            config.MapHttpAttributeRoutes();

            // 📍 Ruta por convención (respaldo)
            // Si un controlador no usa atributos de ruta, seguirá este patrón:
            // Ejemplo → /api/usuario/5
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // ✅ Puedes agregar más configuración aquí si tu proyecto crece,
            // por ejemplo filtros globales o manejo de errores centralizado.
        }
    }
}
