using MenuAdminAPI.Application.Services;
using MenuAdminAPI.Domain.Entities;
using MenuAdminAPI.Infrastructure.Repositories;
using MenuAdminAPI.Presentation.Controllers.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MenuAdminAPI.Presentation.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UploadController : BaseController
{
    private readonly IBlobStorageService _blobStorageService;
    private readonly IImagemProdutoRepository _imagemProdutoRepository;
    private readonly ILogger<UploadController> _logger;
    private const long MaxFileSize = 10 * 1024 * 1024; // 10MB

    public UploadController(
        IBlobStorageService blobStorageService,
        IConfiguration configuration,
        ILogger<UploadController> logger)
    {
        _blobStorageService = blobStorageService;
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        _imagemProdutoRepository = new ImagemProdutoRepository(connectionString);
        _logger = logger;
    }

    [HttpPost("produtos/{produtoId:guid}/imagem")]
    [RequestSizeLimit(10_000_000)] // 10MB
    [ProducesResponseType(typeof(UploadImageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UploadProdutoImagem(Guid produtoId, IFormFile file, CancellationToken ct)
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

            // Fazer upload (original + thumbnail)
            var uploadResult = await _blobStorageService.UploadProductImageAsync(produtoId, file, ct);

            // Salvar registro no banco
            var imagemProduto = new ImagemProduto
            {
                Id = Guid.NewGuid(),
                ProdutoId = produtoId,
                BlobOriginal = uploadResult.OriginalBlobName,
                BlobThumb = uploadResult.ThumbBlobName,
                ContentType = file.ContentType,
                CreatedAt = DateTime.UtcNow
            };

            await _imagemProdutoRepository.CriarAsync(imagemProduto);

            _logger.LogInformation("Upload de imagem realizado com sucesso para produto {ProdutoId}: {OriginalBlob}", 
                produtoId, uploadResult.OriginalBlobName);

            return Ok(new UploadImageResponse
            {
                Id = imagemProduto.Id,
                OriginalBlob = uploadResult.OriginalBlobName,
                ThumbBlob = uploadResult.ThumbBlobName,
                ContentType = file.ContentType
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validação de upload falhou para produto {ProdutoId}", produtoId);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer upload de imagem para produto {ProdutoId}", produtoId);
            return InternalErrorResponse();
        }
    }

    [HttpGet("sas")]
    [ProducesResponseType(typeof(SasUrlResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetSasUrl([FromQuery] string blobName, [FromQuery] int minutes = 30)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(blobName))
                return BadRequest(new { message = "blobName é obrigatório" });

            var ttl = TimeSpan.FromMinutes(Math.Clamp(minutes, 1, 180));
            var sas = _blobStorageService.GenerateReadSas(blobName, ttl);

            return Ok(new SasUrlResponse
            {
                Url = sas.Url,
                ExpiresAt = sas.ExpiresAt
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar SAS URL para blob {BlobName}", blobName);
            return InternalErrorResponse();
        }
    }
}

public class UploadImageResponse
{
    public Guid Id { get; set; }
    public string OriginalBlob { get; set; } = string.Empty;
    public string ThumbBlob { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
}

public class SasUrlResponse
{
    public string Url { get; set; } = string.Empty;
    public DateTimeOffset ExpiresAt { get; set; }
}
