using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace CybsClass.WebApi.Service.Services.JWTProcessing
{
    public static class JWTDeCode
    {
        public static string DeCodeJWT(string jwtToken)
        {
            // Create a JwtSecurityTokenHandler
            var handler = new JwtSecurityTokenHandler();

            // Validate if the token is well-formed
            if (handler.CanReadToken(jwtToken))
            {
                // Read the token
                var jwtTokenObj = handler.ReadJwtToken(jwtToken);

                // Print out the header
                Console.WriteLine("Header:");
                foreach (var header in jwtTokenObj.Header)
                {
                    Console.WriteLine($"{header.Key}: {header.Value}");
                }

                // Print out the payload (claims)
                Console.WriteLine("\nPayload:");
                foreach (var claim in jwtTokenObj.Claims)
                {
                    Console.WriteLine($"{claim.Type}: {claim.Value}");
                }
                return jwtTokenObj.ToString();
            }
            else
            {
                Console.WriteLine("The token is not well-formed.");
                string error = "The token is not well-formed.";
                return error;
            }
        }
    }
}
