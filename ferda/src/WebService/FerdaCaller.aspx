<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FerdaCaller.aspx.cs" Inherits="Ferda.WebService.FerdaCaller" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ferda caller</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="DataProvider" runat="server" Width="400px"/><br />
        <asp:TextBox ID="ConnectionString" runat="server" Width="400px"/><br />
        <asp:TextBox ID="DataTableName" runat="server" Width="400px"/><br />
        <asp:FileUpload ID="PmmlUpload" runat="server" Width="400px" /><br />
        <asp:Button ID="CallButton" runat="server" Text="Call" 
            onclick="CallButton_Click" />
    </div>
    <p>
        <asp:TextBox ID="Result" runat="server" Height="196px" Width="800px"/>
    </p>
    </form>
</body>
</html>
