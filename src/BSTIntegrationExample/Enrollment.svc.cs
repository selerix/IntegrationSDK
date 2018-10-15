using System;
using System.Collections.Generic;
using System.ServiceModel;
using Selerix.BusinessObjects;
using Selerix.Foundation.Data;
using System.ServiceModel.Activation;
using Selerix.Foundation;

namespace BSTIntegrationExample
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Enrollment" in code, svc and config file together.
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class Enrollment : IEnrollment
    {
        private static List<Portfolio> _Portfolios = new List<Portfolio>();

        /// <summary>
        /// <employee id, transmittal with census>
        /// </summary>
        private static Dictionary<string, List<Transmittal>> _Census = new Dictionary<string, List<Transmittal>>();

        /// <summary>
        /// Allows single sign on applicant with unique id. Has to be logged first with CheckUser
        /// </summary>
        /// <param name="uniqueID">The unique ID.</param>
        /// <returns></returns>
        public Guid GetLoginGUID(Guid portfolioID, Guid uniqueID)
        {
            return Guid.NewGuid();
        }

        public string Query(string transmittal)
        {
            Transmittal response = new Transmittal();
            response.Result = new Result();
            response.Result.Status = ResultStatus.OK;

            Transmittal request = null;

            try
            {
                if (string.IsNullOrEmpty(transmittal))
                    throw new Exception("Transmittal parameter is not valid xml.");

                request = (Transmittal)SerializationHelper.DeserializeFromString(transmittal, typeof(Transmittal));

                switch (request.Type)
                {
                    case TransmittalType.GetPortfolio:
                        Portfolio p = FindPortfolio(request);

                        if (p == null)
                            if (request.Portfolio.UniqueID != Guid.Empty)
                                throw new Exception(
                                    String.Format("Couldn't find portfolio \"{0}\" [{1}] or user doesn't have access to it", request.Portfolio.Name, request.Portfolio.UniqueID));
                            else
                                throw new Exception(
                                    String.Format("Couldn't find portfolio \"{0}\" or user doesn't have access to it", request.Portfolio.Name));


                        response.Portfolio = p;

                        break;
                    case TransmittalType.Query:

                        Transmittal t = FindCensus(request);

                        if (t == null)
                            throw new Exception("Couldn't find employee");

                        response.Applicants = t.Applicants;
                        response.Applications = t.Applications;
                        response.Group = t.Group;
                        
                        break;
                    default:
                        throw new Exception("Not supported transmittal type");
                }
            }
            catch (Exception e)
            {
                response.Result.Status = ResultStatus.Error;
                response.Result.Error = e.Message;
            }

            string result = SerializationHelper.SerializeToString(response);

            return result;
        }

        private Transmittal FindCensus(Transmittal request)
        {
            if (request.Group == null || string.IsNullOrEmpty(request.Group.GroupName))
                throw new Exception("Request missing group name");

            if(request.Applicants == null || request.Applicants.Count == 0)
                throw new Exception("Request missing applicants collection");

            Applicant employee = null;

            if(request.Applicants != null)
                foreach (var applicant in request.Applicants)
                {
                    if (applicant.Relationship == Relationship.Employee || applicant.Relationship == Relationship.Unknown)
                    {
                        if (employee != null)
                            throw new Exception("Multiple employees in request");

                        employee = applicant;
                    }
                }

            if (employee == null)
                throw new Exception("Couldn't find employee in request");

            if (string.IsNullOrEmpty(employee.SSN))
                throw new Exception("Employee is missing SSN");

            Transmittal result = null;

            if (_Census.ContainsKey(request.Group.GroupName))
            {
                foreach (var transmittal in _Census[request.Group.GroupName])
                {
                    Applicant transmittalEmployee = null;

                    foreach (var applicant in transmittal.Applicants)
                    {
                        if (applicant.Relationship == Relationship.Employee || applicant.Relationship == Relationship.Unknown)
                        {
                            transmittalEmployee = applicant;
                        }
                    }

                    if (transmittalEmployee != null && transmittalEmployee.SSN == employee.SSN)
                    {
                        result = transmittal;

                        break;
                    }
                }
            }

            return result;
        }

        private Portfolio FindPortfolio(Transmittal request)
        {
            if (
                    (request.Group == null || string.IsNullOrEmpty(request.Group.GroupName)) &&
                    request.PortfolioID == Guid.Empty &&
                    (
                     request.Portfolio == null ||
                     (string.IsNullOrEmpty(request.Portfolio.Name) && request.Portfolio.UniqueID == Guid.Empty)
                    )
                )
                throw new Exception("Portfolio search criteria (PortfolioID, Name or UniqueID) is missing.");

            Portfolio portfolio = null;

            if (request.PortfolioID != Guid.Empty)
            {
                foreach (var p in _Portfolios)
                {
                    if (p.UniqueID == request.PortfolioID)
                    {
                        portfolio = p;
                        break;
                    }
                }
            }

            if(portfolio == null && request.Portfolio != null && request.Portfolio.UniqueID != Guid.Empty)           
                foreach (var p in _Portfolios)
                {
                    if (p.UniqueID == request.Portfolio.UniqueID)
                    {
                        portfolio = p;
                        break;
                    }
                }
                

            string groupName = request.Group != null ? request.Group.GroupName : request.Portfolio != null ? request.Portfolio.Name : null;
            string groupNumber = request.Group != null ? request.Group.GroupNumber : request.Portfolio != null ? request.Portfolio.GroupNumber : null;

            if (portfolio == null && (!string.IsNullOrEmpty(groupName) || !string.IsNullOrEmpty(groupNumber)))
            {
                if (portfolio == null && !string.IsNullOrEmpty(groupName))
                {
                    List<Portfolio> portfolios = new List<Portfolio>();

                    foreach (var p in _Portfolios)
                    {
                        if (p.Name == groupName.Trim())
                        {
                            portfolios.Add(p);
                        }
                    }

                    if (portfolios.Count > 1 && !string.IsNullOrEmpty(groupNumber))
                    {
                        List<Portfolio> filteredPortfolios = new List<Portfolio>();

                        foreach (var p in portfolios)
                        {

                            if (p.GroupNumber == groupNumber)
                                filteredPortfolios.Add(p);
                        }

                        portfolios = filteredPortfolios;
                    }

                    if (portfolios.Count > 0)
                        portfolio = portfolios[0];
                }
            }

            return portfolio;
        }

        public string Upload(string transmittal)
        {
            Transmittal response = new Transmittal();
            response.Result = new Result();
            response.Result.Status = ResultStatus.OK;

            Transmittal request = null;

            try
            {
                if (string.IsNullOrEmpty(transmittal))
                    throw new Exception("Transmittal parameter is not valid xml.");

                request = (Transmittal)SerializationHelper.DeserializeFromString(transmittal, typeof(Transmittal));

                switch (request.Type)
                {
                    case TransmittalType.UploadPortfolio:
                        Portfolio portfolio = FindPortfolio(request);

                        if (portfolio != null)
                        {
                            request.Portfolio.UniqueID = portfolio.UniqueID;
                            _Portfolios.Remove(portfolio);
                            _Portfolios.Add(request.Portfolio);

                            response.PortfolioID = request.Portfolio.UniqueID;
                        }
                        else
                        {
                            //create new one.

                            request.Portfolio.UniqueID = Guid.NewGuid();

                            _Portfolios.Add(request.Portfolio);

                            response.PortfolioID = request.Portfolio.UniqueID;
                        }

                        break;
                    case TransmittalType.UploadApplicants:

                        Portfolio p = FindPortfolio(request);

                        if (p == null)
                            throw new Exception("Portfolio is not found");

                        Transmittal t = FindCensus(request);

                        if (t != null)
                        {
                            _Census[request.Group.GroupName].Remove(t);                           
                        }

                        foreach (var applicant in request.Applicants)
                                if (applicant.UniqueID == Guid.Empty)
                                    applicant.UniqueID = Guid.NewGuid();

                        if (!_Census.ContainsKey(request.Group.GroupName))
                            _Census.Add(request.Group.GroupName, new List<Transmittal>());

                       _Census[request.Group.GroupName].Add(request);

                        break;
                    default:
                        throw new Exception("Not supported transmittal type");
                }
            }
            catch (Exception e)
            {
                response.Result.Status = ResultStatus.Error;
                response.Result.Error = e.Message;
            }

            string result = SerializationHelper.SerializeToString(response);

            return result;
        }
    }
}
