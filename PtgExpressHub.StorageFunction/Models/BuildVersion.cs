namespace PtgExpressHub.StorageFunction.Models;

public class BuildVersion
{
    public int Major { get; set; }

    public int Minor { get; set; }

    public int Patch { get; set; }

    public BuildVersion(int major, int minor, int patch)
    {
        Major = major;
        Minor = minor;
        Patch = patch;
    }

    public BuildVersion IncrementMajor()
    {
        return new BuildVersion(++Major, 0, 0);
    }

    public BuildVersion IncrementMinor()
    {
        return new BuildVersion(Major, ++Minor, 0);
    }

    public BuildVersion IncrementPatch()
    {
        return new BuildVersion(Major, Minor, ++Patch);
    }

    public override string ToString()
    {
        return $"{Major}.{Minor}.{Patch}";
    }

    public static BuildVersion Parse(string version)
    {
        string[] parts = version.Split('.');

        int major = int.Parse(parts[0]);
        int minor = int.Parse(parts[1]);
        int patch = int.Parse(parts[2]);

        return new BuildVersion(major, minor, patch);
    }
}
