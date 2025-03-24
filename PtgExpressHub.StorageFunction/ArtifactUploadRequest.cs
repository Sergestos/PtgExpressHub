﻿namespace PtgExpressHub.StorageFunction;

internal class ArtifactUploadRequest
{
    public required string ApplicationBuildProductionName { get; set; }

    public required string ApplicationBuildBlobPath { get; set; }

    public required string Version { get; set; }

    public required string RepositoryUrl { get; set; }

    public string? ChangeLog { get; set; }
}
