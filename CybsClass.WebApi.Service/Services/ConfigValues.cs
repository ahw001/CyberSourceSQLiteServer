using Microsoft.Extensions.Configuration;
using CybsClass.Cybersource.Authentication;

namespace CybsClass.WebApi.Service.Services
{
    public class ConfigValues
    {
        private readonly IConfiguration _configuration;

        public ConfigValues(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetKeyPass()
        {
            string? keyPass = _configuration["AuthCredentialFile:KeyPass"];
            if (keyPass is not null)
            {
                return keyPass;
            }
            else
            {
                keyPass = "false";
                return keyPass;
            }
        }

        public string GetIsPortfolioID()
        {
            string? isPortfolioID = _configuration["AuthCredentialFile:IsPortfolioCredential"];
            if (isPortfolioID is not null)
            {
                return isPortfolioID;
            }
            else
            {
                isPortfolioID = "false";
                return isPortfolioID;
            }
        }


        public string GetMerchantID()
        {
            string? merchantID = _configuration["AuthCredentialFile:MerchantID"];
            if (merchantID is not null)
            {
                return merchantID;
            }
            else
            {
                merchantID = "";
                return merchantID;
            }
        }
        public string GetP12Value()
        {
            string? p12JwtCredential = _configuration["AuthCredentialFile:RestP12JwtCredential"];
            if (p12JwtCredential is not null) 
            {
                return p12JwtCredential; 
            }
            else 
            { 
                p12JwtCredential = "";
                return p12JwtCredential;
            }
            
        }

        //Initialize(string p12JwtCredential, string isPortfolioCredential, string merchantID, string keyPass,
        //string keyId, string sharedSecret, string webServiceUrl, string baseUrlAddress)
        public string GetKeyID()
        {
            string? keyId = _configuration["AuthSecretKey:KeyId"];
            if (keyId is not null)
            {
                return keyId;
            }
            else
            {
                keyId = "false";
                return keyId;
            }
        }

        public string GetSharedSecret()
        {
            string? sharedSecret = _configuration["AuthSecretKey:SharedSecret"];
            if (sharedSecret is not null)
            {
                return sharedSecret;
            }
            else
            {
                sharedSecret = "false";
                return sharedSecret;
            }
        }

        public string GetWebServiceUrl()
        {
            string? webServiceUrl = _configuration["WebServiceUrl"];
            if (webServiceUrl is not null)
            {
                return webServiceUrl;
            }
            else
            {
                webServiceUrl = "false";
                return webServiceUrl;
            }
        }

        public string GetBaseUrlAddress()
        {
            string? baseUrlAddress = _configuration["BaseUrlAddress"];
            if (baseUrlAddress is not null)
            {
                return baseUrlAddress;
            }
            else
            {
                baseUrlAddress = "false";
                return baseUrlAddress;
            }
        }

        public string GetBasePosUrlAddress()
        {
            string? basePosUrlAddress = _configuration["BasePosUrlAddress"];
            if (basePosUrlAddress is not null)
            {
                return basePosUrlAddress;
            }
            else
            {
                basePosUrlAddress = "false";
                return basePosUrlAddress;
            }
        }

        public string GetAcceptanceMid()
        {
            string? accepanceMid = _configuration["AcceptanceDeviceInfo:AcceptanceMerchantId"];
            if (accepanceMid is not null)
            {
                return accepanceMid;
            }
            else
            {
                accepanceMid = "false";
                return accepanceMid;
            }
        }

        public string GetAcceptanceSecret()
        {
            string? accepanceSecret = _configuration["AcceptanceDeviceInfo:AcceptanceSecret"];
            if (accepanceSecret is not null)
            {
                return accepanceSecret;
            }
            else
            {
                accepanceSecret = "false";
                return accepanceSecret;
            }
        }

        public string GetAcceptanceDeviceSerialNumber()
        {
            string? accepanceSerialNumber = _configuration["AcceptanceDeviceInfo:AcceptanceDeviceSerialNumber"];
            if (accepanceSerialNumber is not null)
            {
                return accepanceSerialNumber;
            }
            else
            {
                accepanceSerialNumber = "none";
                return accepanceSerialNumber;
            }
        }

    }
}
