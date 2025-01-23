using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace crombie_ecommerce.Services
{
    public class s3Service
    {
        private readonly IAmazonS3 _amazonS3;
        private readonly string _bucketName;

        public s3Service(IConfiguration configuration)
        {
            _amazonS3 = new AmazonS3Client(new BasicAWSCredentials(configuration["AccessKeyId"], configuration["SecretAcessKey"]));
            _bucketName = configuration["BucketName"] ?? "";
        }

        public async Task<string> UploadFileAsync(
            Stream fileStream, string fileName, string contentType)
        {
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName,
                InputStream = fileStream,
                ContentType = contentType
            };

            var response = await _amazonS3.PutObjectAsync(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return $"Successfully uploaded {fileName} to {_bucketName}.";
                
            }
            else
            {
                return $"Could not upload {fileName} to {_bucketName}.";
            }
        }

        public async Task<GetObjectResponse> DownloadObjectFromBucketAsync(string fileName)
        {
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName
            };
            
            return await _amazonS3.GetObjectAsync(request);
        }

        public async Task<string> DeleteObjectFromBucketAsync(string fileName)
        {
            try
            {
                var request = new GetObjectMetadataRequest
                {
                    BucketName = _bucketName,
                    Key = fileName
                };

                // Check if file exists first
                await _amazonS3.GetObjectMetadataAsync(request);

                var deleteRequest = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileName
                };
                var response = await _amazonS3.DeleteObjectAsync(deleteRequest);

                return $"Successfully deleted {fileName} from {_bucketName}.";
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return $"File {fileName} not found in {_bucketName}.";
            }
        }
    }
}
