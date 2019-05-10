<%@ Page Title="" Language="C#" MasterPageFile="~/eHRMaster.master" AutoEventWireup="true" CodeFile="DataList_Driver.aspx.cs" Inherits="DataList_SysManager" %>

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
     </style>
     <%--<link rel="stylesheet" type="text/css" href="Styles/FormTable.css" />--%>
   <script type="text/javascript" src="Scripts/Calendar/WdatePicker.js" charset="gb2312"></script>  

   <script type="text/javascript">
       $(function () {

           $("#ContentPlaceHolder1_txt_NewUserID").keyup(search);
       });
       
       function search() {
           var v = $("#ContentPlaceHolder1_txt_NewUserID").val();
//            alert(v);
           var v1 = DataList_SysManager.GetUserName(v).value;        //alert(v1);

           if (v1 != "") {
               $("#ContentPlaceHolder1_txt_NewUserName").text(v1);
               //                   alert($("#ContentPlaceHolder1_txt_NewUserName").text());
               $("#ContentPlaceHolder1_tmp_Name").val(v1);

           }
           else {
               $("#ContentPlaceHolder1_txt_NewUserName").val("無此人！");
           }
       }


    </script>
    
    <%--表格上方空隔--%>
    <div style="margin-top:15px;"> 
      
    <div id="page_matter">
    <asp:HiddenField ID="tmp_Name" runat="server" />
        <table class="tb01">
            <caption >車輛信息清單</caption>
            </table>
       
       <asp:Panel ID="Panel_Content" runat="server">
              <table class="table01">
                  <tr>
                      <th class="style3">
                          廠區</th>
                      <td>
                          <asp:DropDownList ID="txt_FactoryArea" runat="server" onchange="showfactoryarea()" 
                              RepeatDirection="Horizontal" Width="40%">
                              <asp:ListItem Value="0">---Please Select---</asp:ListItem>
                              <asp:ListItem Value="TW">TW</asp:ListItem>
                              <asp:ListItem Value="SJ">SJ</asp:ListItem>
                              <asp:ListItem Value="KS">KS</asp:ListItem>
                              <asp:ListItem Value="ZZ">ZZ</asp:ListItem>
                              <asp:ListItem Value="WSJ">WSJ</asp:ListItem>
                          </asp:DropDownList>
                      </td>
                      <th>
                          車牌號</th>
                      <td>
                          <asp:TextBox ID="txt_CarNo" runat="server"></asp:TextBox>
                      </td>
                  </tr>

                  <tr>
                      <th class="style3">
                          司機姓名</th>
                      <td>
                          <asp:TextBox ID="txt_DriverName" runat="server"></asp:TextBox>
                      </td>
                      <th class="style5">
                          司機電話</th>
                      <td>
                          <asp:TextBox ID="txt_DriverPhoneNo" runat="server"></asp:TextBox>
                      </td>
                  </tr>

                  <tr>
                      <th class="style3">
                          備註</th>
                      <td colspan="3">
                          <asp:TextBox ID="txt_Remark" runat="server" Width="50%"></asp:TextBox>
                      </td>
                  </tr>

                  </table>
                  <table class="table01">
                  <tr>
                      <td>
                          <asp:Button ID="btnAdd" runat="server" CssClass="btn" OnClick="lbtn_Yes_Click" 
                              Text="新增" />
                          <asp:Button ID="btnQuery" runat="server" CssClass="btn" OnClick="btnQueryClick" 
                              Text="查詢" />
                          </span>
                      </td>
                  </tr>
     </table> 

        <%--</div>--%>
        <div>		
            <asp:GridView ID="GridView" runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                    Height="40px" Width="100%" CssClass="tb02"
                    EmptyDataText="尚無資料" ShowHeaderWhenEmpty="True" AllowPaging="True" 
                PageSize="20" onrowdatabound="GridView_RowDataBound" OnPageIndexChanging="GridView_PageIndexChanging"
                 OnRowDeleting="GridView_RowDeleting" OnRowUpdating="GridView_RowUpdating" OnRowEditing="GridView_RowEditing"  OnRowCancelingEdit="GridView_RowCancelingEdit">
                            <Columns>
                                <asp:BoundField HeaderText="行號" ReadOnly="true"/>
                                <%--<asp:BoundField DataField="FactoryArea" HeaderText="廠區"/>--%>

                                <asp:TemplateField HeaderText="廠區">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="E_FactoryArea" runat="server" Text='<%# Bind("FactoryArea") %>' Width="40px"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="L_FactoryArea" runat="server" Text='<%# Bind("FactoryArea") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--<asp:BoundField DataField="CarNo" HeaderText="車牌號"/>--%>

                                <asp:TemplateField HeaderText="車牌號">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="E_CarNo" runat="server" Text='<%# Bind("CarNo") %>' Width="100px"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="L_CarNo" runat="server" Text='<%# Bind("CarNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--<asp:BoundField DataField="DriverName" HeaderText="司機姓名"/>--%>

                                <asp:TemplateField HeaderText="司機姓名">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="E_DriverName" runat="server" Text='<%# Bind("DriverName") %>' Width="100px"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="L_DriverName" runat="server" Text='<%# Bind("DriverName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--<asp:BoundField DataField="DriverPhoneNo" HeaderText="司機電話"/>--%>

                                 <asp:TemplateField HeaderText="司機電話">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="E_DriverPhoneNo" runat="server" Text='<%# Bind("DriverPhoneNo") %>' Width="200px"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="L_DriverPhoneNo" runat="server" Text='<%# Bind("DriverPhoneNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--<asp:BoundField DataField="Remark" HeaderText="Remark"/>--%>

                                <asp:TemplateField HeaderText="Remark">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="E_Remark" runat="server" Text='<%# Bind("Remark") %>' Width="100px"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="L_Remark" runat="server" Text='<%# Bind("Remark") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--<asp:BoundField DataField="Creator" HeaderText="建立人員"  HeaderStyle-Width="10%" ItemStyle-Width="10%"/>--%>

                                <asp:TemplateField HeaderText="建立人員">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="E_Creator" runat="server" Text='<%# Bind("Creator") %>' Width="80px" Enabled="false"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="L_Creator" runat="server" Text='<%# Bind("Creator") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--<asp:BoundField DataField="CreateDate" HeaderText="建立時間"  HeaderStyle-Width="15%" ItemStyle-Width="15%"/>--%>

                                <asp:TemplateField HeaderText="建立時間">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="E_CreateDate" runat="server" Text='<%# Bind("CreateDate") %>'  Width="90px"  Enabled="false"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="L_CreateDate" runat="server" Text='<%# Bind("CreateDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:CommandField ShowEditButton="True" ShowCancelButton="true" ShowDeleteButton="true"> 
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

