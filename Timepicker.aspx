<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Timepicker.aspx.cs" Inherits="Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
<%--<link href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" rel="stylesheet" type="text/css" />
<script src="http://code.jquery.com/jquery-1.9.1.js"></script>
<script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
<script src="http://bililite.com/inc/jquery.timepickr.js"></script>--%> 
<link href="Styles/jquery-ui.css" rel="stylesheet" type="text/css" />
<script src="Scripts/Timepicker/jquery-1.9.1.js"></script>
<script src="Scripts/Timepicker/jquery-ui.js"></script>
<script src="Scripts/Timepicker/jquery.timepickr.js"></script>
    <script type="text/javascript">
     
        $(function () {
            $('.timepicker').timepickr({
          convention: 12,
          rangeMin: ['00', '05', '10', '15', '20', '25', '30', '35', '40', '45', '50', '55'],
                timeFormat:  "hh:mm:ss"
          }).mouseover();
        });
        

</script>

<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>


<body>
    <form runat="server">
                <!-- AJAX tool 局部刷新的Script-->
     <asp:ScriptManager ID="SendScriptManager" runat="server" AsyncPostBackTimeout="600"></asp:ScriptManager> 
    <asp:TextBox ID="TextBox1" runat="server" CssClass="timepicker" OnTextChanged="TextBox1_TextChanged" AutoPostBack="true"></asp:TextBox>
                <br />
<input name="TextBox2" type="text" cssclass="timepicker" onchange="javascript:setTimeout('__doPostBack(\'TextBox2s\',\'\')', 0)" onkeypress="if (WebForm_TextBoxKeyHandler(event) == false) return false;" id="TextBox2" />
        <br />
        <asp:UpdatePanel ID="DaysUpdatePanel" runat="server">
            <ContentTemplate>
        <asp:Label ID="testasb" runat="server"></asp:Label>
                </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID ="TextBox1" EventName ="TextChanged" />
        </Triggers>
        </asp:UpdatePanel>
    </form>
  
    
</body>
</html>
