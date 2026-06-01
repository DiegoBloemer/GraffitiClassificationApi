namespace GraffitiClassificationApi.Api.Services;

/// <summary>
/// Interface para serviços de armazenamento de arquivos.
/// </summary>
public interface IStorageService
{
    /// <summary>
    /// Faz upload de um arquivo para o storage.
    /// </summary>
    /// <param name="file">Arquivo a ser enviado</param>
    /// <param name="folder">Pasta de destino (ex: "occurrences")</param>
    /// <returns>URL pública do arquivo</returns>
    Task<string> UploadFileAsync(IFormFile file, string folder);

    /// <summary>
    /// Exclui um arquivo do storage.
    /// </summary>
    /// <param name="fileUrl">URL do arquivo a ser excluído</param>
    Task DeleteFileAsync(string fileUrl);

    /// <summary>
    /// Verifica se o bucket existe e cria se necessário.
    /// </summary>
    Task EnsureBucketExistsAsync();
}
