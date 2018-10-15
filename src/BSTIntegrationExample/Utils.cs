using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography.X509Certificates;
using System.IO;

namespace BSTIntegrationExample
{
    public static class Utils
    {
        public static X509Certificate2 GetSelerixCertificateFromStorage()
        {
            return GetCertificateFromStorage("43FB6F77077ED18C4208B47DE6ABDCF5");
        }

        public static X509Certificate2 GetVendorCertificateFromStorage()
        {
            return GetCertificateFromStorage("8100164C5310EEB6490CE323DD3F5B1C");
        }

        public static X509Certificate2 GetCertificateFromStorage(string serialNumber)
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);

            X509Certificate2 result = null;

            foreach (X509Certificate2 cert in store.Certificates)
            {
                if (string.Compare(cert.GetSerialNumberString(), serialNumber, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    result = cert;

                    break;
                }
            }

            return result;
        }

        public static X509Certificate2 GetCertificateFromFileSystem(string fileName, string password)
        {
            if (!File.Exists(fileName))
                return null;

            return new X509Certificate2(fileName, password);
        }
    }
}