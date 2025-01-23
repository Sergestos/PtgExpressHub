using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PtgExpressHub.Data.Domain;

namespace PtgExpressHub.Web.Components.Pages;

public partial class Dashboard
{
    private bool isAuthenticated;

    private Dictionary<Guid, ComportApplicationVersion> _selectedApplicationVersions = new Dictionary<Guid, ComportApplicationVersion>();

    public IList<ComportApplication>? ComportApplications { get; set; }

    protected override async Task OnInitializedAsync()
    {
        isAuthenticated = await _authService.IsAuthorizedAsync();

        if (!isAuthenticated)
        {
            _navigationManager.NavigateTo("/auth/login");
        }

        var result = await _applicationRepository.GetAllApplicationsAsync(CancellationToken.None);
        ComportApplications = result.OrderBy(item => item.ApplicationVersions!.Max(x => x.UploadDate))
            .Reverse()
            .ToList();
    }

    public async Task DownloadFile(ComportApplication application)
    {
        string applicationUrl = string.Empty;
        if (_selectedApplicationVersions.ContainsKey(application.ApplicationId))
            applicationUrl = _selectedApplicationVersions[application.ApplicationId].BlobUrl;
        else
            applicationUrl = application.ApplicationVersions!.OrderByDescending(x => x.UploadDate).First()!.BlobUrl;

        Stream fileStream = new MemoryStream([0, 1]);
        var fileName = "application.txt";
        using var streamRef = new DotNetStreamReference(stream: fileStream);

        await _jsRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
    }

    public void OnSelectChanged(ComportApplication application, ChangeEventArgs e)
    {
        var selectedApplicationVersionId = Guid.Parse(e.Value!.ToString()!);
        _selectedApplicationVersions[application.ApplicationId] =            
            application.ApplicationVersions!.First(x => x.ApplicationVersionId == selectedApplicationVersionId);
    }
}
