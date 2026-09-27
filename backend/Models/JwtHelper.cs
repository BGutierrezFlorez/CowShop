using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using System.Collections.Generic;

namespace CowShop.Helpers
{
    public class JwtHelper
    {
        private static string SecretKey = "TuClaveSuperSecreta123456";

        public static string GenerarToken(int idUsuario, string tipoUsuario)
        {
            var payload = new Dictionary<string, object>
            {
                { "id", idUsuario },
                { "rol", tipoUsuario },
                { "exp", DateTimeOffset.UtcNow.AddHours(2).ToUnixTimeSeconds() }
            };

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // Algoritmo de encriptación
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            return encoder.Encode(payload, SecretKey);
        }
    }
}
