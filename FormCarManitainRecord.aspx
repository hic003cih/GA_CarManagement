<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/eHRMaster.master" CodeFile="FormCarManitainRecord.aspx.cs" Inherits="FormCarManitainRecord" MaintainScrollPositionOnPostback="true"%>  


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript" src="Scripts/Calendar/WdatePicker.js" charset="gb2312"></script> 
    <div style="margin-top:15px;">
    <%--<form id="CarMaintain" runat="server">--%>
    <table class="table01">
    <caption>申請者基本資料</caption>
    <tr>
        <th class="style91">申請者工號</th>
        <td class="style92">
        <asp:Label ID="ApplyManNo" runat="server"></asp:Label>
        </td>
        <th class="style91"> 申請者 </th>
        <td class="style92" >
        <asp:Label ID="ApplyMan" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <th>申請單位</th>
        <td><asp:Label ID="ApplyDept" runat="server"></asp:Label></td>
        <th>申請日期</th>
        <td><asp:Label ID="ApplyDate" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <th>分機</th>
        <td><asp:Label ID="ApplyPhoneNo" runat="server"></asp:Label></td>
        <th>申請者廠區</th>
        <td><asp:Label ID="ApplyArea" runat="server"></asp:Label></td>
    </tr>
    </table>
       <table class="table01" id="YIsBudget"  >
            <tr >
                <th width="150px">車號<span class="requis">*</span></th>
                <th width="100px">維修日期<span class="requis">*</span></th>
                <th width="150px">維修憑證<span class="requis">*</span></th>    
                <th width="100px">掃描<span class="requis"></span></th>
                <th width="300px">備註<span class="requis"></span></th>
                <%--<th width="100px">預估金額</th>--%>
                <th width="100px">新增動作<span class="requis"></span></th>
                <%--<th width="100px">查詢動作<span class="requis"></span></th>--%>
            </tr>
             <tr >
                 <td width="150px" ><asp:DropDownList ID="CarNo" runat="server" Width="135px" Height="25px" AppendDataBoundItems="True" AutoPostBack="True"><asp:ListItem Value="0">請選擇</asp:ListItem></asp:DropDownList></td>
                 <td ><asp:TextBox ID="MaintainDate" runat="server" class="Wdate" onclick="WdatePicker({minDate:'%y-%M-#{%d}'})" Width="123px" Height="25px"></asp:TextBox></td>
                 <td width="150px" ><asp:TextBox runat="server" ID="MaintainCert" Height="25px" Width="131px"></asp:TextBox></td>
                 <td ><asp:TextBox runat="server" ID="ScanFiles" Width="122px" Height="25px"></asp:TextBox></td>
                 <td ><asp:TextBox runat="server" ID="Notice" width="300px" Height="25px"></asp:TextBox></td>
                 <%--<td ><asp:Label runat="server" ID="Money" width="100px"></asp:Label></td>--%>
                 <td width="100px"><asp:Button ID="Save"  runat="server" Text="新增" OnClick="Save_Click" class="table01_BtSubmit" Height="26px" Width="117px"/></td>
                 <%--<td width="100px"><asp:Button ID="Search"  runat="server" Text="查詢" OnClick="Search_Click" class="table01_BtSubmit" Height="26px" Width="117px"/></td>--%>
            </tr>
        </table>
        <%--GridView--%>
        <table class="table03">
              <asp:GridView ID="GRV1" runat="server" AutoGenerateColumns="False" DataKeyNames="CarNo" 
                     CssClass="tb04"
                     onrowdatabound="GridView_RowDataBound" OnPageIndexChanging="GridView_PageIndexChanging" OnRowEditing="GridView_RowEditing" OnRowUpdating="GridView_RowUpdating" OnRowCancelingEdit="GridView_RowCancelingEdit" OnRowDeleting="GridView_RowDeleting" EmptyDataText="尚未有任何資料" >
                  <EmptyDataTemplate>
                        <font color="red" size="5pt" style="font-family: 微軟正黑體; font-size: medium; color: #FF0000">尚無記錄</font>
                    </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="No" InsertVisible="False" >
                                <EditItemTemplate>                  
                                </EditItemTemplate>
                                <ItemTemplate >
                                 <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                                <ItemStyle Width="30px" />
                                </asp:TemplateField> 
                                <%--<asp:BoundField DataField="ID" HeaderText="表單單號"/>--%>
                                <%--<asp:TemplateField HeaderText="專案號碼">
                                    <ItemTemplate>
                                    <asp:HyperLink ID="HyperLink1" runat="server" Text='<%# Eval("UserNo") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>                         
                               <%-- <asp:BoundField DataField="ID" HeaderText="序號" readonly="true"/>--%>
                                <asp:BoundField DataField="CarNo" HeaderText="車牌號碼" readonly="true"/>
                                <%--<asp:BoundField DataField="MaintainDate" HeaderText="維修日期" />--%>
                                <asp:TemplateField HeaderText="維修日期">
                                <EditItemTemplate>
                                <asp:TextBox ID="MaintainDate" runat="server" class="Wdate" HtmlEncode="false" onclick="WdatePicker({startDate:'%y-%M-#{%d+1}',dateFmt:'yyyy/MM/dd'})" Height="20px" Text='<%# Bind("MaintainDate","{0:yyyy/MM/dd}") %>' ></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                <asp:Label ID="MaintainDate_Label1" runat="server"  HtmlEncode="false"  Text='<%# Bind("MaintainDate","{0:yyyy/MM/dd}") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="250px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="維修憑證">
                                <EditItemTemplate>
                                <asp:TextBox ID="MaintainCert" runat="server" Height="20px"  Text='<%# Bind("MaintainCert") %>' ></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                <asp:Label ID="MaintainCert_Label" runat="server" Text='<%# Bind("MaintainCert") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="250px" />
                                </asp:TemplateField>
                                <%--<asp:BoundField DataField="MaintainCert" HeaderText="維修憑證" />--%>
                                <%--<asp:BoundField DataField="Notice" HeaderText="備註"/>--%>
                                <asp:TemplateField HeaderText="備註">
                                <EditItemTemplate>
                                <asp:TextBox ID="Notice" runat="server" Height="20px"  Text='<%# Bind("Notice") %>' ></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                <asp:Label ID="Notice_Label1" runat="server" Text='<%# Bind("Notice") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="250px" />
                                </asp:TemplateField>
                                <asp:CommandField ShowEditButton="True" ShowDeleteButton="true" HeaderStyle-Width="50px" Visible="true">
                                <ControlStyle CssClass="btn04" />
                                <HeaderStyle Width="100px" />
                                <ItemStyle Width="60px" />
                                </asp:CommandField>
                            </Columns>
                    <RowStyle  />
                <%--<FooterStyle CssClass="GridViewFooterStyle" />
                <RowStyle CssClass="GridViewRowStyle" />   
                <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                <PagerStyle CssClass="GridViewPagerStyle"  HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                <HeaderStyle CssClass="GridViewHeaderStyle" />--%>
            </asp:GridView>
                   
                    </table>
       <%-- </form>--%>
    </div>
    
</asp:Content>





<%--<asp:Content ID="Content2" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style1 {
            width: 160px;
        }
    </style>
</asp:Content>--%>






