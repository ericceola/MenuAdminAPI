using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MenuAdminAPI.Application.Services;

public interface IBlobStorageService
{
    Task<string> UploadImageAsync(Stream imageStream, string fileName, string contentType);
    Task<bool> DeleteImageAsync(string blobUrl);
}

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;
    private readonly ILogger<BlobStorageService> _logger;

    public BlobStorageService(IConfiguration configuration, ILogger<BlobStorageService> logger)
    {
        var connectionString = configuration.GetConnectionString("AzureBlobStorage");
        _containerName = configuration["AzureBlobStorage:ContainerName"] ?? "produto-imagens";
        _blobServiceClient = new BlobServiceClient(connectionString);
        _logger = logger;
    }

    public async Task<string> UploadImageAsync(Stream imageStream, string fileName, string contentType)
    {
        try
        {
            // Criar container se não existir
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            // Gerar nome único para o arquivo
            var extension = Path.GetExtension(fileName);
            var blobName = $"{Guid.NewGuid()}{extension}";

            // Fazer upload
            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(imageStream, new BlobHttpHeaders { ContentType = contentType });

            _logger.LogInformation("Imagem {FileName} enviada com sucesso como {BlobName}", fileName, blobName);

            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer upload da imagem {FileName}", fileName);
            throw;
        }
    }

    public async Task<bool> DeleteImageAsync(string blobUrl)
    {
        try
        {
            var uri = new Uri(blobUrl);
            var blobName = Path.GetFileName(uri.LocalPath);

            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            var result = await blobClient.DeleteIfExistsAsync();
            _logger.LogInformation("Imagem {BlobName} deletada: {Result}", blobName, result.Value);

            return result.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar imagem {BlobUrl}", blobUrl);
            return false;
        }
    }
}
