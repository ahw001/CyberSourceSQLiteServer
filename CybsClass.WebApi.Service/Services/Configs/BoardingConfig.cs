namespace CybsClass.WebApi.Service.Services.Configs
{
    public static class BoardingConfig
    {
        public static string BoardingPackageId { get; private set; } = string.Empty;
        public static string CardProcessingTemplateId { get; private set; } = string.Empty;
        public static string VirtualTerminalTemplateId { get; private set; } = string.Empty;
        public static string TokenManagementTemplateId { get; private set; } = string.Empty;
        public static string PayerAuthenticationTemplateId { get; private set; } = string.Empty;

        public static void Initialize(
            string boardingPackageId,
            string cardProcessingTemplateId,
            string virtualTerminalTemplateId,
            string tokenManagementTemplateId,
            string payerAuthenticationTemplateId)
        {
            BoardingPackageId = boardingPackageId;
            CardProcessingTemplateId = cardProcessingTemplateId;
            VirtualTerminalTemplateId = virtualTerminalTemplateId;
            TokenManagementTemplateId = tokenManagementTemplateId;
            PayerAuthenticationTemplateId = payerAuthenticationTemplateId;
        }
    }
}
