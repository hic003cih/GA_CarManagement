<%@ Page Title="" Language="C#" MasterPageFile="~/eHRMaster.master" AutoEventWireup="true" CodeFile="V_FormCancle.aspx.cs" Inherits="V_FormCancle" %>

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
     </style>
     <script type="text/javascript" src="Scripts/Calendar/WdatePicker.js" charset="gb2312"></script>   

   <script type="text/javascript">
       
       $(function () {
           $("#ContentPlaceHolder1_btnExport").attr("disabled", true);
       })

       function Show(S1, S2, S3, S4, S5, S6, S7, S8, S9) {
           document.getElementById("td1").innerText = "用車廠區：" + S1;
           document.getElementById("td2").innerText = "費用代碼：" + S2;
           document.getElementById("td3").innerText = "用車日期：" + S3;
           document.getElementById("td4").innerText = "用車時間：" + S4;
           document.getElementById("td5").innerText = "用車人員：" + S5;
           document.getElementById("td6").innerText = "用車人數：" + S6;
           document.getElementById("td7").innerText = "聯繫電話：" + S7;
           document.getElementById("td8").innerText = "目的地：" + S8;
           document.getElementById("td9").innerText = "用車事由：" + S9;


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
    <%--<link rel="stylesheet" type="text/css" href="Styles/FormTable.css" />--%>
     <%--表格上方空隔--%>
    <div style="margin-top:15px;"> 
   
         <table class="tb01">
            <caption >文件可作廢清單</caption>
            </table>

    <div id="page_matter">
        
       <table class="table01">
		     <tr>
                 <th>
                    廠區 </th>
                 <td>
                 <asp:RadioButtonList ID="FactoryArea" runat="server" RepeatColumns="5" 
                         RepeatDirection="Horizontal">
                         <asp:ListItem>TW</asp:ListItem>
                         <asp:ListItem>SJ</asp:ListItem>
                         <asp:ListItem>KS</asp:ListItem>
                         <asp:ListItem>WSJ</asp:ListItem>
                         <asp:ListItem>ZZ</asp:ListItem>
                     </asp:RadioButtonList>
                 </td>
                 <th>
                     表單單號</th>
                 <td>
                     <asp:TextBox ID="txt_Flowformid" runat="server" CssClass="ddl130 "></asp:TextBox>
                 </td>
                 <th>
                     執行動作:</th>
             </tr>
         <tr>
            <th>日期類別</th>
            <td >
                <asp:RadioButtonList ID="DateType"  runat="server" RepeatColumns="2" 
                    RepeatDirection="Horizontal">
                    <asp:ListItem>申請日期</asp:ListItem>
                    <asp:ListItem>用車日期</asp:ListItem>
                </asp:RadioButtonList>

            </td>
            <th>日期範圍</th>
            <td>
                <asp:TextBox ID="txt_applyDate"  runat="server" CssClass="ddl60" 
                    onclick="WdatePicker()"></asp:TextBox>
                至<asp:TextBox ID="txt_applyDate2"  runat="server" CssClass="ddl60" 
                    onclick="WdatePicker()"></asp:TextBox>
             </td>
            
            <td>
                <asp:Button ID="btnQuery"  CssClass="btn" runat="server" Text="查詢" onclick="btnQuery_Click"  />
               <%-- <asp:Button ID="btnExport" CssClass="btn" runat="server" Text="匯出" onclick="btnExport_Click" Visible="false"/>--%>
            </td>              
         </tr>	
</table> 
        <div>		
            <asp:GridView ID="GridView" runat="server" AutoGenerateColumns="False" DataKeyNames="FormID"
                    Height="40px" Width="100%" CssClass="tb02"
                    EmptyDataText="沒有記錄" ShowHeaderWhenEmpty="True" AllowPaging="True" 
                PageSize="20" onrowdatabound="GridView_RowDataBound" OnPageIndexChanging="GridView_PageIndexChanging"
                OnRowCommand="GridView_RowCommand" OnRowDeleting="GridView_RowDeleting" Font-Size="10px"
                >
                            <Columns>
                                <asp:BoundField HeaderText="行號" ReadOnly="true" />
                                <asp:BoundField DataField="DeleteFlagChi" HeaderText="表單狀態" />
                                <asp:TemplateField HeaderText="表單單號">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lbl_id" runat="server" Text='<%# Eval("FormNo") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="SDate" HeaderText="用車日期"/>
                                <asp:BoundField DataField="OutTime" HeaderText="用車時間"/>
                                <asp:BoundField DataField="Destination" HeaderText="目的地"/>
                                <asp:BoundField DataField="Form_Status" HeaderText="簽核狀態"/>
                                <asp:BoundField DataField="ApplyManName" HeaderText="申請者" />
                                <asp:BoundField DataField="ApplyDate" DataFormatString="{0:d}"  HeaderText="申請日期"/>
                                <asp:TemplateField HeaderText="動作">
                                    <ItemTemplate>
                                       
                                       <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" CommandArgument='<%#Bind("FormID") %>' 
                                       CommandName='<%#Eval("DeleteFlag").ToString()=="1"?"updateY":"updateDel" %>' 
                                       Text='<%#Eval("DeleteFlag").ToString()=="1"?"恢復":"作廢" %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ControlStyle Width="40px" />
                                </asp:TemplateField>

                                 <asp:TemplateField HeaderText="FormUrl" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="FormUrl" runat="server" Text='<%# Eval("FormUrl")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            <asp:TemplateField HeaderText="FormType" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="FormType" runat="server" Text='<%# Eval("FormType")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            <asp:TemplateField HeaderText="FormID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="FormID" runat="server" Text='<%# Eval("FormID")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="FormNo1" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="FormNo" runat="server" Text='<%# Eval("FormNo")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               
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
    </div>


    <div id="Popup" class="transparent" style=" Z-INDEX: 200"> 
             
        <table border="0" cellpadding="0" style="font-size:13px;" width="250px">
            <tr>
            <td valign ="middle" bgcolor="gray"><font color="white">表單詳細信息</font></td>
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
                             
        </table>
    </div>

</asp:Content>

