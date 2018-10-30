using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace BSTIntegrationExample.WebService
{
    /// <summary>
    /// Enrollment service. Surrports single sign to enrollment site.
    /// </summary>
    [WebService(Namespace = "https://benefits-selection.com/qx/enrollment")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Enrollment : System.Web.Services.WebService
    { 
        [WebMethod(EnableSession = true)]
        public string Upload(string user, string passwd, string transmittal)
        {
            BSTIntegrationExample.Enrollment svc = new BSTIntegrationExample.Enrollment();

            return svc.Upload(transmittal);
        }

        [WebMethod(EnableSession = true)]
        public string Query(string user, string passwd, string transmittal)
        {
            BSTIntegrationExample.Enrollment svc = new BSTIntegrationExample.Enrollment();

            return svc.Query(transmittal);
        }

        [WebMethod(EnableSession = true)]
        public Guid GetLoginGUID(string user, string passwd, Guid portfolioID, Guid uniqueID)
        {
            BSTIntegrationExample.Enrollment svc = new BSTIntegrationExample.Enrollment();

            return svc.GetLoginGUID(portfolioID, uniqueID);
        }
    }
}
