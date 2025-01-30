namespace AzureBlobFileUploader.DTOs
{
    public class BlobResponseDTO
    {
        public BlobResponseDTO()
        {
            Blob = new BlobDTO();
        }
        public string? Status { get; set; }
        public bool Error { get; set; }
        public BlobDTO Blob { get; set; }
    }
}
