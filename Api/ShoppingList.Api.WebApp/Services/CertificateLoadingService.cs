using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace ProjectHermes.ShoppingList.Api.WebApp.Services
{
    public class CertificateLoadingService
    {
        public X509Certificate2 GetCertificate(string crtFilePath, string privateKeyFilePath)
        {
            string[] passwordFileLines = File.ReadAllLines(privateKeyFilePath);
            var certificate = new X509Certificate2(crtFilePath);

            using var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(GetBytesFromPem(passwordFileLines), out _);

            // exporting the certificate is a workaround for a windows bug
            // see: https://github.com/dotnet/runtime/issues/23749#issuecomment-388231655
            return new X509Certificate2(certificate.CopyWithPrivateKey(rsa).Export(X509ContentType.Pkcs12));
        }

        private static byte[] GetBytesFromPem(string[] lines)
        {
            return Convert.FromBase64String(lines
                .Where(l => !l.Contains('-') && !string.IsNullOrWhiteSpace(l))
                .Aggregate("", (current, next) => current + next));
        }
    }
}