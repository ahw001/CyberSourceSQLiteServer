using System;
using System.Security.Cryptography;
using System.Numerics;

namespace CybsClass.WebApi.Service.Services.Utilities
{
    public class RandomNumberGeneratorUtility
    {
        private static string GenerateRandomNumber(int digits)
        {
            byte[] randomNumber = new byte[8];
            RandomNumberGenerator.Fill(randomNumber);
            // Convert the random bytes to a positive integer
            var num = new BigInteger(randomNumber);
            if (num < 0) num = -num;
            // Ensure the number is long enough
            while (num.ToString().Length < digits)
            {
                num *= 10;
            }
            // Take only the first number of digits
            return num.ToString()[..digits];
        }
    }
}
