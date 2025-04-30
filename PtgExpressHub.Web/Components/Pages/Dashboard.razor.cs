using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using PtgExpressHub.Domain.Entities;

namespace PtgExpressHub.Web.Components.Pages;

public partial class Dashboard
{
    public bool IsLoading;
    public bool IsShowCopied = false;

    private bool _isAuthenticated;
    private Dictionary<Guid, ApplicationBuildVersion> _selectedApplicationVersions = new();

    public bool IsModalOpen = false;
    public string[]? ChangeLogs = null;

    public IList<ApplicationBuild>? ComportApplications { get; set; }

    protected override async Task OnInitializedAsync()    
    {
        _isAuthenticated = _authService.IsUserAuthenticated();

        if (!_isAuthenticated)
        {
            _navigationManager.NavigateTo("/auth/login");
        }

        IsLoading = true;
        var result = await _applicationRepository.GetAllApplicationsBuildAsync(CancellationToken.None);
        ComportApplications = result.OrderBy(item => item.ApplicationBuildVersions!.Max(x => x.UploadDate))
            .Reverse()
            .ToList();

        IsLoading = false;
        foreach (var item in ComportApplications)
            _selectedApplicationVersions[item.ApplicationBuildId] = item.ApplicationBuildVersions!.OrderByDescending(x => x.UploadDate).First()!;
    }

    public async Task DownloadFile(ApplicationBuild application)
    {
        if (IsModalOpen)
            return;

        string applicationBlobUrl = _selectedApplicationVersions[application.ApplicationBuildId].BlobUrl;

        var downloadedData = await _storageService.DownloadFromStorage(applicationBlobUrl);
        if (downloadedData != null)
        {
            await _jsRuntime.InvokeVoidAsync("downloadFileFromStream", application.ApplicationBuildUserName, "application/zip", downloadedData);
        }
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
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim().Insert(0, " "))
                .ToArray();
        }        
    }
    public async Task GenerateDownloadLinkAsync(ApplicationBuild application)
    {
        string applicationBlobUrl = _selectedApplicationVersions[application.ApplicationBuildId].BlobUrl;
        string safeLink =_safeLinkService.GenerateSafeLink(application.ApplicationBuildUserName, applicationBlobUrl);

        await _jsRuntime.InvokeVoidAsync("copyTextToClipboard", safeLink);

        IsShowCopied = true;
        StateHasChanged();
        await Task.Delay(5000);

        StateHasChanged();
        IsShowCopied = false;        
    }

    public void NavigateToLink(ApplicationBuild application)
    {
        _navigationManager.NavigateTo(application.ApplicationRepositoryUrl);
    }
}
