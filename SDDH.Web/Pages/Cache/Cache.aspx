<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Cache.aspx.cs" Inherits="SDDH.Web.Pages.Cache" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lbKey" runat="server" Text="缓存Key："></asp:Label><asp:TextBox ID="txtKey" runat="server"></asp:TextBox><br />
            <asp:Label ID="lbValue" runat="server" Text="缓存Value："></asp:Label><asp:TextBox ID="txtValue" runat="server"></asp:TextBox><br />
            <asp:Button ID="btnSet" runat="server" Text="Set缓存" OnClick="btnSet_Click" />
            <asp:Button ID="btnGet" runat="server" Text="Get缓存" OnClick="btnGet_Click" />
            <asp:Button ID="btnRemove" runat="server" Text="Remove缓存" OnClick="btnRemove_Click" /><br />
            <textarea id="txtResult" runat="server" cols="20" rows="2" style="width: 400px; height: 300px;"></textarea>
        </div>
    </form>
</body>
</html>
