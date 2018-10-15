<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BSTIntegrationExample.Default" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server"> 
    <div>
        <asp:Label runat="server">Welcome to Selerix's System Integration SDK:</asp:Label>
        <br />
        <br />
        <a href="Saml2Example.aspx">1. Applicant SSO using SAML 2.0 version</a>
        <br />
        <a href="Saml1Example.aspx">2. Applicant SSO using SAML 1.1 version</a>
        <br />
        <a href="Saml2BAAgentExample.aspx">3. Agent SSO using SAML 2.0 version</a>
        <br />
        <a href="ServicesExample.aspx">4. Web Service call examples</a>
        <br />
    </div>
    </form>
</body>
</html>
