<%@ Page Language="C#" MasterPageFile="~/eHRMaster.master" AutoEventWireup="true" CodeFile="V_FormAllot.aspx.cs" Inherits="V_FormAllot" %>

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
         .txtStyle{font-size:12px; Width:100%; height:23px;width:100px;border:1px solid gray;}
         .ddlStyle{border: 1px solid #C0C0C0;width: 120px;height: 30px;clip: rect( 0px, 181px, 20px, 0px );
                   overflow: hidden;}
     </style>
     <script type="text/javascript" src="Scripts/Calendar/WdatePicker.js" charset="gb2312"></script> 
          <script type="text/javascript">
              function CheckRow(i) {
                  document.getElementById("Rowindex").value = i
              }

              $(function () {
                  var no = $("#Rowindex").val();

                  function search() {
                      var no = $("#Rowindex").val();
              //        alert("no=" + no);
                      var v = $("#ContentPlaceHolder1_GridView_txtTheOwner_" + no + "").val();
                      var v1 = V_FormAllot.GetUserName(v).value;
             //         alert("v1="+v1);
                      if (v1 != "") {
                          $("#ContentPlaceHolder1_GridView_txtTheName_" + no + "").val(v1);
             //             alert($("#ContentPlaceHolder1_GridView_txtTheName_" + no + "").val());
                      }
                      else {
                          $("#ContentPlaceHolder1_GridView_txtTheName_" + no + "").val("無此人！");
                      }
                  }
                  for (var i = 0; i < 20; i++) {
                      $("#ContentPlaceHolder1_GridView_txtTheOwner_" + i + "").keyup(search);
             //         alert("i=" + i);
                  }

              })

            </script>

   <script type="text/javascript">
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
    <div id="Div1">
        <table class="tb01">
            <caption >文件變更簽核者(變更當站人員)</caption>
            </table>
      
    <div id="page_matter">

    <asp:Panel ID="Panel_Text" runat="server" ForeColor="Red">
             抱歉,您不是系統管理員,無權查看,謝謝!</asp:Panel>

          <asp:Panel ID="Panel_Content" runat="server">
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
                 <td>
                     </td>
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
            
		
            <asp:GridView ID="GridView" runat="server" AutoGenerateColumns="False" DataKeyNames="FormNo"
                    Height="142px" Width="100%" CssClass="tb02"
                    EmptyDataText="沒有記錄" ShowHeaderWhenEmpty="True" AllowPaging="True" 
                PageSize="20" onrowdatabound="GridView_RowDataBound" 
                OnPageIndexChanging="GridView_PageIndexChanging"
                OnRowCommand="GridView_RowCommand">
                            <Columns>
                                <asp:BoundField HeaderText="行號" ReadOnly="true" />
                                <asp:TemplateField HeaderText="表單ID" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="txtFormID" runat="server" Text='<%# Eval("FormID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText=" 表單單號 ">
                                    <ItemTemplate>
                                        <%--<asp:HyperLink ID="lbl_id" runat="server" NavigateUrl='<%#Eval("FormUrl")+"?formNo=" + Eval("FormNo")+"&formType="+Eval("FormType")+"&formTypeName="+Eval("FormTypeName")+"&formID="+Eval("FormID")+"&status="+Eval("NodeName")%>' Text='<%# Eval("FormNo") %>'></asp:HyperLink>--%>
                                        <asp:HyperLink ID="lbl_id" runat="server" Text='<%# Eval("FormNo") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="表單名稱" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="txtFormType" runat="server" Text='<%# Eval("FormTypeName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="流程名" Visible="false" >
                                     <ItemTemplate>
                                          <asp:Label ID="txtFormtype1" runat="server" Text='<%# Eval("FormType") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="表單日期" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="txtApplyDate" runat="server" Text='<%# Eval("ApplyDate","{0:yyyy-MM-dd}").ToString() %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="申請者" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="txtApplyManNo" runat="server" Text='<%# Eval("ApplyManNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="當前工號">
                                    <ItemTemplate>
                                        <asp:Label ID="txtOwnerList" runat="server" Text='<%# Eval("OwnerList") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="當前姓名">
                                    <ItemTemplate>
                                        <asp:Label ID="txtOwnerName" runat="server" Text='<%# Eval("OwnerListName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="變更後工號">
                                    <ItemTemplate>
                                       <asp:TextBox ID="txtTheOwner" runat="server" CssClass="txtStyle"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="變更後姓名">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTheName" runat="server" CssClass="txtStyle"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField> 
                                <asp:TemplateField HeaderText="備註">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtComment" runat="server" CssClass="txtStyle"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="確定">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="Send" runat="server" 
                                            CausesValidation="False"  CommandArgument='<%# Bind("FormNo") %>' CommandName="Send" 
                                            Text="確定">
                                        </asp:LinkButton>
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
            </asp:Panel>
<input id="Rowindex" type="hidden" value="0"/>
        </div>
      </div>
    
    

       <div id="Popup" class="transparent" style=" Z-INDEX: 200"> 
             
        <table border="0" cellpadding="0" style="font-size:13px;" width="250px">
            <tr>
            <td valign ="middle" bgcolor="gray"><font color="white">表單詳細信息nt></td>
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
