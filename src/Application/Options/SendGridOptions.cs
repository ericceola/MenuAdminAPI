namespace MenuAdminAPI.Application.Options
{
    /// <summary>
    /// Opções de configuração para SendGrid
    /// </summary>
    public class SendGridOptions
    {
        /// <summary>
        /// Chave de API do SendGrid
        /// </summary>
        public string ApiKey { get; set; } = string.Empty;

        /// <summary>
        /// Email do remetente
        /// </summary>
        public string FromEmail { get; set; } = "noreply@menuadminapi.com";

        /// <summary>
        /// Nome do remetente
        /// </summary>
        public string FromName { get; set; } = "Menu Admin";
    }
}
