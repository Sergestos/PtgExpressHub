﻿using System.ComponentModel.DataAnnotations;

namespace PtgExpressHub.Domain.Entities;

public class ApplicationBuild
{
    [Key]
    public required Guid ApplicationBuildId { get; set; }

    [Required]
    public required string ApplicationBuildProductionName { get; set; }

    [Required]
    public required string ApplicationBuildUserName { get; set; }

    [Required]
    public required string ApplicationRepositoryUrl { get; set; }

    public virtual ICollection<ApplicationBuildVersion>? ApplicationBuildVersions { get; set; }
}
