<%@ Page Title="" Language="C#" MasterPageFile="~/eHRMaster.master" AutoEventWireup="true" CodeFile="FormInFlow.aspx.cs" Inherits="FormInFlow" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  
     <script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>  
    <script type="text/javascript" src="Scripts/Calendar/WdatePicker.js" charset="gb2312"></script>

    <script type="text/javascript">
        $(function () {
            $("#ContentPlaceHolder1_btnExport").attr("disabled", true);
        })

        function Show(FormNo, FormTypeName, ApplyDate, ApplyMan) {
            document.getElementById("td1").innerText = "表單編號：" + FormNo;
            document.getElementById("td2").innerText = "表單名稱：" + FormTypeName;
            document.getElementById("td6").innerText = "申請時間：" + ApplyDate;
            document.getElementById("td11").innerText = "申請人姓名：" + ApplyMan;

            //获得鼠标的X轴的坐标
            x = event.clientX + document.body.scrollLeft;

            //获得鼠标的Y轴的坐标


            y = event.clientY + document.body.scrollTop + 20;

            //显示弹出窗体
            Popup.style.display = "block";

            //设置窗体的X,Y轴的坐标
            Popup.style.left = x;
            Popup.style.top = y;
        }

        //隐藏弹出窗体
        function Hide() {
            //隐藏窗体
            Popup.style.display = "none";
        }

        //頁面全屏控制
        function FullScreen() {
            window.resizeTo(screen.availWidth, screen.availHeight);
            window.moveTo(0, 0);
        }
        window.onload = FullScreen;

    </script>
    <link rel="stylesheet" type="text/css" href="Styles/FormTable.css" />
    <table class="table01_systemtitle">
        <tr>
            <td>未歸檔文件清單(For Manager)
            </td>
        </tr>
    </table>
    <div id="page_matter">
        <asp:Panel ID="Panel_Content" runat="server">
            <table class="table01">
                <tr>
                    <th class="auto-style1">申請單別:</th>
                    <td class="auto-style1">
                        <asp:DropDownList ID="txt_formType" runat="server">
                        </asp:DropDownList>
                    </td>
                    <th class="auto-style1">申請者:</th>
                    <td class="auto-style1">
                        <asp:TextBox ID="txt_applyMan" runat="server" CssClass="ddl60"></asp:TextBox>
                    </td>
                    <th class="auto-style1" rowspan="2">動作:</th>
                    <td class="auto-style1" rowspan="2">
                        <asp:Button ID="btnQuery" CssClass="btn" runat="server" Text="查詢" OnClick="btnQuery_Click" />
                        <asp:Button ID="btnExport" CssClass="btn" runat="server" Text="匯出" OnClick="btnExport_Click" />
                    </td>
                </tr>
                <tr>
                    <th>申請單號:</th>
                    <td>
                        <asp:TextBox ID="txt_NoSendformId" runat="server"></asp:TextBox></td>
                    <th>申請日期:</th>
                    <td>
                        <asp:TextBox ID="txt_applyDate" runat="server" CssClass="ddl60" onclick="WdatePicker()"></asp:TextBox>
                        至<asp:TextBox ID="txt_applyDate2" runat="server" CssClass="ddl60" onclick="WdatePicker()"></asp:TextBox>
                    </td>
                </tr>
            </table>

            <%--</div>--%>
            <div>
                <asp:GridView ID="GridView" runat="server" AutoGenerateColumns="False" DataKeyNames="FormNo"
                    Height="14px" Width="100%" CssClass="GridViewStyle"
                    EmptyDataText="尚無權限" ShowHeaderWhenEmpty="True" AllowPaging="True"
                    PageSize="20" OnRowDataBound="GridView_RowDataBound"
                    OnPageIndexChanging="GridView_PageIndexChanging"
                    OnRowCommand="GridView_RowCommand">
                    <Columns>
                        <asp:BoundField HeaderText="行號" ReadOnly="true" />
                        <asp:TemplateField HeaderText="表單單號">
                            <ItemTemplate>
                                <asp:HyperLink ID="lbl_id" runat="server" NavigateUrl='<%#Eval("FormUrl")+"?formNo=" + Eval("FormNo")+"&formType="+Eval("FormType")+"&formTypeName="+Eval("FormTypeName")+"&formID="+Eval("FormID")+"&status="+Eval("NodeName")%>' Text='<%# Eval("FormNo") %>'></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="FormTypeName" HeaderText="表單名稱" />
                        <asp:BoundField DataField="Form_Status" HeaderText="當前狀態" />
                        <%--<asp:TemplateField HeaderText="出差行程">
                            <ItemTemplate>
                                <asp:Label ID="Day" runat="server" Text='<%#  Eval("FromDay","{0:yyyy/MM/dd}")+"-"+Eval("Today","{0:yyyy/MM/dd}")%>'></asp:Label>
                            </ItemTemplate>
                            <FooterStyle Font-Size="8pt" />
                        </asp:TemplateField>--%>
                        <asp:BoundField DataField="Register_Name" HeaderText="申請者" />
                        <asp:BoundField DataField="ApplyDate" DataFormatString="{0:d}" HeaderText="申請日期" />

                        <asp:TemplateField HeaderText="動作">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" CommandArgument='<%#Bind("FormID") %>'
                                    CommandName="updateDel"
                                    Text="作廢"></asp:LinkButton>
                            </ItemTemplate>
                            <ControlStyle Width="40px" />
                        </asp:TemplateField>

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
                    <RowStyle CssClass="GridViewRowStyle" />
                    <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                    <PagerStyle CssClass="GridViewPagerStyle" />
                    <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                    <HeaderStyle CssClass="GridViewHeaderStyle" />
                </asp:GridView>
            </div>
        </asp:Panel>
    </div>

    <div id="Popup" class="transparent" style="Z-INDEX: 200">

        <table border="0" cellpadding="0" style="font-size: 13px;" width="250px">
            <tr>
                <td valign="middle" bgcolor="gray"><font color="white">表單詳細信息</font></td>
            </tr>
            <tr>
                <td id="td1"></td>
            </tr>
            <tr>
                <td id="td2"></td>
            </tr>
            <tr>
                <td id="td3"></td>
            </tr>
            <tr>
                <td id="td4"></td>
            </tr>
            <tr>
                <td id="td5"></td>
            </tr>
            <tr>
                <td id="td6"></td>
            </tr>
            <tr>
                <td id="td7"></td>
            </tr>
            <tr>
                <td id="td8"></td>
            </tr>
            <tr>
                <td id="td9"></td>
            </tr>
            <tr>
                <td id="td10"></td>
            </tr>
            <tr>
                <td id="td11"></td>
            </tr>
            <tr>
                <td id="td12"></td>
            </tr>

        </table>
    </div>

</asp:Content>

