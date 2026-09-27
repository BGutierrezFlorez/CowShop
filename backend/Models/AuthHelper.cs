using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using System.Collections.Generic;

namespace CowShop.Helpers
{
    public class AuthHelper
    {
        private static string SecretKey = "TuClaveSuperSecreta123456";

        public static IDictionary<string, object> ValidarToken(string token)
        {
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

                var payload = decoder.DecodeToObject<IDictionary<string, object>>(token, SecretKey, verify: true);
                return payload;
            }
            catch (TokenExpiredException)
            {
                return null;
            }
            catch (SignatureVerificationException)
            {
                return null;
            }
        }
    }
}
