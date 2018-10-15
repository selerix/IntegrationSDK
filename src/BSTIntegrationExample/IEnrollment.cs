using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BSTIntegrationExample
{
    [ServiceContract(Namespace = "benefitagent.com")]
    public interface IEnrollment
    {
        [OperationContract]
        Guid GetLoginGUID(Guid portfolioID, Guid uniqueID);

        [OperationContract]
        string Query(string data);

        [OperationContract]
        string Upload(string data);
    }
}
