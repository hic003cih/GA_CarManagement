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

public partial class DataList_SysManager : System.Web.UI.Page
{
    string msg = "";
    int effectedRow = 0;
    int rowcount = 0;
    string userid = "";
    string sysid = "006";
    string sysname = "GA_CarManagerment";
    
    int effectRow = 0;
    string creator = "";

    string NewUserName = "";
    string NewUserID = "";
    string areaname = "";
    string tableid = "";
    int UserRoleExistYN = 0;
    string row_id = "";
    

    public class aa
    {
        public static string IsSysmanager = "N";
    }

    string strCon = ConfigurationManager.ConnectionStrings["BaseConnectionString"].ToString();


    protected void Page_Load(object sender, EventArgs e)
    {

        Ajax.Utility.RegisterTypeForAjax(typeof(DataList_SysManager));  //將該類注冊為ajax類型

        if (!IsPostBack)
        {

            if (Session["UserID"] == null)
            {
                Response.Redirect("http://10.56.69.77/GA_CarManagement");
            }
            else
            {
                userid = Session["UserID"].ToString().Trim();//获取当前登录用户

                aa.IsSysmanager = "N";

               checkmanager(sysid, userid);
               checksysman(userid);
                if (aa.IsSysmanager == "N")
                {
                    btnAdd.Visible = false;

                }
                else
                {
                    btnAdd.Visible = true;

                }
                BindData();
          //      setAreaList();   //取得廠區list
                setTableList();
            }

        }

    }

    public void BindData()
    {
        System.Data.DataTable dt = new DataTable();

        dt = getResult();
        if (dt != null)
        {
            DataView view = dt.AsDataView();

            GridView.DataSource = view;
            ////将数据库表中的主键字段放入GridView控件的DataKeyNames属性中
            //GridView.DataKeyNames = new string[] { "UserID" };
            //表頭內容不換行
            GridView.Style.Add("word-break", "keep-all");
            GridView.Style.Add("word-wrap", "normal");
            //绑定数据库表中数据
            GridView.DataBind();
        }
    }

    protected void btnQueryClick(object sender, EventArgs e)
    {
        System.Data.DataTable dt = new DataTable();

        dt = getResult();

        if (dt != null)
        {
            //设置GridView控件的数据源为创建的数据集ds
            GridView.DataSource = dt;
            //将数据库表中的主键字段放入GridView控件的DataKeyNames属性中
            GridView.DataKeyNames = new string[] { "ID" };
            //绑定数据库表中数据
            GridView.DataBind();
        }
    }

    //翻頁時進行處理
    protected void GridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView.PageIndex = e.NewPageIndex;//獲取當前分頁索引值
        BindData();
    }


    public DataTable getResult()
    {
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbrep = new StringBuilder();
        StringBuilder sbval = new StringBuilder();

        string txtnewuserid = txt_NewUserID.Text.ToString().Trim();
        string txtareaname = AreaName.SelectedValue.ToString().Trim();
        string txttableid = TableName.SelectedValue.ToString().Trim();


        if (txtnewuserid != "") sbrep.Append(" and UserID=@P1{?,?} ");
        else sbrep.Append("{?,?}");
        if (txtareaname != "0") sbrep.Append(" and AreaName=@P2{?,?} ");
        else sbrep.Append("{?,?}");
        if (txttableid != "") sbrep.Append(" and TableID=@P3{?,?} ");
        else sbrep.Append("{?,?}");
        sbrep.Append(sysname + ".dbo.List_SysManager");

        sbval.Append(txtnewuserid).Append("{?,?}")
            .Append(txtareaname).Append("{?,?}")
            .Append(txttableid).Append("{?,?}")
            .Append(sysid).Append("{?,?}");


        sbrep.ToString();
        sbval.ToString();


        try
        {
            
            //將查詢數據以DataTable形式取出
            dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetSysMan", 0, 1, sbrep.ToString(), sbval.ToString(), "{?,?}", out msg, out effectedRow);

        }
        catch (Exception ex)
        {
            Response.Write("GA_CarManagerment_PK0338_GetSysMan error,Detail:" + ex.Message);
            sbrep.Clear();
            sbval.Clear();
        }

        if ("" != msg)
        {
            Response.Write("GA_CarManagerment_PK0338_GetSysMan Failure:" + msg);
            return null;
        }

        sbrep.Clear();
        sbval.Clear();

        return dt;

    }


    /// <summary>
    /// 新增事件---確定按鈕事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtn_Yes_Click(object sender, EventArgs e)
    {

        
        userid = Session["UserID"].ToString().Trim();

        creator = GetUserName(userid);

        System.Data.DataTable dt = new DataTable();
        StringBuilder sbval = new StringBuilder();

        tableid = TableName.SelectedValue.ToString();
        if (tableid == "")
        {

            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請選擇表單名稱!');", true);
            return;
        }
        
        areaname = AreaName.SelectedValue.ToString().Trim();
        if (areaname == "")
        {

            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請選擇廠區!');", true);
            return;
        }

        NewUserID = txt_NewUserID.Text.Trim();
        if (NewUserID == "")//數據為空提示
        {
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "", "window.alert('工號不能為空!');", true);
            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請填寫工號!');", true);
            return;
        }

        
        NewUserName = tmp_Name.Value.ToString().Trim();
        if (NewUserName == "")
        {

            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('該人員姓名不存在,請再次確認,謝謝!');", true);
            return;
        }

        try
        {
        UserRoleExistYN = GetExistYN(NewUserID, areaname, tableid);

         if (UserRoleExistYN > 0)
         {

             ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('該用戶對應的這個權限已存在!');", true);
             return;
         }

                    StringBuilder sbrep = new StringBuilder();
                    DBAccessProc.setConnStr(strCon);
                    sbval.Append(userid).Append(",")
                    .Append(NewUserID).Append(",")
                    .Append(NewUserName).Append(",")
                    .Append(areaname).Append(",")
                    .Append(tableid).Append(",");
                    sbrep.Append(sysname + ".dbo.List_SysManager");

                    DBAccessProc.ExecuteSQL("Enotes_PK0338_AddSysManagerForSys", sbrep.ToString(), sbval.ToString(), ",", out msg, out rowcount, out effectedRow);
                    this.GridView.DataSource = dt.DefaultView;
                    this.BindData();
                   if (msg == "")
                    {
                       ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('新增人員權限成功!');", true);
                    }

                }
                catch (Exception ex)
                {
                    Response.Write("GA_CarManagerment_PK0338_AddSysManagerForSys function error,Detail:" + ex.Message);
                    sbval.Clear();
                }

                TableName.SelectedIndex = 0;
                AreaName.SelectedIndex = 0;
                txt_NewUserID.Text = "";
                txt_NewUserName.Text = "";
                BindData();

    }


    /// <summary>
    /// 新增按鈕事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_NewInsert_Click(object sender, EventArgs e)
    {

        GridView.ShowFooter = true;//顯示
        this.BindData();
    }

    /// <summary>
    /// 新增事件---取消按鈕事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtn_No_Click(object sender, EventArgs e)
    {
        GridView.ShowFooter = false;//隱藏
        this.BindData();
    }

    public int GetExistYN(string userid,string areaname,string tableid)
    {
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbrep = new StringBuilder();
        StringBuilder sbval = new StringBuilder();
        int countNo = 0;//聲明一個變量接受查詢數據
        try
        {
            //設定連接字串
            DBAccessProc.setConnStr(strCon);
            sbval.Append(tableid).Append(",")
                 .Append(userid).Append(",")
                 .Append(areaname).Append(",");

            sbrep.Append(sysname + ".dbo.List_SysManager");

            //將查詢數據以DataTable形式取出
            dt = DBAccessProc.GetDataTable("Enotes_PK0338_CheckSysmanForTable", 0, 1, sbrep.ToString(), sbval.ToString(), ",", out msg, out effectedRow);
            if (dt != null)
            {
                countNo = int.Parse(dt.Rows[0]["num"].ToString());
            }
        }
        catch (Exception ex)
        {
            Response.Write("GA_CarManagerment_PK0338_CheckSysmanForTable function error,Detail:" + ex.Message);
            sbrep.Clear();
            sbval.Clear();
        }

        if ("" != msg)
        {
            Response.Write("GA_CarManagerment_PK0338_CheckSysmanForTable Failure:" + msg);

        }

        sbrep.Clear();
        sbval.Clear();

        return countNo;

    }






    /// <summary>
    /// 激活編輯事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridView_RowEditing(object sender, GridViewEditEventArgs e)
    {
        //一定要在BindData()之前獲取Lable控件，否則會空指針異常
        Label lblRoleName = (Label)GridView.Rows[e.NewEditIndex].FindControl("lbl_UserID");

        this.GridView.EditIndex = e.NewEditIndex;
        BindData();
    }
    /// <summary>
    /// 取消事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        this.GridView.EditIndex = -1;//取消編輯狀態
        BindData();//綁定數據

    }


    /// <summary>
    /// 光標停留行變色
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //每行編號
        if (e.Row.RowIndex != -1)
        {
            int id = e.Row.RowIndex + 1;
            e.Row.Cells[0].Text = id.ToString();
        }
        if (e.Row.RowType == DataControlRowType.DataRow)//光標變色
        {
            e.Row.Cells[1].Attributes.Add("onmouseover", "this.oldcolor=this.style.backgroundColor;this.style.backgroundColor='#C8F7FF';this.style.cursor='hand';");
            e.Row.Cells[1].Attributes.Add("onmouseout", "this.style.backgroundColor=this.oldcolor;Hide();");
        }
    }

    protected void GridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (aa.IsSysmanager == "N")
        {

            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('非管理員不得刪除資料!');", true);
            return;
        }
        else
        {
            StringBuilder sbval = new StringBuilder();
            StringBuilder sbrep = new StringBuilder();

            try
            {
                if (Session["UserID"] == null)
                {
                    Response.Redirect("http://10.56.69.77/GA_CarManagement");
                }
                else
                {
                    userid = Session["UserID"].ToString().Trim();//获取当前登录用户
                }

                row_id = GridView.DataKeys[e.RowIndex].Value.ToString();
                sbval.Append(row_id).Append(",")
                    .Append(userid).Append(",");

                sbrep.Append(sysname + ".dbo.List_SysManager");

                sbval.ToString();
                DBAccessProc.ExecuteSQL("Enotes_PK0338_DeleteSysMan", sbrep.ToString(), sbval.ToString(), ",", out msg, out rowcount, out effectedRow);
                sbval.Clear();

                if (msg != "")
                {
                    Response.Write("GA_CarManagerment_PK0338_DeleteManager Error:" + msg);
                }
            }
            catch (Exception ex)
            {
                Response.Write("GA_CarManagerment_PK0338_DeleteManager function error:" + ex.Message);
            }

            BindData();
        }
    }

    [Ajax.AjaxMethod]
    public string GetUserName(string userId)
    {

        System.Data.DataTable dt = new DataTable();
        StringBuilder sbrep = new StringBuilder();
        StringBuilder sbval = new StringBuilder();
        string UserName = "";//聲明一個變量接受查詢數據
        try
        {
            //設定連接字串
            DBAccessProc.setConnStr(strCon);
            sbval.Append(userId).Append(","); //定義變量用於獲取變更的參數

            //將查詢數據以DataTable形式取出
            dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetUserName", 0, 1, "", sbval.ToString(), ",", out msg, out effectedRow);

            foreach (DataRow dr in dt.Rows)
            {

                UserName = dr["Name"].ToString();
            }
        }
        catch (Exception ex)
        {
            Response.Write("Enotes_PK0338_GetUserName function error,Detail:" + ex.Message);
            sbrep.Clear();
            sbval.Clear();
        }

        if ("" != msg)
        {
            Response.Write("GetUserName Failure:" + msg);

        }

        sbrep.Clear();
        sbval.Clear();

        return UserName;
    }

   

    //設定廠區欄位DropDownList綁定
    protected void setAreaList()
    {
        System.Data.DataTable dt = new DataTable();
        try
        {
            DBAccessProc.setConnStr(strCon);
            dt = DBAccessProc.GetDataTable("GA_CarManagerment_PK0338_GetAreaList", 0, 1, "", "", "", out msg, out effectRow);
        }
        catch (Exception ex)
        {
            Response.Write("GA_CarManagerment_PK0338_GetAreaList function error,Detail:" + ex.Message);
        }

        if ("" != msg)
        {
            Response.Write("GA_CarManagerment_PK0338_GetAreaList Failure:" + msg);
            return;
        }
        if (dt != null)
        {
            AreaName.DataSource = dt.DefaultView;
            AreaName.DataValueField = "FactoryArea";
            AreaName.DataTextField = "FactoryArea";
            AreaName.DataBind();
            //AreaName.SelectedIndex = 0;
            AreaName.Items.Insert(0, new ListItem("------Please Select------", ""));
            AreaName.SelectedIndex = 0;
        }
    }

    

         //設定表單下拉選項
    protected void setTableList()
    {
        System.Data.DataTable dt = new DataTable();
        try
        {
            DBAccessProc.setConnStr(strCon);
            dt = DBAccessProc.GetDataTable("Enotes_PK0338_GetTableList", 0, 1, "", sysid, "", out msg, out effectRow);
        }
        catch (Exception ex)
        {
            Response.Write("Enotes_PK0338_GetTableList function error,Detail:" + ex.Message);
        }

        if ("" != msg)
        {
            Response.Write("Enotes_PK0338_GetTableList Failure:" + msg);
            return;
        }
        if (dt != null)
        {
            TableName.DataSource = dt.DefaultView;
            TableName.DataValueField = "TableID";
            TableName.DataTextField = "TableName";
            TableName.DataBind();
            TableName.Items.Insert(0, new ListItem("------Please Select------", ""));
            TableName.SelectedIndex = 0;
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
                aa.IsSysmanager = "Y";
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
                aa.IsSysmanager = "Y";
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