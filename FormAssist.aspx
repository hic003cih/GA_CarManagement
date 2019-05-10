<%@ Page Title="" Language="C#" MasterPageFile="~/eHRMaster.master" AutoEventWireup="true" CodeFile="FormAssist.aspx.cs" Inherits="FormAssist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

        <style type="text/css" >
        .transparent
        {
         BORDER-RIGHT:indiuanred 0px solid;
         BORDER-TOP:indianred 0px solid;
         display:none;
         FILTER:alpha(opacity=100);
         BORDER-LEFT:inianred 0px solid;
         border-bottom:inianred 0px solid;
         POSITION:absolute ;
         BACKGROUND-COLOR:infobackground;
         font-family:Arial ;
        }
            .table01
            {
                width: 932px;
            }
        </style>

   <script type="text/javascript" src="Scripts/Calendar/WdatePicker.js" charset="gb2312"></script>      
    <script type="text/javascript">
        $(function () {
            $("#ContentPlaceHolder1_btnExport").attr("disabled", true);
        })
        //tooltip顯示值的內容
        function Show(S1, S2, S3, S4, S5, S6, S7, S8, S9, S10) {
            document.getElementById("td1").innerText = "申請單號：" + S1;
            document.getElementById("td2").innerText = "申請者：" + S2;
            document.getElementById("td3").innerText = "申請部門：" + S3;
            document.getElementById("td4").innerText = "出差類別：" + S4;
            document.getElementById("td5").innerText = "接機地點：" + S5;
            document.getElementById("td6").innerText = "送機地點：" + S6;
            document.getElementById("td7").innerText = "啟動代理人：" + S7;
            document.getElementById("td8").innerText = "是否預支：" + S8;
            document.getElementById("td9").innerText = "是否報銷：" + S9;
            document.getElementById("td10").innerText = "文件狀態：" + S10;

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
    </script>
     <link rel="stylesheet" type="text/css" href="Styles/FormTable.css" />
    <div id="page_matter">
     <%--<span id="ContentPlaceHolder1_title">我申請的文件</span>--%>
        <table class="table01_systemtitle">
        <tr>
        <td>
            出差協辦資訊
        </td>
        </tr>
        </table> 
         <table class="table01">
		 <tr>
            <th>出差日期:</th>
            <td>
                <asp:TextBox ID="txt_applyDate"  runat="server" onclick="WdatePicker()"></asp:TextBox>
                至<asp:TextBox ID="txt_applyDate2"  runat="server"  onclick="WdatePicker()"></asp:TextBox>
             </td>
            <th>申請者:</th>
            <td><asp:TextBox ID="txt_Flowapplier" runat="server"></asp:TextBox></td>
            <th rowspan="2">執行動作:</th>
            <td rowspan="2">
                <asp:Button ID="btnQuery"  CssClass="btn" runat="server" Text="查詢" onclick="btnQuery_Click"  />
                <asp:Button ID="btnExport" CssClass="btn" runat="server" Text="匯出" onclick="btnExport_Click" />
            </td>  
         </tr>
         <tr>
            <th>申請廠區:</th>
            <td>
                <asp:TextBox ID="FactoryArea"  runat="server"  onclick="WdatePicker()"></asp:TextBox>
             </td>
            <th>申請類別:</th>
            <td><asp:TextBox ID="ApplyType" runat="server"></asp:TextBox></td>
         </tr>
         </table> 
            
            <div>
              <asp:GridView ID="GridView" runat="server" AutoGenerateColumns="False" DataKeyNames="MainFormNo"
                    Height="10px" Width="100%" CssClass="GridViewStyle"
                    EmptyDataText="沒有記錄" ShowHeaderWhenEmpty="True" AllowPaging="True" 
                PageSize="20" onrowdatabound="GridView_RowDataBound" 
                    OnPageIndexChanging="GridView_PageIndexChanging" 
                   >
                            <Columns>
                                <asp:BoundField HeaderText="行號" ReadOnly="true" >
                                <ControlStyle Font-Size="8pt" />
                                </asp:BoundField>
                                <%--<asp:BoundField DataField="FormNo" HeaderText="表單單號"/>--%>                                
                                <asp:TemplateField HeaderText="申請人">
                                    <ItemTemplate>
                                         <asp:Label ID="Register_Name" runat="server" Text='<%# Eval("Register_Name")%>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterStyle Font-Size="8pt" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="出差行程">
                                    <ItemTemplate>
                                         <asp:Label ID="Day" runat="server" Text='<%#  Eval("FromDay","{0:yyyy/MM/dd}")+"-"+Eval("Today","{0:yyyy/MM/dd}")%>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterStyle Font-Size="8pt" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="出差天數">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="Days" runat="server" Text='<%# Eval("Days") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                     <FooterStyle Font-Size="8pt" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="出差地點">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="BusinessTravel_Main" runat="server" Text='<%# Eval("BusinessTravel_Main_OT")!=""?Eval("BusinessTravel_Main")+"("+Eval("BusinessTravel_Main_OT")+")":Eval("BusinessTravel_Main") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                    <FooterStyle Font-Size="8pt" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="協助票務">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="AssistMatter" runat="server" Text='<%# Eval("AssistMatter_Visa")!=""?Eval("AssistMatter_Visa"):"無"%>'></asp:HyperLink>
                                    </ItemTemplate>
                                    <FooterStyle Font-Size="8pt" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="協助簽證">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="AssistMatter" runat="server" Text='<%#Eval("AssistMatter_Other")==""?"無":Eval("AssistMatter_OT")!="" ?Eval("AssistMatter_Other")+"("+Eval("AssistMatter_OT")+")":Eval("AssistMatter_Other")%>'></asp:HyperLink>
                                    </ItemTemplate>
                                    <FooterStyle Font-Size="8pt" />
                                </asp:TemplateField>
                               <asp:TemplateField HeaderText="協助接機">
                                    <ItemTemplate>
                                        <asp:Label ID="Send" runat="server" Text='<%# Eval("IsPickup_Send")%>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterStyle Font-Size="8pt" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="協助送機">
                                    <ItemTemplate>
                                        <asp:Label ID="Back" runat="server" Text='<%# Eval("IsPickup_Back")%>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterStyle Font-Size="8pt" />
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
                <RowStyle CssClass="GridViewRowStyle"/>   
                <SelectedRowStyle CssClass="GridViewSelectedRowStyle" />
                <PagerStyle CssClass="GridViewPagerStyle" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
                <HeaderStyle CssClass="GridViewHeaderStyle"  />
            </asp:GridView>
        </div>

        </div>
        <div id="Popup" class="transparent" style=" Z-INDEX: 200"> 
             
        <table border="0" cellpadding="0" style="font-size:9px;" width="250px">
            <tr>
            <td valign ="middle" bgcolor="gray"><font color="white">表單詳細信息</font></td>
            </tr>
            <tr>
            <td id="td1" style="text-align: left"></td>
            </tr>
            <tr>
            <td id="td2"  style="text-align: left"></td>
            </tr>
            <tr>
            <td id="td3"  style="text-align: left"></td>
            </tr>
            <tr>
            <td id="td4"  style="text-align: left"></td>
            </tr>
            <tr>
            <td id="td5" style="text-align: left"></td>
            </tr>
            <tr>
            <td id="td6"  style="text-align: left"></td>
            </tr>
            <tr>
            <td id="td7"  style="text-align: left"></td>
            </tr>
            <tr>
            <td id="td8"  style="text-align: left"></td>
            </tr>
            <tr>
            <td id="td9"  style="text-align: left"></td>
            </tr>
            <tr>
            <td id="td10"  style="text-align: left"></td>
            </tr>
                             
        </table>
    </div>

</asp:Content>

