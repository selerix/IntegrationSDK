using System;
using System.Web.UI;
using Selerix.BusinessObjects;
using Selerix.Foundation.Data;
using BSTIntegrationExample.QXEnrollmentService;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.ServiceModel.Security;
using Selerix.Foundation;

namespace BSTIntegrationExample
{
    public partial class ServicesExample : System.Web.UI.Page
    {
        private Guid _LastPortfolioID
        {
            get
            {
                object obj = ViewState["LastPortfolioID"];

                if (obj != null)
                    return (Guid)obj;

                return Guid.Empty;
            }
            set
            {
                ViewState["LastPortfolioID"] = value;
            }
        }

        private Guid _LastEmployeeID
        {
            get
            {
                object obj = ViewState["LastEmployeeID"];

                if (obj != null)
                    return (Guid)obj;

                return Guid.Empty;
            }
            set
            {
                ViewState["LastEmployeeID"] = value;
            }
        }

        private string _LastEmployeeSSN
        {
            get
            {
                object obj = ViewState["LastEmployeeSSN"];

                if (obj != null)
                    return (string)obj;

                return "111-11-1111";
            }
            set
            {
                ViewState["LastEmployeeSSN"] = value;
            }
        }

        private string _LastPortfolioName
        {
            get
            {
                object obj = ViewState["LastPortfolioName"];

                if (obj != null)
                    return (string)obj;

                return "Test Group";
            }
            set
            {
                ViewState["LastPortfolioName"] = value;
            }
        }

        private void SetDefaultQueryTransmittal()
        {
            Transmittal result = new Transmittal();

            if (queryType.SelectedValue == "GetCensus")
            {
                result.Type = TransmittalType.Query;

                result.Group = new Group();
                result.Group.GroupName = _LastPortfolioName;

                //if (string.IsNullOrEmpty(result.Group.GroupName))
                //    result.Group.GroupName = "Test Group";

                Applicant employee = new Applicant();
                employee.Relationship = Relationship.Employee;
                employee.SSN = _LastEmployeeSSN;

                result.Applicants = new ApplicantCollection();
                result.Applicants.Add(employee);
            }
            else if (queryType.SelectedValue == "GetGroup")
            {
                result.Type = TransmittalType.GetPortfolio;

                result.Group = new Group();
                result.Group.GroupName = _LastPortfolioName;

                //if (string.IsNullOrEmpty(result.Group.GroupName))
                //    result.Group.GroupName = "Test Group";
            }

            txtQueryTransmittalBox.Text = SerializationHelper.SerializeToString(result);
        }

        private void SetDefaultUploadTransmittal()
        {
            Transmittal result = new Transmittal();

            if (uploadType.SelectedValue == "UploadCensus")
            {
                result.Type = TransmittalType.UploadApplicants;

                result.Group = new Group();
                result.Group.GroupName = _LastPortfolioName;

                Applicant employee = new Applicant();
                employee.Relationship = Relationship.Employee;
                employee.FirstName = "TestFirst";
                employee.LastName = "TestLast";
                employee.BirthDate = new DateTime(1980, 12, 24);
                employee.Sex = Gender.Male;

                employee.SSN = _LastEmployeeSSN;

                result.Applicants = new ApplicantCollection();
                result.Applicants.Add(employee);
            }
            else if (uploadType.SelectedValue == "UploadGroup")
            {
                result.Type = TransmittalType.UploadPortfolio;

                result.Portfolio = new Portfolio();
                result.Portfolio.Name = _LastPortfolioName;
                result.Portfolio.GroupNumber = "TESTXXXX";

                //Enrollment info
                result.Portfolio.EnrollmentStartDate = new DateTime(2010, 12, 1);
                result.Portfolio.EnrollmentEndDate = new DateTime(2011, 2, 15);
                result.Portfolio.PlanYearStartDate = new DateTime(2011, 1, 1);

                //Employer Info
                result.Portfolio.Employer = new Employer();
                result.Portfolio.Employer.Name = "Test Employer";

                result.Portfolio.Employer.Address = new Address();
                result.Portfolio.Employer.Address.Line1 = "123 Main Ln";
                result.Portfolio.Employer.Address.Line2 = null;
                result.Portfolio.Employer.Address.City = "Chicago";
                result.Portfolio.Employer.Address.State = "IL";
                result.Portfolio.Employer.Address.Zip = "54342";

                //Payroll provider
                result.Portfolio.PayrollProviders = new PayrollProviderCollection();

                PayrollProvider payrollProvider = new PayrollProvider();
                payrollProvider.Name = "Payroll Dept.";

                result.Portfolio.PayrollProviders.Add(payrollProvider);

                //Relationships included in enrollment
                result.Portfolio.DependentRelationships = new RelationshipCCCollection();

                result.Portfolio.DependentRelationships.Add(new RelationshipCC(Relationship.Employee));
                result.Portfolio.DependentRelationships.Add(new RelationshipCC(Relationship.Spouse));
                result.Portfolio.DependentRelationships.Add(new RelationshipCC(Relationship.Child));
                
                if (_LastPortfolioID != null)
                    result.Portfolio.UniqueID = _LastPortfolioID;
            }

            txtUploadTransmittalBox.Text = SerializationHelper.SerializeToString(result);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            queryType.SelectedIndexChanged += new EventHandler(queryType_SelectedIndexChanged);
            uploadType.SelectedIndexChanged += new EventHandler(uploadType_SelectedIndexChanged);
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnUpload.Click += new EventHandler(btnUpload_Click);
            btnGetLoginGuid.Click += new EventHandler(btnGetLoginGuid_Click);

            if (!Page.IsPostBack)
            {
                //set default upload transmittal
                SetDefaultUploadTransmittal();
            }
        }

        void btnGetLoginGuid_Click(object sender, EventArgs e)
        {
            try
            {
                Guid result = DoGetLoginGuid(new Guid(txtPortfolioID.Text), new Guid(txtEmployeeID.Text));

                lblLoginGuid.Text = "login_guid=" + result.ToString();
            }
            catch (Exception ex)
            {
                lblLoginGuid.Text = "login_guid= Error: " + ex.Message;
            }
        }

        void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                txtUploadResult.Text = DoUpdate(txtUploadTransmittalBox.Text);

                try
                {
                    Transmittal transmittal = (Transmittal)SerializationHelper.DeserializeFromString(txtUploadTransmittalBox.Text, typeof(Transmittal));
                    Transmittal resultTransmittal = (Transmittal)SerializationHelper.DeserializeFromString(txtUploadResult.Text, typeof(Transmittal));

                    if (uploadType.SelectedValue == "UploadCensus")
                    {
                        foreach (var applicant in transmittal.Applicants)
                        {
                            if (applicant.Relationship == Relationship.Employee || applicant.Relationship == Relationship.Unknown)
                            {
                                if (!string.IsNullOrEmpty(applicant.SSN))
                                    _LastEmployeeSSN = applicant.SSN;

                                break;
                            }
                        }
                    }
                    else if (uploadType.SelectedValue == "UploadGroup")
                    {
                        if (transmittal.Portfolio != null && !string.IsNullOrEmpty(transmittal.Portfolio.Name))
                            _LastPortfolioName = transmittal.Portfolio.Name;

                        if (resultTransmittal.PortfolioID != Guid.Empty)
                            _LastPortfolioID = resultTransmittal.PortfolioID;
                    }
                }
                catch
                {
                }

                SetDefaultQueryTransmittal();
            }
            catch (Exception ex)
            {
                txtUploadResult.Text = "Error: " + ex.Message;
            }

            SetDefaultGetLoginParameters();
        }

        private void SetDefaultGetLoginParameters()
        {
            if (_LastPortfolioID != Guid.Empty)
                txtPortfolioID.Text = _LastPortfolioID.ToString();

            if (_LastEmployeeID != Guid.Empty)
                txtEmployeeID.Text = _LastEmployeeID.ToString();
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                txtQueryResult.Text = DoQuery(txtQueryTransmittalBox.Text);

                try
                {
                    Transmittal resultTransmittal = (Transmittal)SerializationHelper.DeserializeFromString(txtQueryResult.Text, typeof(Transmittal));

                    if (queryType.SelectedValue == "GetCensus")
                    {
                        foreach (var applicant in resultTransmittal.Applicants)
                        {
                            if (applicant.Relationship == Relationship.Employee || applicant.Relationship == Relationship.Unknown)
                            {
                                _LastEmployeeID = applicant.UniqueID;

                                break;
                            }
                        }
                    }
                }
                catch
                {
                }
            }
            catch(Exception ex)
            {
                txtQueryResult.Text = "Error: " + ex.Message;
            }

            SetDefaultGetLoginParameters();
        }

        void uploadType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetDefaultUploadTransmittal();
        }

        void queryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetDefaultQueryTransmittal();
        }

        Guid DoGetLoginGuid(Guid portfolioGuid, Guid employeeGuid)
        {
            // Uncomment next line if you want use soap http client instead WCF
            //return CreateQXWebServiceClient().GetLoginGUID("username", "password", portfolioGuid, employeeGuid);

            return CreatQXEnrollmentServiceClient().GetLoginGUID(portfolioGuid, employeeGuid);
        }

        public BSTIntegrationExample.QXWebService.Enrollment CreateQXWebServiceClient()
        {
            BSTIntegrationExample.QXWebService.Enrollment webService = new QXWebService.Enrollment();
            webService.Url = System.Configuration.ConfigurationManager.AppSettings["QXWebServiceURL"];

            return webService;
        }

        public EnrollmentClient CreatQXEnrollmentServiceClient()
        {
            //Switch from certificate encryption mode to username/password mode.
            bool isCertificateEncrypted = true;

            EnrollmentClient webService = null;

            if (isCertificateEncrypted)
            {
                Uri uri = new Uri(System.Configuration.ConfigurationManager.AppSettings["QXEnrollmentServiceCertificateURL"]);
                var serverCertificate = GetSelerixCertificate();

                string certSubject = BindingHelper.GetCertificateCN(serverCertificate);

                EndpointAddress endpointAddress = new EndpointAddress(uri, EndpointIdentity.CreateDnsIdentity(certSubject), new AddressHeaderCollection());
                
                //var bind = BindingHelper.CreateCustomBinding(true, uri.Scheme == "https");

                webService = new BSTIntegrationExample.QXEnrollmentService.EnrollmentClient(new CustomBinding("EncryptCertificateBinding"), endpointAddress);

                webService.ClientCredentials.ClientCertificate.Certificate = GetVendorCertificate();
                webService.ClientCredentials.ServiceCertificate.DefaultCertificate = serverCertificate;
            }
            else
            {
                Uri uri = new Uri(System.Configuration.ConfigurationManager.AppSettings["QXEnrollmentServiceURL"]);
                EndpointAddress endpointAddress = new EndpointAddress(uri);

                var bind = BindingHelper.CreateCustomBinding(false, uri.Scheme == "https");

                webService = new BSTIntegrationExample.QXEnrollmentService.EnrollmentClient(bind, endpointAddress);

                webService.ClientCredentials.UserName.UserName = "username";
                webService.ClientCredentials.UserName.Password = "password";
            }
            
            return webService;
        }

        /// <summary>
        /// Encryption certificate.
        /// </summary>
        /// <returns></returns>
        private X509Certificate2 GetSelerixCertificate()
        {
            X509Certificate2 result = Utils.GetSelerixCertificateFromStorage();

            if (result == null)
                result = Utils.GetCertificateFromFileSystem(Path.Combine(Request.PhysicalApplicationPath, "Server.pfx"), "123");

            return result;
        }

        /// <summary>
        /// Identification certificate.
        /// </summary>
        /// <returns></returns>
        private X509Certificate2 GetVendorCertificate()
        {
            X509Certificate2 result = Utils.GetVendorCertificateFromStorage();

            if (result == null)
                result = Utils.GetCertificateFromFileSystem(Path.Combine(Request.PhysicalApplicationPath, "Client.pfx"), "123");

            return result;
        }

        string DoQuery(string data)
        {
            // Uncomment next line if you want use soap http client instead WCF
            return CreateQXWebServiceClient().Query("username", "password", data);

            //return CreatQXEnrollmentServiceClient().Query(data);
        }

        string DoUpdate(string data)
        {
            // Uncomment next line if you want use soap http client instead WCF
            return CreateQXWebServiceClient().Upload("username", "password", data);
            
            //return CreatQXEnrollmentServiceClient().Upload(data);
        }
    }   

    /// <summary>
    /// BindingHelper
    /// </summary>
    /// <remarks></remarks>
    static class BindingHelper
    {
        /// <summary>
        /// Creates the custom binding.
        /// </summary>
        /// <param name="isCertificate">If set to true certificate security will be used.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Binding CreateCustomBinding(bool isCertificate, bool isHttps)
        {
            BindingElement securityBinding = null;

            BindingElement transport = null;

            if (isHttps)
            {
                HttpsTransportBindingElement httpsTransport = new HttpsTransportBindingElement();
                transport = httpsTransport;
            }
            else
            {
                HttpTransportBindingElement httpTransport = new HttpTransportBindingElement();
                transport = httpTransport;
            }

            TextMessageEncodingBindingElement messageEncoding = new TextMessageEncodingBindingElement();

            if (!isCertificate)
            {
                TransportSecurityBindingElement messageSecurity = SecurityBindingElement.CreateUserNameOverTransportBindingElement();

                messageSecurity.IncludeTimestamp = false;

                securityBinding = messageSecurity;

                messageSecurity.AllowInsecureTransport = !isHttps;
            }
            else
            {
                AsymmetricSecurityBindingElement messageSecurity =
                     (AsymmetricSecurityBindingElement)SecurityBindingElement.CreateMutualCertificateDuplexBindingElement
                         (
                            MessageSecurityVersion
                            .WSSecurity10WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11BasicSecurityProfile10
                         );

                messageSecurity.MessageProtectionOrder = MessageProtectionOrder.EncryptBeforeSign;
                messageSecurity.AllowInsecureTransport = !isHttps;

                securityBinding = messageSecurity;
                messageEncoding.MessageVersion = MessageVersion.Soap11;
            }

            CustomBinding result = new CustomBinding(securityBinding,
                messageEncoding,
                transport);

            return result;
        }

        /// <summary>
        /// Gets the certificate CN.
        /// </summary>
        /// <param name="certificate">The certificate.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetCertificateCN(X509Certificate2 certificate)
        {
            string name = certificate.Subject;

            int index1 = name.IndexOf("CN=");
            index1 += 3;

            int index2 = name.IndexOf(",", index1);

            if (index2 < 0)
                index2 = name.Length;

            return name.Substring(index1, index2 - index1);
        }
    }
}