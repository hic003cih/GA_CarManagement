<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormSignForward.aspx.cs" Inherits="FormSignForward" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <title>轉簽窗口</title>
    <base target="_self" />
    <script type="text/javascript" src="Scripts/jquery-1.4.1-vsdoc.js"></script>
    <script type="text/javascript" src="Scripts/jquery.ui.core.js"></script>
    <script type="text/javascript" src="Scripts/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="Scripts/jquery.ui.tabs.js"></script>
    <link rel="stylesheet" type="text/css" href="Styles/tabs/Css-Pub/ui.all.css"/>
    <link rel="stylesheet" type="text/css" href="Styles/tabs/Css-Pub/demos.css"/>
   <link rel="stylesheet" type="text/css" href="Styles/FormSignForward.css"/>

    <script type="text/javascript">

        var initPageIndex_dept = 1; //定義全局變量,初始頁索引為1   部門
        var initPageIndex_emp = 1; //  員工
        var strtmp = "";
        var deptSelected = ""; //用戶所選部門
        var deptPageCount = 0; //部門總頁數
        var empPageCount = 0; //員工總頁數

        var ddlValue = '<%=GetDefaultClassLevel() %>'; //第三個頁簽，頁面初始默認為登錄人所屬部級單位下所轄‘課級單位’
        var txtInputEmpNo = "";

        //接收ShowModalDialog傳的值:<11個參數>
        //NodeName;NextSeq_NextName;NextOwnerListName;NextOwnerList;PreSelectNode;
        //PreNodeName;SelectList;SelectListShow;LastAction;formType;formID
        var nextNodeInfo = window.dialogArguments; //接收變量的值
        //alert('showModalDialog傳遞過來的值:...' + nextNodeInfo);
        var nextNodeInfoArr = nextNodeInfo.split("{?+?}");

        $(function () {

            $("#tmp_FormName").val(nextNodeInfoArr[13]); //寫入FormName
            //一.頁面初始狀態

            //showModalDialog值
            $("#CurrentFlow").html(nextNodeInfoArr[0]); //當期流程

            $("#hidden_formTypeID").val(nextNodeInfoArr[9] + ";" + nextNodeInfoArr[10]); //保存formType和formID
            $("#hidden_deptCode").val(nextNodeInfoArr[11]);
            $("#hidden_formInfo").val(nextNodeInfoArr[12]);

            //轉簽  ‘點選’按鈕   彈出Div層
            $(".Forward_but").click(function () {

                initPageIndex_dept = 1; //每次點選加載，都從第一頁開始
                getFirstTabEmp(); //通知  第一個頁簽
                getSecondTabEmp(); //通知  第二個頁簽
                getThirdTabDept(ddlValue, initPageIndex_dept); //通知  第三個頁簽

                $("#Mail_faqbg").css({ display: "block", height: $(document).height() });
                var yscroll = document.documentElement.scrollTop;
                $("#Mail_faqdiv").css("top", "30px");
                $("#Mail_faqdiv").css("display", "block");
                document.documentElement.scrollTop = 0;

            });

            $("#Forward_MailReset").click(function () {//清空
                $("#ForwardList").val("");
            })

            $(".Forward_close").click(function () {//‘關閉’則隱藏
                $("#Mail_faqbg").css("display", "none");
                $("#Mail_faqdiv").css("display", "none");
            });

            //選項卡
            $("#tabs").tabs({
                //设置各选项卡在切换时的动画效果
                fx: { opacity: "toggle", height: "toggle" },
                //通过單擊鼠标事件切换选项卡
                event: "click"
            })

            $("#btnCreateDiv").click(function () {
                initPageIndex_dept = 1;
                initPageIndex_emp = 1;
                var ddlSelect1 = $("#ddlSupiorDept").val(); //DDL值處級
                var ddlSelect2 = $("#ddlBySupiorDept").val(); //DDL值部級
                txtInputEmpNo = $("#txtUserNo").val(); //用戶輸入工號
                //工號有輸入值時，按工號選擇
                if (txtInputEmpNo == "") {
                    //處級單位存在,部級單位不存在時
                    if ((ddlSelect1 != "") && (ddlSelect1 != null) && ((ddlSelect2 == "") || (ddlSelect2 == null))) {
                        getEmpBySupiorDept(ddlSelect1); //處級單位下員工
                    }
                    //部級單位存在時
                    if ((ddlSelect2 != "") && (ddlSelect2 != null)) {
                        getEmpBySupiorDept(ddlSelect2); //部級單位下員工
                        getThirdTabDept(ddlSelect2, initPageIndex_dept); //該部級單位下‘課級部門’
                    }
                } else {
                    empPageCount = FormSignForward.EmpPageCount(txtInputEmpNo).value; //根據用戶輸入的信息UserID/UserName獲取員工分頁總頁數
                    getEmpByUserInput(txtInputEmpNo, initPageIndex_emp);
                }
            })

            //三、操作彈出的Div層

            /*******************'firstTab'  操作***********************/

            $("#firstTab_InfoOK").click(function () {//確定

                $("#Mail_faqbg").css("display", "none");
                $("#Mail_faqdiv").css("display", "none");

                var newMailList = $("#firstTab_txt_inputEmp").val();
                $("#ForwardList").val(newMailList);
            })
            $("#firstTab_InfoEmpty").click(function () { //清空
                $("#firstTab_txt_inputEmp").val("");
                $(".firstTab_Emp input:checkbox").attr("checked", false);
            })

            /*******************'secondTab'  操作***********************/
            $("#secondTab_InfoOk").click(function () {
                $("#Mail_faqbg").css("display", "none");
                $("#Mail_faqdiv").css("display", "none");

                var newMailList = $("#secondTab_txt_inputEmp").val();
                $("#ForwardList").val(newMailList);
            })
            $("#secondTab_InfoEmpty").click(function () { //清空
                $("#secondTab_txt_inputEmp").val("");
                $(".secondTab_Emp input:checkbox").attr("checked", false);
            })

            /*******************'thirdTab'  選擇************************/
            $("#thirdTab_Emp_btnNextEmp").click(function () { //下一頁    部門內人員
                //alert("當前頁:" + initPageIndex_emp);
                //alert("總頁數:" + empPageCount);
                if (initPageIndex_emp < empPageCount) {
                    initPageIndex_emp += 1;

                    getCurrnentEmp(deptSelected, initPageIndex_emp);
                    getEmpByUserInput(txtInputEmpNo, initPageIndex_emp); //根據用戶輸入的信息UserID/UserName
                } else {
                    alert("已經是最後一頁了!");

                }
            })

            $("#thirdTab_Emp_btnPreviousEmp").click(function () { //上一頁    部門內人員
                if (initPageIndex_emp > 1) {
                    initPageIndex_emp -= 1;

                    getCurrnentEmp(deptSelected, initPageIndex_emp);
                    getEmpByUserInput(txtInputEmpNo, initPageIndex_emp); //根據用戶輸入的信息UserID/UserName
                } else {
                    alert("已經是第一頁了!");

                }
            })

            $("#thirdTab_InfoOk").click(function () { //選擇人員后 確定
                $("#Mail_faqbg").css("display", "none");
                $("#Mail_faqdiv").css("display", "none");

                var newMailList = $("#thirdTab_txt_inputEmp").val();
                $("#ForwardList").val(newMailList);
            })

            $("#thirdTab_InfoEmpty").click(function () { //清空
                $("#thirdTab_txt_inputEmp").val("");
                $(".thirdTab_Emp input:checkbox").attr("checked", false);
            })
        })
        //jQuery之外

        //’firstTab‘   最常用人員清單
        function getFirstTabEmp() {
            $(".firstTab_Emp").text("");
            var empNoName = ""; //員工姓名(工號)
            var vEmp = FormSignForward.GetCommonEmp().value;
            
            empNoName = vEmp.split(";");

            for (var i = 0; i < empNoName.length - 1; i++) {
                strtmp = '<div id="' + empNoName[i] + 'DetailDiv" style="float:left;width:19%;border:0px solid blue;margin:2px 0px 2px 0px" class="currentEmpDiv_Tab1">';
                strtmp += '<input id="' + empNoName[i] + 'DetailCHK" type="checkbox" value="' + empNoName[i] + '" name="currentEmpCHK_Tab1" class="commonCHK" />';
                strtmp += '<a>' + empNoName[i] + '</a></div>';
                $(".firstTab_Emp").append(strtmp);
            }

            $(".currentEmpDiv_Tab1 :checkbox").click(function () {//--轉簽只能選擇一個人
                var existEmp = $("#ForwardList").val();
                var inputEmp = $("#firstTab_txt_inputEmp").val();

                if (this.checked) {
                    $(".currentEmpDiv_Tab1 :checkbox").not($(this)).removeAttr("checked"); //checkbox只能选中一个
                    $("#firstTab_txt_inputEmp").val($(this).val());
                }
                else {
                    $("#firstTab_txt_inputEmp").val("");
                }
            })

        }

        // ‘secondTab’   獲取部級/部級以上主管
        function getSecondTabEmp() {
            //alert('getSecondTabEmp......');
            $(".secondTab_Emp").text("");
            var empNoName = ""; //員工姓名(工號)
            var vEmp = FormSignForward.GetCommonEmp2().value;
            //alert('vEmp:'+vEmp);
            empNoName = vEmp.split(";");

            for (var i = 0; i < empNoName.length - 1; i++) {
                strtmp = '<div id="' + empNoName[i] + 'DetailDiv" style="float:left;width:19%;border:0px solid blue;margin:2px 0px 2px 0px" class="currentEmpDiv_Tab2">';
                strtmp += '<input id="' + empNoName[i] + 'DetailCHK" type="checkbox" value="' + empNoName[i] + '" name="currentEmpCHK_Tab2" class="commonCHK" />';
                strtmp += '<a>' + empNoName[i] + '</a></div>';
                $(".secondTab_Emp").append(strtmp);
            }


            $(".currentEmpDiv_Tab2 :checkbox").click(function () {//--轉簽只能選擇一個人

                var existEmp = $("#ForwardList").val();
                var inputEmp = $("#secondTab_txt_inputEmp").val();
                if (this.checked) {
                    $(".currentEmpDiv_Tab2 :checkbox").not($(this)).removeAttr("checked"); //checkbox只能选中一个
                    $("#secondTab_txt_inputEmp").val($(this).val());
                }
                else {
                    $("#secondTab_txt_inputEmp").val("");
                }
            })

        }

        //thirdTab   按部門/工號/姓名選擇
        function getThirdTabDept(supiorDeptTmp, currentPageTmp) {//當前頁  <根據所選擇‘部級單位’加載‘課級’>
            $(".thirdTab_Dept").text("");
            //var currentDept = FormSignForward.GetCurrentPageDept_thirdTab(currentPageIndex_dept).value; //第一頁,“.value必須加”；Ajax方式調用後臺有參函數
            var currentDept = FormSignForward.GetClassLevel(supiorDeptTmp, currentPageTmp).value;
            //alert(initPageIndex_dept);
            var currentDeptArr = currentDept.split(";");
            for (var i = 0; i < currentDeptArr.length - 1; i++) {
                strtmp = '<div id="' + currentDeptArr[i] + 'DetailDiv" style="float:left;width:24%;border:0px solid blue;margin:1px 1px 0px 1px" class="currentDeptDiv_Tab3">';
                strtmp += '<input id="' + currentDeptArr[i] + 'DetailRadio" type="radio" value="' + currentDeptArr[i] + '"  name="currentDeptRadio_Tab3" class="currentDeptRadio_Tab3"/>';
                strtmp += '<a>' + currentDeptArr[i] + '</a></div>';
                $(".thirdTab_Dept").append(strtmp);
            }

            $(".currentDeptDiv_Tab3 :radio").click(function () {

                deptSelected = $(this).val(); //獲取選中部門
                empPageCount = FormSignForward.EmpPageCount(deptSelected).value; //該部門員工分頁總頁數
                //alert('總頁數:'+empPageCount);
                initPageIndex_emp = 1; //每次選擇部門時，初始化
                //alert('dept selected:.....' + deptSelected);
                getCurrnentEmp(deptSelected, initPageIndex_emp); //選中部門，創建動態人員

            })
        }

        //根據部門獲取員工<處級部門/部級部門:處級員工/部級員工>
        function getEmpBySupiorDept(deptTmp) {//Ajax方法，獲取員工
            $(".thirdTab_Emp").text("");
            var currentEmp = FormSignForward.GetEmpByDept(deptTmp, 1).value; //當前頁,“.value必須加”；Ajax方式調用後臺有參函數
            //alert('currentEmp:' + currentEmp);
            CreateEmpByDept(currentEmp);
        }

        //根據‘課級部門’獲取課級下員工
        function getCurrnentEmp(temp1, temp2) { //‘當前頁’ 員工

            $(".thirdTab_Emp").text("");
            var currentEmp = FormSignForward.GetEmpByDept(temp1, temp2).value; //當前頁,“.value必須加”；Ajax方式調用後臺有參函數

            CreateEmpByDept(currentEmp);
        }

        //根據'工號/姓名'，獲取員工
        function getEmpByUserInput(noTmp, currentPageTmp) {
            $(".thirdTab_Emp").text("");
            var currentEmp = FormSignForward.GetEmpByUserNo(noTmp, currentPageTmp).value; //當前頁,“.value必須加”；Ajax方式調用後臺有參函數
            if (currentEmp == "") {
                alert('信息不存在!');
            } else {
                CreateEmpByDept(currentEmp);
            }
        }

        function CreateEmpByDept(empTmp) {//通過‘員工’創建Div
            var currentEmpArr = empTmp.split(";");
            for (var i = 0; i < currentEmpArr.length - 1; i++) {
                strtmp = '<div id="' + currentEmpArr[i] + 'DetailDiv" style="float:left;width:20%;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;border:0px solid blue;margin:1px 0px 1px 0px" class="currentEmpDiv_Tab3">';
                strtmp += '<input id="' + currentEmpArr[i] + 'DetailCHK" type="checkbox" value="' + currentEmpArr[i] + '"  name="currentEmpCHK__Tab3" class="currentEmpCHK_Tab3"/>';
                strtmp += '<a>' + currentEmpArr[i] + '</a></div>';
                $(".thirdTab_Emp").append(strtmp);
            }

            
            $(".currentEmpDiv_Tab3 :checkbox").click(function () {//--轉簽只能選擇一個人
                
                var existEmp = $("#ForwardList").val();
                var inputEmp = $("#thirdTab_txt_inputEmp").val();

                if (this.checked) {
                    $(".currentEmpDiv_Tab3 :checkbox").not($(this)).removeAttr("checked"); //checkbox只能选中一个
                    $("#thirdTab_txt_inputEmp").val($(this).val());
                }
                else {
                    $("#thirdTab_txt_inputEmp").val("");
                }
            })
            
        }
        //'確定'按鈕時判斷
        function checkInfo() {
            return checkISNull();
//            var checkStatus = checkCA();
//            if (checkStatus != "CAOK") {
//                CANO_OK();
//            }
        }

        //判斷用戶輸入是否為空
        function checkISNull() {
            getForwardList();
            var currentFlow = $("#CurrentFlow").text(); //當前流程
            var comment = $("#Forward_txtArea").val(); //轉簽意見
            var forwardList = $("#ForwardList").val(); //轉簽人員清單

           // alert('currentFlow...' + currentFlow + ';comment...' + comment + ';forwardList...' + forwardList);
            if ((currentFlow == "") || (comment == "") || (forwardList == "")) {

                alert("輸入不能為空，請檢查!");
                return false;
            }

            $("#Forward_submit").attr("disabled", true).val("處理中...");
            __doPostBack('Forward_submit', '');
            return true;
        }

        function getForwardList() {

            //SignCounterList
            var forwardList = $("#ForwardList").val();
            //alert('forwardList:' + forwardList);
            //林建智(P1151);
            signCounterList = forwardList.substring(forwardList.indexOf("(") + 1, forwardList.indexOf(")"));
            $("#hiddenForwaList").val(signCounterList);
            //alert('轉簽人員:' + $("#hiddenForwaList").val());

        }

        //證書判斷
        var checkCA = function () {
            var CAStatus = '<%=CASTATUS%>';
            var SerialNumber = '<%=SerialNumberString%>';

            if (CAStatus == "CA_NONE")
                return "CANONE";
            else if (CAStatus == "CA_OK")
                return "CAOK";
            else if (CAStatus == "CA_NO_STAFF")
                return "CANOSTAFF";
            else if (CAStatus == "CA_NO_MATCH")
                return "CANOMATCH";
            else if (SerialNumber == "")
                return "CASerialError";
            else
                return "CAError";
        }

        function CANO_OK(statusTemp) {
            var CA_Info = '<%=CAINFO %>';
            if (statusTemp == "CANOSTAFF") {

                alert("無法從證書中獲取工號信息！證書信息:[" + CA_Info + "]");
                btnDisable();
                window.location.href = "CADownload.aspx";

            }
            else if (statusTemp == "CANOMATCH") {
                alert("證書用戶與登錄用戶不匹配!請下載您的個人證書。錯誤碼[" + statusTemp + "]");
                btnDisable();
                window.location.href = "CADownload.aspx";

            }
            else {
                alert("不允許操作，證書錯誤！錯誤碼[" + statusTemp + "]");
                btnDisable();
                window.location.href = "CADownload.aspx";

            }

        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Forward_main">

        <div class="Forward_mess">
            <table>
                <tr>
                    <td>當前流程:</td>
                    <td><asp:Label ID="CurrentFlow" runat="server" Text="xxxxx"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                    <td>簽核動作:</td>
                    <td><asp:Label ID="CurrentAction" runat="server" Text="轉簽"></asp:Label></td>
                </tr>
            </table>
        </div>


        <div class="Forward_marginTop">
            <div class="Forward_txt">轉簽意見:</div>
            <div>
                <textarea id="Forward_txtArea" cols="60" rows="1" runat="server" class="Forward_txtWidth">轉簽!</textarea>
            </div>
        </div>

        <div class="Forward_marginTop">
            <div class="Forward_txt">
            指定轉簽人員</div>
            <div class="Forward_Notify">
                <asp:TextBox ID="ForwardList" runat="server" CssClass="Forward_txtWidth" onfocus="this.blur()" TextMode="MultiLine" />
                <input type="button" id="Forward_MailClick" value="點選" class="Forward_but" style="height:20px;width:45px;font-size:12px;margin-left:2px;"/>
                <input type="button" id="Forward_MailReset" value="清空" style="height:20px;width:45px;font-size:12px;margin-left:2px;"/>
            </div>
        </div>

        <div class="Forward_marginTop">
            <%--隱藏文本域，存放formType和formID--%>
            <input id="hidden_formTypeID" type="hidden" runat="server" />
            <%--隱藏文本域，存放ForWarList--%>
            <input id="hiddenForwaList" type="hidden" runat="server" />
            <%--隱藏文本域，存放deptCode--%>
            <input id="hidden_deptCode" type="hidden" runat="server" />
             <%--隱藏文本域，存放表單信息--%>
            <input id="hidden_formInfo" type="hidden" runat="server" />

            <asp:Button ID="Forward_submit" runat="server" Text="確定" 
                OnClientClick="return checkInfo()" onclick="Forward_submit_Click" cssClass="form_button01"/>
            <asp:Button ID="Forward_back" runat="server" Text="關閉" OnClientClick="window.close();" cssClass="form_button01"/>
        </div>

        <%--彈出的Div層【轉簽】--%>
        <div id="Mail_faqbg"></div>
        <div id="Mail_faqdiv" style="display: none">
            <h2>人員選擇<a href="#" class="Forward_close">关闭</a></h2>
            <div class="signInfo" id="tabs">
                <ul>
                    <%--<li><a href="#firstTab">常用簽核人員</a></li>
                    <li><a href="#secondTab">常用主管(部級及部級以上)</a></li>--%>
                    <li><a href="#thirdTab">按部門/工號/姓名選擇</a></li>
                </ul>

                 <%-- 第一個頁簽--%>
                <%--<div id="firstTab" style=" font-size:12px;" >
                    <div style=" color:Red">
                        根據簽核頻率排序
                    </div>
                    <div class="firstTab_Emp" style=" border:0px solid red; width:100%;">
                        
                    </div>

                    <div style="width:100%; margin-top:10px; margin-bottom:10px;"><hr /></div>

                    <div>已選擇:
                        <input id="firstTab_txt_inputEmp" type="text" style=" width:70%; font-size:10px; margin-left:10px;" onfocus="this.blur()"/>
                        
                        <a href="#" id="firstTab_InfoOK">確定</a>
                        <a href="#" id="firstTab_InfoEmpty">清空</a>
                    </div>
                </div>--%>


                 <%-- 第二個頁簽--%>
                <%--<div id="secondTab" style=" font-size:12px;">
                    <div style="color:Red">
                        根據工號排序
                    </div>
                    <div class="secondTab_Emp" style="width:100%; border:0px solid red">
                        

                    </div>
                   
                     <div style="width:100%; margin-top:10px; margin-bottom:10px;"><hr /></div>

                    <div>已選擇
                        <input id="secondTab_txt_inputEmp" type="text" style=" width:70%; margin-left:20px;" onfocus="this.blur()"/>
                        <a href="#" id="secondTab_InfoOk">確定</a>
                        <a href="#" id="secondTab_InfoEmpty">清空</a>
                    </div>
                </div>--%>

               <%-- 第三個頁簽--%>
                <div id="thirdTab" style=" font-size:12px;">
                    <div style=" border:0px solid #ADADA4; float:left; background-color:#E7E7E7;">
                         <%--用於無刷新。。。Begin--%>
                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                        </asp:ScriptManager>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div style="border:0px solid blue; float:left;">
                                <span style="color:red">按部門:</span>
                                處級:<asp:DropDownList ID="ddlSupiorDept" runat="server" AutoPostBack="true"  OnSelectedIndexChanged="ddlSupiorDept_SelectedIndexChanged"></asp:DropDownList>
                                部級:<asp:DropDownList ID="ddlBySupiorDept" runat="server" AutoPostBack="true" ></asp:DropDownList>
                                <span style="color:red">按工號/姓名:</span><input type="text" id="txtUserNo" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <%--用於無刷新。。。Over--%>
                        <div>
                            <input id="btnCreateDiv" type="button" value="查詢" style=" margin-left:5px;" />
                        </div>
                    </div>
                    
                    <%--<div>部門</div>--%>
                    <div class="thirdTab_Dept" style="width:100%;font-size:10px; border:0px solid red">
                        
                    </div>
                    
                    <div style="width:100%;"><hr /></div>

                    <div class="thirdTab_Emp" style=" width:100%">
                        
                    </div>
                     <div style=" float:right;">
                         <asp:LinkButton ID="thirdTab_Emp_btnNextEmp" runat="server" OnClientClick="return false">下一頁</asp:LinkButton>
                         <asp:LinkButton ID="thirdTab_Emp_btnPreviousEmp" runat="server" OnClientClick="return false">上一頁</asp:LinkButton>
                    </div>

                    <div style="width:100%"><hr /></div>

                    <div>已選擇
                        <input id="thirdTab_txt_inputEmp" type="text" style=" width:70%; margin-left:20px;" onfocus="this.blur()"/>
              
                        <a href="#" id="thirdTab_InfoOk">確定</a>
                        <a href="#" id="thirdTab_InfoEmpty">清空</a>
                    </div>
                </div>
              
            </div>
        </div>

    </div>
    <asp:HiddenField ID="tmp_FormName" runat="server" />
    </form>
</body>
</html>
