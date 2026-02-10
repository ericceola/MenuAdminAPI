using MenuAdminAPI.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MenuAdminAPI.Presentation.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de usuários
    /// </summary>
    [ApiController]
    [Route("api/v1/usuarios")]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(IUnitOfWork unitOfWork, IEmailService emailService, ILogger<UsuariosController> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtém todos os usuários de um estabelecimento
        /// </summary>
        [HttpGet("estabelecimento/{estabelecimentoId}")]
        public async Task<IActionResult> ObterPorEstabelecimento(Guid estabelecimentoId)
        {
            try
            {
                if (estabelecimentoId == Guid.Empty)
                    return BadRequest("ID do estabelecimento inválido");

                var usuarios = await _unitOfWork.Usuarios.ObterPorEstabelecimentoAsync(estabelecimentoId);
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao obter usuários: {ex.Message}");
                return StatusCode(500, new { erro = "Erro ao obter usuários", detalhes = ex.Message });
            }
        }

        /// <summary>
        /// Cria um novo usuário e envia e-mail com credenciais
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarUsuarioRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Nome) || 
                    string.IsNullOrWhiteSpace(request.Email) || 
                    request.EstabelecimentoId == Guid.Empty)
                    return BadRequest("Nome, email e estabelecimento são obrigatórios");

                // Gerar senha temporária
                var senhaTemporaria = GerarSenhaTemporaria();
                var senhaHash = HashSenha(senhaTemporaria);

                // Criar usuário
                var usuario = new Usuario
                {
                    Id = Guid.NewGuid(),
                    Nome = request.Nome,
                    Email = request.Email,
                    SenhaHash = senhaHash,
                    Perfil = request.Perfil ?? "operador",
                    EstabelecimentoId = request.EstabelecimentoId,
                    Ativo = true,
                    DataCriacao = DateTime.UtcNow
                };

                // Obter nome do estabelecimento
                var estabelecimento = await _unitOfWork.Estabelecimentos.ObterPorIdAsync(request.EstabelecimentoId);
                var nomeEstabelecimento = estabelecimento?.Nome ?? "Menu Admin";

                // Salvar usuário
                await _unitOfWork.Usuarios.AdicionarAsync(usuario);
                await _unitOfWork.SaveChangesAsync();

                // Enviar e-mail com credenciais
                var emailEnviado = await _emailService.SendNewUserCredentialsAsync(
                    usuario.Email,
                    usuario.Nome,
                    senhaTemporaria,
                    nomeEstabelecimento
                );

                _logger.LogInformation($"Usuário criado: {usuario.Email}. E-mail enviado: {emailEnviado}");

                return CreatedAtAction(nameof(ObterPorEstabelecimento), 
                    new { estabelecimentoId = usuario.EstabelecimentoId }, 
                    new { usuario.Id, usuario.Nome, usuario.Email, emailEnviado });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao criar usuário: {ex.Message}");
                return StatusCode(500, new { erro = "Erro ao criar usuário", detalhes = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza um usuário existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarUsuarioRequest request)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("ID do usuário inválido");

                var usuario = await _unitOfWork.Usuarios.ObterPorIdAsync(id);
                if (usuario == null)
                    return NotFound("Usuário não encontrado");

                // Atualizar campos
                if (!string.IsNullOrWhiteSpace(request.Nome))
                    usuario.Nome = request.Nome;

                if (!string.IsNullOrWhiteSpace(request.Perfil))
                    usuario.Perfil = request.Perfil;

                if (request.Ativo.HasValue)
                    usuario.Ativo = request.Ativo.Value;

                await _unitOfWork.Usuarios.AtualizarAsync(usuario);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation($"Usuário atualizado: {usuario.Email}");
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao atualizar usuário: {ex.Message}");
                return StatusCode(500, new { erro = "Erro ao atualizar usuário", detalhes = ex.Message });
            }
        }

        /// <summary>
        /// Deleta um usuário
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("ID do usuário inválido");

                var usuario = await _unitOfWork.Usuarios.ObterPorIdAsync(id);
                if (usuario == null)
                    return NotFound("Usuário não encontrado");

                await _unitOfWork.Usuarios.RemoverAsync(usuario);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation($"Usuário deletado: {usuario.Email}");
                return Ok(new { mensagem = "Usuário deletado com sucesso" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao deletar usuário: {ex.Message}");
                return StatusCode(500, new { erro = "Erro ao deletar usuário", detalhes = ex.Message });
            }
        }

        /// <summary>
        /// Gera uma senha temporária aleatória
        /// </summary>
        private string GerarSenhaTemporaria()
        {
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var senha = new System.Text.StringBuilder();

            for (int i = 0; i < 8; i++)
            {
                senha.Append(caracteres[random.Next(caracteres.Length)]);
            }

            return senha.ToString();
        }

        /// <summary>
        /// Gera hash da senha
        /// </summary>
        private string HashSenha(string senha)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }

    /// <summary>
    /// Request para criar novo usuário
    /// </summary>
    public class CriarUsuarioRequest
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Perfil { get; set; }
        public Guid EstabelecimentoId { get; set; }
    }

    /// <summary>
    /// Request para atualizar usuário
    /// </summary>
    public class AtualizarUsuarioRequest
    {
        public string Nome { get; set; }
        public string Perfil { get; set; }
        public bool? Ativo { get; set; }
    }
}
