using System;
using System.Collections.Generic;
using System.Web;
using System.ServiceModel.Channels;
using System.Xml;
using System.ServiceModel.Description;

namespace BSTIntegrationExample
{
    /// <summary>
    /// Summary description for HttpTransportBindingElementWithWSDL
    /// </summary>
    public sealed class HttpTransportBindingElementWithWSDL : HttpTransportBindingElement, ITransportTokenAssertionProvider, IPolicyExportExtension
    {
        /// <summary>
        /// Initializes a new instance of the HttpTransportBindingElementWithWSDL class. 
        /// </summary>
        public HttpTransportBindingElementWithWSDL() : base() 
        { }

        /// <summary>
        /// Initializes a new instance of the HttpTransportBindingElementWithWSDL class using another binding element.
        /// </summary>
        /// <param name="elementToBeCloned">An HttpTransportBindingElementWithWSDL object used to initialize this instance.</param>
        public HttpTransportBindingElementWithWSDL(HttpTransportBindingElementWithWSDL elementToBeCloned) : base(elementToBeCloned) 
        { }

        /// <summary>
        /// Creates a new instance that is a copy of the current binding element.
        /// </summary>
        /// <returns>A new instance that is a copy of the current binding element.</returns>
        public override BindingElement Clone()
        {
            HttpTransportBindingElementWithWSDL retval = new HttpTransportBindingElementWithWSDL(this);
            return retval;
        }

        #region ITransportTokenAssertionProvider Members

        /// <summary>
        /// Gets a transport token assertion.
        /// </summary>
        /// <returns>An XmlElement that represents a transport token assertion.</returns>
        public XmlElement GetTransportTokenAssertion()
        {
            string secpolNS0 = "http://schemas.xmlsoap.org/ws/2005/07/securitypolicy";
            XmlDocument xmldoc = new XmlDocument();
            XmlElement TransTokAssert = xmldoc.CreateElement("sp", "HttpToken", secpolNS0);
            return TransTokAssert;
        }

        #endregion


        #region IPolicyExportExtension Members

        /// <summary>
        /// Implement to include for exporting a custom policy assertion about bindings.
        /// </summary>
        /// <param name="exporter">The MetadataExporter that you can use to modify the exporting process.</param>
        /// <param name="context">The PolicyConversionContext that you can use to insert your custom policy assertion.</param>
        void IPolicyExportExtension.ExportPolicy(MetadataExporter exporter, PolicyConversionContext context)
        {
            HttpsTransportBindingElement httpsTBE = new HttpsTransportBindingElement();
            ((IPolicyExportExtension)httpsTBE).ExportPolicy(exporter, context);
        }

        #endregion
    }
}
