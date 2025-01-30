using AzureBlobFileUploader.DTOs;

namespace AzureBlobFileUploader.Interfaces
{
    public interface IFileService
    {
        Task<List<BlobDTO>> ListAsync();
        Task<BlobResponseDTO> UploadAsync(IFormFile blob);
        Task<BlobDTO?> DownloadAsync(string blobFilename);
        Task<BlobResponseDTO> DeleteAsync(string blobFilename);

    }
}
