using System.IdentityModel.Selectors;
using System;
using System.ServiceModel;
namespace BSTIntegrationExample
{
    public class ServiceUserValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            if (null == userName || null == password)
            {
                throw new ArgumentNullException();
            }

            if(userName != "username" || password != "password")
                throw new FaultException("Unknown Username or Incorrect Password");
        }
    }
}