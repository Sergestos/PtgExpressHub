using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PtgExpressHub.Domain.Entities;

namespace PtgExpressHub.Web.Components.Pages;

public partial class Dashboard
{
    private bool isAuthenticated;

    private Dictionary<Guid, ApplicationBuildVersion> _selectedApplicationVersions = new Dictionary<Guid, ApplicationBuildVersion>();

    public IList<ApplicationBuild>? ComportApplications { get; set; }

    protected override async Task OnInitializedAsync()
    {
        isAuthenticated = await _authService.IsAuthorizedAsync();

        if (!isAuthenticated)
        {
            _navigationManager.NavigateTo("/auth/login");
        }

        var result = await _applicationRepository.GetAllApplicationsBuildAsync(CancellationToken.None);
        ComportApplications = result.OrderBy(item => item.ApplicationBuildVersions!.Max(x => x.UploadDate))
            .Reverse()
            .ToList();
    }

    public async Task DownloadFile(ApplicationBuild application)
    {
        string applicationUrl = string.Empty;
        if (_selectedApplicationVersions.ContainsKey(application.ApplicationBuildId))
            applicationUrl = _selectedApplicationVersions[application.ApplicationBuildId].BlobUrl;
        else
            applicationUrl = application.ApplicationBuildVersions!.OrderByDescending(x => x.UploadDate).First()!.BlobUrl;

        Stream fileStream = new MemoryStream([0, 1]);
        var fileName = "application.txt";
        using var streamRef = new DotNetStreamReference(stream: fileStream);

        await _jsRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
    }

    public void OnSelectChanged(ApplicationBuild application, ChangeEventArgs e)
    {
        var selectedApplicationVersionId = Guid.Parse(e.Value!.ToString()!);
        _selectedApplicationVersions[application.ApplicationBuildId] =            
            application.ApplicationBuildVersions!.First(x => x.ApplicationVersionId == selectedApplicationVersionId);
    }
}
