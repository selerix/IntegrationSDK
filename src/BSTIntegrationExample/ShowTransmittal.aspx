<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowTransmittal.aspx.cs" Inherits="BSTIntegrationExample.ShowTransmittal" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <script type="text/javascript">
    <!--
        function popup(mylink, windowname) {
            if (!window.focus) return true;
            var href; 
            if (typeof (mylink) == 'string')
                href = mylink;
            else
                href = mylink.href;
            window.open(href, windowname);
            return false;
        }
    //-->
    </script>
    <form id="form1" runat="server">
    <div>
        <a href="ShowTransmittal.aspx?Download=saml" onclick="return popup(this, 'SAML')">SAML XML</a>
        <div style="overflow: scroll; width: 100%; position: relative; height: 230pt; background-color: #efefff;
        text-align: left">
            <p style="font-size: 10pt; font-family: 'Arial'; background-color: #efefff;">
                <asp:Literal ID="litSaml" runat="server"></asp:Literal>
            </p>
        </div>
        <br />
        <asp:Literal id="litParameters" runat="server"></asp:Literal>
        <br />
        <a href="ShowTransmittal.aspx?Download=transmittal" onclick="return popup(this, 'Transmittal')">Transmittal</a>
                <div style="overflow: scroll; width: 100%; position: relative; height: 230pt; background-color: #efefff;
        text-align: left">
            <p style="font-size: 10pt; font-family: 'Arial'; background-color: #efefff;">
                <asp:Literal ID="litTransmittal" runat="server"></asp:Literal>
            </p>
        </div>
    </div>
    </form>
</body>
</html>
