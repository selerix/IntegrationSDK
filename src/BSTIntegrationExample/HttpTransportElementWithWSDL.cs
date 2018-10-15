using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.ServiceModel.Configuration;
using System.Configuration;

namespace BSTIntegrationExample
{
    /// <summary>
    /// Summary description for HttpTransportElementWithWSDL
    /// </summary>
    public class HttpTransportElementWithWSDL : HttpTransportElement
    {
        /// <summary>
        /// Gets the type of binding.
        /// </summary>
        public override Type BindingElementType
        {
            get
            {
                return typeof(HttpTransportBindingElementWithWSDL);
            }
        }

        /// <summary>
        /// Returns a custom binding element object with default values.
        /// </summary>
        /// <returns>A custom BindingElement object with default values.</returns>
        protected override TransportBindingElement CreateDefaultBindingElement()
        {
            HttpTransportBindingElementWithWSDL retval =
                new HttpTransportBindingElementWithWSDL();
            return retval;
        }

        /// <summary>
        /// Applies the settings of the specified BindingElement to this configuration element.
        /// </summary>
        /// <param name="bindingElement">The BindingElement to this configuration element.</param>
        public override void ApplyConfiguration(BindingElement bindingElement)
        {
            base.ApplyConfiguration(bindingElement);
        }

        /// <summary>
        /// Creates a new custom binding element object whose properties are copied from the settings of this configuration element.
        /// </summary>
        /// <returns>A custom BindingElement object.</returns>
        protected override BindingElement CreateBindingElement()
        {
            var result = base.CreateBindingElement();

            return result;
        }
    }
}
