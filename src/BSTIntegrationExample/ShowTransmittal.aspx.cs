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
using Selerix.Foundation.Data;
using System.Xml;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace BSTIntegrationExample
{
    public partial class ShowTransmittal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string samlXml = string.Empty;
            string transmittalXML = string.Empty;

            if (Session["SAML_XML"] != null)
                samlXml = Session["SAML_XML"].ToString();
            else
            {
                samlXml = "SAML Xml not found in session";
            }

            if (Session["SAMLParameters"] != null)
            {
                StringBuilder htmlOut = new StringBuilder();
                foreach (var pair in (Dictionary<string, string>)Session["SAMLParameters"])
                {
                    if (string.Compare(pair.Key, "Transmittal", true) != 0)
                    {
                        htmlOut.AppendFormat("Parameter : <b>[{0}]</b> Value : [{1}]<br/>", Page.Server.HtmlEncode(pair.Key), Page.Server.HtmlEncode(pair.Value));
                    }
                }

                litParameters.Text = htmlOut.ToString();

                if (string.IsNullOrEmpty(litParameters.Text))
                    litParameters.Text = "Parameters are not found in saml xml.";
            }

            if (Session["Transmittal"] != null)
                transmittalXML = SerializationHelper.SerializeToString(Session["Transmittal"]);
            else
                transmittalXML = "Transmittal is not found in saml xml.";

            if (Request["Download"] == "saml")
            {
                Response.Clear();
                Response.ContentType = "application/xml";
                Response.Write(samlXml);
                Response.End();
            }

            if (Request["Download"] == "transmittal")
            {
                Response.Clear();
                Response.ContentType = "application/xml";
                Response.Write(transmittalXML);
                Response.End();
            }

            try
            {
                litTransmittal.Text = FormatXml(transmittalXML);
            }
            catch
            {
                litTransmittal.Text = transmittalXML;
            }

            litSaml.Text = FormatXml(samlXml);
        }

        private string FormatXml(string sUnformattedXml)
        {
            //load unformatted xml into a dom

            XmlDocument xd = new XmlDocument();
            xd.LoadXml(sUnformattedXml);

            //will hold formatted xml

            StringBuilder sb = new StringBuilder();

            //pumps the formatted xml into the StringBuilder above

            StringWriter sw = new StringWriter(sb);

            //does the formatting

            XmlTextWriter xtw = null;

            try
            {
                //point the xtw at the StringWriter

                xtw = new XmlTextWriter(sw);

                //we want the output formatted

                xtw.Formatting = Formatting.Indented;

                //get the dom to dump its contents into the xtw 

                xd.WriteTo(xtw);
            }
            finally
            {
                //clean up even if error

                if (xtw != null)
                    xtw.Close();
            }

            //return the formatted xml

            string result = sb.ToString();

            //result = result.Replace("\n", "<BR/>");
            return PrepareSourceCode(result);
        }

        private string PrepareSourceCode(string src)
        {
            string res = "";

            if (!string.IsNullOrEmpty(src))
            {
                //int lineNumber = 0;
                foreach (string line in src.Split('\n'))
                {
                    if (res != "")
                        res += "<br/>";

                    res += //(++lineNumber) + ": " +
                        line.Replace("\t", "&nbsp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\r", "").Replace(" ", "&nbsp;");
                }
            }

            return res;
        }
    }
}
