<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ServicesExample.aspx.cs" ValidateRequest="false" Inherits="BSTIntegrationExample.ServicesExample" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <a href="Default.aspx">Home</a>
    <br />
    <table width="100%">
        <tr> 
            <td style="width:50%">
                <asp:Label runat="server">1) Upload Example:</asp:Label>
            </td>
            <td style="width:50%">
                <asp:Label runat="server">2) Query Example:</asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width:50%">
                <asp:DropDownList Width="150px" ID="uploadType" AutoPostBack="true" runat="server">
                    <asp:ListItem Text="Upload Group" Value="UploadGroup"  Selected="True"/>
                    <asp:ListItem Text="Upload Census" Value="UploadCensus"/>
                </asp:DropDownList>
            </td>
            <td style="width:50%">
                <asp:DropDownList Width="150px" ID="queryType" AutoPostBack="true" runat="server">
                    <asp:ListItem Text="Get Group" Value="GetGroup" Selected="True"/>
                    <asp:ListItem Text="Get Census" Value="GetCensus"/>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width:50%">
                <asp:TextBox TextMode="MultiLine" runat="server" ID="txtUploadTransmittalBox" ReadOnly="false" Height="300px" Width="100%">
                </asp:TextBox>
            </td>
            <td style="width:50%">
                <asp:TextBox TextMode="MultiLine" runat="server" ID="txtQueryTransmittalBox" ReadOnly="false" Height="300px" Width="100%">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width:50%">
                <asp:Button runat="server" ID="btnUpload" Text="Upload" />
            </td>
            <td style="width:50%">
                <asp:Button runat="server" ID="btnQuery" Text="Query" />
            </td>
        </tr>
        <tr>
            <td style="width:50%">
                <asp:Label runat="server">Result:</asp:Label>
            </td>
            <td style="width:50%">
                <asp:Label runat="server">Result:</asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width:50%">
                <asp:TextBox TextMode="MultiLine" runat="server" ID="txtUploadResult" ReadOnly="true" Height="300px" Width="100%">
                </asp:TextBox>
            </td>
            <td style="width:50%">
                <asp:TextBox TextMode="MultiLine" runat="server" ID="txtQueryResult" ReadOnly="true" Height="300px" Width="99%">
                </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label runat="server">3) Get Login Guid Example:</asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table>
                    <tr>
                        <td align="left">
                            <asp:Label runat="server">Portfolio ID</asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtPortfolioID" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label runat="server">Employee ID</asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtEmployeeID" Width="300px" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnGetLoginGuid" runat="server" Text="Get" />
                <asp:Label ID="lblLoginGuid" runat="server">login_guid=</asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
