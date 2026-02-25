using MenuAdminAPI.Application.Services;
using MenuAdminAPI.Presentation.Controllers.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MenuAdminAPI.Presentation.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UploadController : BaseController
{
    private readonly IBlobStorageService _blobStorageService;
    private readonly ILogger<UploadController> _logger;
    private const long MaxFileSize = 5 * 1024 * 1024; // 5MB
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

    public UploadController(IBlobStorageService blobStorageService, ILogger<UploadController> logger)
    {
        _blobStorageService = blobStorageService;
        _logger = logger;
    }

    [HttpPost("Imagem")]
    [ProducesResponseType(typeof(UploadImageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UploadImagem(IFormFile file)
    {
        try
        {
            // Validar se arquivo foi enviado
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "Nenhum arquivo foi enviado" });
            }

            // Validar tamanho
            if (file.Length > MaxFileSize)
            {
                return BadRequest(new { message = $"Arquivo muito grande. Tamanho máximo: {MaxFileSize / 1024 / 1024}MB" });
            }

            // Validar extensão
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
            {
                return BadRequest(new { message = $"Formato não permitido. Formatos aceitos: {string.Join(", ", AllowedExtensions)}" });
            }

            // Fazer upload
            using var stream = file.OpenReadStream();
            var imageUrl = await _blobStorageService.UploadImageAsync(stream, file.FileName, file.ContentType);

            _logger.LogInformation("Upload de imagem realizado com sucesso: {ImageUrl}", imageUrl);

            return Ok(new UploadImageResponse
            {
                Url = imageUrl,
                FileName = file.FileName,
                Size = file.Length
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer upload de imagem");
            return InternalErrorResponse();
        }
    }
}

public class UploadImageResponse
{
    public string Url { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public long Size { get; set; }
}
