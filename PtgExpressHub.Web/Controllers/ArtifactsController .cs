using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PtgExpressHub.Web.Runtime;

[ApiController]
[Route("api/[controller]")]
public class ArtifactsController : ControllerBase
{
    private readonly StorageService _storageService;
    private readonly SafeLinkService _safeLinkService;
    

    public ArtifactsController(StorageService storageService, SafeLinkService safeLinkService)
    {
        _storageService = storageService;
        _safeLinkService= safeLinkService;
    }

    [AllowAnonymous]
    [HttpGet("upload-from-url")]
    public async Task<IActionResult> UploadFromUrl([FromQuery] string safeLink)
    {
        if (string.IsNullOrWhiteSpace(safeLink))
            return BadRequest("Exception: SafeLink is not provided.");

        var safeLinkData =_safeLinkService.DecodeFromSaveLink(safeLink);

        if (DateTime.UtcNow > DateTimeOffset.FromUnixTimeSeconds(safeLinkData.ExpirationTimeStamp).DateTime)
            return RedirectToAction("AuthorizedUploadFromUrl", new { appName = safeLinkData.ApplicationName, appLoadUrl = safeLinkData.ApplicationRootUrl });

        return await DownloadFromUrl(safeLinkData.ApplicationName, safeLinkData.ApplicationRootUrl);
    }

    [Authorize]
    [HttpGet("redirect-upload-from-url")]
    public async Task<IActionResult> AuthorizedUploadFromUrl([FromQuery] string appName, [FromQuery] string appLoadUrl)
    {
        return await DownloadFromUrl(appName, appLoadUrl);
    }

    private async Task<IActionResult> DownloadFromUrl(string appName, string appLoadUrl)
    {
        try
        {
            var fileBytes = await _storageService.DownloadFromStorage(appLoadUrl);
            return File(fileBytes, "application/zip", appName + ".zip");
        }
        catch (Exception ex)
        {
            return BadRequest($"Failed to download content: {ex.Message}");
        }
    }
}
