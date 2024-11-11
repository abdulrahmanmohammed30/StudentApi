using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.VisualBasic;

namespace StudentApi.Controllers
{
    [Route("api/ImageUpload")]
    [ApiController]
    public class ImageUploadController : ControllerBase
    {
        [HttpPost("Upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadImage(IFormFile imageFile)
        {
            // Practice What you learnt - Understand then practice 

            // I need to make sure the imageFile is not not or empty 
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest("Image can be null or empty");
            }

            // Logically, What do I need to do? 
            // Find a diretory to place the image in done 
            // If the directory doesn't exist create one first 
            var imageExtension = Path.GetExtension(imageFile.FileName);
            var directory = "F:\\Main Desktop\\APS.NET Projects\\StudentApi\\Uploads";
            var filePath = Path.Combine(directory, imageFile.FileName.Replace(imageExtension, "") +'-' +Guid.NewGuid().ToString().ToLower().Substring(0, 8)  + imageExtension); ;

            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }

            // Now How to upload the image file 
            // convert it to bytes and upload it? 
            // no a better way is to use the custom copytoasync 
            // that's already created for that
            // also learn how it works 
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }


            return Ok(new { filePath });
        }

        [HttpPost("GetImage/{FileName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetImage(string FileName) {
            
            if (string.IsNullOrEmpty(FileName))
            {
                return BadRequest("Image URL can not be null or empty");

            }
            try
            {
                var directory = "F:\\Main Desktop\\APS.NET Projects\\StudentApi\\Uploads";

                var filePath = Path.Combine(directory, FileName);

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound("The specified image file does not exist");
                }

                var image = System.IO.File.OpenRead(filePath);
                var mimeType = GetMimeType(filePath);
                return File(image, mimeType);

            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred during processing the reques");
            }
            
           
        }

        private string GetMimeType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };
        }
    }
}
