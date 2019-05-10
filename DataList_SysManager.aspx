<%@ Page Title="" Language="C#" MasterPageFile="~/eHRMaster.master" AutoEventWireup="true" CodeFile="DataList_SysManager.aspx.cs" Inherits="DataList_SysManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <style type="text/css">
         .transparent 
         { 
             BORDER-RIGHT: indianred 0px solid; 
             BORDER-TOP: indianred 0px solid; 
             DISPLAY: none; 
             FILTER: alpha(opacity=100); 
             BORDER-LEFT: indianred 0px solid; 
             BORDER-BOTTOM: indianred 0px solid; 
             POSITION: absolute; 
             BACKGROUND-COLOR: infobackground;
             font-family:Arial;  
         }
         .txtStyle{font-size:12px; Width:100%; height:40px;width:150px;border:1px solid gray;}
         .ddlStyle{border: 1px solid #C0C0C0;width: 120px;height: 30px;clip: rect( 0px, 181px, 20px, 0px );
                   overflow: hidden;}
        .style3
        {
            width: 97px;
        }
        .style5
        {
            width: 100px;
        }
        .style6
        {
            width: 534px;
        }
     </style>
     <%--<link rel="stylesheet" type="text/css" href="Styles/FormTable.css" />--%>
   <script type="text/javascript" src="Scripts/Calendar/WdatePicker.js" charset="gb2312"></script>  

   <script type="text/javascript">
       $(function () {

           $("#ContentPlaceHolder1_txt_NewUserID").keyup(search);
       });
       
       function search() {

           var v = $("#ContentPlaceHolder1_txt_NewUserID").val();

           if (v == "") {
               $("#ContentPlaceHolder1_txt_NewUserName").text("");
               $("#ContentPlaceHolder1_tmp_Name").val("");
           }

           else {
               var v1 = DataList_SysManager.GetUserName(v).value;

               if (v1 != "") {
                   $("#ContentPlaceHolder1_txt_NewUserName").text(v1);
                   $("#ContentPlaceHolder1_tmp_Name").val(v1);
               }
               else {
                   $("#ContentPlaceHolder1_txt_NewUserName").text("無此人！");
                   $("#ContentPlaceHolder1_tmp_Name").val("");
               }

           }

       }


    </script>
    
   <%--表格上方空隔--%>
    <div style="margin-top:15px;"> 
      
    <div id="page_matter">
    <asp:HiddenField ID="tmp_Name" runat="server" />
        <table class="tb01">
            <caption >管理員清單</caption>
            </table>
       
       <asp:Panel ID="Panel_Content" runat="server">
              <table class="table01">
                  <tr>
                      <th class="style3">
                          表單名稱</th>
                      <td>
                          <asp:DropDownList ID="TableName" runat="server" Width="230px">
                          
                          </asp:DropDownList>
                      </td>
                      <th class="style5">
                          廠區</th>
                      <td>
                          <asp:DropDownList ID="AreaName" runat="server"  Width="230px">
                              <asp:ListItem Value="0">---Please Select---</asp:ListItem>
                              <asp:ListItem Value="TW">TW</asp:ListItem>
                              <asp:ListItem Value="SJ">SJ</asp:ListItem>
                              <asp:ListItem Value="KS">KS</asp:ListItem>
                              <asp:ListItem Value="ZZ">ZZ</asp:ListItem>
                              <asp:ListItem Value="WSJ">WSJ</asp:ListItem>
                          </asp:DropDownList>
                      </td>
                  </tr>

                  </table>
                  <table class="table01">
                  <tr>
                      <th>
                          工號</th>
                      <td class="style6">
                          <asp:TextBox ID="txt_NewUserID" runat="server"></asp:TextBox>
                          &nbsp;<asp:Label ID="txt_NewUserName" runat="server" ForeColor="Blue"></asp:Label>
                      </td>
                      <td>
                          <asp:Button ID="btnAdd" runat="server" CssClass="btn" OnClick="lbtn_Yes_Click" 
                              Text="新增" />
                          <asp:Button ID="btnQuery" runat="server" CssClass="btn" 
                              OnClick="btnQueryClick" Text="查詢" />
                      </td>
                  </tr>

     </table> 

        <%--</div>--%>
        <div>		
            <asp:GridView ID="GridView" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                    Height="40px" Width="100%" CssClass="tb02"
                    EmptyDataText="尚無資料" ShowHeaderWhenEmpty="True" AllowPaging="True" 
                PageSize="20" onrowdatabound="GridView_RowDataBound" OnPageIndexChanging="GridView_PageIndexChanging"
                 OnRowDeleting="GridView_RowDeleting">
                            <Columns>
                                <asp:BoundField HeaderText="行號" ReadOnly="true" />
                                <asp:BoundField DataField="TableName" HeaderText="權限表單"/>
                                <asp:BoundField DataField="AreaName" HeaderText="權限廠區"/>
                                 <asp:TemplateField HeaderText="工號">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_UserID" runat="server" Text='<%# Bind("UserID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                         </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="姓名">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_UserName" runat="server" Text='<%# Bind("UserName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                         </asp:TemplateField>
                                                         <asp:BoundField DataField="CreateMan" HeaderText="建立人員"/>
                                                         <asp:BoundField DataField="CreateDate" HeaderText="建立時間"/>
                                                         <asp:CommandField ShowDeleteButton="true"> 
                                                         
                                                         </asp:CommandField>

                            </Columns>
                    <EmptyDataTemplate>
                        <font color="red" size="5pt" style="font-family: 微軟正黑體; font-size: medium; color: #FF0000">尚無資料</font>
                    </EmptyDataTemplate>
                <FooterStyle CssClass="GridViewFooterStyle" />
                <RowStyle CssClass="GridViewRowStyle"/>   
                <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                <PagerStyle CssClass="GridViewPagerStyle" />
                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                <HeaderStyle CssClass="GridViewHeaderStyle"  />
            </asp:GridView>
        </div>
        </asp:Panel>
    </div>
     
    
</asp:Content>

