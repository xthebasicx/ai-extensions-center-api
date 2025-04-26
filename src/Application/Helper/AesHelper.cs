using System.Security.Cryptography;
using System.Text;

namespace AIExtensionsCenter.Application.Helper;
public static class AesHelper
{
    private static readonly string SecretKey = "c17e4a89f3b24d0d9aa1e2c6b548fe12";

    public static string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(SecretKey);
        aes.GenerateIV();
        var iv = aes.IV;
        using var encryptor = aes.CreateEncryptor(aes.Key, iv);
        using var ms = new MemoryStream();
        ms.Write(iv, 0, iv.Length);
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using (var sw = new StreamWriter(cs))
            sw.Write(plainText);
        return Convert.ToBase64String(ms.ToArray());
    }

    public static string Decrypt(string encryptedText)
    {
        var fullCipher = Convert.FromBase64String(encryptedText);
        using var aes = Aes.Create();
        var iv = fullCipher[..16];
        var cipher = fullCipher[16..];
        aes.Key = Encoding.UTF8.GetBytes(SecretKey);
        aes.IV = iv;
        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(cipher);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);
        return sr.ReadToEnd();
    }
}

