using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestSystem.Core.Dtos;
using TestSystem.Extensions;

namespace TestSystem.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly ILogger<FileController> _logger;
    private readonly IWebHostEnvironment _environment;

    public FileController(ILogger<FileController> logger, IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    /// <summary>
    /// Upload a file for questions or test content
    /// </summary>
    [HttpPost("upload")]
    [RequestSizeLimit(10 * 1024 * 1024)] // 10MB limit
    [ProducesResponseType(typeof(ApiResponseDto<FileUploadResponse>), 200)]
    public async Task<IActionResult> UploadFile(IFormFile file, [FromQuery] string category = "general")
    {
        try
        {
            if (file == null || file.Length == 0)
                return this.BadRequestResponse<string>("No file provided");

            // Validate file type
            var allowedTypes = new[] { ".jpg", ".jpeg", ".png", ".gif", ".pdf", ".mp4", ".mp3", ".wav" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            
            if (!allowedTypes.Contains(extension))
                return this.BadRequestResponse<string>("File type not allowed");

            // Generate unique filename
            var fileName = $"{Guid.NewGuid()}{extension}";
            var uploadPath = Path.Combine(_environment.WebRootPath, "uploads", category);
            
            // Ensure directory exists
            Directory.CreateDirectory(uploadPath);
            
            var filePath = Path.Combine(uploadPath, fileName);
            
            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileUrl = $"/uploads/{category}/{fileName}";
            var response = new FileUploadResponse(fileName, fileUrl, file.Length, extension);
            
            _logger.LogInformation("File uploaded: {FileName}", fileName);
            return this.OkResponse(response, "File uploaded successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file");
            return this.ExceptionResponse<string>(ex);
        }
    }

    /// <summary>
    /// Delete an uploaded file
    /// </summary>
    [HttpDelete("{category}/{fileName}")]
    [Authorize(Roles = "Administrator,Manager")]
    [ProducesResponseType(typeof(ApiResponseDto<string>), 200)]
    public async Task<IActionResult> DeleteFile(string category, string fileName)
    {
        try
        {
            var filePath = Path.Combine(_environment.WebRootPath, "uploads", category, fileName);
            
            if (!System.IO.File.Exists(filePath))
                return this.NotFoundResponse<string>("File not found");

            System.IO.File.Delete(filePath);
            
            _logger.LogInformation("File deleted: {FileName}", fileName);
            return this.OkResponse("File deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file: {FileName}", fileName);
            return this.ExceptionResponse<string>(ex);
        }
    }
}

public record FileUploadResponse(string FileName, string FileUrl, long FileSize, string FileType);