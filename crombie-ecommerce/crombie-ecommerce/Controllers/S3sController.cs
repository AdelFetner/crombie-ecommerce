using Amazon.S3;
using crombie_ecommerce.Services;
using Microsoft.AspNetCore.Mvc;

namespace crombie_ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class S3sController : ControllerBase
    {
        private readonly s3Service _s3Service;

        public S3sController(s3Service s3Service)
        {
            _s3Service = s3Service;
        }

        [HttpGet("download")]
        public async Task<IActionResult> DownloadObject(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    return BadRequest("File name is required");
                }

                using var response = await _s3Service.DownloadObjectFromBucketAsync(fileName);
                if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                {
                    return BadRequest("Error downloading file from S3");
                }

                Response.Headers.Add("Content-Disposition", $"attachment; filename={fileName}");
                Response.Headers.Add("Content-Type", response.Headers.ContentType);

                await response.ResponseStream.CopyToAsync(Response.Body);
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error downloading file: {ex.Message}");
            }
        }


        [HttpPost("upload")]
        public async Task<ActionResult<string>> PutObject(IFormFile fileObject,string folderName)
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

                var fileName = $"{folderName}/{Guid.NewGuid()}-{fileObject.FileName}";

                var url = await _s3Service.UploadFileAsync(stream, fileName, fileObject.ContentType);

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
                if (string.IsNullOrEmpty(fileName))
                {
                    return BadRequest("File name is required");
                }

                var result = await _s3Service.DeleteObjectFromBucketAsync(fileName);
                if (result.Contains("Successfully"))
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting file: {ex.Message}");
            }
        }
    }
}
