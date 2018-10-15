using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;
using System.ServiceModel.Activation;
using System.IdentityModel.Selectors;
using System.IO;

namespace BSTIntegrationExample
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Enrollment" in code, svc and config file together.
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class EnrollmentCert : Enrollment
    {
    }

    ///// <summary>
    ///// The factory class derived from ServiceHostFactory used to create the CertificateServiceHost class.
    ///// Useful when hosting the service with IIS where you cannot specify what class to create but you can specify the host factory.
    ///// </summary>
    //public class CertificateServiceHostFactory : ServiceHostFactory
    //{
    //    protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
    //    {
    //        return new CertificateServiceHost(serviceType, baseAddresses);
    //    }
    //}

    ///// <summary>
    ///// A class that derive from the ServiceHost system class to automatically set the 
    ///// server certificate used for service authentication.
    ///// This class set the Credentials.ServiceCertificate.Certificate property override any certificate configuration.
    ///// Consider anyway that you must correctly configure the binding security.
    ///// </summary>
    //public class CertificateServiceHost : ServiceHost
    //{
    //    public CertificateServiceHost(Type serviceType, Uri[] baseAddresses)
    //        : base(serviceType, baseAddresses)
    //    {
    //    }

    //    protected override void ApplyConfiguration()
    //    {
    //        base.ApplyConfiguration();

    //        X509ClientCertificateAuthentication authentication =
    //                        this.Credentials.ClientCertificate.Authentication;

    //        authentication.CertificateValidationMode =
    //            System.ServiceModel.Security.X509CertificateValidationMode.Custom;

    //        authentication.CustomCertificateValidator =
    //            new CustomCertificateValidator();

    //        this.Credentials.ServiceCertificate.Certificate = GetSelerixCertificate();
    //    }

    //    /// <summary>
    //    /// Encryption certificate.
    //    /// </summary>
    //    /// <returns></returns>
    //    private X509Certificate2 GetSelerixCertificate()
    //    {
    //        X509Certificate2 result = Utils.GetSelerixCertificateFromStorage();

    //        if (result == null)
    //            result = Utils.GetCertificateFromFileSystem(Path.Combine(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "Server.pfx"), "123");

    //        return result;
    //    }
    //}
}
