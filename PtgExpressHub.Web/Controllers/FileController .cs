using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PtgExpressHub.Web.Runtime;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly StorageService _storageService;

    public FileController(StorageService storageService)
    {
        _storageService = storageService;
    }

    [HttpGet("upload-from-url")]
    public async Task<IActionResult> UploadFromUrl([FromQuery] string? name, [FromQuery] string? request)
    {
        if (string.IsNullOrWhiteSpace(request))
            return BadRequest("Exception: Request query is required.");

        if (string.IsNullOrWhiteSpace(name))
            return BadRequest("Exception: Name query is required.");

        try
        {
            var fileBytes = await _storageService.DownloadFromStorage(request);

            return File(fileBytes, "application/zip", name + ".zip");
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to download content: {ex.Message}");
        }
    }
}
