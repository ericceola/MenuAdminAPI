using System;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MenuAdminAPI.Application.Services
{
    /// <summary>
    /// Serviço de envio de e-mails usando Azure SendGrid
    /// </summary>
    public interface IEmailService
    {
        Task<bool> SendNewUserCredentialsAsync(string email, string nome, string senha, string estabelecimento);
        Task<bool> SendPasswordResetAsync(string email, string novaSenha);
        Task<bool> SendWelcomeEmailAsync(string email, string nome);
    }

    public class EmailService : IEmailService
    {
        private readonly SendGridClient _sendGridClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;

            var apiKey = configuration["SendGrid:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("SendGrid API Key não configurada. Configure a variável de ambiente SendGrid:ApiKey");
            }

            _sendGridClient = new SendGridClient(apiKey);
            _fromEmail = configuration["SendGrid:FromEmail"] ?? "noreply@menuadminapi.com";
            _fromName = configuration["SendGrid:FromName"] ?? "Menu Admin";
        }

        /// <summary>
        /// Envia e-mail com credenciais de novo usuário
        /// </summary>
        public async Task<bool> SendNewUserCredentialsAsync(string email, string nome, string senha, string estabelecimento)
        {
            try
            {
                var subject = "Bem-vindo ao Menu Admin - Suas Credenciais de Acesso";
                var htmlContent = GetNewUserEmailTemplate(nome, email, senha, estabelecimento);

                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(_fromEmail, _fromName),
                    Subject = subject,
                    HtmlContent = htmlContent
                };

                msg.AddTo(new EmailAddress(email, nome));

                var response = await _sendGridClient.SendEmailAsync(msg);

                if (response.StatusCode == System.Net.HttpStatusCode.Accepted ||
                    response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    _logger.LogInformation($"E-mail de credenciais enviado com sucesso para {email}");
                    return true;
                }

                _logger.LogError($"Erro ao enviar e-mail para {email}. Status: {response.StatusCode}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção ao enviar e-mail para {email}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Envia e-mail de reset de senha
        /// </summary>
        public async Task<bool> SendPasswordResetAsync(string email, string novaSenha)
        {
            try
            {
                var subject = "Sua Senha foi Resetada - Menu Admin";
                var htmlContent = GetPasswordResetEmailTemplate(email, novaSenha);

                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(_fromEmail, _fromName),
                    Subject = subject,
                    HtmlContent = htmlContent
                };

                msg.AddTo(new EmailAddress(email));

                var response = await _sendGridClient.SendEmailAsync(msg);

                if (response.StatusCode == System.Net.HttpStatusCode.Accepted ||
                    response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    _logger.LogInformation($"E-mail de reset de senha enviado para {email}");
                    return true;
                }

                _logger.LogError($"Erro ao enviar e-mail de reset para {email}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção ao enviar e-mail de reset para {email}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Envia e-mail de boas-vindas
        /// </summary>
        public async Task<bool> SendWelcomeEmailAsync(string email, string nome)
        {
            try
            {
                var subject = "Bem-vindo ao Menu Admin";
                var htmlContent = GetWelcomeEmailTemplate(nome);

                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(_fromEmail, _fromName),
                    Subject = subject,
                    HtmlContent = htmlContent
                };

                msg.AddTo(new EmailAddress(email, nome));

                var response = await _sendGridClient.SendEmailAsync(msg);

                if (response.StatusCode == System.Net.HttpStatusCode.Accepted ||
                    response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    _logger.LogInformation($"E-mail de boas-vindas enviado para {email}");
                    return true;
                }

                _logger.LogError($"Erro ao enviar e-mail de boas-vindas para {email}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção ao enviar e-mail de boas-vindas para {email}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Template de e-mail para novo usuário com credenciais
        /// </summary>
        private string GetNewUserEmailTemplate(string nome, string email, string senha, string estabelecimento)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 20px; text-align: center; border-radius: 5px 5px 0 0; }}
        .content {{ background: #f9f9f9; padding: 20px; border: 1px solid #ddd; border-radius: 0 0 5px 5px; }}
        .credentials {{ background: white; padding: 15px; border-left: 4px solid #667eea; margin: 20px 0; }}
        .credentials p {{ margin: 10px 0; }}
        .label {{ font-weight: bold; color: #667eea; }}
        .footer {{ text-align: center; margin-top: 20px; font-size: 12px; color: #999; }}
        .button {{ display: inline-block; background: #667eea; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; margin-top: 20px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Bem-vindo ao Menu Admin! 🎉</h1>
        </div>
        
        <div class='content'>
            <p>Olá <strong>{nome}</strong>,</p>
            
            <p>Sua conta foi criada com sucesso no <strong>Menu Admin</strong> para o estabelecimento <strong>{estabelecimento}</strong>.</p>
            
            <p>Abaixo estão suas credenciais de acesso. Por favor, guarde-as com segurança:</p>
            
            <div class='credentials'>
                <p><span class='label'>E-mail:</span> {email}</p>
                <p><span class='label'>Senha:</span> {senha}</p>
            </div>
            
            <p><strong>Próximos passos:</strong></p>
            <ol>
                <li>Acesse o painel administrativo</li>
                <li>Faça login com suas credenciais</li>
                <li>Altere sua senha na primeira vez</li>
            </ol>
            
            <p>Se você tiver dúvidas ou precisar de suporte, entre em contato com o administrador do sistema.</p>
            
            <p>Atenciosamente,<br><strong>Equipe Menu Admin</strong></p>
        </div>
        
        <div class='footer'>
            <p>Este é um e-mail automático. Por favor, não responda a este e-mail.</p>
            <p>&copy; 2026 Menu Admin. Todos os direitos reservados.</p>
        </div>
    </div>
</body>
</html>";
        }

        /// <summary>
        /// Template de e-mail para reset de senha
        /// </summary>
        private string GetPasswordResetEmailTemplate(string email, string novaSenha)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%); color: white; padding: 20px; text-align: center; border-radius: 5px 5px 0 0; }}
        .content {{ background: #f9f9f9; padding: 20px; border: 1px solid #ddd; border-radius: 0 0 5px 5px; }}
        .credentials {{ background: white; padding: 15px; border-left: 4px solid #f5576c; margin: 20px 0; }}
        .credentials p {{ margin: 10px 0; }}
        .label {{ font-weight: bold; color: #f5576c; }}
        .footer {{ text-align: center; margin-top: 20px; font-size: 12px; color: #999; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Sua Senha foi Resetada</h1>
        </div>
        
        <div class='content'>
            <p>Sua senha foi resetada com sucesso.</p>
            
            <p>Sua nova senha temporária é:</p>
            
            <div class='credentials'>
                <p><span class='label'>E-mail:</span> {email}</p>
                <p><span class='label'>Nova Senha:</span> {novaSenha}</p>
            </div>
            
            <p><strong>Recomendações:</strong></p>
            <ul>
                <li>Faça login com a nova senha</li>
                <li>Altere a senha para uma que você possa memorizar</li>
                <li>Não compartilhe sua senha com ninguém</li>
            </ul>
            
            <p>Se você não solicitou este reset, entre em contato com o administrador do sistema imediatamente.</p>
        </div>
        
        <div class='footer'>
            <p>Este é um e-mail automático. Por favor, não responda a este e-mail.</p>
            <p>&copy; 2026 Menu Admin. Todos os direitos reservados.</p>
        </div>
    </div>
</body>
</html>";
        }

        /// <summary>
        /// Template de e-mail de boas-vindas
        /// </summary>
        private string GetWelcomeEmailTemplate(string nome)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 20px; text-align: center; border-radius: 5px 5px 0 0; }}
        .content {{ background: #f9f9f9; padding: 20px; border: 1px solid #ddd; border-radius: 0 0 5px 5px; }}
        .footer {{ text-align: center; margin-top: 20px; font-size: 12px; color: #999; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Bem-vindo, {nome}!</h1>
        </div>
        
        <div class='content'>
            <p>Obrigado por usar o <strong>Menu Admin</strong>!</p>
            
            <p>Estamos felizes em tê-lo como parte da nossa comunidade. Com o Menu Admin, você pode gerenciar facilmente seu menu, pedidos e clientes.</p>
            
            <p><strong>Recursos principais:</strong></p>
            <ul>
                <li>Gestão de Menu e Produtos</li>
                <li>Acompanhamento de Pedidos</li>
                <li>Gerenciamento de Clientes</li>
                <li>Relatórios e Análises</li>
            </ul>
            
            <p>Se tiver dúvidas, consulte nossa documentação ou entre em contato com o suporte.</p>
            
            <p>Bom trabalho!<br><strong>Equipe Menu Admin</strong></p>
        </div>
        
        <div class='footer'>
            <p>Este é um e-mail automático. Por favor, não responda a este e-mail.</p>
            <p>&copy; 2026 Menu Admin. Todos os direitos reservados.</p>
        </div>
    </div>
</body>
</html>";
        }
    }
}
