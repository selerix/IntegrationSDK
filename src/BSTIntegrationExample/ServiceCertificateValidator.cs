using System.IdentityModel.Selectors;
using System;
using System.Security.Cryptography.X509Certificates;
namespace BSTIntegrationExample
{

    /// <summary>
    /// A class derived from X509CertificateValidator to validate the client certificate using a specific 
    /// list of certificates.
    /// If the certificate is not in the list of valid certificate this validator try to use the default PeerOrChainTrust validator.
    /// </summary>
    public class ServiceCertificateValidator : X509CertificateValidator
    {
        public ServiceCertificateValidator()
        {
        }

        public override void Validate(X509Certificate2 certificate)
        {
            // Check that there is a certificate.
            if (certificate == null)
                throw new ArgumentNullException("certificate");

            //throw SecurityTokenvalidationException if certificate is not valid.
            if (certificate.SerialNumber != "8100164C5310EEB6490CE323DD3F5B1C") // CN=Client
                throw new System.IdentityModel.Tokens.SecurityTokenValidationException("Unknown client certificate");

            return;
        }
    }
}