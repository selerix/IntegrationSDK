using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

using ComponentSpace.SAML2;
using ComponentSpace.SAML2.Assertions;
using ComponentSpace.SAML2.Profiles.SSOBrowser;
using ComponentSpace.SAML2.Protocols;

using Selerix.BusinessObjects;
using Selerix.Foundation.Data;

namespace BSTIntegrationExample
{
    public partial class Saml2Request : Page
    {
        private TextBox _EmployeeID;

        //Transmittal options.
        private CheckBoxList _TransmittalOptionsList;
        private TextBox _XMLText;

        private System.Web.UI.WebControls.Table _Table;

        private void CreateControls()
        {
            this.gridPlaceHolder.Controls.Clear();
            BuildQuestions();
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            CreateControls();

            if (!Page.IsPostBack)
                PopulateQuestions();
        }

        private void BuildQuestions()
        {
            _Table = BuildTable(5, 2, "clsQuestion");
            _Table.ID = "table";
            _Table.Width = Unit.Percentage(100);

            _Table.Rows[0].Cells[0].Width = Unit.Percentage(20);
            _Table.Rows[0].Cells[1].Width = Unit.Percentage(80);

            //Email START -------------------------------------------------------------------------------------
            _Table.Rows[0].Cells[0].VerticalAlign = VerticalAlign.Top;
            _Table.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Right;
            _Table.Rows[0].Cells[0].Text = "Employee ID:";

            _EmployeeID = new TextBox();
            _EmployeeID.ID = "employeeID";
            _EmployeeID.Width = Unit.Pixel(250);

            _Table.Rows[0].Cells[1].Controls.Add(_EmployeeID);

            //URL START ---------------------------------------------------------------------------------------
            _Table.Rows[1].Visible = false;

            //XML START ---------------------------------------------------------------------------------------
            _Table.Rows[2].Cells[0].VerticalAlign = VerticalAlign.Top;
            _Table.Rows[2].Cells[0].HorizontalAlign = HorizontalAlign.Right;
            _Table.Rows[2].Cells[0].Text = "XML:";

            _XMLText = new TextBox();
            _XMLText.ID = "xmlText";
            _XMLText.Width = Unit.Percentage(100);
            _XMLText.Rows = 20;
            _XMLText.TextMode = TextBoxMode.MultiLine;

            _Table.Rows[2].Cells[1].Controls.Add(_XMLText);

            //Options START -----------------------------------------------------------------------------------
            _Table.Rows[3].Cells[0].VerticalAlign = VerticalAlign.Top;
            _Table.Rows[3].Cells[0].HorizontalAlign = HorizontalAlign.Right;
            _Table.Rows[3].Cells[0].Text = "Layout Options:";

            _TransmittalOptionsList = new CheckBoxList();
            _TransmittalOptionsList.ID = "transmittalOptionsList";

            _TransmittalOptionsList.Items.Add(new ListItem("&nbsp;Header and Footer", "HeaderAndFooter"));
            _TransmittalOptionsList.Items.Add(new ListItem("&nbsp;Sidebar", "Sidebar"));
            _TransmittalOptionsList.Items.Add(new ListItem("&nbsp;Personal Information", "PersonalInfo"));
            _TransmittalOptionsList.Items.Add(new ListItem("&nbsp;Welcome", "Welcome"));
            _TransmittalOptionsList.Items.Add(new ListItem("&nbsp;Review", "Review"));

            _Table.Rows[3].Cells[1].Controls.Add(_TransmittalOptionsList);
            
            //Send Button START -------------------------------------------------------------------------------
            Button submitButton = new Button();
            submitButton.ID = "submitButton";
            submitButton.Text = "Submit";
            submitButton.Click += new EventHandler(submitButton_Click);

            _Table.Rows[4].Cells[0].ColumnSpan = 2;
            _Table.Rows[4].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            _Table.Rows[4].Cells[0].Controls.Add(submitButton);

            this.gridPlaceHolder.Controls.Add(_Table);
        }

        private void PopulateQuestions()
        {
            string sampleXML = Server.MapPath("~/transmittal.xml");

            if (File.Exists(sampleXML))
                using (var file = File.OpenText(sampleXML))
                    _XMLText.Text = file.ReadToEnd();

            _EmployeeID.Text = "131193";

            //Pre-Answer Transmittal Options
            for (int i = 0; i < _TransmittalOptionsList.Items.Count; i++)
            {
                if (_TransmittalOptionsList.Items[i].Value == "HeaderAndFooter")
                    _TransmittalOptionsList.Items[i].Selected = false;
                else if (_TransmittalOptionsList.Items[i].Value == "Sidebar")
                    _TransmittalOptionsList.Items[i].Selected = false;
                else if (_TransmittalOptionsList.Items[i].Value == "PersonalInfo")
                    _TransmittalOptionsList.Items[i].Selected = false;
                else if (_TransmittalOptionsList.Items[i].Value == "Welcome")
                    _TransmittalOptionsList.Items[i].Selected = true;
                else if (_TransmittalOptionsList.Items[i].Value == "Review")
                    _TransmittalOptionsList.Items[i].Selected = false;
            }
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

        /// <summary>
        /// Handles the Click event of the submitButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void submitButton_Click(object sender, EventArgs e)
        {
            Transmittal transmittal = null;
            string employeeID = this._EmployeeID.Text;

            if (!string.IsNullOrEmpty(this._XMLText.Text))
            {
                try
                {
                    transmittal = (Transmittal)SerializationHelper.DeserializeFromString(this._XMLText.Text, typeof(Transmittal));
                } 
                catch (Exception exception)
                {
                    this._XMLText.Text = exception.Message;
                    Exception inner = exception.InnerException;

                    while (inner != null)
                    {
                        this._XMLText.Text += "\n" + inner.Message;
                        inner = inner.InnerException;
                    }

                    this._XMLText.Text = PrepareSourceCode(this._XMLText.Text);
                }
            }

            if (!string.IsNullOrEmpty(employeeID) && transmittal != null && transmittal.Applicants != null && transmittal.Applicants.Count > 0)
                transmittal.Applicants[0].EmployeeIdent = employeeID;

            Session["Transmittal"] = transmittal;

            //Creating SAML responce
            X509Certificate2 vendorCertificate = GetVendorCertificate();
            X509Certificate2 selerixCertificate = GetSelerixCertificate();

            string assertionConsumerServiceURL = "SamlResponse.aspx";
            string audienceName = "whatever audience";

            SAMLResponse samlResponse = new SAMLResponse();
            samlResponse.Destination = assertionConsumerServiceURL;
            Issuer issuer = new Issuer("Vendor");
            samlResponse.Issuer = issuer;
            samlResponse.Status = new Status(SAMLIdentifiers.PrimaryStatusCodes.Success, null);
            
            SAMLAssertion samlAssertion = new SAMLAssertion();
            samlAssertion.Issuer = issuer;

            Subject subject = null;
            
//          subject = new Subject(new EncryptedID(new NameID(employeeID), selerixCertificate, new EncryptionMethod(EncryptedXml.XmlEncTripleDESUrl))); //employee ID
            subject = new Subject(new NameID(employeeID)); //employee ID
            
            SubjectConfirmation subjectConfirmation = new SubjectConfirmation(SAMLIdentifiers.SubjectConfirmationMethods.Bearer);
            SubjectConfirmationData subjectConfirmationData = new SubjectConfirmationData();
            subjectConfirmationData.Recipient = assertionConsumerServiceURL;
            subjectConfirmationData.NotOnOrAfter = DateTime.UtcNow.AddHours(1);
            subjectConfirmation.SubjectConfirmationData = subjectConfirmationData;
            subject.SubjectConfirmations.Add(subjectConfirmation);

            samlAssertion.Subject = subject;

            Conditions conditions = new Conditions(new TimeSpan(1, 0, 0));
            AudienceRestriction audienceRestriction = new AudienceRestriction();
            audienceRestriction.Audiences.Add(new Audience(audienceName));
            conditions.ConditionsList.Add(audienceRestriction);
            samlAssertion.Conditions = conditions;

            AuthnStatement authnStatement = new AuthnStatement();
            authnStatement.AuthnContext = new AuthnContext();
            authnStatement.AuthnContext.AuthnContextClassRef = new AuthnContextClassRef(SAMLIdentifiers.AuthnContextClasses.Unspecified);
            samlAssertion.Statements.Add(authnStatement);

            AttributeStatement attributeStatement = new AttributeStatement();

            if (transmittal != null)
            {
                attributeStatement.Attributes.Add(new SAMLAttribute("Transmittal", SAMLIdentifiers.AttributeNameFormats.Basic, null, SerializationHelper.SerializeToString(transmittal)));

                if (transmittal.Applicants != null && transmittal.Applicants.Count > 0)
                {
                    transmittal.Applicants[0].EmployeeIdent = employeeID;
                }
            }

            //Check for Transmittal Options
            for (int i = 0; i < _TransmittalOptionsList.Items.Count; i++)
            {
                string answer = "no";

                if (_TransmittalOptionsList.Items[i].Selected)
                    answer = "yes";
                
                if (_TransmittalOptionsList.Items[i].Value == "HeaderAndFooter")
                    attributeStatement.Attributes.Add(new SAMLAttribute("HeaderAndFooter", SAMLIdentifiers.AttributeNameFormats.Basic, null, answer));
                else if (_TransmittalOptionsList.Items[i].Value == "Sidebar")
                    attributeStatement.Attributes.Add(new SAMLAttribute("Sidebar", SAMLIdentifiers.AttributeNameFormats.Basic, null, answer));
                else if (_TransmittalOptionsList.Items[i].Value == "PersonalInfo")
                    attributeStatement.Attributes.Add(new SAMLAttribute("PersonalInfo", SAMLIdentifiers.AttributeNameFormats.Basic, null, answer));
                else if (_TransmittalOptionsList.Items[i].Value == "Welcome")
                    attributeStatement.Attributes.Add(new SAMLAttribute("Welcome", SAMLIdentifiers.AttributeNameFormats.Basic, null, answer));
                else if (_TransmittalOptionsList.Items[i].Value == "Review")
                    attributeStatement.Attributes.Add(new SAMLAttribute("Review", SAMLIdentifiers.AttributeNameFormats.Basic, null, answer));
            }

            samlAssertion.Statements.Add(attributeStatement);

//          EncryptedAssertion encryptedAssertion = new EncryptedAssertion(samlAssertion, selerixCertificate, new EncryptionMethod(EncryptedXml.XmlEncTripleDESUrl));
//          samlResponse.Assertions.Add(encryptedAssertion);
            samlResponse.Assertions.Add(samlAssertion);

            //Created SAML response

            //Sending SAML response

            // Serialize the SAML response for transmission.
            XmlElement samlResponseXml = samlResponse.ToXml();

            // Sign the SAML response.
            SAMLMessageSignature.Generate(samlResponseXml, vendorCertificate.PrivateKey, vendorCertificate);

            HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache");
            HttpContext.Current.Response.AddHeader("Pragma", "no-cache");

            IdentityProvider.SendSAMLResponseByHTTPPost(HttpContext.Current.Response, assertionConsumerServiceURL, samlResponseXml, "");// for test purposes
        }

        private static System.Web.UI.WebControls.Table BuildTable(int intRows, int intCols, string tableCellClass)
        {
            System.Web.UI.WebControls.Table table = new System.Web.UI.WebControls.Table();

            for (int r = 0; r < intRows; r++)
                table.Rows.Add(CreateTableRow(intCols, tableCellClass));

            return table;
        }

        private static TableRow CreateTableRow(int intCols, string tableCellClass)
        {
            TableRow tr = new TableRow();

            for (int c = 0; c < intCols; c++)
            {
                TableCell tc = new TableCell();

                if (!string.IsNullOrEmpty(tableCellClass))
                    tc.CssClass = tableCellClass;

                tr.Cells.Add(tc);
            }

            return tr;
        }

        private string PrepareSourceCode(string src)
        {
            string res = "";

            if (!string.IsNullOrEmpty(src))
            {
                int lineNumber = 0;
                foreach (string line in src.Split('\n'))
                {
                    if (res != "")
                        res += "<br/>";

                    res += (++lineNumber) + ": " + line.Replace("\t", "&nbsp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\r", "");
                }
            }

            return res;
        }
    }
}
