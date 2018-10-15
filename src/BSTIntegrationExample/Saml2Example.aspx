<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Saml2Example.aspx.cs" Inherits="BSTIntegrationExample.Saml2Example" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title> 
</head>
<body>
    <form id="form1" runat="server">
        <table width="100%">
            <tr style="height:40px">
                <td align="left" style="width:auto"><a href="Default.aspx">Home</a></td>
                <td align="center" colspan="2" style="height:40px">
                    Header.
                </td>
            </tr>
            <tr>
                <td align="center" style="width:100px">
                    Left part
                </td>
                <td>
                    <table width="100%">
                        <tr>
                            <td align="center">
                                <iframe width="100%" height="700px" runat="server" id="bstIframe" src="Saml2Request.aspx" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td align="center" style="width:100px">
                    Right part
                </td>
            </tr>
            <tr style="height:40px">
                <td align="center" colspan="3" style="height:40px">
                    Footer.
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
