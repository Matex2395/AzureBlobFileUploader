﻿using Azure.Storage;
using Azure.Storage.Blobs;
using AzureBlobFileUploader.DTOs;
using AzureBlobFileUploader.Interfaces;

namespace AzureBlobFileUploader.Services
{
    public class FileService : IFileService
    {
        private readonly BlobContainerClient _filesContainer;

        public FileService(IConfiguration configuration)
        {
            var credential =
                new StorageSharedKeyCredential(
                    configuration["AzureBlob:StorageAccount"]!,
                    configuration["AzureBlob:StorageKey"]!);
            var blobUri =
                $"https://{configuration["AzureBlob:StorageAccount"]}.blob.core.windows.net";
            var blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
            _filesContainer = blobServiceClient.GetBlobContainerClient("files");
        }

        public async Task<List<BlobDTO>> ListAsync()
        {
            List<BlobDTO> files = new List<BlobDTO>();

            await foreach (var file in _filesContainer.GetBlobsAsync())
            {
                string uri = _filesContainer.Uri.ToString();
                var name = file.Name;
                var fullUri = $"{uri}/{name}";

                files.Add(new BlobDTO
                {
                    Uri = fullUri,
                    Name = name,
                    ContentType = file.Properties.ContentType
                });
            }
            return files;
        }

        public async Task<BlobResponseDTO> UploadAsync(IFormFile blob)
        {
            BlobResponseDTO response = new();
            BlobClient client = _filesContainer.GetBlobClient(blob.FileName);

            await using (Stream? data = blob.OpenReadStream())
            {
                await client.UploadAsync(data);
            }

            response.Status = $"Archivo: {blob.FileName} Subido con Éxito";
            response.Error = false;
            response.Blob.Uri = client.Uri.AbsoluteUri;
            response.Blob.Name = client.Name;

            return response;
        }

        public async Task<BlobDTO?> DownloadAsync(string blobFilename)
        {
            BlobClient file = _filesContainer.GetBlobClient(blobFilename);

            if (await file.ExistsAsync())
            {
                var data = await file.OpenReadAsync();
                Stream blobContent = data;

                var content = await file.DownloadContentAsync();

                string name = blobFilename;
                string contentType = content.Value.Details.ContentType;
                return new BlobDTO
                {
                    Content = blobContent,
                    Name = name,
                    ContentType = contentType
                };
            }
            return null;
        }

        public async Task<BlobResponseDTO> DeleteAsync(string blobFilename)
        {
            BlobClient file = _filesContainer.GetBlobClient(blobFilename);

            await file.DeleteAsync();

            return new BlobResponseDTO
            {
                Error = false,
                Status = $"Archivo: {blobFilename} Eliminado con Éxito"
            };
        }
    }
}
