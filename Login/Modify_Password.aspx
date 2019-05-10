<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Modify_Password.aspx.cs" Inherits="Modify_Password" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>修改密碼</title>
    <link href="../App_Themes/login.css" rel="stylesheet" type="text/css" />
</head>
<body class="modi">
<form runat="server">
    <div id="modi">
        <table style="width:100%">
    <tr>
    <td><asp:TextBox ID="OldPS" style="width: 180px" runat="server" CssClass="text" required="true" TextMode="Password"></asp:TextBox>
    </td>
    </tr>
    <tr>
    <td><asp:TextBox ID="NewPS" style="width: 180px" runat="server" CssClass="text" required="true" TextMode="Password"></asp:TextBox>
    </td>
    </tr>
    <tr>
    <td><asp:TextBox ID="ReNewPS" style="width: 180px" runat="server" CssClass="text" required="true" TextMode="Password" ></asp:TextBox>
    </td>
    </tr>
    <tr>
    <td><span><asp:Button ID="btnCancel" runat="server" Text="" CssClass="cancel-button" onclick="btnCancel_Click"  /></span>
        <span><asp:Button ID="btnSubmit" runat="server" Text="" CssClass="submit-button" onclick="btnSubmit_Click"  /></span></td>
    </tr>
    </table>
    </div>    
    <div id="copyright">Copyright &copy; 2013.06 Design by ITD</div>
    </form>
</body>
</html>
