<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormList.aspx.cs" Inherits="FormList" MasterPageFile="~/eHRMaster.master"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">     
    <script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>   
     <%--  <script type="text/javascript">
           $(function () {
               var formCount = '<%=getFormListCountByUserID()%>';
           $("#ContentPlaceHolder1_Label1").text("(" + formCount + ")");
       });
    </script>   --%>
    <%--表格上方空隔--%>
    <div style="margin-top:15px;"> 
  <div>
    <table border="0" cellspacing="2" cellpadding="2" class="tb01">
    <caption>待簽核文件清單
        <%--<asp:Label ID="Label1" runat="server" style="color: red;font-family: Arial;font-size:larger" Text=" (X)"></asp:Label> --%><!-- 待處理筆數-->
    </caption>
    </table>
  </div>
    <div >
        <table class="tb02" >
            <asp:GridView ID="GridView" runat="server" AutoGenerateColumns="False" DataKeyNames="FormNo"
                     Width="100%" class="tb02"     ShowHeaderWhenEmpty="True" AllowPaging="True"           
                 OnRowDataBound="GridView_RowDataBound" OnPageIndexChanging="GridView_PageIndexChanging" >
                            <Columns>
                                <asp:BoundField HeaderText="序號" ReadOnly="true"   />
                                <%--<asp:BoundField DataField="FormNo" HeaderText="表單單號"/>--%>
                                <asp:TemplateField HeaderText="表單單號">
                                    <ItemTemplate>
                                       <%-- <asp:HyperLink ID="lbl_id" runat="server" NavigateUrl='<%#Eval("FormUrl")+"?formNo=" + Eval("FormNo")+"&formTypeName="+Eval("FormTypeName")+"&formType="+Eval("FormType")+"&formID="+Eval("FormID")+"&status="+Eval("Form_Status")%>' Text='<%# Eval("FormNo") %>'></asp:HyperLink>--%>
                                     <%-- <asp:HyperLink ID="lbl_id" runat="server" NavigateUrl='<%#  Eval("UrlLink")+"&uid="+Session["UserID"].ToString().Trim()%>' Text='<%# Eval("FormNo") %>'></asp:HyperLink>--%>
                                        <asp:HyperLink ID="lbl_id" runat="server" Text='<%# Eval("FormNo") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="FormTypeName" HeaderText="表單名稱"/>
                                <asp:BoundField DataField="Form_Status" HeaderText="狀態" />
                                <asp:BoundField DataField="Register_Name" HeaderText="申請者" />
                                <asp:BoundField DataField="ApplyDate" DataFormatString="{0:d}"  HeaderText="申請日期"/>
                            <asp:TemplateField HeaderText="FormID" Visible="false">
                                    <ItemTemplate>
                                       <asp:Label ID="FormID" runat="server" Text='<%# Eval("FormID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="UrlLink" Visible="false">
                                    <ItemTemplate>
                                       <asp:Label ID="UrlLink" runat="server" Text='<%# Eval("UrlLink") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="FormUrl" Visible="false">
                                    <ItemTemplate>
                                       <asp:Label ID="FormUrl" runat="server" Text='<%# Eval("FormUrl") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="FormType" Visible="false">
                                    <ItemTemplate>
                                       <asp:Label ID="FormType" runat="server" Text='<%# Eval("FormType") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                    <EmptyDataTemplate>
                        <font color="red" size="5pt" style="font-family: 微軟正黑體; font-size: medium; color: #FF0000">尚無資料</font>
                    </EmptyDataTemplate>
                    <RowStyle  />
                    <PagerTemplate>
                        當前第:
                        <asp:Label ID="LabelCurrentPage" runat="server" Text="<%# ((GridView)Container.NamingContainer).PageIndex + 1 %>"></asp:Label>
                        頁/共:
                        <asp:Label ID="LabelPageCount" runat="server" Text="<%# ((GridView)Container.NamingContainer).PageCount %>"></asp:Label>
                        頁
                        <asp:LinkButton ID="LinkButtonFirstPage" runat="server" CommandArgument="First" CommandName="Page"
                            Visible='<%#((GridView)Container.NamingContainer).PageIndex != 0 %>'>首頁</asp:LinkButton>
                        <asp:LinkButton ID="LinkButtonPreviousPage" runat="server" CommandArgument="Prev"
                            CommandName="Page" Visible='<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>'>上一頁</asp:LinkButton>
                        <asp:LinkButton ID="LinkButtonNextPage" runat="server" CommandArgument="Next" CommandName="Page"
                            Visible='<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>'>下一頁</asp:LinkButton>
                        <asp:LinkButton ID="LinkButtonLastPage" runat="server" CommandArgument="Last" CommandName="Page"
                            Visible='<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>'>尾頁</asp:LinkButton>
                        轉到第
                        <asp:TextBox ID="txtNewPageIndex" runat="server" Width="20px" Text='<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>' />頁
                        <asp:LinkButton ID="btnGo" runat="server" CausesValidation="False" CommandArgument="-2"
                            CommandName="Page" Text="GO" />
                    </PagerTemplate>
                <FooterStyle CssClass="GridViewFooterStyle" />
                <RowStyle CssClass="GridViewRowStyle" />   
                <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                <PagerStyle CssClass="GridViewPagerStyle"  HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                <HeaderStyle CssClass="GridViewHeaderStyle" />
            </asp:GridView>
        </table>     
        </div>
      </div>   
    <!--footer-->
<div id="footer">
<div id="footer_line"><img src="images/line.png" width="1" height="1" alt=""/>
<table width="1100" border="0" cellspacing="0" cellpadding="0" align="center" style=" background-image:url(images/line.png); background-repeat:repeat-x; background-position:top;">
  
    <tr class="footer_text">
      <td align="center" style="padding-top:20px;">連絡資訊  TW : 518-2374 (偉琳) / 518-2252 (芸安)  |   CN : 570-60862 (穎琦)</td>
    </tr>
 
</table></div>
</div>
</asp:Content>
