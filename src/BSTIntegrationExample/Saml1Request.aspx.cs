using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Selerix.BusinessObjects;
using System.Security.Cryptography.X509Certificates;
using Selerix.Foundation.Data;
using Selerix.Foundation;
using System.Xml;
using System.IO;
using System.Collections.Specialized;

namespace BSTIntegrationExample
{
    public partial class Saml1Request : System.Web.UI.Page
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

            ComponentSpace.SAML.Protocol.Response samlResponse = new ComponentSpace.SAML.Protocol.Response();
            samlResponse.Recipient = assertionConsumerServiceURL;
            samlResponse.Status = new ComponentSpace.SAML.Protocol.Status(ComponentSpace.SAML.Protocol.StatusCode.Codes.Success);

            ComponentSpace.SAML.Assertions.Assertion samlAssertion = new ComponentSpace.SAML.Assertions.Assertion();
            samlAssertion.Issuer = "Vendor";

            ComponentSpace.SAML.Assertions.SubjectConfirmation subjectConfirmation = new ComponentSpace.SAML.Assertions.SubjectConfirmation(ComponentSpace.SAML.Assertions.ConfirmationMethod.Methods.Bearer);
            ComponentSpace.SAML.Assertions.Subject subject = new ComponentSpace.SAML.Assertions.Subject(new ComponentSpace.SAML.Assertions.NameIdentifier("", "", employeeID), subjectConfirmation);

            ComponentSpace.SAML.Assertions.SubjectConfirmationData subjectConfirmationData = new ComponentSpace.SAML.Assertions.SubjectConfirmationData();
            subjectConfirmation.SubjectConfirmationData = subjectConfirmationData;
            ComponentSpace.SAML.Assertions.Conditions conditions = new ComponentSpace.SAML.Assertions.Conditions(new TimeSpan(1, 0, 0));
            ComponentSpace.SAML.Assertions.AudienceRestrictionCondition audienceRestriction = new ComponentSpace.SAML.Assertions.AudienceRestrictionCondition();
            audienceRestriction.Audiences.Add(new ComponentSpace.SAML.Assertions.Audience(audienceName));
            conditions.ConditionsList.Add(audienceRestriction);
            samlAssertion.Conditions = conditions;

            ComponentSpace.SAML.Assertions.AuthenticationStatement authnStatement = new ComponentSpace.SAML.Assertions.AuthenticationStatement();
            authnStatement.Subject = subject;
            authnStatement.AuthenticationMethod = ComponentSpace.SAML.Assertions.AuthenticationStatement.AuthenticationMethods.Unspecified;
            samlAssertion.Statements.Add(authnStatement);

            ComponentSpace.SAML.Assertions.AttributeStatement attributeStatement = new ComponentSpace.SAML.Assertions.AttributeStatement();

            if (Session["Transmittal"] != null)
            {
                attributeStatement.Attributes.Add(new ComponentSpace.SAML.Assertions.Attribute("Transmittal", "", SerializationHelper.SerializeToString(transmittal)));
            }

            //Check for Transmittal Options
            for (int i = 0; i < _TransmittalOptionsList.Items.Count; i++)
            {
                string answer = "no";

                if (_TransmittalOptionsList.Items[i].Selected)
                    answer = "yes";

                if (_TransmittalOptionsList.Items[i].Value == "HeaderAndFooter")
                    attributeStatement.Attributes.Add(new ComponentSpace.SAML.Assertions.Attribute("HeaderAndFooter", "", answer));
                else if (_TransmittalOptionsList.Items[i].Value == "Sidebar")
                    attributeStatement.Attributes.Add(new ComponentSpace.SAML.Assertions.Attribute("Sidebar", "", answer));
                else if (_TransmittalOptionsList.Items[i].Value == "PersonalInfo")
                    attributeStatement.Attributes.Add(new ComponentSpace.SAML.Assertions.Attribute("PersonalInfo", "", answer));
                else if (_TransmittalOptionsList.Items[i].Value == "Welcome")
                    attributeStatement.Attributes.Add(new ComponentSpace.SAML.Assertions.Attribute("Welcome", "", answer));
                else if (_TransmittalOptionsList.Items[i].Value == "Review")
                    attributeStatement.Attributes.Add(new ComponentSpace.SAML.Assertions.Attribute("Review", "", answer));
            }

            samlAssertion.Statements.Add(attributeStatement);
            samlResponse.Assertions.Add(samlAssertion);


            //Created SAML response

            //Sending SAML response

            // Serialize the SAML response for transmission.
            XmlElement samlResponseXml = samlResponse.ToXml();

            // Sign the SAML response.
            ComponentSpace.SAML.Protocol.ResponseSignature.Generate(samlResponseXml, vendorCertificate.PrivateKey, vendorCertificate);

            HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache");
            HttpContext.Current.Response.AddHeader("Pragma", "no-cache");

            RemotePost remotePost = new RemotePost();
            remotePost.Add("SAMLResponse", ComponentSpace.SAML.SAML.ToBase64String(samlResponseXml));
            remotePost.Url = assertionConsumerServiceURL;
            remotePost.Post();
        }
    }

    /// <summary>
    /// Summary description for RemotePost.
    /// </summary>
    public class RemotePost
    {
        /// <summary>
        /// Named list of post variables
        /// </summary>
        private readonly NameValueCollection Inputs = new NameValueCollection();

        /// <summary>
        /// Designates the url of the remote post.
        /// </summary>
        public string Url { get; set; } = "";

        /// <summary>
        /// Designates the Post method to send the form.
        /// </summary>
        public string Method { get; set; } = "post";

        /// <summary>
        /// Designates the name of the form.
        /// </summary>
        public string FormName { get; set; } = "RemoteForm";

        /// <summary>
        /// Adds the specified name of the form.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void Add(string name, string value)
        {
            Inputs.Add(name, value);
        }

        /// <summary>
        /// Posts the specified page.
        /// </summary>
        /// <remarks></remarks>
        public void Post()
        {
            Post(string.Empty);
        }

        /// <summary>
        /// Posts the specified page.
        /// </summary>
        /// <param name="newWindowName">New name of the window.</param>
        /// <remarks></remarks>
        public void Post(string newWindowName)
        {
            System.Text.StringBuilder form = new System.Text.StringBuilder();

            form.Append("<!DOCTYPE html>");
            form.Append("<html><head>");

            form.Append(string.Format("</head><body>", FormName));
            form.Append(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\"", FormName, Method, Url));
            if (!string.IsNullOrEmpty(newWindowName))
            {
                form.Append(string.Format(" target=\"{0}\"", newWindowName));
            }

            form.Append(">");

            for (int i = 0; i < Inputs.Keys.Count; i++)
            {
                form.Append(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", Inputs.Keys[i], System.Web.HttpContext.Current.Server.HtmlEncode(Inputs[Inputs.Keys[i]])));
            }

            form.Append(@"
	            <script type='text/javascript'>
                <!--
		            var submitCount = 0;
                    window.onload = function() { if (submitCount == 0) document.forms[0].submit(); submitCount = submitCount + 1; }
                //-->       
	            </script>");

            form.Append("</form>");
            form.Append("</body></html>");

            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.Write(form.ToString());
            System.Web.HttpContext.Current.Response.End();
        }
    }
}
