using System.ComponentModel.DataAnnotations;

namespace PtgExpressHub.Domain.Entities;

public class ApplicationBuildVersion
{
    [Key]
    public required Guid ApplicationVersionId { get; set; }    

    public required Guid ApplicationId { get; set; }

    public required ApplicationBuild ApplicationBuild { get; set; }

    [Required]
    public required DateTime UploadDate { get; set; }

    [Required]
    public required string Version { get; set; }

    [Required]
    public required string BlobUrl { get; set; }
    
    public string? ChangeLog { get; set; }
}
