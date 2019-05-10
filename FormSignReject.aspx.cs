﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using FOX.DAL;
using System.Configuration;
using System.Text;

public partial class FormSignReject : System.Web.UI.Page
{
    string strCon = ConfigurationManager.ConnectionStrings["BASEConnectionString"].ToString();
    string msg = "";
    int effectRow = 0;
    int rowCount = 0;

    int effectRow2 = 0;//部門總個數
    int pageSize_Dept = 16;//每頁顯示部門個數
    int pageCount_Dept = 0;//部門總頁數

    int effectRow3 = 0;
    int empCount = 0;////員工總數【根據部門】
    int pageSize_Emp = 30;//每頁顯示人員個數
    int pageCount_Emp = 0;//員工總頁數

    int pageSize_Dept4 = 20;//根據部級獲取其下面的‘課級單位’

    string UserID = "";
    string frontFormID = "";
    string formName = "";
    string FormIDCol = "FORMID";
    string formtype = "";

    public string CASTATUS = "";//CA狀態
    public string SerialNumberString = "";//CA序號
    public string CAINFO = "";//CA全部信息

    protected void Page_Load(object sender, EventArgs e)
    {
        //將該類注冊為ajax類型
        Ajax.Utility.RegisterTypeForAjax(typeof(FormSignReject));

        if (!IsPostBack) 
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("http://10.56.69.77/GA_CarManagement");
            }
            else
            {
                UserID = Session["UserID"].ToString().Trim();//获取当前登录用户
            }
            //UserID = "PK0338";
            setDDLSupiorDept();
            formName = tmp_FormName.Value.ToString();
        }

        
    }

    //確定
    protected void Reject_submit_Click(object sender, EventArgs e)
    {
        DoSign();
    }

    //DoSign --EXEC TEST.dbo.WFDoApprove @P1,@P2,@P3,@P4,@P5,@P6,@P7,@P8,@P9,@P10,@P11,@P12,@P13 OUT
    //參數: @FormTable = 'TEST.dbo.EPS_EFORM_LIST',@FormIDCol = 'FormID', @FormType = N'01',
    //@FormIDValue =,@UserID = '',@Action = '', @COMMENTS = N'',
    //@SetNextList = '',@SetSelectSeq = '',@SetSelectList = N'', 
    //@NotifyList = N'',@NotifyMSG = N'',@MSG = ''
    protected void DoSign()
    {
        string deptCode = this.hidden_deptCode.Value.ToString().Trim();//隱藏域保存用戶所在部門編碼
        string formInfo = this.hidden_formInfo.Value.ToString().Trim();

        //獲取
        //string CANO = Session["CAString"] == null ? "" : Session["CAString"].ToString().Trim();
        string CANO = "";

        //string FormTable = "FormCustVisit";
        string formTypeID = this.hidden_formTypeID.Value.ToString().Trim();//隱藏域文本框值
        string[] formTypeIDArr = formTypeID.Split(new char[] { ';' });
        string formType = formTypeIDArr[0].ToString().Trim();
        string formID = formTypeIDArr[1].ToString().Trim();
        string userID = "";
        formName = tmp_FormName.Value.ToString();

        //userID = "PK0338";
        if (Session["UserID"] != null)
        {
            userID = Session["UserID"].ToString().Trim();
        }
        string Action = "Reject";
        string comment = this.Reject_txtArea.Value;//簽核意見
        string setNextList = this.hiddenNextList.Value.ToString().Trim();//駁回給人員的List
        string setNextDeptCode = this.hiddenNextDeptCode.Value.ToString().Trim();   //增加部門代碼
        string setSelectSeq = this.hiddenNextSeq.Value.ToString().Trim();//駁回到的關卡
        string setSelectList ="";
        string notifyList = this.hidden_NotifyList.Value.ToString().Trim();
        string notifyMsg = this.Reject_NotifyMsg.Text.ToString().Trim();
        string NowDeptCode = this.hiddenNowDeptCode.Value.ToString().Trim();        //記錄當前簽核者的部門代碼

        StringBuilder sbval = new StringBuilder();
        try
        {
            sbval.Append(formName).Append("{?+?}").Append(FormIDCol).Append("{?+?}")
                 .Append(formType).Append("{?+?}").Append(formID).Append("{?+?}")
                 .Append(userID).Append("{?+?}")
                 .Append(deptCode).Append("{?+?}")
                 .Append(Action).Append("{?+?}")
                 .Append(comment).Append("{?+?}").Append(setNextList).Append("{?+?}")
                 .Append(setSelectSeq).Append("{?+?}").Append(setSelectList).Append("{?+?}")
                 .Append(notifyList).Append("{?+?}").Append(notifyMsg).Append("{?+?}")
                 .Append(CANO).Append("{?+?}")
                 .Append(formInfo).Append("{?+?}")
                 .Append(setNextDeptCode).Append("{?+?}")   //增加下一站人員的部門代碼
                 .Append(NowDeptCode).Append("{?+?}")   //增加當前簽核者的部門代碼
                 .Append(";").Append("{?+?}");//表單信息加入
            
            sbval.ToString();
            //EXEC TEST.dbo.WFDoApprove @P1,@P2,@P3,@P4,@P5,@P6,@P7,@P8,@P9,@P10,@P11,@P12,@P13,msg
            DBAccessProc.ExecuteSQL("Enotes_PK0338_DoSign_AddDept", "", sbval.ToString(), "{?+?}", out msg, out rowCount, out effectRow);
            sbval.Clear();

            if (msg != "")
            {
                Response.Write("<script language=javascript>window.alert('操作失敗!" + msg + "');</script>");
                return;
            }
            Response.Write("<script language=javascript>window.alert('駁回成功!');window.close();</script>");
            this.Reject_submit.Enabled = false;//確定按鈕灰色

        }
        catch (Exception e)
        {
            Response.Write("Reject DoSign function error:" + e.Message);
        }

    }

    
    //駁回指定
    [Ajax.AjaxMethod]
    public string setDDLRejectList(string formID, string fn, string sysname)
    {
        string rejectList = "";
        System.Data.DataTable dtNodeSeq = new DataTable();
        string currentNodeSeq = "";
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbvalNodeSeq = new StringBuilder();
        StringBuilder sbval = new StringBuilder();
    //    StringBuilder sbrep = new StringBuilder();
        try
        {
            formName = fn;
            //formName = tmp_FormName.Value.ToString();
            sbvalNodeSeq.Append(formID).Append(",");
            sbvalNodeSeq.Append(formName).Append(",");
            
            
            DBAccessProc.setConnStr(strCon);
            
            //取出當前節點
            dtNodeSeq = DBAccessProc.GetDataTable("Enotes_PK0338_GetCurrentNode", 0, 1, "", sbvalNodeSeq.ToString(), ",", out msg, out effectRow);
            if (dtNodeSeq != null)
            {
                foreach (DataRow dr in dtNodeSeq.Rows)
                {

                    currentNodeSeq = dr["Nodeseq"].ToString().Trim();//當前節點    
                }
            
            currentNodeSeq.ToString();
            //取出當前節點之前的簽核信息
            sbval.Append(currentNodeSeq).Append("{?,?}").Append(formID).Append("{?,?}").Append(formName).Append("{?,?}");

        //    sbrep.Append(sysname + ".dbo.V_EPS_AutoProcess").Append("{?,?}");

            dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetRejectNodeList_2", 0, 1, "", sbval.ToString(), "{?,?}", out msg, out effectRow);
            }
        }
        catch (Exception ex)
        {
            Response.Write("GA_CarManagerment_PK0338_GetRejectNodeList function error,Detail:" + ex.Message);
        }

        if ("" != msg)
        {
            Response.Write("GA_CarManagerment_PK0338_GetRejectNodeList Failure:" + msg);
            return "";
        }
        if (dt != null)
        {
            foreach (DataRow dr in dt.Rows)
            {
                rejectList += dr["RejectNode"].ToString().Trim() + ";";
            }
            rejectList.ToString();
        }
            return rejectList;
        
    }

    //駁回指定---增加部門判斷
    [Ajax.AjaxMethod]
    public string setDDLRejectList_dept(string formID, string fn, string sysname)
    {
        string rejectList = "";
        System.Data.DataTable dtNodeSeq = new DataTable();
        string currentNodeSeq = "";
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbvalNodeSeq = new StringBuilder();
        StringBuilder sbval = new StringBuilder();
        //    StringBuilder sbrep = new StringBuilder();
        try
        {
            formName = fn;
            //formName = tmp_FormName.Value.ToString();
            sbvalNodeSeq.Append(formID).Append(",");
            sbvalNodeSeq.Append(formName).Append(",");


            DBAccessProc.setConnStr(strCon);

            //取出當前節點
            dtNodeSeq = DBAccessProc.GetDataTable("Enotes_PK0338_GetCurrentNode", 0, 1, "", sbvalNodeSeq.ToString(), ",", out msg, out effectRow);
            if (dtNodeSeq != null)
            {
                foreach (DataRow dr in dtNodeSeq.Rows)
                {

                    currentNodeSeq = dr["Nodeseq"].ToString().Trim();//當前節點    
                }

                currentNodeSeq.ToString();
                //取出當前節點之前的簽核信息
                sbval.Append(currentNodeSeq).Append("{?,?}").Append(formID).Append("{?,?}").Append(formName).Append("{?,?}");

                //    sbrep.Append(sysname + ".dbo.V_EPS_AutoProcess").Append("{?,?}");

                dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetRejectNodeList_Dept", 0, 1, "", sbval.ToString(), "{?,?}", out msg, out effectRow);
            }
        }
        catch (Exception ex)
        {
            Response.Write("TestSys_PK0338_GetRejectNodeList_Dept function error,Detail:" + ex.Message);
        }

        if ("" != msg)
        {
            Response.Write("TestSys_PK0338_GetRejectNodeList_Dept Failure:" + msg);
            return "";
        }
        if (dt != null)
        {
            foreach (DataRow dr in dt.Rows)
            {
                rejectList += dr["RejectNode"].ToString().Trim() + ";";
            }
            rejectList.ToString();
        }
        return rejectList;

    }

    //firstTab    最常用人員清單
    [Ajax.AjaxMethod]
    public string GetCommonEmp()
    {
        string commonEmpStr = "";

        System.Data.DataTable dt = new DataTable();
        StringBuilder sbval = new StringBuilder();
        try
        {
            DBAccessProc.setConnStr(strCon);
            dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetCommonEmp", 0, 1, "", sbval.ToString(), ",", out msg, out effectRow);

            if ("" != msg)
            {
                Response.Write("GetCommonEmp Failure:" + msg);
                return "";
            }
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    commonEmpStr += dr["UserIDName"].ToString().Trim() + ";";
                }
                commonEmpStr.ToString();
            }
        }
        catch (Exception ex)
        {
            Response.Write("GetCommonEmp function error,Detail:" + ex.Message);
        }
        return commonEmpStr;
    }


    //secondTab   部級及以上
    [Ajax.AjaxMethod]
    public string GetCommonEmp2()
    {

        string commonEmpStr = "";
        DataTable dt = new DataTable();
        try
        {
            DBAccessProc.setConnStr(strCon);
            //pageSize_Dept->每頁顯示多少條  currentPage->當前頁是第幾頁
            dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetCommon2", 0, 1, "", "", ",", out msg, out effectRow);
            if ("" != msg)
            {
                Response.Write("GetCommonEmp2 Failure:" + msg);
                return "";
            }
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    commonEmpStr += dr["UserNameID"].ToString().Trim() + ";";
                }
                commonEmpStr.ToString();
            }
        }
        catch (Exception ex)
        {
            Response.Write("Reject GetCommonEmp2 function error,Detail:" + ex.Message);
        }
        return commonEmpStr;

    }


    //設定處級單位DropDownList綁定
    protected void setDDLSupiorDept()
    {
        System.Data.DataTable dt = new DataTable();
        try
        {
            DBAccessProc.setConnStr(strCon);
            dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetSupiorDept", 0, 1, "", "", "", out msg, out effectRow);
        }
        catch (Exception ex)
        {
            Response.Write("Reject setDDLSupiorDept function error,Detail:" + ex.Message);
        }

        if ("" != msg)
        {
            Response.Write("setDDLSupiorDept Failure:" + msg);
            return;
        }
        if (dt != null)
        {
            ddlSupiorDept.DataSource = dt.DefaultView;
            ddlSupiorDept.DataValueField = "dept";
            ddlSupiorDept.DataTextField = "dept";
            ddlSupiorDept.DataBind();
            ddlSupiorDept.SelectedIndex = 0;
            ddlSupiorDept.Items.Insert(0, new ListItem("------請選擇------", ""));
            ddlSupiorDept.SelectedIndex = 0;
        }
    }


    //聯動---處級聯動  帶出部級
    protected void ddlSupiorDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        string supiorDept = ddlSupiorDept.SelectedValue.ToString();//選擇的處級單位

        System.Data.DataTable dt = new DataTable();
        StringBuilder sbval = new StringBuilder();
        sbval.Append(supiorDept).Append(",");
        try
        {
            DBAccessProc.setConnStr(strCon);
            dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetDeptBySupiorDept", 0, 1, "", sbval.ToString(), ",", out msg, out effectRow);

            if ("" != msg)
            {
                Response.Write("ddlSupiorDept_SelectedIndexChanged Failure:" + msg);
                return;
            }

        }
        catch (Exception ex)
        {
            Response.Write("Reject ddlSupiorDept_SelectedIndexChanged function error,Detail:" + ex.Message);
        }
        if (dt != null)
        {
            ddlBySupiorDept.DataSource = dt.DefaultView;
            ddlBySupiorDept.DataValueField = "Dept";
            ddlBySupiorDept.DataTextField = "Dept";
            ddlBySupiorDept.DataBind();
            //ddlBySupiorDept.SelectedIndex = 0;
            ddlBySupiorDept.Items.Insert(0, new ListItem("------請選擇------", ""));
            ddlBySupiorDept.SelectedIndex = 0;
        }
    }


    //根據部級單位，獲取‘課級部門’
    [Ajax.AjaxMethod]
    public string GetClassLevel(string deptSelected, int currentPage)
    {
        if (deptSelected == "")
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", "window.alert('請選擇部門!')", true);
            return "";
        }
        else
        {
            string dept = "";
            System.Data.DataTable dt = new DataTable();

            try
            {
                DBAccessProc.setConnStr(strCon);
                dt = DBAccessProc.GetDataTable("CarApply_PK0338_GetClassLevel", pageSize_Dept4, currentPage, "", deptSelected.ToString(), ",", out msg, out effectRow);

                if ("" != msg)
                {
                    Response.Write("getClassLevel Failure:" + msg);
                    return "";
                }
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dept += dr["dept"].ToString().Trim() + ";";
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("Reject getClassLevel function error,Detail:" + ex.Message);
            }

            return dept;
        }
    }



    //獲取部門內員工
    [Ajax.AjaxMethod]
    public string GetEmpByDept(string deptSelected, int currentPage)
    {
        string emp = "";
        System.Data.DataTable dt = new DataTable();

        try
        {
            DBAccessProc.setConnStr(strCon);
            dt = DBAccessProc.GetDataTable("CarApply_PK0338_GetEmpByDept", pageSize_Emp, currentPage, "", deptSelected.ToString(), ",", out msg, out effectRow);

            if ("" != msg)
            {
                Response.Write("getEmpByDept Failure:" + msg);
                return "";
            }
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    emp += dr["Name"].ToString().Trim() + "(" + dr["UserID"].ToString().Trim() + ")" + ";";
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("Reject getEmpByDept function error,Detail:" + ex.Message);
        }
        return emp;
    }


    //獲取員工總頁數  infoStr:可能是部門，可能是工號，可能是姓名
    [Ajax.AjaxMethod]
    public int EmpPageCount(string infoStr)
    {
        DataTable dt = new DataTable();
        try
        {
            DBAccessProc.setConnStr(strCon);
            //effectRow3員工總數    pageSize_Emp 每頁顯示個數
            // SELECT COUNT(UserID) UserID FROM BASE.dbo.BRM_USER_INFO WHERE Dept=@P1 OR UserID LIKE '%'+@P1+'%' OR Name LIKE '%'+@P1+'%'
            dt = DBAccessProc.GetDataTable("CarApply_PK0338_GetEmpCountByDept", 0, 1, "", infoStr.ToString().Trim(), ",", out msg, out effectRow3);

            if ("" != msg)
            {
                Response.Write("DeptPageCount Failure:" + msg);
                return 0;
            }
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    empCount = Convert.ToInt32(dr["UserID"].ToString().Trim());
                }

                float i = (float)empCount / pageSize_Emp;
                pageCount_Emp = Convert.ToInt32(Math.Ceiling(i));
            }

        }
        catch (Exception ex)
        {
            Response.Write("Reject DeptPageCount function error,Detail:" + ex.Message);
        }

        return pageCount_Emp;
    }

    //根據工號/姓名獲取員工
    [Ajax.AjaxMethod]
    public string GetEmpByUserNo(string userTmp, int currentPage)
    {
        string emp = "";
        System.Data.DataTable dt = new DataTable();

        try
        {
            DBAccessProc.setConnStr(strCon);
            // SELECT PAGESIZE NameID INTOTEMP FROM TEST.dbo.V_EPS_ALLUserInfo where Name not like '%兼%' and (UserID like '%'+@P1+'%' OR Name LIKE '%'+@P1+'%')
            dt = DBAccessProc.GetDataTable("CarApply_PK0338_GetUserInfo", pageSize_Emp, currentPage, "", userTmp.ToString(), ",", out msg, out effectRow);

            if ("" != msg)
            {
                Response.Write("getEmpByDept Failure:" + msg);
                return "";
            }
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    emp += dr["NameID"].ToString().Trim() + ";";
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("Reject GetEmpByUserNo function error,Detail:" + ex.Message);
        }
        return emp;
    }

    //根據登錄人員工號，獲取此人所在部級部門，在‘輔助對話框頁簽中’，課級單位默認顯示該部級部門下
    public string GetDefaultClassLevel()
    {
        UserID = Session["UserID"].ToString().Trim();//获取当前登录用户
        //UserID = "PK0338";
        string defaultDept = "";
        System.Data.DataTable dt = new DataTable();

        try
        {
            DBAccessProc.setConnStr(strCon);

            dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetDefaultSupiorDept", 0, 1, "", UserID, ",", out msg, out effectRow);

            if ("" != msg)
            {
                Response.Write("GetDefaultClassLevel Failure:" + msg);
                return "";
            }
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    defaultDept = dr["DeptName"].ToString().Trim();
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("Reject GetDefaultClassLevel function error,Detail:" + ex.Message);
        }
        return defaultDept;
    }
}