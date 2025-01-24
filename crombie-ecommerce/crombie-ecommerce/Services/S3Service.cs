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
            // get the bucket and keys from user secrets
            _amazonS3 = new AmazonS3Client(new BasicAWSCredentials(configuration["AccessKeyId"], configuration["SecretAcessKey"]));
            _bucketName = configuration["BucketName"] ?? "";
        }

        public async Task<GetObjectResponse> GetObjectFromBucketAsync(string fileName)
        {
            // sets up the obj request
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName
            };

            return await _amazonS3.GetObjectAsync(request);
        }

        public async Task<string> UploadFileAsync(
            Stream fileStream, string fileName, string contentType, string bucketFolder)
        {
            // sets up the obj request
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = $"{bucketFolder}/{fileName}",
                InputStream = fileStream,
                ContentType = contentType
            };

            var response = await _amazonS3.PutObjectAsync(request);

            // checks if the status is a 200
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return $"Successfully uploaded {fileName} to {_bucketName}.";

            }
            else
            {
                return $"Could not upload {fileName} to {_bucketName}.";
            }
        }

        public async Task<bool> DeleteObjectFromBucketAsync(string fileName)
        {
            try
            {
                // sets up the obj request
                var request = new GetObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileName
                };

                // Check if file exists first
                await _amazonS3.GetObjectAsync(request);

                var deleteRequest = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileName
                };
                var response = await _amazonS3.DeleteObjectAsync(deleteRequest);

                return true;
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
        }
    }
}
