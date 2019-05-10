<%@ Page Language="C#"  MasterPageFile="~/eHRMaster.master" AutoEventWireup="true" CodeFile="V_FormReportSearch.aspx.cs" Inherits="V_FormReportSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
<%--日曆外掛--%>
<script type="text/javascript" src="Scripts/Calendar/WdatePicker.js" charset="gb2312"></script>


    <asp:Panel ID="Panel_Content" runat="server">
    <div>
         <table class="table01">             
         <tr>
              <th>日期範圍(出發時間)
                     </th>
                 <td>
                     <asp:TextBox ID="txt_Departuretime"  runat="server" CssClass="ddl60" 
                    onclick="WdatePicker()"></asp:TextBox>
                    至<asp:TextBox ID="txt_Departuretime2"  runat="server" CssClass="ddl60" 
                    onclick="WdatePicker()"></asp:TextBox>
             </td>
             <td>
                <asp:Button ID="btnQuery0"  CssClass="btn" runat="server" Text="查詢" onclick="btnQuery_Click"  />
               <%-- <asp:Button ID="btnExport" CssClass="btn" runat="server" Text="匯出" onclick="btnExport_Click" Visible="false"/>--%>
            </td>
             <td>
                 <asp:Button ID="Button1"  CssClass="btn" runat="server" Text="匯出Excel" onclick="Button1_Click"  />
               <%-- <asp:Button ID="btnExport" CssClass="btn" runat="server" Text="匯出" onclick="btnExport_Click" Visible="false"/>--%>
            </td>
        </tr>
                 </table>
             <div style="width:1100px;">
                         <asp:GridView ID="GRV1" runat="server" AutoGenerateColumns="False" DataKeyNames="FormNo"
                    Height="40px" Width="100%" CssClass="tb02"
                    EmptyDataText="沒有記錄" ShowHeaderWhenEmpty="True" AllowPaging="True" 
                PageSize="20" Font-Size="10px" onrowdatabound="GridView_RowDataBound" 
                    OnPageIndexChanging="GridView_PageIndexChanging">
            <Columns>
                <asp:BoundField HeaderText="行號" ReadOnly="true" />
                <asp:BoundField DataField="FormNo" HeaderText="編號" />
                <asp:BoundField DataField="ApplyManNo" HeaderText="工號" />
                <asp:BoundField DataField="ApplyMan" HeaderText="姓名" />
                <asp:BoundField DataField="ApplyDate" HeaderText="日期" />
                <asp:BoundField DataField="FactoryArea" HeaderText="廠區" />
                <asp:BoundField DataField="CarType" HeaderText="用車類型" />
                <%--<asp:BoundField DataField="ApplyDept" HeaderText="使用車種" />--%>
                <asp:BoundField DataField="ManList" HeaderText="用車人員"/>
                <asp:BoundField DataField="Departuretime" HeaderText="出發時間" />
                <asp:BoundField DataField="Prereturntime" HeaderText="預返時間" />
                <asp:BoundField DataField="FeeCode" HeaderText="費用代碼" />
                <asp:BoundField DataField="Destination" HeaderText="目的地"  />
                <asp:BoundField DataField="Purpose" HeaderText="事由"  />
                <asp:BoundField DataField="CarNo" HeaderText="車牌" />
                <asp:BoundField DataField="DriverName" HeaderText="司機" />
                <asp:BoundField DataField="DisManNo" HeaderText="乘車評價人員" />
                <asp:BoundField DataField="Feedback_Service" HeaderText="服務"  />
                <asp:BoundField DataField="Feedback_Clean" HeaderText="衛生"  />
                <asp:BoundField DataField="Feedback_Satisfied" HeaderText="滿意度" />
            </Columns>
            <EmptyDataTemplate>
                        <font color="red" size="5pt" style="font-family: 微軟正黑體; font-size: medium; color: #FF0000">尚無資料</font>
            </EmptyDataTemplate>
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
                <RowStyle CssClass="GridViewRowStyle"/>   
                <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                <PagerStyle CssClass="GridViewPagerStyle" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                <HeaderStyle CssClass="GridViewHeaderStyle" Width="10px"  />
        </asp:GridView>
             </div>

    </div>
    </asp:Panel>
</body>
</asp:Content>