using crombie_ecommerce.BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace crombie_ecommerce.Controllers
{
    [ApiController]
    public class S3sController : ControllerBase
    {
        private readonly s3Service _s3Service;
        private readonly string _bucketName;

        public S3sController(s3Service s3Service, IConfiguration configuration)
        {
            _s3Service = s3Service;
            _bucketName = configuration["BucketName"] ?? "";
        }

        [HttpGet("/file/{fileName}")]
        public async Task<IActionResult> GetObject(string fileName)
        {
            try
            {

                // needed to make files with reserved characters work, image.jpg works, but products/image.png does not, because the / gets changed to %2F. 
                var objectName = Uri.UnescapeDataString(fileName);

                // checks if its empty
                if (string.IsNullOrEmpty(objectName))
                {
                    return BadRequest("File name is required");
                }


                var response = await _s3Service.GetObjectFromBucketAsync(objectName);

                // checks if the response status is anything other than a http 200
                if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                {
                    return BadRequest("Error downloading file from S3");
                }

                // file needs a file stream to read it, and the content type
                return File(response.ResponseStream, response.Headers.ContentType);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error downloading file: {ex.Message}");
            }
        }

        [HttpGet("download")]
        public async Task<IActionResult> DownloadObject(string fileName)
        {
            try
            {
                // checks if its empty or null
                if (string.IsNullOrEmpty(fileName))
                {
                    return BadRequest("File name is required");
                }

                using var response = await _s3Service.GetObjectFromBucketAsync(fileName);

                // checks if the response status is anything other than a http 200
                if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                {
                    return BadRequest("Error downloading file from S3");
                }

                // sets the headers for the file
                Response.Headers.Append("Content-Disposition", $"attachment; filename={fileName}");
                Response.Headers.Append("Content-Type", response.Headers.ContentType);

                await response.ResponseStream.CopyToAsync(Response.Body);
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error downloading file: {ex.Message}");
            }
        }


        [HttpPost("upload")]
        public async Task<ActionResult<string>> PutObject(IFormFile fileObject, string folderName)
        {
            try
            {
                // Checks for null and empty files
                if (fileObject == null || fileObject.Length == 0)
                    return BadRequest("File is missing");

                // Checks for any invalid characters in the fileName
                if (fileObject.FileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                    return BadRequest("File name contains invalid characters");

                using var stream = fileObject.OpenReadStream();

                // checks if it is a readable stream (e.g: think of corrupted or locked files)
                if (!stream.CanRead)
                    return BadRequest("File stream is not readable");

                // Checks for a file size of 1MB. Length on files is based on bytes, and each multiplier is separated by 1024 units. Byte -> KiloByte -> MegaByte
                if (stream.Length > 1 * 1024 * 1024)
                    return BadRequest("File size is too large, maximum up to 1MB");

                // Checks for a content type that equals jpeg/jpg or png
                if (!fileObject.ContentType.Equals("image/jpeg") && !fileObject.ContentType.Equals("image/png"))
                    return BadRequest($"File type is not supported, only jpg and png are allowed. Your file type: {fileObject.ContentType}");

                var fileName = $"{Guid.NewGuid()}-{fileObject.FileName}";

                var url = await _s3Service.UploadFileAsync(stream, fileName, fileObject.ContentType, folderName);

                return Ok($"{url}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteObject(string fileName)
        {
            try
            {
                // checks for null or empty
                if (string.IsNullOrEmpty(fileName))
                {
                    return BadRequest("File name is required");
                }

                var result = await _s3Service.DeleteObjectFromBucketAsync(fileName);

                // if the deletion result is true, 200, else 400
                if (result)
                {
                    return Ok($"Successfully deleted {fileName} from {_bucketName}.");
                }
                else
                {
                    return BadRequest($"File {fileName} not found in {_bucketName}.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting file: {ex.Message}");
            }
        }
    }
}
