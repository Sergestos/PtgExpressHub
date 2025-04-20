using System.Security.Cryptography;
using System.Text;

namespace PtgExpressHub.Web.Runtime;

public class SafeLinkService
{
    private readonly IConfiguration _configuration;
    private readonly byte[] _enc_key;
    private readonly byte[] _enc_iv;

    public SafeLinkService(IConfiguration configuration)
    {
        _configuration = configuration;

        _enc_key = Encoding.UTF8.GetBytes(_configuration["SafeLink:Key"]!);
        _enc_iv = Encoding.UTF8.GetBytes(_configuration["SafeLink:IV"]!);
    }

    public string GenerateSafeLink(string name, string artifactsRootLink)
    {
        long expiryTimeStamp = DateTimeOffset.UtcNow
            .AddMinutes(int.Parse(_configuration["SafeLink:ExpiryMinutesOffset"]!))
            .ToUnixTimeSeconds();

        SafeLinkData data = new SafeLinkData()
        {
            ApplicationName = name,
            ApplicationRootUrl = artifactsRootLink,
            ExpirationTimeStamp = expiryTimeStamp
        };

        string uri = $"{_configuration["Resourse:Uri"]!}/api/artifacts/upload-from-url";
        string query = $"?safelink={EncryptSafeLinkData(data.ToSerializableString())}";

        return string.Concat(uri, query);
    }

    public SafeLinkData DecodeFromSaveLink(string safeLink)
    {
        return new SafeLinkData(DecryptSafeLinkData(safeLink));
    }

    private string EncryptSafeLinkData(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = _enc_key;
        aes.IV = _enc_iv;

        using var encryptor = aes.CreateEncryptor();
        byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] encrypted = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

        return Convert.ToBase64String(encrypted).Replace('+', '-').Replace('/', '_').TrimEnd('=');
    }

    private string DecryptSafeLinkData(string base64)
    {
        using var aes = Aes.Create();
        aes.Key = _enc_key;
        aes.IV = _enc_iv;

        byte[] encrypted = Convert.FromBase64String(NormalizeBase64(base64));

        using var decryptor = aes.CreateDecryptor();
        byte[] decryptedBytes = decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);
        return Encoding.UTF8.GetString(decryptedBytes);
    }

    private string NormalizeBase64(string input)
    {
        string normalizedBase64 = input.Replace('-', '+').Replace('_', '/');
        switch (normalizedBase64.Length % 4)
        {
            case 2: normalizedBase64 += "=="; break;
            case 3: normalizedBase64 += "="; break;
            case 1: throw new FormatException("Invalid Base64 string length.");
        }

        return normalizedBase64;
    }
}

public class SafeLinkData
{
    public string ApplicationName { get; set; } = string.Empty;

    public string ApplicationRootUrl { get; set; } = string.Empty;

    public long ExpirationTimeStamp { get; set; }

    public SafeLinkData() { }

    public SafeLinkData(string decodedObj)
    {
        var parts = decodedObj.Split('|');

        ApplicationName = parts[0];
        ApplicationRootUrl = parts[1];
        ExpirationTimeStamp = long.Parse(parts[2]);
    }

    public string ToSerializableString()
    {
        return $"{ApplicationName}|{ApplicationRootUrl}|{ExpirationTimeStamp}";
    }
}
