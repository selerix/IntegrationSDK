<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeBehind="Saml2BAAgentExample.aspx.cs" Inherits="BSTIntegrationExample.Saml2BAAgentExample" %>

<!DOCTYPE html>

<html>

<head runat="server">
    <title>SAML 2.0 BenefitAgent Agent Example</title>
    <link type="text/css" rel="Stylesheet" href="default.css" />  
</head>
     
<body>
    <form id="form1" runat="server">
        <div id="SamlAgentDiv" style="position: absolute; z-index: 100; visibility: hidden; margin: 0px 0px 0px 0px; padding: 0px 0px 0px 0px; width: 100%; height: 100%; background-color: white;" runat="server">
            <table style="width: 100%; height: 100%;">
                <tr>
                    <td style="vertical-align: middle; text-align: center;">
                        <img src="images/gears_ani.gif" alt="" />
                        <br /><br />
                        <h2 style="color: #4DA7DC;">'Single Sign On' BenefitAgent SAML 2.0 Trasmission is underway ...</h2>
                        <br /><br /><br /><br />
                    </td>
                </tr>
            </table>
        </div>
        <br /><a style="margin-left:50px;font-size:medium" href="Default.aspx">Home</a>
        <table class="SamlAgentTable" cellpadding="0" cellspacing="0" border="0" style="border: 0px;">
            <tr>
                <td width="15"><img src="images/inner_top_left.gif" alt="" /></td>
                <td colspan="2" class="clsMainTitleCell">Agent 'Single Sign On' BenefitAgent SAML 2.0 Example</td>
                <td width="15"><img src="images/inner_top_right.gif" alt="" /></td>
            </tr>
            <tr>
                <td class="clsMainCtrlLeftCell"></td>
                <td class="SamlAgentTableHeader" colspan="2"><b>Current Users:</b> 'Name', 'Phone' and 'Mailing Address' are not required.</td>
                <td class="clsMainCtrlRightCell"></td>
            </tr>
            <tr>
                <td class="clsMainCtrlLeftCell"></td>
                <td class="SamlAgentTableHeader"><asp:PlaceHolder ID="gridPlaceHolder" runat="server" /></td>
                <td class="SamlAgentTableHeader" width="240"><img src="images/intro_pic2.gif" alt="" /></td>
                <td class="clsMainCtrlRightCell"></td>
            </tr>
            <tr>
                <td class="clsMainCtrlBottomLeftCell"></td>
                <td colspan="2" class="clsMainCtrlBottomCell"></td>
                <td class="clsMainCtrlBottomRightCell"></td>
            </tr>
        </table>
    </form>
</body>
</html>
