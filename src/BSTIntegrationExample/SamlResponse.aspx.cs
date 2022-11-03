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
using System.Security.Cryptography.X509Certificates;
using ComponentSpace.SAML2.Protocols;
using System.Xml;
using ComponentSpace.SAML2.Profiles.SSOBrowser;
using ComponentSpace.SAML2.Assertions;
using System.Collections.Generic;
using Selerix.BusinessObjects;
using System.IO;

namespace BSTIntegrationExample
{
    public partial class SamlResponse : System.Web.UI.Page
    {
        private string CheckSamlVersion()
        {
            try
            {
                byte[] bytes = System.Convert.FromBase64String(Request.Params["SAMLResponse"]);
                string xml = System.Text.ASCIIEncoding.ASCII.GetString(bytes);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                if (doc.DocumentElement.Attributes["Version"] != null)
                    return doc.DocumentElement.Attributes["Version"].Value;
                else if (doc.DocumentElement.Attributes["MajorVersion"] != null && doc.DocumentElement.Attributes["MinorVersion"] != null)
                {
                    return doc.DocumentElement.Attributes["MajorVersion"].Value + "." + doc.DocumentElement.Attributes["MinorVersion"].Value;
                }
            }
            catch (Exception e)
            {
                throw new Selerix.Foundation.SelerixException("Cannot identify SAML version", e);
            }

            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string samlVersion = CheckSamlVersion();

            if (samlVersion == "2.0")
                ProcessSAMLResponse();
            else if (samlVersion == "1.1")
                ProcessSAML1Response();
            else
                throw new Selerix.Foundation.SelerixException(String.Format("Unknown version of SAML [{0}]", samlVersion));

        }

        private void ProcessSAML1Response()
        {
            // Receive the SAML response.
            ComponentSpace.SAML.Protocol.Response samlResponse = null;

            ReceiveSAML1Response(out samlResponse);

            // Check whether the SAML response indicates success or an error and process accordingly.
            if (samlResponse.Status.IsSuccess())
            {
                ProcessSuccessSAML1Response(samlResponse);
            }
            else
            {
                ProcessErrorSAML1Response(samlResponse);
            }
        }

        private void ProcessSuccessSAML1Response(ComponentSpace.SAML.Protocol.Response samlResponse)
        {
            //Processing successful SAML response

            // Extract the asserted identity from the SAML response.
            ComponentSpace.SAML.Assertions.Assertion samlAssertion = null;

            if (samlResponse.GetAssertions().Count > 0)
            {
                samlAssertion = samlResponse.GetAssertions()[0];
            }
            else if (samlResponse.GetSignedAssertions().Count > 0)
            {
                samlAssertion = new ComponentSpace.SAML.Assertions.Assertion(samlResponse.GetSignedAssertions()[0]);
            }
            else
            {
                throw new ArgumentException("No assertions in response");
            }

            Dictionary<string, string> outputData = new Dictionary<string, string>();

            foreach (ComponentSpace.SAML.Assertions.AttributeStatement attributeStatement in samlAssertion.GetAttributeStatements())
            {
                foreach (ComponentSpace.SAML.Assertions.Attribute samlAttribute in attributeStatement.Attributes)
                {
                    foreach (ComponentSpace.SAML.Assertions.AttributeValue attributeValue in samlAttribute.Values)
                    {
                        if (!outputData.ContainsKey(samlAttribute.Name))
                            outputData.Add(samlAttribute.Name, attributeValue.ToString());
                        else
                            outputData[samlAttribute.Name] = attributeValue.ToString();
                    }
                }
            }

            // prevent the output of aspx page from being cached by the browser
            Response.AddHeader("Cache-Control", "no-cache");
            Response.AddHeader("Pragma", "no-cache");

            if (outputData.ContainsKey("Transmittal"))
                Session["Transmittal"] = Selerix.Foundation.Data.SerializationHelper.DeserializeFromString(outputData["Transmittal"], typeof(Transmittal));
            else
                Session["Transmittal"] = null;

            Session["SAMLParameters"] = outputData;

            Response.Redirect("~/ShowTransmittal.aspx", false);

            //"Processed successful SAML response");
        }

        private void ProcessErrorSAML1Response(ComponentSpace.SAML.Protocol.Response samlResponse)
        {
            //"Processing error SAML response");

            string errorMessage = null;

            if (samlResponse.Status.StatusMessage != null)
            {
                errorMessage = samlResponse.Status.StatusMessage.Message;
            }

            //Response.Redirect("~/Login.aspx", false);

            //"Processed error SAML response");
        }

        private void ReceiveSAML1Response(out ComponentSpace.SAML.Protocol.Response samlResponse)
        {
            // Receive the SAML response over the specified binding.
            XmlElement samlResponseXml = null;

            samlResponseXml = ComponentSpace.SAML.SAML.FromBase64String(Request.Params["SAMLResponse"]);

            Session["SAML_XML"] = samlResponseXml.OuterXml;

            // Verify the response's signature.
            if (ComponentSpace.SAML.Protocol.ResponseSignature.IsSigned(samlResponseXml))
            {
                //"Verifying response signature");
                X509Certificate2 x509Certificate = GetVendorCertificate();

                if (!ComponentSpace.SAML.Protocol.ResponseSignature.Verify(samlResponseXml, x509Certificate))
                {
                    throw new ArgumentException("The SAML response signature failed to verify.");
                }
            }

            // Deserialize the XML.
            samlResponse = new ComponentSpace.SAML.Protocol.Response(samlResponseXml);
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


        // Process the SAML response returned by the identity provider in response
        // to the authentication request sent by the service provider.
        private void ProcessSAMLResponse()
        {
            // Receive the SAML response.
            SAMLResponse samlResponse = null;
            string relayState = null;

            ReceiveSAMLResponse(out samlResponse, out relayState);

            // Check whether the SAML response indicates success or an error and process accordingly.
            if (samlResponse.IsSuccess())
            {
                ProcessSuccessSAMLResponse(samlResponse, relayState);
            }
            else
            {
                ProcessErrorSAMLResponse(samlResponse);
            }
        }

        // Receive the SAML response from the identity provider.
        private void ReceiveSAMLResponse(out SAMLResponse samlResponse, out string relayState)
        {
            // Receive the SAML response over the specified binding.
            XmlElement samlResponseXml = null;

            ServiceProvider.ReceiveSAMLResponseByHTTPPost(Request, out samlResponseXml, out relayState);

            Session["SAML_XML"] = samlResponseXml.OuterXml;

            // Verify the response's signature.
            if (SAMLMessageSignature.IsSigned(samlResponseXml))
            {
               //Verifying response signature

                X509Certificate2 x509Certificate = GetVendorCertificate();

                if (!SAMLMessageSignature.Verify(samlResponseXml, x509Certificate))
                {
                    throw new ArgumentException("The SAML response signature failed to verify.");
                }
            }

            // Deserialize the XML.
            samlResponse = new SAMLResponse(samlResponseXml);
        }

        // Process a successful SAML response.
        private void ProcessSuccessSAMLResponse(SAMLResponse samlResponse, string relayState)
        {
            //Processing successful SAML response

            // Load the decryption key.
            X509Certificate2 x509Certificate = GetSelerixCertificate();

            // Extract the asserted identity from the SAML response.
            SAMLAssertion samlAssertion;

            if (samlResponse.GetUnsignedAssertions().Count > 0)
            {
                samlAssertion = samlResponse.GetUnsignedAssertions().FirstOrDefault();
            }
            else if (samlResponse.GetEncryptedAssertions().Count > 0)
            {
                //"Decrypting assertion");
                samlAssertion = samlResponse.GetEncryptedAssertions()[0].Decrypt(x509Certificate.PrivateKey, null);
            }
            else if (samlResponse.GetSignedAssertions().Count > 0)
            {
                samlAssertion = new SAMLAssertion(samlResponse.GetSignedAssertions()[0]);
            }
            else
            {
                throw new ArgumentException("No assertions in response");
            }

            // Get the subject name identifier.
            string userName = null;

            if (samlAssertion.Subject.NameID != null)
            {
                userName = samlAssertion.Subject.NameID.NameIdentifier;
            }
            else if (samlAssertion.Subject.EncryptedID != null)
            {
                //"Decrypting ID");
                NameID nameID = samlAssertion.Subject.EncryptedID.Decrypt(x509Certificate.PrivateKey, null);
                userName = nameID.NameIdentifier;
            }
            else
            {
                throw new ArgumentException("No name in subject");
            }

            Dictionary<string, string> outputData = new Dictionary<string, string>();

            foreach (AttributeStatement attributeStatement in samlAssertion.GetAttributeStatements())
            {
                foreach (SAMLAttribute samlAttribute in attributeStatement.GetUnencryptedAttributes())
                {
                    foreach (AttributeValue attributeValue in samlAttribute.Values)
                    {
                        if (!outputData.ContainsKey(samlAttribute.Name))
                            outputData.Add(samlAttribute.Name, attributeValue.ToString());
                        else
                            outputData[samlAttribute.Name] = attributeValue.ToString();
                    }
                }
                foreach (EncryptedAttribute encryptedAttribute in attributeStatement.GetEncryptedAttributes())
                {
                    SAMLAttribute samlAttribute = encryptedAttribute.Decrypt(x509Certificate.PrivateKey, null);
                    foreach (AttributeValue attributeValue in samlAttribute.Values)
                    {
                        if (!outputData.ContainsKey(samlAttribute.Name))
                            outputData.Add(samlAttribute.Name, attributeValue.ToString());
                        else
                            outputData[samlAttribute.Name] = attributeValue.ToString();
                    }
                }
            }

            // prevent the output of aspx page from being cached by the browser
            Response.AddHeader("Cache-Control", "no-cache");
            Response.AddHeader("Pragma", "no-cache");

            if (outputData.ContainsKey("Transmittal"))
                Session["Transmittal"] = Selerix.Foundation.Data.SerializationHelper.DeserializeFromString(outputData["Transmittal"], typeof(Transmittal));
            else
                Session["Transmittal"] = null;

            Session["SAMLParameters"] = outputData;
            
            Response.Redirect("~/ShowTransmittal.aspx", false);

            //"Processed successful SAML response");
        }

        // Process an error SAML response.
        private void ProcessErrorSAMLResponse(SAMLResponse samlResponse)
        {
            //"Processing error SAML response");

            string errorMessage = null;

            if (samlResponse.Status.StatusMessage != null)
            {
                errorMessage = samlResponse.Status.StatusMessage.Message;
            }

            //Response.Redirect("~/Login.aspx", false);

            //"Processed error SAML response");
        }
    }
}
