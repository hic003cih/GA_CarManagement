<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sign Form Portal Login</title> 
    <link rel="stylesheet" type="text/css" href="../Styles/Master_style.css" /> 
    <style type="text/css">
        .style1
        {
            width: 244px;
        }
    </style>
</head>
<body style="background: url(/GA_CarManagement/images/content_bg.png);">
<form id="Form1" runat="server">
<input type="hidden" id="hiddenDeptCode" runat="server" />
<div id="main_container">
    <div id="header" style="background:url(/GA_CarManagement/images/banner.jpg) no-repeat ;" >
    <div class="banner_adds">
    <!--login -->
       <!-- login -->  
       <div id="logo">
        <a href="http://10.56.69.77/FitiGroup/"><img src="/GA_CarManagement/images/fitigroup.png" width="150" height="48" alt="線上簽核系統" title="線上簽核系統" border="0" /></a>
       </div> 
        
    </div>
        <div class="system_title" style ="font-family:微軟正黑體, Arial, Helvetica, sans-serif, Roboto;">線上簽核系統</div>
    </div> 
   <div id="login">
        <table style="width: 100%">
            <tr>
                <td>
                    <asp:TextBox ID="UserID" style="width: 180px" runat="server" CssClass="text" required="true" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="UserPS" style="width: 180px" runat="server" CssClass="text" required="true"  TextMode="Password" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="rem">
                        <asp:CheckBox ID="RememberMe" runat="server" CssClass="ck" Text="記住我"/>
                    </span>
                   <span>
                        <asp:Button ID="btnLogin" runat="server" Text="" CssClass="login-button" OnClick="login_Click" />
                    </span>
                </td>
            </tr>
            <tr>
            <td>
                <span>
                    <asp:Button ID="btnForget" runat="server" Text="" CssClass="forget-button" onclick="btnForget_Click" />
                </span>
                <span>
                    <asp:Button ID="btnModify" runat="server" Text="" CssClass="modify-button" onclick="btnModify_Click" />
                </span>
            </td>
            </tr>
        </table>   
    </div>   
<div class="clear"></div>

        <!-- End Services -->
        <div id="footer">
        <div id="copyrights">
             FITI ITD .&copy; All Rights Reserved 2015 <br />
             連絡資訊 : <br /> 
             TW : 518-2374 (偉琳) / 518-2252 (芸安)&nbsp; 
             |&nbsp;&nbsp;
             CN : 570-60862 (穎琦)
        </div> 
    </div>
    </div>
    </form>
</body>
</html>
