using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FOX.DAL;
using System.Configuration;
using Excel = Microsoft.Office.Interop.Excel;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using FOX.COMMON;

public partial class V_FormAllot : System.Web.UI.Page
{
    MyLibs.encrypt.Base64 base64 = new MyLibs.encrypt.Base64();
    string msg = "";
    int effectRow = 0;
    int rowCount = 0;

    string userID = "";
    string userName = "";
    public string CASTATUS = "";//CA狀態
    public string SerialNumberString = "";//CA序號
    public string CAINFO = "";//CA全部信息
    string formNo = "";
    string formID = "";
    string action = "FORWARDSIGN";
    string comment = "";
    string setSelectSeq = "";
    string delimiter = ";";
    string deptCode = "";
    string formInfo = "";
    string formTable = "FormCarApply";
    string formIDCol = "FORMID";
    string sysid = "006";
    string formType = "";
    string factoryarea = "";
    string beginDate1 = "";
    string beginDate2 = "";
    string txtFormID = "";
    string sysname = "GA_CarManagerment";


     string strCon = ConfigurationManager.ConnectionStrings["BASEConnectionString"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {

        Ajax.Utility.RegisterTypeForAjax(typeof(V_FormAllot));
        
        GetInitValue();
        if (!IsPostBack)
        {
            Session.Add("car_IsSysmanager", "N");

            if (Session["UserID"] == null)
            {
                Response.Redirect("http://10.56.69.77/GA_CarManagement");
            }
            else
            {
                userID = Session["UserID"].ToString().Trim();//获取当前登录用户

                checkmanager(sysid, userID);
                checksysman(userID);

                string ismag = Session["car_IsSysmanager"].ToString().Trim();

                if (ismag == "Y")
                {
                    Panel_Text.Visible = false;
                    Panel_Content.Visible = true;
                    BindData();

                }
                else
                {
                    Panel_Text.Visible = true;
                    Panel_Content.Visible = false;
                }
            }
        }

    }
    //翻頁時進行處理
    protected void GridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView.PageIndex = e.NewPageIndex;//獲取當前分頁索引值
        BindData();
    }
    //GridView綁定數據
    protected void BindData()
    {
        DataTable dt = new DataTable();
        dt = getResult();
        if (dt != null)
        {
            DataView view = dt.AsDataView();

            //设置GridView控件的数据源为创建的数据集ds
            GridView.DataSource = view;
            //将数据库表中的主键字段放入GridView控件的DataKeyNames属性中
            GridView.DataKeyNames = new string[] { "FormID" };
            //表頭內容不換行
            GridView.Style.Add("word-break", "keep-all");
            GridView.Style.Add("word-wrap", "normal");
            //绑定数据库表中数据
            GridView.DataBind();
        }
    } 
   
    protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //每行編號
        if (e.Row.RowIndex != -1)
        {
            int id = e.Row.RowIndex + 1;
            e.Row.Cells[0].Text = id.ToString();
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView dv = (DataRowView)e.Row.DataItem;


            string S1 = dv["FactoryArea"].ToString().Trim();  //
            string S2 = dv["FeeCode"].ToString().Trim(); //
            string S3 = dv["SDate"].ToString().Trim(); //
            string S4 = dv["OutTime"].ToString().Trim(); //
            string S5 = dv["ManList"].ToString().Trim();    //
            string S6 = dv["ManNum"].ToString().Trim() + " 人"; //
            string S7 = dv["PhoneNo"].ToString().Trim();   //           
            string S8 = dv["Destination"].ToString().Trim(); //
            S8 = S8.Replace("\r\n", "; ");
            string S9 = dv["Purpose"].ToString().Trim();   //
            S9 = S9.Replace("\r\n", "; ");

            //如果要使鼠标移到每行的每个单元格都显示就把e.Row.Cells[0].Attributes.Add换成e.Row.Attributes.Add
            e.Row.Cells[1].Attributes.Add("onmouseover", "this.oldcolor=this.style.backgroundColor;this.style.backgroundColor='#C8F7FF';this.style.cursor='hand';");
            e.Row.Cells[1].Attributes.Add("onmousemove", "Show('" + S1 + "','" + S2 + "','" + S3 + "','" + S4 + "','" + S5 + "','" + S6 + "','" + S7 + "','" + S8 + "','" + S9 + "');");

            e.Row.Cells[2].Attributes.Add("onmouseout", "this.style.backgroundColor=this.oldcolor;Hide();");
            e.Row.Attributes.Add("onclick", "CheckRow('" + e.Row.RowIndex + "')");

            HyperLink lbl_id = (HyperLink)e.Row.FindControl("lbl_id");
            Label FormUrl = (Label)e.Row.FindControl("FormUrl");
            Label FormType = (Label)e.Row.FindControl("FormType");
            Label FormID = (Label)e.Row.FindControl("FormID");
            Label FormNo = (Label)e.Row.FindControl("FormNo");
            lbl_id.NavigateUrl = FormUrl.Text + "?formType=" + base64.base64encode(FormType.Text) + "&formID=" + base64.base64encode(FormID.Text) + "&formno=" + base64.base64encode(FormNo.Text);

        }
    }
    protected void GridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Send")
        {
            userID = Session["UserID"].ToString().Trim();
            userName = Session["UserName"].ToString().Trim();
            string tmp_no = "";
            string strno = "";
            GridViewRow row = (GridViewRow)((Control)e.CommandSource).Parent.Parent;

            HyperLink txtFormNo = (HyperLink)row.FindControl("lbl_id");   //表單編號 
            tmp_no = txtFormNo.Text.ToString();

            if (tmp_no != "")
            {
                string[] nocollection = tmp_no.Split('-');
                strno = nocollection[0];
            }


                string TheOwner = ((TextBox)row.FindControl("txtTheOwner")).Text.Trim().ToString().ToUpper();
                string TheName = ((TextBox)row.FindControl("txtTheName")).Text.Trim().ToString();
                string OwnerList = ((Label)row.FindControl("txtOwnerList")).Text.Trim().ToString().ToUpper();
                string OwnerName = ((Label)row.FindControl("txtOwnerName")).Text.Trim().ToString().ToUpper();
                string ApplyDate = ((Label)row.FindControl("txtApplyDate")).Text.ToString().Trim();
                formNo = (string)e.CommandArgument;//表單信息
                formID = ((Label)row.FindControl("txtFormID")).Text.ToString().Trim();
                formType = ((Label)row.FindControl("txtFormtype1")).Text.ToString().Trim();
                comment = "( 此表單當前簽核人員被" + userName + "(" + userID + ")從" + OwnerName + "變更為" + TheName + "(" + TheOwner + "))" + ((TextBox)row.FindControl("txtComment")).Text.ToString().Trim();//意見
                formInfo = "formNO=" + formNo + ";FormDate=" + ApplyDate;
                if ((TheOwner != "" && TheName != "無此人！") && (TheOwner != "" && TheName != ""))
                {
                    DoSign(formTable, formIDCol, formType, formID, userID, deptCode, action
        , comment, TheOwner, setSelectSeq, OwnerList, TheOwner
        , comment, "", formInfo, delimiter);
                }
                else
                {
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('變更後人員填寫不正確，無法變更！');", true);
                    ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('變更後人員填寫不正確，無法變更!');", true);
                }
            }
    } 

    //查詢
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string a1 = DateType.SelectedValue.ToString();
        string a2 = txt_applyDate.Text.ToString();
        string a3 = txt_applyDate2.Text.ToString();
        if (a1 != "" && (a2 == "" && a3 == ""))
        {
            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請選擇日期範圍!');", true);
            return;
        }
        if (a2 != "" && a3 != "")
        {
            if (a1 == "")
            {
                ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請選擇日期類別!');", true);
                return;
            }
            if (Convert.ToDateTime(a2) > Convert.ToDateTime(a3))
            {
                ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('起始日期不能大於終止日期!');", true);
                return;
            }
        }
            DataTable dt = new DataTable();
            dt = getResult();
            if (dt != null)
            {
                GridView.DataSource = dt;
                GridView.DataKeyNames = new string[] { "FormID" };
                GridView.DataBind();
            }
  
    }

    //獲取數據，用於綁定GridView
    protected DataTable getResult()
    {
        userID = Session["UserID"].ToString().Trim();//获取当前登录用户

        System.Data.DataTable dt = new DataTable();
        StringBuilder sbrep = new StringBuilder();
        StringBuilder sbval = new StringBuilder();

        txtFormID = txt_Flowformid.Text.ToString();
        string datetype = DateType.SelectedValue.ToString();
        beginDate1 = txt_applyDate.Text.ToString().Trim();
        beginDate2 = txt_applyDate2.Text.ToString().Trim();

        factoryarea = FactoryArea.SelectedValue.ToString().Trim();

        if (txtFormID != "") sbrep.Append(" and FormNo=@P2{?,?} ");
        else sbrep.Append("{?,?}");
        if (factoryarea != "") sbrep.Append(" and FactoryArea=@P3{?,?} ");
        else sbrep.Append("{?,?}");



        if (datetype == "用車日期" && (beginDate1 != "" || beginDate2 != ""))
        {

            if (beginDate1 != "") sbrep.Append(" and (SDate>=@P4){?,?} ");
            else sbrep.Append("{?,?}");
            if (beginDate2 != "") sbrep.Append(" and (SDate<=@P5){?,?} ");
            else sbrep.Append("{?,?}");

        }
        else
        {
            if (beginDate1 != "") sbrep.Append(" and ApplyDate>=@P4{?,?} ");
            else sbrep.Append("{?,?}");
            if (beginDate2 != "") sbrep.Append(" and ApplyDate<=@P5{?,?} ");
            else sbrep.Append("{?,?}");
        }


        sbval.Append(userID).Append("{?,?}")
            .Append(txtFormID).Append("{?,?}")
            .Append(factoryarea).Append("{?,?}");

        sbval.Append(beginDate1).Append("{?,?}")
            .Append(beginDate2).Append("{?,?}");

        sbrep.ToString();
        sbval.ToString();
        try
        {
            DBAccessProc.setConnStr(strCon);
            dt = DBAccessProc.GetDataTable("GA_CarManagerment_PK0338_ChangeCurPerson", 0, 1, sbrep.ToString(), sbval.ToString(), "{?,?}", out msg, out effectRow);
            if ("" != msg)
            {
                Response.Write("GA_CarManagerment_PK0338_ChangeCurPerson Failure:" + msg);
                return null;
            }
        }
        catch (Exception ex)
        {
            Response.Write("GA_CarManagerment_PK0338_ChangeCurPerson error,Detail:" + ex.Message);
        }
        return dt;
    }

    [Ajax.AjaxMethod]
    public string GetUserName(string user)
    {
        //string user = this.TextBox1.Text.ToString();
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbrep = new StringBuilder();
        StringBuilder sbval = new StringBuilder();
        string UserName = "";//聲明一個變量接受查詢數據
        try
        {
            //設定連接字串
            DBAccessProc.setConnStr(strCon);
            sbval.Append(user).Append(","); //定義變量用於獲取變更的參數

            //將查詢數據以DataTable形式取出
            dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetUserName", 0, 1, "", sbval.ToString(), ",", out msg, out effectRow);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {

                    UserName = dr["Name"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("Enotes_PK0338_GetUserName error,Detail:" + ex.Message);
            sbrep.Clear();
            sbval.Clear();
        }

        if ("" != msg)
        {
            Response.Write("Enotes_PK0338_GetUserName Failure:" + msg);

        }

        sbrep.Clear();
        sbval.Clear();

        return UserName;
    }

 
    protected void DoSign(string FormTable, string FormIDCol, string FormType, string FormID
       , string UserID, string DeptCode, string Action, string Comment, string SetNextList,
       string SetSelectSeq, string SetSelectList, string NotifyList, string NofityMsg,
       string CaNo, string FormInfo, string Delimiter)
    {
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbval = new StringBuilder();
        try
        {
            sbval.Append(FormTable).Append("{?+?}").Append(FormIDCol).Append("{?+?}")
                 .Append(FormType).Append("{?+?}").Append(FormID).Append("{?+?}")
                 .Append(UserID).Append("{?+?}").Append(DeptCode).Append("{?+?}")
                 .Append(Action).Append("{?+?}").Append(Comment).Append("{?+?}")
                 .Append(SetNextList).Append("{?+?}").Append(SetSelectSeq).Append("{?+?}")
                 .Append(SetSelectList).Append("{?+?}").Append(NotifyList).Append("{?+?}")
                 .Append(NofityMsg).Append("{?+?}").Append(CaNo).Append("{?+?}")
                 .Append(FormInfo).Append("{?+?}").Append(";").Append("{?+?}");

            sbval.ToString();
            DBAccessProc.setConnStr(strCon);
            DBAccessProc.ExecuteSQL("Enotes_PK0338_DoSign", "", sbval.ToString(), "{?+?}", out msg, out rowCount, out effectRow);
            sbval.Clear();

            if (msg != "")
            {
                //Response.Write("<script language=javascript>window.alert('管理員變更人員!" + msg + "');</script>");
                ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('管理員變更人員!" + msg + "');", true);
                return;
            }
            //Response.Write("<script language=javascript>window.alert('變更成功!');window.location.href=window.location.href;</script>");
            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('變更成功!');window.location.href=window.location.href;", true);
        }
        catch (Exception e)
        {
            Response.Write("DoSign function error:" + e.Message);
        }
    }

    //取基本數據值
    protected void GetInitValue()
    {
        txtFormID = this.txt_Flowformid.Text.ToString().Trim();
        beginDate1 = this.txt_applyDate.Text.ToString().Trim();
        beginDate2 = "";
        if (beginDate1 == "")   //為避免資料量過多時,頁面負載過重,default顯示三個月以內的文件
        {
            beginDate1 = Convert.ToDateTime(DateTime.Today.AddMonths(-12)).ToString("yyyy-MM-dd").Trim();
            //     beginDate2 = Convert.ToDateTime(DateTime.Today.AddMonths(12)).ToString("yyyy-MM-dd").Trim();
        }
    }

    //判斷當前人員是否是平台管理員
    public void checkmanager(string sysid, string userid)
    {
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbval = new StringBuilder();
        try
        {
            DBAccessProc.setConnStr(strCon);//設定連接字串
            sbval.Append(sysid).Append(",")
                .Append(userid).Append(","); //定義變量用於獲取變更的參數

            //將查詢數據以DataTable形式取出
            dt = DBAccessProc.GetDataTable("Enotes_PK0338_IsManagerExist_1", 0, 1, "", sbval.ToString(), ",", out msg, out effectRow);
            if (dt.Rows.Count != 0)
            {
                Session.Add("car_IsSysmanager", "Y");
            }
        }
        catch (Exception ex)
        {
            Response.Write("Enotes_PK0338_IsManagerExist function error,Detail:" + ex.Message);
            sbval.Clear();
        }

        if ("" != msg)
        {
            Response.Write("Enotes_PK0338_IsManagerExist Failure:" + msg);

        }
        sbval.Clear();

    }

    //判斷當前人員是否是當前系統管理員
    public void checksysman(string userid)
    {
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbrep = new StringBuilder();
        StringBuilder sbval = new StringBuilder();
        try
        {
            DBAccessProc.setConnStr(strCon);//設定連接字串
            sbval.Append(userid).Append(","); //定義變量用於獲取變更的參數
            sbrep.Append(sysname + ".dbo.List_SysManager");

            //將查詢數據以DataTable形式取出
            dt = DBAccessProc.GetDataTable("Enotes_PK0338_CheckSysman", 0, 1, sbrep.ToString(), sbval.ToString(), ",", out msg, out effectRow);
            if (dt.Rows.Count != 0)
            {
                Session.Add("car_IsSysmanager", "Y");
            }
        }
        catch (Exception ex)
        {
            Response.Write("Enotes_PK0338_CheckSysman function error,Detail:" + ex.Message);
            sbrep.Clear();
            sbval.Clear();
        }

        if ("" != msg)
        {
            Response.Write("Enotes_PK0338_CheckSysman Failure:" + msg);

        }

        sbrep.Clear();
        sbval.Clear();

    }

    

}