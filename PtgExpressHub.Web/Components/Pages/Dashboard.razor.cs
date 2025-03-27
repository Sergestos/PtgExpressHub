using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using PtgExpressHub.Domain.Entities;

namespace PtgExpressHub.Web.Components.Pages;

public partial class Dashboard
{
    private bool _isAuthenticated;
    private Dictionary<Guid, ApplicationBuildVersion> _selectedApplicationVersions = new();

    public bool IsModalOpen = false;
    public string[]? ChangeLogs = null;

    public IList<ApplicationBuild>? ComportApplications { get; set; }

    protected override async Task OnInitializedAsync()    
    {
        _isAuthenticated = await _authService.IsAuthorizedAsync();

        if (!_isAuthenticated)
        {
            _navigationManager.NavigateTo("/auth/login");
        }

        var result = await _applicationRepository.GetAllApplicationsBuildAsync(CancellationToken.None);
        ComportApplications = result.OrderBy(item => item.ApplicationBuildVersions!.Max(x => x.UploadDate))
            .Reverse()
            .ToList();

        foreach (var item in ComportApplications)
            _selectedApplicationVersions[item.ApplicationBuildId] = item.ApplicationBuildVersions!.OrderByDescending(x => x.UploadDate).First()!;
    }

    public async Task DownloadFile(ApplicationBuild application)
    {
        if (IsModalOpen)
            return;

        string applicationUrl = _selectedApplicationVersions[application.ApplicationBuildId].BlobUrl;

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

    public void ShowModal(ApplicationBuild application)
    {        
        var version = _selectedApplicationVersions[application.ApplicationBuildId];
        if (!string.IsNullOrEmpty(version.ChangeLog))
        {
            IsModalOpen = true;
            ChangeLogs = version.ChangeLog
                .Split(';')
                .Select(x => x.Trim().Insert(0, " "))
                .ToArray();
        }        
    }
    public void NavigateToLink(ApplicationBuild application)
    {
        _navigationManager.NavigateTo(application.ApplicationRepositoryUrl);
    }
}
