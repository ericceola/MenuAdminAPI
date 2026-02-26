using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace MenuAdminAPI.Application.Services;

public record UploadImageResult(
    string OriginalBlobName,
    string ThumbBlobName
);

public record SasUrlResult(
    string Url,
    DateTimeOffset ExpiresAt
);

public interface IBlobStorageService
{
    Task<UploadImageResult> UploadProductImageAsync(Guid produtoId, IFormFile file, CancellationToken ct);
    Task<bool> DeleteImageAsync(string blobName);
    SasUrlResult GenerateReadSas(string blobName, TimeSpan ttl);
}

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _containerClient;
    private readonly string _cdnBaseUrl;
    private readonly ILogger<BlobStorageService> _logger;

    public BlobStorageService(IConfiguration configuration, ILogger<BlobStorageService> logger)
    {
        var connectionString = configuration.GetConnectionString("AzureBlobStorage");
        var containerName = configuration["AzureBlobStorage:ContainerName"] ?? "assets";
        _cdnBaseUrl = configuration["AzureBlobStorage:CdnBaseUrl"] ?? "";
        
        var blobServiceClient = new BlobServiceClient(connectionString);
        _containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        _logger = logger;
    }

    public async Task<UploadImageResult> UploadProductImageAsync(Guid produtoId, IFormFile file, CancellationToken ct)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Arquivo inválido.");

        // Aceitar apenas imagens
        var allowed = new[] { "image/jpeg", "image/png", "image/webp" };
        if (!allowed.Contains(file.ContentType))
            throw new ArgumentException("Formato não permitido. Use JPG/PNG/WEBP.");

        // Criar container se não existir
        await _containerClient.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);

        // Determinar extensão
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(ext))
        {
            ext = file.ContentType switch
            {
                "image/jpeg" => ".jpg",
                "image/png" => ".png",
                "image/webp" => ".webp",
                _ => ".img"
            };
        }

        var guid = Guid.NewGuid().ToString("N");
        var originalBlobName = $"produtos/{produtoId}/original/{guid}{ext}";
        var thumbBlobName = $"produtos/{produtoId}/thumb/{guid}{ext}";

        // 1) Upload original (stream direto)
        var originalBlob = _containerClient.GetBlobClient(originalBlobName);
        using (var stream = file.OpenReadStream())
        {
            await originalBlob.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType }, cancellationToken: ct);
        }

        _logger.LogInformation("Imagem original {OriginalBlobName} enviada com sucesso", originalBlobName);

        // 2) Gerar thumbnail (ImageSharp)
        using var input = file.OpenReadStream();
        using var image = await Image.LoadAsync(input, ct);

        // Thumb: largura máxima 400px mantendo proporção
        var maxWidth = 400;
        if (image.Width > maxWidth)
        {
            var newHeight = (int)Math.Round(image.Height * (maxWidth / (double)image.Width));
            image.Mutate(x => x.Resize(maxWidth, newHeight));
        }

        // Encode thumb no mesmo formato do upload
        IImageEncoder encoder = GetEncoder(file.ContentType);

        await using var thumbMs = new MemoryStream();
        await image.SaveAsync(thumbMs, encoder, ct);
        thumbMs.Position = 0;

        var thumbBlob = _containerClient.GetBlobClient(thumbBlobName);
        await thumbBlob.UploadAsync(thumbMs, new BlobHttpHeaders { ContentType = file.ContentType }, cancellationToken: ct);

        _logger.LogInformation("Thumbnail {ThumbBlobName} gerado e enviado com sucesso", thumbBlobName);

        return new UploadImageResult(originalBlobName, thumbBlobName);
    }

    public async Task<bool> DeleteImageAsync(string blobName)
    {
        try
        {
            var blobClient = _containerClient.GetBlobClient(blobName);
            var result = await blobClient.DeleteIfExistsAsync();
            _logger.LogInformation("Imagem {BlobName} deletada: {Result}", blobName, result.Value);
            return result.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar imagem {BlobName}", blobName);
            return false;
        }
    }

    public SasUrlResult GenerateReadSas(string blobName, TimeSpan ttl)
    {
        var blob = _containerClient.GetBlobClient(blobName);
        var expires = DateTimeOffset.UtcNow.Add(ttl);

        var sas = blob.GenerateSasUri(BlobSasPermissions.Read, expires);

        // Se tiver CDN, troca o host base (mantém query SAS)
        if (!string.IsNullOrWhiteSpace(_cdnBaseUrl))
        {
            var cdnBase = _cdnBaseUrl.TrimEnd('/');
            var path = sas.AbsolutePath;        // /container/...
            var query = sas.Query;              // ?sv=...
            var url = $"{cdnBase}{path}{query}";
            return new SasUrlResult(url, expires);
        }

        return new SasUrlResult(sas.ToString(), expires);
    }

    private static IImageEncoder GetEncoder(string contentType) =>
        contentType switch
        {
            "image/jpeg" => new JpegEncoder { Quality = 85 },
            "image/png" => new PngEncoder(),
            "image/webp" => new WebpEncoder { Quality = 80 },
            _ => new JpegEncoder { Quality = 85 }
        };
}
