/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package qxwebservices;

import java.net.MalformedURLException;
import java.net.URL;
import java.util.UUID;
import javax.xml.ws.BindingProvider;

/**
 *
 * @author pavel
 */
public class QXWebServices {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        
		//Fidler support
        //System.setProperty("http.proxyHost", "127.0.0.1");
        //System.setProperty("https.proxyHost", "127.0.0.1");
        //System.setProperty("http.proxyPort", "8888");
        //System.setProperty("https.proxyPort", "8888");
        //System.setProperty("javax.net.ssl.trustStore", "C:/Projects/FiddlerKeystore");
        //System.setProperty("javax.net.ssl.trustStorePassword", "test98");
        
        
        String endpointURL = "https://demo.benselect.com/qx/enrollment.asmx";
        String user = "user";
        String password = "password";
        
        String senderID = UUID.randomUUID().toString();
        String portfolioID = "60C9BEEE-DCCF-437D-9C6A-E2B064C73442";
        
        //applicant unique id is hardcoded for sample only. ID will be returned as part of applicant upload transaction.
        String applicantID = "7ae8d9b9-77bc-421e-8910-dc6130ba2b95";
        
        URL url = null;
        
        try {
            url = new URL(endpointURL + "?WSDL");
        } catch (MalformedURLException ex) {
            System.out.println("Wrong URL: " + ex.getMessage());
            return;
        }
        
        //Create QX Enrollment web service client
        https.benefits_selection_com.qx.enrollment.Enrollment service = new https.benefits_selection_com.qx.enrollment.Enrollment(url);
        https.benefits_selection_com.qx.enrollment.EnrollmentSoap port = service.getEnrollmentSoap();
        
        //Set NEW Endpoint Location
        BindingProvider bp = (BindingProvider)port;
        bp.getRequestContext().put(BindingProvider.ENDPOINT_ADDRESS_PROPERTY, endpointURL);
        
        String request = null;
        
        //Portfolio query
        request = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
"<Transmittal Type=\"GetPortfolio\" SenderID=\"" + senderID + "\" PortfolioID=\"" + portfolioID + "\">\n" +
"</Transmittal>";
                        
        System.out.println("Server responce [Portfolio]:");
        System.out.println(port.query(user, password, request));
        
        //Upload applicant
        request = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" +
"<Transmittal Type=\"UploadApplicants\" SenderID=\"" + senderID + "\" PortfolioID=\"" + portfolioID + "\">\n" +
"  <Applicants>\n" +
"    <Applicant ID=\"1\" EmployeeID=\"1\">\n" +
"      <AsOfDate>2009-10-15T15:46:09</AsOfDate>\n" +
"      <Address Type=\"Personal\">\n" +
"        <Country>USA</Country>\n" +
"        <Line1>5110 E 107th Place</Line1>\n" +
"        <City>Frankford</City>\n" +
"        <State>KY</State>\n" +
"        <Zip>61849</Zip>\n" +
"      </Address>\n" +
"      <PhoneHome>(456) 461-8495</PhoneHome>\n" +
"      <SSN>556-66-2678</SSN>\n" +
"      <FirstName>Kathleen</FirstName>\n" +
"      <LastName>Abercrombie</LastName>\n" +
"      <Sex>Female</Sex>\n" +
"      <Employment Status=\"Active\">\n" +
"        <Employer>National Health</Employer>\n" +
"        <HireDate>1974-06-17T00:00:00</HireDate>\n" +
"        <EligibilityDate>1974-06-17T00:00:00</EligibilityDate>\n" +
"        <Title>RN</Title>\n" +
"        <Department>Unspecified</Department>\n" +
"        <DepartmentNumber />\n" +
"        <Location>CALIFORNIA</Location>\n" +
"        <JobClass>FT</JobClass>\n" +
"        <PayGroup>26/24</PayGroup>\n" +
"        <PayrollFrequency>26</PayrollFrequency>\n" +
"        <DeductionFrequency>24</DeductionFrequency>\n" +
"        <Salary>110000.0000</Salary>\n" +
"        <HourlyWage>0.0000</HourlyWage>\n" +
"        <FTERate>1.0000</FTERate>\n" +
"        <HoursPerWeek>36</HoursPerWeek>\n" +
"        <PTOBalance>0.0000</PTOBalance>\n" +
"        <PTOCost>0.0000</PTOCost>\n" +
"        <FederalTax>0</FederalTax>\n" +
"        <FederalUnemploymentTax>0</FederalUnemploymentTax>\n" +
"        <StateUnemploymentTax>0</StateUnemploymentTax>\n" +
"        <SocialSecurityTax>0</SocialSecurityTax>\n" +
"        <MedicareTax>0</MedicareTax>\n" +
"        <WorkersComp>0</WorkersComp>\n" +
"        <Bonus>0</Bonus>\n" +
"        <Commissions>0</Commissions>\n" +
"        <Overtime>0</Overtime>\n" +
"        <StockOptionGrantValue>0</StockOptionGrantValue>\n" +
"      </Employment>\n" +
"      <LegalStatus>Employee</LegalStatus>\n" +
"      <Relationship>Employee</Relationship>\n" +
"      <BirthDate>1951-10-29T00:00:00</BirthDate>\n" +
"      <BirthCountry>US</BirthCountry>\n" +
"      <SmokerStatus>Never</SmokerStatus>\n" +
"      <MaritalStatus>Married</MaritalStatus>\n" +
"      <Student>false</Student>\n" +
"      <Disabled>false</Disabled>\n" +
"      <EmployeeIdent>161849</EmployeeIdent>\n" +
"      <PIN>1849</PIN>\n" +
"      <PaymentInfo>\n" +
"        <PaymentType>Payroll</PaymentType>\n" +
"        <BankDraftDay>1</BankDraftDay>\n" +
"      </PaymentInfo>\n" +
"    </Applicant>\n" +
"  </Applicants>\n" +
"</Transmittal>";
        
        System.out.println("Server responce [Upload applicant]:");
        System.out.println(port.upload(user, password, request));
        
        //Get login token
        System.out.println("Server responce [get login token]:");
        System.out.println(port.getLoginGUID(user, password, portfolioID, applicantID));
               
        //Applicant query by unique id
        request = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
"<Transmittal Type=\"GetApplicants\" SenderID=\"" + senderID + "\" PortfolioID=\"" + portfolioID + "\">\n" +
"  <Applicants>\n" +
"    <Applicant UniqueID=\"" + applicantID + "\">\n" +
"    </Applicant>\n" +
"  </Applicants>\n" +
"</Transmittal>";
                        
        System.out.println("Server responce [Applicant by unique id]:");
        System.out.println(port.query(user, password, request));
        
        //Applicant query by SSN
        request = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
"<Transmittal Type=\"GetApplicants\" SenderID=\"" + senderID + "\" PortfolioID=\"" + portfolioID + "\">\n" +
"  <Applicants>\n" +
"    <Applicant>\n" +
"      <SSN>556-66-2678</SSN>\n" +
"    </Applicant>\n" +
"  </Applicants>\n" +
"</Transmittal>";
                        
        System.out.println("Server responce [Applicant by SSN]:");
        System.out.println(port.query(user, password, request));
    }
}
