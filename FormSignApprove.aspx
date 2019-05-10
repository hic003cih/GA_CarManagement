<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeFile="FormSignApprove.aspx.cs" Inherits="FormSignApprove" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <title>簽核通過窗口</title>
    <base target="_self" />
    <script type="text/javascript" src="Scripts/jquery-1.4.1-vsdoc.js"></script>
    <script type="text/javascript" src="Scripts/jquery.ui.core.js"></script>
    <script type="text/javascript" src="Scripts/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="Scripts/jquery.ui.tabs.js"></script>
    <link rel="stylesheet" type="text/css" href="Styles/tabs/Css-Pub/ui.all.css"/>
    <link rel="stylesheet" type="text/css" href="Styles/tabs/Css-Pub/demos.css"/>
    <link rel="stylesheet" type="text/css" href="Styles/FormSignApprove.css"/>
     <script type="text/javascript">
         var nextNodeInfo = window.dialogArguments;
  //       alert(nextNodeInfo);
         var nextNodeInfoArr = nextNodeInfo.split("{?+?}");
  //       alert(nextNodeInfoArr[15]);
         var initPageIndex_dept = 1; //定義全局變量,初始頁索引為1   部門
         var initPageIndex_emp = 1; //  員工
         var strtmp = "";
         var deptSelected = ""; //用戶所選部門
         var deptPageCount = 0; //部門總頁數
         var empPageCount = 0; //員工總頁數
         var ddlValue = '<%=GetDefaultClassLevel()%>'; //第三個頁簽，頁面初始默認為登錄人所屬部級單位下所轄‘課級單位’
         var txtInputEmpNo = "";
         var NotifyInputValue = $("#Approve_txtNofity").val(); //通知文本域內值

         $(function () {

             $("#tmp_FormName").val(nextNodeInfoArr[13]); //寫入FormName
             $("#CurrentFlow").html(nextNodeInfoArr[0]); //當期流程

             $("#hiddenNowDeptCode").val(nextNodeInfoArr[16]);   //紀錄當前簽核者的部門代碼


             var nextNodeSeq_Name = nextNodeInfoArr[1]; //2_經辦者課級主管
             if (nextNodeSeq_Name.charAt(0) == 0) { //如果到最後一關
                 $("#ddl_nextOwnerList").append("<option>流程結束!</option>");
                 $("#hiddenNextList").val("流程結束");
             }
             else {
                 //alert('nextNodeInfoArr[2]:' + nextNodeInfoArr[2]);
                 if (nextNodeInfoArr[2].indexOf(",") >= 0) {//如果有多主管
                     $("#ddl_nextOwnerList").append("<option value=''>---------------------請選擇---------------------</option>");
                     var nextOwnerListName = nextNodeInfoArr[2].split(",");
                     var nextOwnerList = nextNodeInfoArr[3].split(",");
                     var nextOwnerDeptCode = nextNodeInfoArr[15].split(",");

                     for (var i = 0; i < nextOwnerListName.length; i++) {
                         $("#ddl_nextOwnerList").append("<option>" + nextNodeSeq_Name + "_" + nextOwnerListName[i] + "(" + nextOwnerList[i] + ")" + nextOwnerDeptCode[i] + "</option>"); //下一簽核人員
                     }
                 }
                 else {
                     $("#ddl_nextOwnerList").append("<option>" + nextNodeSeq_Name + "_" + nextNodeInfoArr[2] + "(" + nextNodeInfoArr[3] + ")" + nextNodeInfoArr[15] + "</option>"); //下一簽核人員
                 }

                 //alert('選擇一個主管簽核nextOwnerList：' + nextNodeInfoArr[3]);
                 var setNextOwnerList1 = $("#ddl_nextOwnerList").val(); //獲取DropDownList下拉值
                 var setNextSeq1 = setNextOwnerList1.substring(0, setNextOwnerList1.indexOf("_"));    //獲取選擇的下一站站別
                 var setNextList1 = setNextOwnerList1.substring(setNextOwnerList1.indexOf("(") + 1, setNextOwnerList1.indexOf(")"));
                 var setNextDeptCode1 = setNextOwnerList1.substring(setNextOwnerList1.indexOf(")") + 1, setNextOwnerList1.length);
                 $("#hiddenNextList").val(setNextList1);
                 $("#hiddenNextDeptCode").val(setNextDeptCode1);
                 $("#hiddenNextSeq").val(setNextSeq1);

                 $("#ddl_nextOwnerList").change(function () {
                     var setNextOwnerList2 = $("#ddl_nextOwnerList").val(); //重新獲取DropDownList下拉值    
                     var setNextSeq1 = setNextOwnerList2.substring(0, setNextOwnerList2.indexOf("_"));    //獲取選擇的下一站站別
                     var setNextList2 = setNextOwnerList2.substring(setNextOwnerList2.indexOf("(") + 1, setNextOwnerList2.indexOf(")"));
                     var setNextDeptCode2 = setNextOwnerList2.substring(setNextOwnerList2.indexOf(")") + 1, setNextOwnerList2.length);

                     $("#hiddenNextList").val(setNextList2);
                     $("#hiddenNextDeptCode").val(setNextDeptCode2);
                     $("#hiddenNextSeq").val(setNextSeq1);
                     //          alert('選擇的工號,setNextList2:' + setNextList2);
                     //          alert('選擇的部門,setNextDeptCode2:' + setNextDeptCode2);

                     //alert('選擇的主管工號:' + $("#hiddenNextList").val());
                 })
             }

             //選簽------preNodeSeq存在並且不是會簽，就創建此段Div
             //alert('與設節點值:' + nextNodeInfoArr[4]);
             //alert('LastAction:'+nextNodeInfoArr[8]);
             if ((nextNodeInfoArr[4] != '') && (nextNodeInfoArr[8] != 'COUNTERSIGN')) {

                 createPreSelectDiv();
             }

             //alert('預設的流程節點名稱:' + nextNodeInfoArr[5]);
             if (nextNodeInfoArr[5].toString() != "") {
                 $("#selectNodeName").html(nextNodeInfoArr[5]); //指定的流程節點名稱
             }
             $("#hidden_Node").val(nextNodeInfoArr[1]);
             $("#hidden_formTypeID").val(nextNodeInfoArr[9] + ";" + nextNodeInfoArr[10]); //保存formType和formID
             $("#hidden_deptCode").val(nextNodeInfoArr[11]);
             $("#hidden_formInfo").val(nextNodeInfoArr[12]);
             $("#hidden_formname").val(nextNodeInfoArr[13]);

             //‘關閉’，
             $(".close").click(function () {//‘關閉’則隱藏
                 $("#faqbg").css("display", "none");
                 $("#faqdiv").css("display", "none");
             });
             //選項卡  
             $("#tabs").tabs({
                 //设置各选项卡在切换时的动画效果
                 fx: { opacity: "toggle", height: "toggle" },
                 //通过單擊鼠标事件切换选项卡
                 event: "click"
             })

             //通知  ‘點選’按鈕   彈出Div層
             $(".Mail_but").click(function () {

                 $("#firstTab_txt_inputEmp").val("");
                 $("#secondTab_txt_inputEmp").val("");
                 $("#thirdTab_txt_inputEmp").val("");

                 initPageIndex_dept = 1; //每次點選加載，都從第一頁開始
                 getFirstTabEmp();
                 getSecondTabDept();
                 getThirdTabDept(ddlValue, initPageIndex_dept); //通知  第三個頁簽

                 $("#Mail_faqbg").css({ display: "block", height: $(document).height() });
                 var yscroll = document.documentElement.scrollTop;
                 $("#Mail_faqdiv").css("top", "30px");
                 $("#Mail_faqdiv").css("display", "block");
                 document.documentElement.scrollTop = 0;

             });
             $("#Approve_MailReset").click(function () {//清空
                 $("#Approve_txtNofity").val("");
                 NotifyInputValue = "";
             })

             $(".Mail_close").click(function () {//‘關閉’則隱藏
                 $("#Mail_faqbg").css("display", "none");
                 $("#Mail_faqdiv").css("display", "none");
             });

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
                 }
                 else {
                     empPageCount = FormSignApprove.EmpPageCount(txtInputEmpNo).value; //根據用戶輸入的信息UserID/UserName獲取員工分頁總頁數
                     getEmpByUserInput(txtInputEmpNo, initPageIndex_emp);
                 }
             })
             /*******************'firstTab'  操作***********************/


             $("#firstTab_InfoOK").click(function () {//確定
                 $("#Mail_faqbg").css("display", "none");
                 $("#Mail_faqdiv").css("display", "none");
                 NotifyInputValue = $("#Approve_txtNofity").val();
                 var newMailList = $("#firstTab_txt_inputEmp").val() + $("#secondTab_txt_inputEmp").val() + $("#thirdTab_txt_inputEmp").val();
                 $("#Approve_txtNofity").val(NotifyInputValue + newMailList);
             })

             $("#firstTab_InfoEmpty").click(function () { //清空
                 $("#firstTab_txt_inputEmp").val("");
                 $(".firstTab_Emp input:checkbox").attr("checked", false);
             })

             /*******************'secondTab'  操作***********************/

             $("#secondTab_InfoOk").click(function () {//確定
                 $("#Mail_faqbg").css("display", "none");
                 $("#Mail_faqdiv").css("display", "none");
                 NotifyInputValue = $("#Approve_txtNofity").val();
                 var newMailList = $("#firstTab_txt_inputEmp").val() + $("#secondTab_txt_inputEmp").val() + $("#thirdTab_txt_inputEmp").val();
                 $("#Approve_txtNofity").val(NotifyInputValue + newMailList);
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

                     getCurrnentEmp(deptSelected, initPageIndex_emp); //根據用戶選擇的部門
                     getEmpByUserInput(txtInputEmpNo, initPageIndex_emp); //根據用戶輸入的信息UserID/UserName

                 } else {
                     alert("已經是最後一頁了!");
                 }
             })

             $("#thirdTab_Emp_btnPreviousEmp").click(function () { //上一頁    部門內人員
                 if (initPageIndex_emp > 1) {
                     initPageIndex_emp -= 1;

                     getCurrnentEmp(deptSelected, initPageIndex_emp); //根據用戶選擇的部門
                     getEmpByUserInput(txtInputEmpNo, initPageIndex_emp); //根據用戶輸入的信息UserID/UserName
                 } else {
                     alert("已經是第一頁了!");
                 }
             })

             $("#thirdTab_InfoOk").click(function () { //選擇人員后 確定
                 $("#Mail_faqbg").css("display", "none");
                 $("#Mail_faqdiv").css("display", "none");
                 var newMailList = $("#thirdTab_txt_inputEmp").val();
                 NotifyInputValue = $("#Approve_txtNofity").val();
                 //var newMailList = $("#firstTab_txt_inputEmp").val() + $("#secondTab_txt_inputEmp").val() + $("#thirdTab_txt_inputEmp").val();
                 $("#Approve_txtNofity").val(NotifyInputValue + newMailList);
             })
             $("#thirdTab_InfoEmpty").click(function () { //清空
                 $("#thirdTab_txt_inputEmp").val("");
                 $(".thirdTab_Emp input:checkbox").attr("checked", false);
             })

         })

        //jQuery之外

        /*選簽............Begin*/
        function createPreSelectDiv() {

            //--動態創建一個checkbox，只有選中時，才能進行選簽操作
            strtmp1 = '<input id="SendChoose_CHK" type="checkbox" checked="true" style="margin-left:2px;margin-top:10px;"/>';
            strtmp1 += '<span class="redStyle">請指定以下流程節點簽核人員</span>';
            $(".redStyle").append(strtmp1);

            //--根據preNodeSeq創建對應的Div
            createpreSelectListDiv();
        }
        function createpreSelectListDiv() {
            //經管審核;6_經管主管審核;蔡宗麟,李晏青;P0256,P1195;;;;;01;1
            var nodeList = nextNodeInfoArr[4].split(","); //5,6,7
            var nodeNameList = nextNodeInfoArr[5].split("|"); //經管審核|經管審核1|經管審核2;
            var ownerList = nextNodeInfoArr[6].split("|"); //PK2075,FK0477,PK2752|PK2764,PK2485|PK2515
            var ownerListName = nextNodeInfoArr[7].split("|"); //CM:梁艷,FY:金冬玲(代),AU:蘇宗鑫|袁偉,楊陽|康育鵬

            //--選簽List
            for (var i = 0; i < nodeList.length; i++) {
                strtmp3 = '<div style="border:0px solid red;margin-left:17px;font-size:10px;" class="morePreNode" id="PreSetRow' + nodeList[i] + '">';
                strtmp3 += '<span class="spanStyle" id="spanpreNodeName" style="margin-left:15%;">' + nodeNameList[i] + '</span>';
                strtmp3 += '<span style="margin-left:10px;">指定人員</span>';
                strtmp3 += '<input type="text" id="txtPreSetRow' + nodeList[i] + '" onfocus="this.blur()" style="width:46%;font-size:10px;margin-left:1px"/>';
                strtmp3 += '<input type="hidden" id="txthiddenNode' + nodeList[i] + '" style="width:10px;" class="hiddenSetSeq">'; //隱藏文本域，保存nodeSeq
                strtmp3 += '<input type="hidden" id="txthiddenList' + nodeList[i] + '" style="width:45px;" class="hiddenSetList">'; //隱藏文本域，保存List
                strtmp3 += '<input type="button" id="clickPreSetRow' + nodeList[i] + '" value="點選"  class="but" style="height:20px;width:45px;font-size:12px;margin-left:6px;"/>';
                strtmp3 += '<input type="button" id="clearPreSetRow' + nodeList[i] + '" value="清空"  class="but" style="height:20px;width:45px;font-size:12px;margin-left:3px;"/></div>';
                $(".Approve_txtNodeNameSize").append(strtmp3);
            }

            btnInit(); //初始化動態生成的Div中’按鈕‘
            //遍歷‘點選’，不同preNodeSeq，對應不同preSelectLsit
            $(".but").each(function (index) {//‘點選’彈出Div層

                $("#clickPreSetRow" + nodeList[index]).click(function () {//'點選'按鈕事件
                    //alert('點選按鈕...clickPreSetRow' + nodeList[index]);
                    $(".signerList").text("");
                    $(".signerListSelect").text("");

                    $("#faqbg").css({ display: "block", height: $(document).height() }); //Div層顯示
                    var yscroll = document.documentElement.scrollTop;
                    $("#faqdiv").css("top", "30px");
                    $("#faqdiv").css("display", "block");
                    document.documentElement.scrollTop = 0;

                    //alert('點選按鈕之后ownerList值:' + ownerList[index]);
                    // alert('點選按鈕之后ownerListName值:' + ownerListName[index]);
                    getEmpList(ownerList[index], ownerListName[index]);

                    strtmp2 = '<div style="width:90%; margin:0px auto 0px auto;"><hr /></div>';
                    strtmp2 += '<div style="width:90%;margin:5px auto 10px auto;border:0px solid red;">已選擇';
                    strtmp2 += '<input type="text" id="txtSelectList' + nodeList[index] + '" style=" width:70%; margin-left:20px; font-size:10px;" onfocus="this.blur()"/>';
                    strtmp2 += '<input type="hidden" id="txtSelectNodeSeq' + nodeList[index] + '"  style=" width:80%; margin-left:20px; font-size:10px;" onfocus="this.blur()"/>';
                    strtmp2 += '<input type="hidden" id="txtSelectListNo' + nodeList[index] + '" style=" width:80%; margin-left:20px; font-size:10px;" onfocus="this.blur()"/>';
                    strtmp2 += '<a href="#" id="txtSelectClick' + nodeList[index] + '">確定</a></div>';
                    $(".signerListSelect").append(strtmp2);

                    var currentSelectListNo = $("#txtSelectListNo" + nodeList[index]).val(); //人員工號

                    //--選擇 選簽人員
                    $(".signerListDiv :checkbox").click(function () {
                        if (this.checked) {
                            $(".signerListDiv :checkbox").not($(this)).removeAttr("checked"); //checkbox只能选中一个
                            $("#txtSelectList" + nodeList[index]).val($(this).val());
                            currentSelectListNo = $(this).attr("ID");
                            $("#txtSelectListNo" + nodeList[index]).val(currentSelectListNo); //獲取選擇的人員List
                            $("#txtSelectNodeSeq" + nodeList[index]).val(nodeList[index]); //獲取選擇的關卡數
                        }
                        else {
                            $("#txtSelectList" + nodeList[index]).val("");
                            $("#txtSelectListNo" + nodeList[index]).val("");
                            $("#txtSelectNodeSeq" + nodeList[index]).val("");
                        }
                    })
                    var currentSignerListNo = "";
                    var hiddenPreSelectListNo = $("#hidden_preSelectList").val(); //已經預選的工號
                    var preNodeSeq = "";
                    var hiddenpreNodeSeq = $("#hidden_preNodeSeq").val();
                    $("#txtSelectClick" + nodeList[index]).click(function () {//確定

                        $("#txthiddenNode" + nodeList[index]).val("");
                        $("#txthiddenList" + nodeList[index]).val("");

                        var setInfo = $("#txtSelectList" + nodeList[index]).val(); //用戶選擇
                        var setSeq = $("#txtSelectNodeSeq" + nodeList[index]).val(); //用戶選擇關數
                        var setList = $("#txtSelectListNo" + nodeList[index]).val(); //對應人員工號

                        $("#txtPreSetRow" + nodeList[index]).val(setInfo);
                        $("#txthiddenNode" + nodeList[index]).val(setSeq);
                        $("#txthiddenList" + nodeList[index]).val(setList);

                        $("#faqbg").css("display", "none");
                        $("#faqdiv").css("display", "none");
                    })

                })//’點選‘按鈕結束
                //清空
                $("#clearPreSetRow" + nodeList[index]).click(function () {
                    $("#txtPreSetRow" + nodeList[index]).val("");
                    $("#txthiddenNode" + nodeList[index]).val("");
                    $("#txthiddenList" + nodeList[index]).val("");
                })

            })
        }
        //--選簽 動態創建后  初始按鈕控制
        function btnInit() {

            //$(".morePreNode :button").attr("disabled", true);
            //checkbox點中，按鈕可用
            $("#SendChoose_CHK").click(function () {
                if (this.checked) {
                    $(".morePreNode :button").attr("disabled", false);
                }
                else {
                    $(".morePreNode :button").attr("disabled", true);
                }
            })
            if ($("#SendChoose_CHK").attr("checked") == true) {
                $(".morePreNode :button").attr("disabled", false);
            }
        }
         //選簽時，Div層內容明細
        function getEmpList(temp1, temp2) {
            var ownerListArr = temp1.split(",");
            var ownerListNameArr = temp2.split(",");
            var ownerListNoName = "";
            for (var i = 0; i < ownerListArr.length; i++) {
                ownerListNoName += ownerListNameArr[i] + "(" + ownerListArr[i] + ")" + ";";
            }
            //alert('處理之後的list列表:' + ownerListNoName);
            var empNoName = ownerListNoName.split(";");
            for (var i = 0; i < empNoName.length - 1; i++) {

                strtmp = '<div id="' + ownerListArr[i] + 'DetailDiv" style="float:left;width:48%;border:0px solid blue;margin:2px 2px 2px 2px" class="signerListDiv">';
                strtmp += '<input id="' + ownerListArr[i] + '" type="checkbox" value="' + empNoName[i] + '" name="sigerListCHK" class="sigerListCHK" />';
                strtmp += '<a>' + empNoName[i] + '</a></div>';
                $(".signerList").append(strtmp);
            }
        } /*--選簽............Over*/


        /*’firstTab‘  已簽核記錄人員*/
        function getFirstTabEmp() {
            $(".firstTab_Emp").text("");
            var empNoName = ""; //員工姓名(工號)
            var vEmp = FormSignApprove.GetCommonEmp().value;
            empNoName = vEmp.split(";");

            for (var i = 0; i < empNoName.length - 1; i++) {
                strtmp = '<div id="' + empNoName[i] + 'DetailDiv" style="float:left;width:19%;border:0px solid blue;margin:2px 0px 2px 0px" class="currentEmpDiv_Tab1">';
                strtmp += '<input id="' + empNoName[i] + 'DetailCHK" type="checkbox" value="' + empNoName[i] + '" name="currentEmpCHK_Tab1" class="commonCHK" />';
                strtmp += '<a>' + empNoName[i] + '</a>';
                $(".firstTab_Emp").append(strtmp);
            }

            $(".currentEmpDiv_Tab1 :checkbox").click(function () {

                NotifyInputValue = $("#Approve_txtNofity").val();
                var inputEmp = $("#firstTab_txt_inputEmp").val();

                if (this.checked) {
                    if ((inputEmp + NotifyInputValue).indexOf(this.value) >= 0) {
                        alert("人員已存在，忽略選擇!");
                    }
                    else {
                        inputEmp += $(this).val() + "; ";

                        $("#firstTab_txt_inputEmp").val(inputEmp);
                    }
                }
                else {
                    inputEmp = inputEmp.replace($(this).val() + ";", "");
                    $("#firstTab_txt_inputEmp").val(inputEmp)
                }
            })
        }
        /* ‘secondTab’  獲取部級/部級以上主管*/
        function getSecondTabDept() {
            $(".secondTab_Emp").text("");
            var empNoName = ""; //員工姓名(工號)
            var vEmp = FormSignApprove.GetCommonEmp2().value; //第一頁,“.value必須加”；Ajax方式調用後臺有參函數
            //alert('currentDept:' + currentDept);
            var empNoName = vEmp.split(";");
            for (var i = 0; i < empNoName.length - 1; i++) {
                strtmp = '<div id="' + empNoName[i] + 'DetailDiv" style="float:left;width:19%;border:0px solid blue;margin:2px 0px 2px 0px" class="currentEmpDiv_Tab2">';
                strtmp += '<input id="' + empNoName[i] + 'DetailCHK" type="checkbox" value="' + empNoName[i] + '" name="currentEmpCHK_Tab2" class="commonCHK" />';
                strtmp += '<a>' + empNoName[i] + '</a></div>';
                $(".secondTab_Emp").append(strtmp);
            }
            $(".currentEmpDiv_Tab2 :checkbox").click(function () {

                NotifyInputValue = $("#Approve_txtNofity").val();
                var inputEmp = $("#secondTab_txt_inputEmp").val();

                if (this.checked) {
                    if ((inputEmp + NotifyInputValue).indexOf(this.value) >= 0) {
                        alert("人員已存在，忽略選擇!");
                    }
                    else {
                        inputEmp += $(this).val() + "; ";
                        $("#secondTab_txt_inputEmp").val(inputEmp);
                    }
                }
                else {
                    inputEmp = inputEmp.replace($(this).val() + ";", "");
                    $("#secondTab_txt_inputEmp").val(inputEmp);
                }
            })
        }
        /*thirdTab  按部門選擇*/
        function getThirdTabDept(supiorDeptTmp, currentPageTmp) {//當前頁  <根據所選擇‘部級單位’加載‘課級’>
            $(".thirdTab_Dept").text("");
            var currentDept = FormSignApprove.GetClassLevel(supiorDeptTmp, currentPageTmp).value;
            var currentDeptArr = currentDept.split(";");
            for (var i = 0; i < currentDeptArr.length - 1; i++) {
                strtmp = '<div id="' + currentDeptArr[i] + 'DetailDiv" style="float:left;width:24%;border:0px solid blue;margin:1px 1px 0px 1px" class="currentDeptDiv_Tab3">';
                strtmp += '<input id="' + currentDeptArr[i] + 'DetailRadio" type="radio" value="' + currentDeptArr[i] + '"  name="currentDeptRadio_Tab3" class="currentDeptRadio_Tab3"/>';
                strtmp += '<a>' + currentDeptArr[i] + '</a></div>';
                $(".thirdTab_Dept").append(strtmp);
            }

            $(".currentDeptDiv_Tab3 :radio").click(function () {
                deptSelected = $(this).val(); //獲取選中部門
                empPageCount = FormSignApprove.EmpPageCount(deptSelected).value; //該部門員工分頁總頁數
                //alert('總頁數:'+empPageCount);
                initPageIndex_emp = 1; //每次選擇部門時，初始化
                //alert('dept selected:.....' + deptSelected);
                getCurrnentEmp(deptSelected, initPageIndex_emp); //選中部門，創建動態人員
            })
        }
        //根據部門獲取員工<處級部門/部級部門:處級員工/部級員工>
        function getEmpBySupiorDept(deptTmp) {//Ajax方法，獲取員工
            $(".thirdTab_Emp").text("");
            var currentEmp = FormSignApprove.GetEmpByDept(deptTmp, 1).value; //當前頁,“.value必須加”；Ajax方式調用後臺有參函數
            //alert('currentEmp:' + currentEmp);
            CreateEmpByDept(currentEmp);
        }

        //根據所選‘課級’部門，獲取員工
        function getCurrnentEmp(deptSelected, currentPageIndex_emp) { //調用Ajax後臺方法，獲取員工
            $(".thirdTab_Emp").text("");
            var currentEmp = FormSignApprove.GetEmpByDept(deptSelected, currentPageIndex_emp).value; //當前頁,“.value必須加”；Ajax方式調用後臺有參函數
            CreateEmpByDept(currentEmp);
        }

        //根據'工號/姓名'，獲取員工
        function getEmpByUserInput(noTmp, currentPageTmp) {
            $(".thirdTab_Emp").text("");
            var currentEmp = FormSignApprove.GetEmpByUserNo(noTmp, currentPageTmp).value; //當前頁,“.value必須加”；Ajax方式調用後臺有參函數
            if (currentEmp == "") {
                alert('信息不存在!');
            } else {
                CreateEmpByDept(currentEmp);
            }
        }
        //員工Div創建
        function CreateEmpByDept(empTmp) {
            var currentEmpArr = empTmp.split(";");
            for (var i = 0; i < currentEmpArr.length - 1; i++) {
                strtmp = '<div id="' + currentEmpArr[i] + 'DetailDiv" style="float:left;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;width:20%;border:0px solid blue;margin:1px 0px 1px 0px" class="currentEmpDiv_Tab3">';
                strtmp += '<input id="' + currentEmpArr[i] + 'DetailCHK" type="checkbox" value="' + currentEmpArr[i] + '"  name="currentEmpCHK__Tab3" class="currentEmpCHK_Tab3"/>';
                strtmp += '<a>' + currentEmpArr[i] + '</a></div>';
                $(".thirdTab_Emp").append(strtmp);
            }


            $(".currentEmpDiv_Tab3 :checkbox").click(function () {

                NotifyInputValue = $("#Approve_txtNofity").val();
                var inputEmp = $("#thirdTab_txt_inputEmp").val(); //文本框的值

                if (this.checked) {
                    if ((inputEmp + NotifyInputValue).indexOf(this.value) >= 0) {
                        alert("人員已存在，忽略選擇!");
                    }
                    else {
                        inputEmp += $(this).val() + "; ";
                        $("#thirdTab_txt_inputEmp").val(inputEmp)
                    }
                }
                else {

                    inputEmp = inputEmp.replace($(this).val() + ";", "");
                    $("#thirdTab_txt_inputEmp").val(inputEmp)
                }
            })

        }
        //'確定'按鈕時判斷
        function checkInfo() {
            return checkISNull();
        }
        //判斷用戶輸入是否為空
        function checkISNull() {
            getPreSetSeqList();
            getMailNoList();
            var currentFlow = $("#CurrentFlow").text(); //當前流程
            var comment = $("#Approve_txtArea").val(); //簽核意見
            var nextOwner = $("#hiddenNextList").val(); //下一關簽核人員
            var preSelectInfo = "";

            if ((nextNodeInfoArr[4] != '') && (nextNodeInfoArr[8] != 'COUNTERSIGN')) {//有預設(并且不是會簽)時，判斷是否選擇了預設
                preSelectInfo = $("#hidden_preNodeSeq").val();
                if ((currentFlow == "") || (comment == "")
                    || (nextOwner == "") || (preSelectInfo == "") || (preSelectInfo == ",")) {

                    alert("輸入不能為空，請檢查!");
                    return false;
                }
            }
            else {//沒有預設時，只判斷這三個字段就OK了
                if ((currentFlow == "") || (comment == "") || (nextOwner == "")) {

                    alert("輸入不能為空，請檢查!");
                    return false;
                }
            }
            $("#Approve_submit").css("display", 'none');
            $("#Approve_submitText").css("display", 'inline-block').attr("disabled", true);
            return true;
        }
        //將工號從選擇人員中抽離出來
        function getMailNoList() {
            var MailList = $("#Approve_txtNofity").val();
            //alert(MailList);
            //林建智(P1151); 顧偉棟(PK1519); 袁偉偉(PK2764); 黃明(FK0546);
            var mailListArr = MailList.split(";");
            var MailNoList = "";
            for (var i = 0; i < mailListArr.length - 1; i++) {

                MailNoList += mailListArr[i].substring(mailListArr[i].indexOf("(") + 1, mailListArr[i].length - 1) + ";";
            }

            $("#hidden_NotifyList").val(MailNoList);
            //alert('通知人員列表:'+$("#hidden_NotifyList").val());
        }
        //將預設的每一關的nodeSeq（關數）和nodeList（工號），放到隱藏域中，以便後臺獲取
        function getPreSetSeqList() {
            var nodeList = nextNodeInfoArr[4].split(","); //5,6,7
            var setSeqStr = "";
            var setListStr = "";

            $(".hiddenSetSeq").each(function (index) {
                setSeqStr += $("#txthiddenNode" + nodeList[index]).val() + ",";
            })

            $(".hiddenSetList").each(function (index) {

                setListStr += $("#txthiddenList" + nodeList[index]).val() + ",";
            })

            $("#hidden_preNodeSeq").val(setSeqStr);
            $("#hidden_preSelectList").val(setListStr);

        }
     </script>
 
</head>

<body>
    <form id="form1" runat="server">
    <div class="Approve_main">

        <div class="Approve_mess">
            <table>
                <tr>
                    <td class="Approve_txt">當前流程:</td>
                    <td><asp:Label ID="CurrentFlow" runat="server" Text="xxxxx"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td>
                    
                    <td style="font-size:14px; margin-left:20px;">簽核動作:</td>
                    <td><asp:Label ID="CurrentAction" runat="server" Text="通過"></asp:Label></td>
                </tr>
            </table>
          
        </div>

        <div class="Approve_marginTop">
            <div class="Approve_txt">簽核意見:</div>
            <div>
                <textarea id="Approve_txtArea" cols="60" rows="3" class="Approve_txtWidth" runat="server">核准!</textarea>
            </div>
        </div>

        <div class="Approve_marginTop">
            <div class="Approve_txt">下一簽核人員:</div>
            <div>
                <asp:DropDownList ID="ddl_nextOwnerList" runat="server"></asp:DropDownList>
                <input id="hiddenNextList" type="hidden" runat="server" />
                <input id="hiddenNextDeptCode" type="hidden" runat="server" />
                <input id="hiddenNowDeptCode" type="hidden" runat="server" />
            </div>
        </div>

        <div class="Approve_moreSign">
            <div class="redStyle">
                
                <%--創建動態Div--%>

            </div>
            <div class="Approve_txtNodeNameSize">
               
               <%--創建動態Div--%>

            </div>
        </div>
        <%--<hr style=" width:90%; margin-top:15px; margin-bottom:5px;" />--%>
        <div class="Approve_marginTop">
            <div class="Approve_txt">通知人員清單:</div>
            <div class="Approve_Notify">
                <asp:TextBox ID="Approve_txtNofity" runat="server" CssClass="Approve_txtWidth" onfocus="this.blur()" TextMode="MultiLine" />
                <input type="button" id="Approve_MailClick" value="點選" class="Mail_but" style="height:20px;width:45px;font-size:12px;margin-left:5px;"/>
                <input type="button" id="Approve_MailReset" value="清空" style="height:20px;width:45px;font-size:12px;margin-left:1px;"/>
            </div>
        </div>
        <div class="Approve_marginTop">
            <div class="Approve_txt">通知信息:</div>
            <div class="Approve_Notify">
                <asp:TextBox ID="Approve_NotifyMsg" runat="server" TextMode="MultiLine" CssClass="Approve_txtWidth"></asp:TextBox>
            </div>
        </div>
        <div class="Approve_marginTop">
            <%--隱藏文本域，存放formType和formID--%>
            <input id="hidden_formTypeID" type="hidden" runat="server" />
            <%--隱藏文本域，存放preNodeSeq--%>
            <input id="hidden_preNodeSeq" type="hidden" runat="server" />
            <%--隱藏文本域，存放NextNodeSeq--%>
            <input id="hiddenNextSeq" type="hidden" runat="server" />
            <%--隱藏文本域，存放preSelectList--%>
            <input id="hidden_preSelectList" type="hidden" runat="server" />
            <%--隱藏文本域，存放deptCode--%>
            <input id="hidden_deptCode" type="hidden" runat="server" />
            <%--隱藏文本域，存放fFormNo+fFormDate--%>
            <input id="hidden_formInfo" type="hidden" runat="server" />
            <%--隱藏文本域，存放表單順序--%>
            <input id="hidden_Node" type="hidden" runat="server" />
            <%--隱藏文本域，存放表單名稱formname--%>
            <input id="hidden_formname" type="hidden" runat="server" />
            <%--隱藏文本域，存放通知人員List值--%>
            <input id="hidden_NotifyList" type="hidden" runat="server" style=" margin-left:100px;" />

            <asp:Button ID="Approve_submit" runat="server" Text="確定" 
                OnClientClick="return checkInfo()" onclick="Approve_submit_Click" cssClass="form_button01"/>
			<asp:Button ID="Approve_submitText" runat="server" Text="處理中..." cssClass="form_button01" style="display:none;"/>
            <asp:Button ID="Approve_close" runat="server" Text="關閉" OnClientClick="window.close();" cssClass="form_button01"/>
        </div>

         <%--彈出的Div層【選簽】--%>
        <div id="faqbg"></div>
        <div id="faqdiv" style="display: none">
            <h2>人員選簽<a href="#" class="close">關閉</a></h2>
            <div style="width:80%;border:1px solid gray; font-size:12px;margin-left:auto;margin-right:auto;">
                
                <div class="signerList" style="border:0px solid red; margin-left:auto;margin-right:auto;">
                
                    <%--動態Div，選擇區域:姓名(工號)--%>

                </div>
                <div class="signerListSelect">
                    
                    <%--動態Div，選擇結果:input文本框顯示所選擇--%>
                    
                </div>

            </div>
        </div>

        <%--彈出的Div層【通知】--%>
        <div id="Mail_faqbg"></div>
        <div id="Mail_faqdiv" style="display: none">
            <h2>人員選擇<a href="#" class="Mail_close">關閉</a></h2>
            <div class="signInfo" id="tabs" style="border:0px solid red">
                <ul>
                    <%--<li><a href="#firstTab">常用選簽人員</a></li>
                    <li><a href="#secondTab">常用主管(部級及以上)</a></li>--%>
                    <li><a href="#thirdTab">按部門/工號/姓名選擇</a></li>
                </ul>

                 <%-- 第一個頁簽--%>
                <%--<div id="firstTab" style=" font-size:12px;border:0px solid red">
                    <div style="color:Red">
                        根據簽核頻率排序
                    </div>
                    <div class="firstTab_Emp" style=" border:0px solid red; width:100%;">
                        
                    </div>

                    <div style="width:100%; margin-top:10px; margin-bottom:10px;"><hr /></div>

                    <div>已選擇:
                        <input id="firstTab_txt_inputEmp" type="text" style=" width:70%; margin-left:10px;" onfocus="this.blur()"/>
                        <a href="#" id="firstTab_InfoOK">確定</a>
                        <a href="#" id="firstTab_InfoEmpty">清空</a>
                    </div>
                </div>--%>


                 <%-- 第二個頁簽--%>
                <%--<div id="secondTab" style=" font-size:12px;border:0px solid red">
                    <div style="color:Red">
                        根據工號排序
                    </div>
                    <div class="secondTab_Emp" style="width:100%; border:0px solid red">
                        

                    </div>

                    <div style="width:100%"><hr /></div>

                    <div>已選擇
                        <input id="secondTab_txt_inputEmp" type="text" style=" width:70%; margin-left:20px;" onfocus="this.blur()"/>
                        
                        <a href="#" id="secondTab_InfoOk">確定</a>
                        <a href="#" id="secondTab_InfoEmpty">清空</a>
                    </div>
                </div>--%>


               <%-- 第三個頁簽--%>
                <div id="thirdTab" style=" font-size:12px;border:0px solid red">
                   <div style=" border:0px solid #ADADA4; float:left; background-color:#E7E7E7;">
                        <%--用於無刷新。。。Begin--%>
                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                        </asp:ScriptManager>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div style="border:0px solid blue; float:left;">
                                    <span style="color:red">按部門:</span>
                                    處級:<asp:DropDownList ID="ddlSupiorDept" runat="server" AutoPostBack="true"  OnSelectedIndexChanged="ddlSupiorDept_SelectedIndexChanged"></asp:DropDownList>
                                    &nbsp;&nbsp;
                                    部級:<asp:DropDownList ID="ddlBySupiorDept" runat="server" AutoPostBack="true" ></asp:DropDownList>
                                    <span style="color:red">按工號/姓名:</span><input type="text" id="txtUserNo" style=" width:60px" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <%--用於無刷新。。。Over--%>
                        <div>
                            <input id="btnCreateDiv" type="button" value="查詢" style=" margin-left:5px; margin-right:3px;" />
                        </div>
                    </div>
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
    <asp:HiddenField ID="hidden_tmpnode" runat="server" />
    
    </form>
</body>
</html>
