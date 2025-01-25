using Amazon.S3.Model;

namespace crombie_ecommerce.Services.Interfaces
{
    public interface Is3Service
    {
        Task<bool> DeleteObjectFromBucketAsync(string fileName);
        Task<GetObjectResponse> GetObjectFromBucketAsync(string fileName);
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, string bucketFolder);
    }
}