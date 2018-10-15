using System;
using Selerix.BusinessObjects;
using Selerix.Foundation;

namespace BSTIntegrationExample
{
    public partial class Saml2Example : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void OnStartButtonClicked(object sender, EventArgs e)
        {
            Session["Transmittal"] = null;

            Transmittal transmittal = new Transmittal();
            transmittal.SenderID = Guid.NewGuid();
            
            Applicant employee = new Applicant();
            employee.ID = "1";
            employee.AsOfDate = DateTime.Now;
            employee.SSN = "123-45-6789";
            employee.FirstName = "Joell";
            employee.MiddleInitial = "K";
            employee.LastName = "AK-Carson";
            employee.LegalStatus = LegalStatus.Employee;

            transmittal.Applicants = new ApplicantCollection();
            transmittal.Applicants.Add(employee);

            Session["Transmittal"] = transmittal;

            bstIframe.Attributes.Add("src", "Saml2Request.aspx");
            //bstIframe.Attributes.Add("src", "ShowTransmittal.aspx");// for test purposes
        }
    }
}
