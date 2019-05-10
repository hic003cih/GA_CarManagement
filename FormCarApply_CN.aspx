    <%@ Page Title="" Language="C#" MasterPageFile="~/eHRMaster.master" AutoEventWireup="true"
    CodeFile="FormCarApply_CN.aspx.cs" Inherits="FormCarApply_CN" Debug="true"  validateRequest="false" EnableEventValidation="false"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">


    <link href="styles/form.css" rel="stylesheet" type="text/css">

    <script src="Scripts/jquery-1.5.2.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.validate.js" type="text/javascript"></script>
    <script src="Scripts/jquery.tokeninput.js" type="text/javascript"></script>
    <script type="text/javascript" src="Scripts/form_tools.js">    </script>
    <script src="Scripts/jquery_UserNo.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="Styles/token-input.css" />
    <link rel="stylesheet" type="text/css" href="Styles/token-input-facebook.css" />
    <link href="Styles/getScrollTop.css" rel="stylesheet" type="text/css" />    


    <script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>
     <script type="text/javascript" src="Scripts/fileUpload.js">    </script>
     <%--附檔上傳用 --%>
    <script type="text/javascript" src="Scripts/jquery.uploadify.v2.1.0.js">    </script>
    <script type="text/javascript" src="Scripts/swfobject.js">    </script>
   
    <link rel="stylesheet" type="text/css" href="Styles/FormSignAlert.css" />
    <link rel="stylesheet" type="text/css" href="Styles/uploadify.css" />
    <script src="Scripts/FormSign.js" type="text/javascript"></script>

     <%--畫流程圖用 --%>
    <link href="App_Themes/ms_pagelayout.css" rel="stylesheet" type="text/css" /> 

     <%--點選人員用 --%>
    <link rel="stylesheet" type="text/css" href="Styles/clickforSelect.css" />

  <%--  <link rel="stylesheet" type="text/css" href="Styles/FormTable.css" /> --%>
   <link rel="stylesheet" type="text/css" href="Styles/view.css" />


    <link href="Styles/forcustomer.css" rel="stylesheet" type="text/css" />



     <style type="text/css">
         * {
	padding: 0;
	margin: 0;
}     
          .TextboxWatermark {
         font-style: italic;
         color: #ACA899;
       }
         .style10 {
          width:45px;
          border:0; 
          background-color :#F8F8F8; 
         }
         .style12 {
         }
         .style89
         {
             width: 297px;
         }
         .style90
         {
             width: 290px;
         }
         .styleDisplayN
         {
            display:none;   
          }
          .styleDisplayS
         {
            display:"";   
          }
         .style91
         {
             width: 250px;
         }
         .style92
         {
             width: 300px;
         }
                  
         #Del_faqbg
        {
            background-color: #666666;
            position:absolute;
            z-index: 99;
            left: 0;
            top: 0;
            display: none;
            width: 100%;
            /*height: 1000px;*/
            
            opacity: 0.5;
            filter: alpha(opacity=50);
            -moz-opacity: 0.5;
        }
        #Del_faqdiv
        {
            position: absolute;
            width: 380px;
            height: 330px;
            
            /*margin-left:-100px; */
            
            z-index: 100;
            background-color: #fff;
            border: 1px #8FA4F5 solid;
            padding: 1px;
        }
        #Del_faqdiv h2
        {
            height: 25px;
            font-size: 14px;
            background-color: #8FA4F5;
            position: relative;
            /*padding-left: 10px;*/
            line-height: 25px;
        }
        #Del_faqdiv h2 a
        {
            position: absolute;
            right: 5px;
            font-size: 12px;
            color: #FF0000;
        }
        #Del_faqdiv .signInfo
        {
            padding: 10px;
        }
         
         </style>
    <%--table01_flowinfo--%>
   <script type="text/javascript" src="Scripts/Calendar/WdatePicker.js" charset="gb2312"></script> 
     <script type="text/javascript" src="Scripts/WaterMark.min.js" ></script>
   <script type="text/javascript">

                var formType = "";
                var formID = "";
                var formNO = "";
                var userID = "";
                var formYEAR = "";
                var formMONTHDAY = "";
                var formDate = "";
                var deptCode = "";
                var AttUpload1 = "Attach_1";

                $(function () {
                    //formInfo給後臺Ajax註冊的GetNextNodeInfo()方法傳遞變量
                    var formInfo = '<%=BackFormInfo()%>'; //formType;formID;UserID;
                    var formInfoArr = formInfo.split("|||");
                    formID = formInfoArr[0]; //表單ID
                    formNO = formInfoArr[1]; //表單編號
                    userID = formInfoArr[2]; //當前登錄帳號
                    formYEAR = formInfoArr[3]; //表單日期的年
                    formMONTHDAY = formInfoArr[4]; //表單日期的月日
                    formType = formInfoArr[5]; //表單流程
                    formDate = formInfoArr[6]; //表單日期
                    deptCode = formInfoArr[7]; //部門編碼

                    $("#ContentPlaceHolder1_DisManNo").keyup(search);


                    //畫流程圖用
                    var strtmp = "";
                    var strtmp2 = "";
                    var currentNode = '<%=GetCurrentNode() %>';
                    var autoProcess = '<%=GetAutoProcess() %>';
                    var processNode = autoProcess.split(";");
                    if(currentNode=="-1")
                    {
                    drawProcessEnd(processNode);
                    }
                    else
                    {
                    drawProcessInFlow(processNode, currentNode);
                    }

             //=========================以下附檔功能用=================================

             showfactoryarea();

             LoadAttach();
             //附檔上傳
             $(".Att_Upload1").click(function () {                          
                 $("#ContentPlaceHolder1_AttFieldName").val(AttUpload1);
                 $("#Mail_faqbg").css({ display: "block", height: $(document).height() });
                 $("#Mail_faqdiv").css("top", "50%");
                 $("#Mail_faqdiv").css("left", "50%");
                 $("#Mail_faqdiv").css("position", "fixed");
                 $("#Mail_faqdiv").css("margin-top", "-160px");
                 $("#Mail_faqdiv").css("margin-left", "-140px");
                 $("#Mail_faqdiv").css("display", "block");
             });

             $(".Mail_close").click(function () {//‘關閉’則隱藏
                 $("#Mail_faqbg").css("display", "none");
                 $("#Mail_faqdiv").css("display", "none");
             });

             //刪除副檔
             $(".Att_Delete1").click(function () {                          

                 $("#ContentPlaceHolder1_AttFieldName").val(AttUpload1);

                 $("#Del_faqbg").css({ display: "block", height: $(document).height() });

                 $("#Del_faqdiv").css("top", "50%");
                 $("#Del_faqdiv").css("left", "50%");
                 $("#Del_faqdiv").css("position", "fixed");
                 $("#Del_faqdiv").css("margin-top", "-160px");
                 $("#Del_faqdiv").css("margin-left", "-140px");
                 $("#Del_faqdiv").css("display", "block");
             });

             $(".Del_close").click(function () {//‘關閉’則隱藏
                 $("#Del_faqbg").css("display", "none");
                 $("#Del_faqdiv").css("display", "none");
             });


             //=========================以上附檔上傳用=================================

                })

          function search() {
            var v = $("#ContentPlaceHolder1_DisManNo").val();
                 if (v == "") {
                     $("#ContentPlaceHolder1_DisManName").text("");
                     $("#ContentPlaceHolder1_tmp_DisMan").val("");
                 }

                 else {
                     var v1 = FormCarApply_CN.GetUserName(v).value;
                     if (v1 != "") {
                         $("#ContentPlaceHolder1_DisManName").text(v1);
                         $("#ContentPlaceHolder1_tmp_DisMan").val(v1);
                     }
                     else {
                         $("#ContentPlaceHolder1_DisManName").text("無此人！");
                         $("#ContentPlaceHolder1_tmp_DisMan").val("");
                     }

                 }

       }

      

         function openDialogApprove() {//通過

             var Action = "Approved";
             var Approve_nextNodeInfo = FormCarApply_CN.GetNextNodeInfo_dept(formType, formID, userID, Action, formNO, formDate, deptCode).value; //Ajax後臺調用
             var style = 'dialogWidth:670px;dialogHeight:450px;center:yes;edge:raised;scroll:no;status:no'; //設置對話框風格
             window.showModalDialog('FormSignApprove.aspx', Approve_nextNodeInfo, style);
             window.location.href = "FormList.aspx";

         }

         function openDialogReject() {//駁回
             var Action = "Reject";
             var Reject_nextNodeInfo = FormCarApply_CN.GetNextNodeInfo_dept(formType, formID, userID, Action, formNO, formDate, deptCode).value; //Ajax後臺調用
             var style = 'dialogWidth:670px;dialogHeight:400px;center:yes;edge:raised ;scroll:no;status:no'; //設置對話框風格
             window.showModalDialog('FormSignReject.aspx', Reject_nextNodeInfo, style);

             window.location.href = "FormList.aspx";

         }

         function openDialogSign() { //會簽
             var Action = "CounterSign";
             var CounterSign_nextNodeInfo = FormCarApply_CN.GetNextNodeInfo_dept(formType, formID, userID, Action, formNO, formDate, deptCode).value; //Ajax後臺調用
             var style = 'dialogWidth:670px;dialogHeight:400px;center:yes;edge:raised ;scroll:no;status:no'; //設置對話框風格
             window.showModalDialog('FormSignCount.aspx', CounterSign_nextNodeInfo, style);

             window.location.href = "FormList.aspx";
         }

         function openDialogDispatch() { //轉簽
             var Action = "ForwardSign";
             var SignForward_nextNodeInfo = FormCarApply_CN.GetNextNodeInfo(formType, formID, userID, Action, formNO, formDate, deptCode).value; //Ajax後臺調用
             var style = 'dialogWidth:670px;dialogHeight:400px;center:yes;edge:raised ;scroll:yes;status:no'; //設置對話框風格
             window.showModalDialog('FormSignForward.aspx', SignForward_nextNodeInfo, style);

             window.location.href = "FormList.aspx";
         }


         //关闭附檔DIV  
         function CloseDiv() {

             document.getElementById(Mail_faqbg).style.display = 'none';
         }

         //关闭確定刪除附檔DIV  
         function CloseDelDiv() {

             document.getElementById(Del_faqbg).style.display = 'none';
         }



         function LoadAttach() {
             $("#Attach_1").empty();
             var attfieldname = "";
             var attfieldnamelist = "";
             var atttemp = "";
             var fileList2 = "";
             var fileName2 = "";
             var fileExtens2 = "";
             if (formID != "" && formYEAR != "" && formMONTHDAY != "") {
                 atttemp = FormCarApply_CN.GetAttachNameList("006", "01").value; //取得當前表單所包含的附檔欄位清單
                 attfieldnamelist = atttemp.split(";");
                 for (var j = 0; j < attfieldnamelist.length; j++) {            //附檔下載
                     attfieldname = attfieldnamelist[j];
                     fileList2 = FormCarApply_CN.getExistFiles(attfieldname, formYEAR, formMONTHDAY, formID).value;
                     if (fileList2 != "")       //判斷是否取到了附檔
                     {
                         fileName2 = fileList2.split(";");
                         for (var i = 0; i < fileName2.length - 1; i++) {
                             fileExtens2 = FormCarApply_CN.getFilesExtens(fileName2[i]).value;
                         //    drawDescFiles(fileExtens2, fileName2[i], formNO, formID, formYEAR, formMONTHDAY, attfieldname);
                             drawDescFiles_FTP(fileExtens2, fileName2[i], filePath[i],attfieldname);
                         }
                     }
                 }

             }
         
         }

          //後台取值時label的值不會???動,所以改寫到前端
       function ForDirverChange(){
      //    var farea = document.all.<%= FactoryArea.ClientID %>;
     //     var fareavalue = farea.options[farea.selectedIndex].value; 
          var farevalue=$("#ContentPlaceHolder1_FactoryArea").text();
          var drname = document.all.<%= DriverName.ClientID %>;
          var drnamevalue = drname.options[drname.selectedIndex].value; 
          
          $("#ContentPlaceHolder1_tmp_DriverName").val(drnamevalue);

           var driverInfo = FormCarApply_CN.GetDriverInfo(fareavalue,drnamevalue).value;
           if(driverInfo!=""){
           var driverInfoArr = driverInfo.split("|||");
           var carno=driverInfoArr[0];
           var driverphno=driverInfoArr[1];

           $("#ContentPlaceHolder1_CarNo").text(carno);
           $("#ContentPlaceHolder1_LCarNo").text(carno);
           $("#ContentPlaceHolder1_tmp_CarNo").val(carno);

           $("#ContentPlaceHolder1_DriverPhoneNo").text(driverphno);
           $("#ContentPlaceHolder1_LDriverPhoneNo").text(driverphno);
           $("#ContentPlaceHolder1_tmp_DriverPhNo").val(driverphno);
           }
           else{
           $("#ContentPlaceHolder1_CarNo").text("");
           $("#ContentPlaceHolder1_LCarNo").text("");
           $("#ContentPlaceHolder1_tmp_CarNo").val("");

           $("#ContentPlaceHolder1_DriverPhoneNo").text("");
           $("#ContentPlaceHolder1_LDriverPhoneNo").text("");
           $("#ContentPlaceHolder1_tmp_DriverPhNo").val("");
           }
       }


            function setcartypelist()    //設定CarType欄位選項list
                {
             //       var factoryarea = document.all.<%= FactoryArea.ClientID %>;
             //       var selectvalue = factoryarea.options[factoryarea.selectedIndex].value;
                    var selectvalue=$("#ContentPlaceHolder1_FactoryArea").text();
                    document.getElementById('<%= CarType.ClientID %>').options.length = 0;
                    document.getElementById('<%= CarType.ClientID %>').options.add(new Option("---Please Select---","0"));
                    var ctype = FormCarApply_CN.GetCarTypeForArea(selectvalue).value;
                    var ctlist=ctype.split("***");
                    var ctypechi = ctlist[0];
                    var ctypeeng = ctlist[1];
                    ctypechiArr=ctypechi.split("|||");
                    ctypeengArr=ctypeeng.split("|||");
                    var cn1="";
                    var cn2="";
                    for (var m = 0; m < ctypechiArr.length; m++)
                      {
                       cn1=ctypechiArr[m];
                       cn2=ctypeengArr[m];
                       document.getElementById('<%= CarType.ClientID %>').options.add(new Option(cn1, cn2));
                      }   
                  }

             function setdrivername()    //給司機欄位添加值
                 {  // alert("11");
                //      var factoryarea = document.all.<%= FactoryArea.ClientID %>;
                //      var selectvalue = factoryarea.options[factoryarea.selectedIndex].value; 
                      var selectvalue=$("#ContentPlaceHolder1_FactoryArea").text();
                      document.getElementById('<%= DriverName.ClientID %>').options.length = 0;
                      document.getElementById('<%= DriverName.ClientID %>').options.add(new Option("---Please Select---","0"));
                 //      alert("12");
                      var driverInfo = FormCarApply_CN.GetDriverInfoForArea(selectvalue,"").value;
                 //      alert("13");
                      var driverInfoArr = driverInfo.split("|||");
                      var dn="";
                      for (var m = 0; m < driverInfoArr.length; m++)
                        {
                         dn=driverInfoArr[m];
                         document.getElementById('<%= DriverName.ClientID %>').options.add(new Option(dn, dn));
                        }
                        $("#ContentPlaceHolder1_CarNo").text("");
                        $("#ContentPlaceHolder1_LCarNo").text("");
                        $("#ContentPlaceHolder1_tmp_CarNo").val("");
                        $("#ContentPlaceHolder1_DriverPhoneNo").text("");
                        $("#ContentPlaceHolder1_LDriverPhoneNo").text("");
                        $("#ContentPlaceHolder1_tmp_DriverPhNo").val("");
                    //     alert("14");
                 }


              function showfactoryarea() {
              //      var factoryarea = document.all.<%= FactoryArea.ClientID %>;
              //      var selectvalue = factoryarea.options[factoryarea.selectedIndex].value; 
                    var cty="";             
                    var ctyselectvalue="";
                       //非TW
                         document.getElementById('<%= th_bc.ClientID %>').style.display = ""; //台灣要求隱藏兩個欄位:是否並車和點評人員(包含兩個tr,兩個th,兩個td),當非台灣廠區是需要將它們開放
                         document.getElementById('<%= td_bc.ClientID %>').style.display = "";
                         document.getElementById('<%= tr_DisMan.ClientID %>').style.display = "";
                         document.getElementById('<%= Lth_bc.ClientID %>').style.display = ""; 
                         document.getElementById('<%= Ltd_bc.ClientID %>').style.display = "";
                         document.getElementById('<%= Ltr_DisMan.ClientID %>').style.display = "";
                         
                         showCartype1();
                        if (formID=="")
                         {//給司機欄位添加值                          
                            document.getElementById('<%= DriverPhoneNo.ClientID %>').innerHTML="";
                            document.getElementById('<%= tmp_DriverPhNo.ClientID %>').value="";
                            document.getElementById('<%= LDriverName.ClientID %>').innerHTML="";
                            document.getElementById('<%= LDriverPhoneNo.ClientID %>').innerHTML="";
                          }     

                        showCartype1();
                        return true;
            }

            function showCartype()             
            {
         //   alert("01");
            setdrivername();
       //     alert("02");
            showCartype1();
            
            }

           function showCartype1()            
            {        
             //        var factoryarea = document.all.<%= FactoryArea.ClientID %>;                    
             //        var selectvalue = factoryarea.options[factoryarea.selectedIndex].value; 

                    //非TW

                      if (document.getElementById('<%= Panel_DriverInfo.ClientID %>').style.display!="none")
                         {                        
                         document.getElementById('<%= tr_Driverinfo.ClientID %>').style.display = "";
                         }

                      if (document.getElementById('<%= LPanel_DriverInfo.ClientID %>').style.display!="none")
                         {                        
                         document.getElementById('<%= Ltr_Driverinfo.ClientID %>').style.display = "";
                         }

                        if (document.getElementById('<%= CarType.ClientID %>').options.length != 0)
                          {
                           var cty = document.all.<%= CarType.ClientID %>;
                           var ctyselectvalue = cty.options[cty.selectedIndex].value;
                          if(ctyselectvalue=="OfficalCar")                                 
                           {
                              document.getElementById('<%= Panel_ApplyCtrl_11.ClientID %>').style.display = "none";
                              document.getElementById('<%= LPanel_ApplyCtrl_11.ClientID %>').style.display = "none";
                              document.getElementById('<%= Panel_Feedback1.ClientID %>').style.display = "";                        
                              document.getElementById('<%= LPanel_Feedback1.ClientID %>').style.display = ""; 
                              document.getElementById('<%= Panel_DriverInfo2.ClientID %>').style.display = ""; 
                              document.getElementById('<%= LPanel_DriverInfo2.ClientID %>').style.display = ""; 
                              document.getElementById('<%= Panel_Apply_ForSelf.ClientID %>').style.display = "none";
                              document.getElementById('<%= LPanel_Apply_ForSelf.ClientID %>').style.display = "none";  
                               
                              if (document.getElementById('<%= Panel_DriverInfo.ClientID %>').style.display!="none")
                           {
                             document.getElementById('<%= Cap_CarInfo.ClientID %>').style.display = "";
                             document.getElementById('<%= tr_Carinfo.ClientID %>').style.display = "";
                             document.getElementById('<%= table_DisMan.ClientID %>').style.display = "";                             
                             document.getElementById('<%= tr_Driverinfo.ClientID %>').style.display = "";
                           }                           
                            if (document.getElementById('<%= LPanel_DriverInfo.ClientID %>').style.display!="none")
                            {
                             document.getElementById('<%= Ltr_Carinfo.ClientID %>').style.display = "";
                             document.getElementById('<%= Ltr_DisMan.ClientID %>').style.display = "";
                             document.getElementById('<%= LCap_CarInfo.ClientID %>').style.display = "";
                             document.getElementById('<%= Ltr_Driverinfo.ClientID %>').style.display = "";                             
                            }                                    
                           }
                        else if(ctyselectvalue=="SelfDrive")
                          {
                              document.getElementById('<%= Panel_ApplyCtrl_11.ClientID %>').style.display = "";
                              document.getElementById('<%= LPanel_ApplyCtrl_11.ClientID %>').style.display = "";
                              document.getElementById('<%= Panel_Feedback1.ClientID %>').style.display = "none";                        
                              document.getElementById('<%= LPanel_Feedback1.ClientID %>').style.display = "none"; 
                              document.getElementById('<%= Panel_DriverInfo2.ClientID %>').style.display = "none"; 
                              document.getElementById('<%= LPanel_DriverInfo2.ClientID %>').style.display = "none"; 
                              document.getElementById('<%= Panel_Apply_ForSelf.ClientID %>').style.display = "";
                              document.getElementById('<%= LPanel_Apply_ForSelf.ClientID %>').style.display = "";
                           }
                         }
                         else
                           {
                             document.getElementById('<%= Panel_ApplyCtrl_11.ClientID %>').style.display = "none";
                              document.getElementById('<%= LPanel_ApplyCtrl_11.ClientID %>').style.display = "none";
                              document.getElementById('<%= Panel_Feedback1.ClientID %>').style.display = "none";                        
                              document.getElementById('<%= LPanel_Feedback1.ClientID %>').style.display = "none"; 
                              document.getElementById('<%= Panel_DriverInfo2.ClientID %>').style.display = "none"; 
                              document.getElementById('<%= LPanel_DriverInfo2.ClientID %>').style.display = "none"; 
                              document.getElementById('<%= Panel_Apply_ForSelf.ClientID %>').style.display = "none";
                              document.getElementById('<%= LPanel_Apply_ForSelf.ClientID %>').style.display = "none";
                           }
                           
                         
                         
                         return true;              
                 }

                  function showSubCarType()
                  {
                  setdrivername();
                  }



        //方框浮水印
                $(function () {
                    $("[id*=ProjectOwnerID]").WaterMark();
                });
               
                /* 凍結action bar */
                function getScrollTop() {
                    var scrollTop = 0;
                    if (document.documentElement && document.documentElement.scrollTop) {
                        scrollTop = document.documentElement.scrollTop;
                    }
                    else if (document.body) {
                        scrollTop = document.body.scrollTop;
                    }
                    return scrollTop;
                }
         


   </script>
   <div id="div_Action">
        <div id="form_header">
        <table width="1100" border="0" cellspacing="0" cellpadding="0" align="center">      
        <tr style ="background:#dde0ec">
          <td colspan="2" width="400">&nbsp;</td>
          <td colspan="2" class="form_title" align="center" width="300">【用車申請單】</td>
          <td colspan="2" width="400">&nbsp;</td>
        </tr>
      
        </table>
    </div>        
    <asp:Panel ID="Panel_Action" runat="server">
    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
       <ContentTemplate> 
         <table id="Table_Action" class="table01_banner">
             <tr>
                 <td id="td_save" class="style10" runat="server" ><asp:Button ID="save" Text="儲存" class="table01_BtSave" runat="server" OnClick="save_click"></asp:Button> </td>
                 <asp:Panel ID="Panel_Action0" runat="server">
                 
                 <td id="td_approve0" class="style10" runat="server" ><asp:Button ID="approve0" Text="送審" class="table01_BtSubmit" runat="server" OnClick="submit_Click"></asp:Button> </td>
                 <td id="td_approve" class="style10" runat="server" ><asp:Button ID="approve" Text="同意" class="table01_BtSubmit" runat="server" onclick="approve_Click" ></asp:Button> </td>         
                 <td id="td_reject" class="style10" runat="server"  ><asp:Button ID="reject" Text="駁回" class="table01_BtReject" runat="server" OnClientClick="return openDialogReject()"  onclick="reject_Click" ></asp:Button> </td>
                 <td id="td_moreSign" class="style10" runat="server" ><asp:Button ID="moreSign" Text="會簽" class="table01_BtComment" runat="server" OnClientClick="return openDialogSign()"></asp:Button> </td>
                 <td id="td_empty" class="style10"  align="right" style="width:850px"><asp:Label ID="Test" Text="" runat="server"></asp:Label></td>
                 </asp:Panel>
             </tr>
             <tr >
             <td id="td1"  align="right"><asp:Label ID="LabelShow" runat="server" ForeColor="Red" Font-Size="X-Small"></asp:Label></td>
             </tr>
         </table>
         </ContentTemplate>
        </asp:UpdatePanel>
        </asp:Panel>  
        
      <asp:Panel ID="Panel_ForStatus" runat="server">
      <asp:UpdatePanel ID="UpdatePanel6" runat="server">
       <ContentTemplate> 
       <table id="Table1" class="table01_banner">
             <tr>
       <td id="td2"  align="center"><asp:Label ID="Label_ForStatus" runat="server" ForeColor="Red" Font-Size="Larger" Font-Bold="true"></asp:Label></td>
       </tr>
         </table>
       </ContentTemplate>
        </asp:UpdatePanel>
        </asp:Panel>        
    </div>
    <script type="text/javascript">
        window.onscroll = function () {
            var t = document.documentElement.scrollTop || document.body.scrollTop;
            if (t >= 110) {
                $("#div_Action").addClass("style_banner01");
            }
            else {
                $("#div_Action").removeClass("style_banner01");
            }
        }
   </script>
    <div id ="Main_Form">
<form id="form1" >  
<asp:UpdatePanel ID="UpdatePanel5" runat="server">
 <ContentTemplate>
   <table class="table01" >      
     <caption>申請者基本資料</caption> 
        <tr>
   <th class="style91"">編碼</th>
   <td class="style92"">
 <asp:label ID="FormNo" runat="server"  ForeColor="#993333"></asp:label>
<%-- <asp:TextBox ID="FormNo" runat="server" ReadOnly="true"></asp:TextBox>--%>
     </td>
   <td  colspan="2" runat="server" id="manager"> 
                &nbsp;</td>
   </tr>
        
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
            <th>
                申請單位</th>
            <td >
                <asp:Label ID="ApplyDept" runat="server"></asp:Label>
                &nbsp;
                <asp:Label ID="ApplyManDeptCode" runat="server"></asp:Label>
            </td>
            <th >
                申請日期</th>
            <td >
                <asp:Label ID="ApplyDate" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <th>分機</th>
            <td >
                <asp:Label ID="ApplyPhoneNo" runat="server"></asp:Label>
            </td>
            <th>申請者廠區</th>
            <td >
                <asp:Label ID="ApplyArea" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    </ContentTemplate>
        </asp:UpdatePanel>
<asp:Panel runat ="server" ID="Panel_ApplyCtrl">
<asp:UpdatePanel ID="UpdatePanel2" runat="server">
       <ContentTemplate>
   <table class="table01"  > 
   <caption>用車資訊(申請者填寫)</caption> 
        <tr >            
            <th class="style91" >用車類型<span class="requis">*</span></th>
        <td  >
             <asp:DropDownList ID="CarType" runat="server" onchange="showCartype()" 
                 Width="150px">
                 <asp:ListItem Value="OfficalCar" Selected="True">公務車</asp:ListItem>
                 <asp:ListItem Value="SelfDrive">自駕私家車</asp:ListItem>
             </asp:DropDownList>
        </td>
            <th>
                費用代碼<span class="requis">*</span></th>
            <td>
                <asp:TextBox ID="FeeCode" runat="server" Width="270px"></asp:TextBox>
            </td>
        </tr>
        <tr>
           <th>
               用車人聯繫電話<span class="requis">*</span></th>
           <td>
               <asp:TextBox ID="PhoneNo" runat="server" Width="270px"></asp:TextBox>
           </td>
           <th>
               用車廠區<span class="requis">*</span></th>
           <td>
               <asp:Label ID="FactoryArea" runat="server"></asp:Label>
               <br />
           </td>
           <%-- minDate:'%y-%M-#{%d}' 最小值必須大於等於今天--%>
       </tr>
       <tr>
           <th class="style91">
               用車人員<span class="requis">*</span></th>
           <td class="style92">
               <asp:Label ID="ManList" runat="server" ></asp:Label>
           </td>
           <th class="style91">
               用車人數<span class="requis">*</span></th>
           <td class="style92">
               <asp:Label ID="ManNum" runat="server"></asp:Label>
           </td>
       </tr>
       <tr>
          <th >出發時間<span class="requis">*</span></th>
          <td > 
                    <asp:Label ID="Departuretime" runat="server" ></asp:Label>

        </td>
              <th>預返時間<span class="requis">*</span></th>
           <td>
               <asp:Label ID="Prereturntime" runat="server"></asp:Label>
           </td>         
       </tr>
       
        
        <tr>
            <th >目的地<span class="requis">*</span></th>
            <td colspan="3">
                <asp:Label ID="Destination" runat="server"></asp:Label>
            </td>
            
        </tr>
       <tr>
           <th>
               用車事由<span class="requis">*</span></th>
           <td colspan="3">
               <asp:Label ID="Purpose" runat="server"></asp:Label>
               <br />
           </td>
       </tr>
       
    </table>
    </ContentTemplate>
       </asp:UpdatePanel>
     <asp:Panel ID="Panel_Apply_ForSelf" runat ="server">
       <table class="table01"  > 
       <tr>
           <th style="width:167px">
               預計自駕路線<span class="requis">*</span></th>
           <td colspan="3">
               <asp:TextBox ID="PreRoutes" runat="server" BackColor="White" Height="50px" 
                   TextMode="MultiLine" Width="700px"></asp:TextBox>
           </td>
       </tr>
       <tr>
           <th>
               預計公里數<span class="requis">*</span></th>
           <td colspan="3">
               <asp:TextBox ID="PreKilometers" runat="server" Width="270px"></asp:TextBox>
           </td>
       </tr>
       <tr>
           <td colspan="4" style="color: #993333">
               ============自駕私車費用結報標準:============
               <br /> 
               1. 按照實際公出路線行駛之公里數，每公里補助油費1元（RMB）.
               <br />
               2. 過路費、停車費報銷費用根據實際產生費用實報實銷。 
               <br />
               3. 所有費用均應提供有效發票之佐證。</td>
       </tr>
       </table>
       </asp:Panel>
    </asp:Panel>



    <asp:Panel runat ="server" ID="LPanel_ApplyCtrl">
   <table class="table01"  > 
   <caption>用車資訊(申請者填寫)</caption> 
        <tr>            
            <th  class="style91">用車廠區<span class="requis">*</span></th>
        <td class="style90"  >
             <asp:Label ID="LFactoryArea" runat="server"></asp:Label>
        </td>
            <th >用車類型<span class="requis">*</span></th>
            <td class="style12">
                <asp:Label ID="LCarType" runat="server"></asp:Label>
                &nbsp;&nbsp;&nbsp;</td>
        </tr>
       <tr>
           <th class="style91">
               用車人員<span class="requis">*</span></th>
           <td class="style92">
               <asp:Label ID="LManList" runat="server"></asp:Label>
           </td>
           <th class="style91">
               用車人數<span class="requis">*</span></th>
           <td class="style92">
               <asp:Label ID="LManNum" runat="server"></asp:Label>
           </td>
       </tr>
       <tr>
          <th >出發時間<span class="requis">*</span></th>
          <td class="style90" > 
                    <asp:Label ID="LDeparturetime" runat="server"></asp:Label>

        </td>
              <th>預返時間<span class="requis">*</span></th>
           <td>
               <asp:Label ID="LPrereturntime" runat="server"></asp:Label>
           </td>         
       </tr>
       <tr>
         <th>費用代碼<span class="requis">*</span></th><td class="style90">
           <asp:Label ID="LFeeCode" runat="server"></asp:Label>
           </td>   
            
           <th>用車人聯繫電話<span class="requis">*</span></th>
           <td>
               <asp:Label ID="LPhoneNo" runat="server"></asp:Label>
               <br />
           </td>
           
           <%-- minDate:'%y-%M-#{%d}' 最小值必須大於等於今天--%>
       </tr>
       <tr>

            <th >航班號</th>
           <td class="style90" >
                 <asp:Label ID="LPlnum" runat="server"></asp:Label>
                 </td>
           <th >
                抵達或起飛時間</th>
            <td>
                <asp:Label ID="LPldate" runat="server"></asp:Label>
            </td>
          </tr>
        
        <tr>
            <th >目的地<span class="requis">*</span></th>
            <td colspan="3">
                <asp:Label ID="LDestination" runat="server"></asp:Label>
            </td>
            
        </tr>
       <tr>
           <th>
               用車事由<span class="requis">*</span></th>
           <td colspan="3">
               <asp:Label ID="LPurpose" runat="server"></asp:Label>
               <br />
           </td>
       </tr>
    </table>
     <asp:Panel ID="LPanel_Apply_ForSelf" runat ="server">
       <table class="table01"  > 
       <tr>
           <th style="width :167px">
               預計自駕路線<span class="requis">*</span></th>
           <td colspan="3" style="width :900px">
               <asp:Label ID="LPreRoutes" runat="server"></asp:Label>
           </td>
       </tr>
       <tr>
           <th>
               預計行車公里數<span class="requis">*</span></th>
           <td colspan="3">
               <asp:Label ID="LPreKilometers" runat="server"></asp:Label>
           </td>
       </tr>
       <tr>
           <td colspan="4" style="color: #993333">
               ============自駕私車費用結報標準:============
               <br /> 
               1. 按照實際公出路線行駛之公里數，每公里補助油費1元（RMB）.
               <br />
               2. 過路費、停車費報銷費用根據實際產生費用實報實銷。 
               <br />
               3. 所有費用均應提供有效發票之佐證。</td>
       </tr>
       </table>
       </asp:Panel>
    </asp:Panel>

    <asp:Panel id="Panel_DriverInfo" runat="server">
     <asp:Panel id="Panel_DriverInfo2" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
       <ContentTemplate>
    <table class="table01" >
        <caption id="Cap_CarInfo" runat="server">車輛資訊(總務人員填寫)</caption>

        <tr id="tr_Driverinfo" runat="server">
            <th class="style91" >司機姓名<span class="requis">*</span></th>
            <td class="style92">
                
                <asp:DropDownList ID="DriverName" runat="server"  Width="150px" onchange="ForDirverChange()">
                </asp:DropDownList>
                <br />                                   
            </td>
             <th class="style91">司機電話<span class="requis">*</span></th>
            <td class="style92">
                <asp:Label ID="DriverPhoneNo" runat="server" Width="150px"></asp:Label>
            </td>
        </tr>
        <tr id="tr_Carinfo" runat="server">

        <th class="style91">車牌號碼<span class="requis">*</span></th>
            <td class="style92">
                <asp:Label ID="CarNo" runat="server" Width="150px"></asp:Label>
            </td>
            
            <th id="th_bc" class="style91">是否並車<span class="requis">*</span></th>
            <td id="td_bc" class="style92">
                <asp:RadioButtonList ID="IsShare" runat="server" AppendDataBoundItems="True" 
                    RepeatDirection="Horizontal" Width="150px" >
                    <asp:ListItem Value="Yes">Yes</asp:ListItem>
                    <asp:ListItem Value="No">No</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            
        </tr>
        
        
    </table>
    </ContentTemplate>
       </asp:UpdatePanel>
       <table  id="table_DisMan" class="table01" runat="server">
       <tr id="tr_DisMan">
            <th style="width:167px">
                選擇乘車評價人員<span class="requis">*</span>
                </th>
            <td colspan="3">
                <asp:TextBox ID="DisManNo" runat="server" Width="120px"></asp:TextBox>
                &nbsp;
                <asp:Label ID="DisManName" runat="server"></asp:Label>
                &nbsp;&nbsp;
                <span class="requis">(請輸入工號)</span>
                <%--<asp:TextBox ID="MeetingManName" runat="server" BackColor="White" Width="12%"></asp:TextBox>--%></td>
        </tr>
        </table>
        </asp:Panel>
        </asp:Panel>

  <asp:Panel id="LPanel_DriverInfo" runat="server">
  <asp:Panel id="LPanel_DriverInfo2" runat="server">
    <table class="table01" >
        <caption id="LCap_CarInfo" runat="server">車輛資訊(總務人員填寫)</caption>
        <tr id="Ltr_Driverinfo" runat="server">
            <th  class="style91">司機姓名<span class="requis">*</span></th>
            <td class="style92">
                <asp:Label ID="LDriverName" runat="server"></asp:Label>
                <br />                                   
            </td>
             <th  class="style91">司機電話<span class="requis">*</span></th>
            <td  class="style92">
                <asp:Label ID="LDriverPhoneNo" runat="server"></asp:Label>
            </td>
        </tr>

        <tr id="Ltr_Carinfo" runat="server">
            <th class="style91">車牌號碼<span class="requis">*</span></th>
            <td class="style92">
                <asp:Label ID="LCarNo" runat="server"></asp:Label>
            </td>
            
            <th id="Lth_bc" class="style91">是否並車<span class="requis">*</span></th>
            <td id="Ltd_bc" class="style92">
                <asp:Label ID="LIsShare" runat="server"></asp:Label>
            </td>
            
        </tr>
        
        <tr id="Ltr_DisMan" runat="server">
            <th>
                乘車後評價人員<span class="requis">*</span></th>
            <td colspan="3">
                <asp:Label ID="LDisMan" runat="server"></asp:Label>
            </td>
        </tr>

    </table>
        </asp:Panel>
        </asp:Panel>
              

<asp:Panel ID="Panel_Feedback" runat="server">
<asp:Panel ID="Panel_Feedback1" runat="server">
<asp:UpdatePanel ID="UpdatePanel3" runat="server">
       <ContentTemplate>
      <table class="table01" >
        <caption>意見反饋(乘車人員填寫)</caption>
        <tr>
            <th  class="style91">服務<span class="requis">*</span></th>
            <td class="style92">
                <asp:RadioButtonList ID="Feedback_Service" runat="server" AppendDataBoundItems="True" 
                    RepeatDirection="Horizontal" Width="200px">
                    <asp:ListItem>好</asp:ListItem>
                    <asp:ListItem>一般</asp:ListItem>
                    <asp:ListItem>差</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <th class="style91">衛生<span class="requis">*</span></th>
            <td class="style92">
                <asp:RadioButtonList ID="Feedback_Clean" runat="server" 
                    AppendDataBoundItems="True" RepeatDirection="Horizontal" Width="200px">
                    <asp:ListItem>好</asp:ListItem>
                    <asp:ListItem>一般</asp:ListItem>
                    <asp:ListItem>差</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <th >滿意度<span class="requis">*</span></th>
            <td>
                <asp:RadioButtonList ID="Feedback_Satisfied" runat="server" 
                    AppendDataBoundItems="True" RepeatDirection="Horizontal" Width="200px">
                    <asp:ListItem>好</asp:ListItem>
                    <asp:ListItem>一般</asp:ListItem>
                    <asp:ListItem>差</asp:ListItem>
                </asp:RadioButtonList>                                  
            </td>
             <th >實際用車完成時間<span class="requis">*</span></th>
            <td >
                <asp:TextBox ID="AcFinishTime" runat="server" class="Wdate" 
                    onclick="WdatePicker({startDate:'%y-%M-%d 17:00:00',dateFmt:'yyyy-MM-dd HH:mm'})" Width="270px"></asp:TextBox>
            </td>
        </tr>
    </table>           
    </ContentTemplate>
       </asp:UpdatePanel>                       
    </asp:Panel>
</asp:Panel>

<asp:Panel ID="LPanel_Feedback" runat="server">
<asp:Panel ID="LPanel_Feedback1" runat="server">
      <table class="table01" >
        <caption>意見反饋(乘車人員填寫)</caption>
        <tr>
            <th  class="style91">服務<span class="requis">*</span></th>
            <td class="style92">
                <asp:Label ID="LFeedback_Service" runat="server"></asp:Label>
            </td>
            <th class="style91">衛生<span class="requis">*</span></th>
            <td class="style92">
                <asp:Label ID="LFeedback_Clean" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <th >滿意度<span class="requis">*</span></th>
            <td>
                <asp:Label ID="LFeedback_Satisfied" runat="server"></asp:Label>
            </td>
             <th >實際用車完成時間<span class="requis">*</span></th>
            <td >
                <asp:Label ID="LAcFinishTime" runat="server"></asp:Label>
            </td>
        </tr>
    </table>                                  
</asp:Panel>
</asp:Panel>

<asp:Panel ID="Panel_ApplyCtrl_1" runat="server">
<asp:Panel ID="Panel_ApplyCtrl_11" runat="server">
        <table class="table01"  > 
            <caption>實際用車資訊(申請者填寫)</caption> 
       <tr>
          <th  class="style91">實際出發時間<span class="requis">*</span></th>
          <td class="style92" > 
                    <asp:TextBox ID="AcDeparturetime" runat="server" class="Wdate" 
                        onclick="WdatePicker({startDate:'%y-%M-#{%d+1} 08:00:00',dateFmt:'yyyy-MM-dd HH:mm'})" Width="270px"></asp:TextBox>

        </td>
              <th class="style91">實際返回時間<span class="requis">*</span></th>
           <td class="style92">
               <asp:TextBox ID="Acreturntime" runat="server" class="Wdate" 
                   onclick="WdatePicker({startDate:'%y-%M-#{%d+1} 17:00:00',dateFmt:'yyyy-MM-dd HH:mm'})" Width="270px"></asp:TextBox>
           </td>         
       </tr>
       <tr>
         <th>實際行車公里數<span class="requis">*<br />
             <br />
             </span></th>
           <td >
               <asp:TextBox ID="Kilometers" runat="server" Width="270px"></asp:TextBox>
           </td>   
            
           <th>實際加油量<span class="requis">*</span></th>
           <td>
               <asp:TextBox ID="OilNum" runat="server" Width="270px"></asp:TextBox>
               <br />
           </td>
           
       </tr>
       <tr>

            <th >實際用車人員<span class="requis">*</span></th>
           <td class="style89" >
                 <asp:TextBox ID="AcManList" runat="server" Width="270px"></asp:TextBox>
                 </td>
           <th >
                實際用車人數<span class="requis">*</span></th>
            <td>
                 <asp:TextBox ID="AcManNum" runat="server" Width="270px"></asp:TextBox>
            </td>
          </tr>
            <tr>
                <th>
                    實際出發地點<span class="requis">*</span></th>
                <td class="style89">
                    <asp:TextBox ID="AcDestination" runat="server" Width="270px"></asp:TextBox>
                </td>
                <th>
                    實際到達地點<span class="requis">*</span></th>
                <td>
                    <asp:TextBox ID="AcArrive" runat="server" Width="270px"></asp:TextBox>
                </td>
            </tr>
        <tr>
            <th >行車路線<span class="requis">*</span></th>
            <td colspan="3">
                <asp:TextBox ID="Routes" runat="server" BackColor="White" TextMode="MultiLine" 
                    Width="700px" Height="50px"></asp:TextBox>
                <br />
            </td>
            
        </tr>
            
    </table>
       </asp:Panel>         
</asp:Panel>

<asp:Panel ID="LPanel_ApplyCtrl_1" runat="server">
<asp:Panel ID="LPanel_ApplyCtrl_11" runat="server">
        <table class="table01"  > 
            <caption>實際用車資訊(申請者填寫)</caption> 
       <tr>
          <th  class="style91">實際出發時間<span class="requis">*</span></th>
          <td class="style92" > 
                    <asp:Label ID="LAcDeparturetime" runat="server"></asp:Label>

        </td>
              <th class="style91">實際返回時間<span class="requis">*</span></th>
           <td class="style92">
               <asp:Label ID="LAcreturntime" runat="server"></asp:Label>
           </td>         
       </tr>
       <tr>
         <th>實際行車公里數<span class="requis">*</span><br /> </th>
           <td class="style89">
               <asp:Label ID="LKilometers" runat="server"></asp:Label>
           </td>   
            
           <th>實際加油量<span class="requis">*</span></th>
           <td>
               <asp:Label ID="LOilNum" runat="server"></asp:Label>
               <br />
           </td>
           
           <%-- minDate:'%y-%M-#{%d}' 最小值必須大於等於今天--%>
       </tr>
       <tr>

            <th >實際用車人員<span class="requis">*</span></th>
           <td class="style89" >
                 <asp:Label ID="LAcManList" runat="server"></asp:Label>
                 </td>
           <th >
                實際用車人數<span class="requis">*</span></th>
            <td>
                 <asp:Label ID="LAcManNum" runat="server"></asp:Label>
            </td>
          </tr>
            <tr>
                <th>
                    實際出發地點<span class="requis">*</span></th>
                <td class="style89">
                    <asp:Label ID="LAcDestination" runat="server"></asp:Label>
                </td>
                <th>
                    實際到達地點<span class="requis">*</span></th>
                <td>
                    <asp:Label ID="LAcArrive" runat="server"></asp:Label>
                </td>
            </tr>
        <tr>
            <th >行車路線<span class="requis">*</span></th>
            <td colspan="3">
                <asp:Label ID="LRoutes" runat="server"></asp:Label>
                <br />
            </td>
            
        </tr>
    </table>
     </asp:Panel>           
</asp:Panel>

<asp:Panel ID="Panel_Invoice" runat="server">
        <table class="table01"  >

        <tr>
                <th style="width:167px">
                    附檔</th>
                <td colspan="4">
                <span id="Attach_1" class="approver"></span>
                &nbsp;&nbsp;
                <asp:Panel ID="Panel_Att0" runat="server" Width="140px">
                <input type="button" id="AttachFile" value="瀏覽" class="Att_Upload1" />

                <input type="button" id="DeleteAtt" value="刪除副檔" class="Att_Delete1" />

                </asp:Panel>
                </td>

            
            </tr>
            </table>    
</asp:Panel>

         <div>  
          <%--table01_flowinfo--%>
        <table class="table01">
        <caption>簽核流程</caption>
        <tr><td>       
         <%--畫流程圖用--%>
        <span id="ContentPlaceHolder1_flowLabel" style="color: #0066FF; background-color: #0066FF;font-size: Large;">
        </span> 
        </td></tr> 
        </table>
        <table class="table01">
            <caption>簽核履歷</caption>

      
            <asp:GridView ID="Flow_GridView" runat="server" CssClass="tb04" BorderStyle="Solid" 
                                BorderWidth="2px" AutoGenerateColumns="False"  >
                            <EmptyDataTemplate>
                        <font color="red" size="5pt" style="font-family: 微軟正黑體; font-size: medium; color: #FF0000">尚無記錄</font>
                    </EmptyDataTemplate>
<Columns>
            <asp:BoundField DataField="NodeSeq" HeaderText="站別" >
            <ItemStyle HorizontalAlign="Left" Width="5%" />
            </asp:BoundField>
            <asp:BoundField DataField="NodeName" HeaderText="簽核流程" >
            <ItemStyle Width="10%" />
            </asp:BoundField>
            <asp:BoundField DataField="OwnerList" HeaderText="簽核者" >
            <ItemStyle Width="15%" />
            </asp:BoundField>
            <asp:BoundField DataField="UpdateTime" HeaderText="時間" >
            <ItemStyle Width="20%" />
            </asp:BoundField>
            <asp:BoundField DataField="Action" HeaderText="動作">
            <ItemStyle Width="10%" />
            </asp:BoundField>
            <asp:BoundField DataField="Comment" HeaderText="意見" >
            </asp:BoundField>
        </Columns>
                    </asp:GridView>
            </table>
        
        </div>

            <asp:Panel ID="Panel_Att" runat="server">
    <div id="Mail_faqbg"></div>
        <div id="Mail_faqdiv" style="display: none">
        
            <h2>上傳附檔<a href="javascript:void(0)" class="Mail_close">關閉</a></h2>
            <div class="divFrame">
                <div class="divTitle">
                    上傳附檔
                </div>
                <div class="divContent">
                    <ul>
                    </ul>
                    <div id="fileQueue" style="clear: both; padding-top: 5px">
                    </div>
                    <div style="padding-top: 5px">
                        <input type="file" name="uploadify" id="uploadify" value="上傳" /></div>
                </div>
             </div>
             <input type="hidden" id="frontname" runat="server"/>
             <div style="float:left">
              
            <%-- <asp:LinkButton ID="SaveFile" runat="server" OnClientClick="return SaveAttFile()">確定保存</asp:LinkButton>--%>
             <asp:LinkButton ID="LinkButton2" runat="server" OnClick="SaveFile_Click">確定保存</asp:LinkButton>

            </div>

            
     </div>
        </asp:Panel>

         <asp:Panel ID="DelAttConfirm" runat="server">
    <div id="Del_faqbg"></div>
        <div id="Del_faqdiv" style="display: none">
         <h2>刪除附檔<a href="javascript:void(0)" class="Del_close">關閉</a></h2>
            <div class="divFrame">
            <br />
               <font color="red">將刪除該欄位中的所有副檔,請提前做好備份</font>
               <br />
               <br />
               <font color="blue">是否確定刪除?</font>
               
             </div>
             <br />
             <div style="float:left">

             <asp:Button ID="LinkButton1" Text="確定" runat="server" OnClick="DeleteAttFile"></asp:Button>
             &nbsp;&nbsp;
             <input type="button" id="Button1" value="取消" class="Del_close" />

            </div>            
     </div>
        </asp:Panel>
       
       <asp:HiddenField ID="AttFieldName" runat="server"  />      
       <asp:HiddenField ID="tmp_DisMan" runat="server" />
       <asp:HiddenField ID="tmp_DriverName" runat="server" />
       <asp:HiddenField ID="tmp_CarNo" runat="server" />
       <asp:HiddenField ID="tmp_DriverPhNo" runat="server" />
       <asp:HiddenField ID="tmp_ID" runat="server" />
       <asp:HiddenField ID="tmp_FType" runat="server" />
       <asp:HiddenField ID="tmp_FNo" runat="server" />
       <asp:HiddenField ID="hid_BTCNo" runat="server" />
       <asp:HiddenField ID="hid_OwnerDeptCode" runat="server" />
       <asp:HiddenField ID="hid_Ownerlist" runat="server" />

        </form>
    </div>


    </asp:Content>