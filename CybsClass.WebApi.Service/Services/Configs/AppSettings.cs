namespace CybsClass.WebApi.Service.Services.Configs
{
    public class AppSettings
    {
        public LoggingSettings Logging { get; set; } = new();
        public string? AllowedHosts { get; set; }
        public string? BaseUrlAddress { get; set; }
        public string? BasePosUrlAddress { get; set; }
        public AuthSecretKeySettings AuthSecretKey { get; set; } = new();
        public AcceptanceDeviceInfoSettings AcceptanceDeviceInfo { get; set; } = new();
        public AuthCredentialFileSettings AuthCredentialFile { get; set; } = new();
        public CorsSettings Cors { get; set; } = new();
        public MleSettings MleSettings { get; set; } = new();
    }

    public class LoggingSettings
    {
        public LogLevelSettings LogLevel { get; set; } = new();
    }

    public class LogLevelSettings
    {
        public string? Default { get; set; }
        public string? MicrosoftAspNetCore { get; set; }
    }

    public class AuthSecretKeySettings
    {
        public string? KeyId { get; set; }
        public string? SharedSecret { get; set; }
    }

    public class AcceptanceDeviceInfoSettings
    {
        public string? AcceptanceMerchantId { get; set; }
        public string? AcceptanceSecret { get; set; }
        public string? AcceptanceDeviceSerialNumber { get; set; }
    }

    public class AuthCredentialFileSettings
    {
        public string? RestP12JwtCredential { get; set; }
        public string? IsPortfolioCredential { get; set; }
        public string? MerchantID { get; set; }
        public string? KeyPass { get; set; }
    }

    public class CorsSettings
    {
        public List<string> AllowedOrigins { get; set; } = new();
    }

    public class MleSettings
    {
        public string? SjcCertificatePath { get; set; }
        public string? ResponseMleKeyPath { get; set; }
        public string? ResponseMleKeyPass { get; set; }
        public string? ResponseMleKid { get; set; }
        public string? LegacyMlePrivateKeyPath { get; set; }
        public string? LegacyMleKid { get; set; }
    }

    public class BoardingSettings
    {
        public string? BoardingPackageId { get; set; }
        public string? CardProcessingTemplateId { get; set; }
        public string? VirtualTerminalTemplateId { get; set; }
        public string? TokenManagementTemplateId { get; set; }
        public string? PayerAuthenticationTemplateId { get; set; }
    }
}
