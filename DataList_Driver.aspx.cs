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

    string drivername = "";
    string driverphoneno = "";
    string factoryarea = "";
    string carno = "";
    string remark = "";
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

        string txtcarno = "%"+txt_CarNo.Text.ToString().Trim()+"%";
        string txtdrivername = "%" + txt_DriverName.Text.ToString().Trim() + "%";
        string txt_driverphoneno = "%" + txt_DriverPhoneNo.Text.ToString().Trim() + "%";
        string txt_factoryarea = txt_FactoryArea.SelectedValue.ToString();


        if (txtcarno != "%%") sbrep.Append(" and CarNo like @P1{?,?} ");
        else sbrep.Append("{?,?}");
        if (txtdrivername != "%%") sbrep.Append(" and DriverName like @P2{?,?} ");
        else sbrep.Append("{?,?}");
        if (txt_driverphoneno != "%%") sbrep.Append(" and DriverPhoneNo like @P3{?,?} ");
        else sbrep.Append("{?,?}");
        if (txt_factoryarea != "0") sbrep.Append(" and FactoryArea=@P4{?,?} ");
        else sbrep.Append("{?,?}");

        sbval.Append(txtcarno).Append("{?,?}")
            .Append(txtdrivername).Append("{?,?}")
            .Append(txt_driverphoneno).Append("{?,?}")
            .Append(txt_factoryarea).Append("{?,?}");


        sbrep.ToString();
        sbval.ToString();


        try
        {
            //將查詢數據以DataTable形式取出
            dt = DBAccessProc.GetDataTable("GA_CarManagerment_PK0338_GetDriverInfo", 0, 1, sbrep.ToString(), sbval.ToString(), "{?,?}", out msg, out effectedRow);

        }
        catch (Exception ex)
        {
            Response.Write("GA_CarManagerment_PK0338_GetDriverInfo error,Detail:" + ex.Message);
            sbrep.Clear();
            sbval.Clear();
        }

        if ("" != msg)
        {
            Response.Write("GA_CarManagerment_PK0338_GetDriverInfo Failure:" + msg);
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

        factoryarea = txt_FactoryArea.SelectedValue.ToString();
        if (factoryarea == "")
        {

            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請選擇廠區!');", true);
            return;
        }

        carno = txt_CarNo.Text.ToString();
        if (carno == "")
        {

            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請填寫車牌號!');", true);
            return;
        }

        drivername = txt_DriverName.Text.ToString().Trim();
        if (drivername == "")
        {

            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請填寫司機姓名!');", true);
            return;
        }

        driverphoneno = txt_DriverPhoneNo.Text.ToString().Trim();
        if (driverphoneno == "")
        {

            ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請填寫司機電話號碼!');", true);
            return;
        }

        remark = txt_Remark.Text.ToString();


        try
        {
            UserRoleExistYN = GetExistYN(carno);

         if (UserRoleExistYN > 0)
         {

             ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('該用戶對應的這個權限已存在!');", true);
             return;
         }

                    DBAccessProc.setConnStr(strCon);
                    sbval.Append(userid).Append(",")
                    .Append(carno).Append(",")
                    .Append(drivername).Append(",")
                    .Append(driverphoneno).Append(",")
                    .Append(factoryarea).Append(",")
                    .Append(remark).Append(",");

                    DBAccessProc.ExecuteSQL("GA_CarManagerment_PK0338_AddDriver", "", sbval.ToString(), ",", out msg, out rowcount, out effectedRow);
                    this.GridView.DataSource = dt.DefaultView;
                    this.BindData();

                    if (msg == "")
                    {
                        ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('新增車輛信息成功!');", true);
                    }
                    
                    txt_CarNo.Text = "";
                    txt_DriverName.Text = "";
                    txt_DriverPhoneNo.Text = "";
                    txt_Remark.Text = "";
                    txt_FactoryArea.SelectedValue = "0";
                }
                catch (Exception ex)
                {
                    Response.Write("GA_CarManagerment_PK0338_AddDriver function error,Detail:" + ex.Message);
                    sbval.Clear();
                }
                

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

    public int GetExistYN(string carno)
    {
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbval = new StringBuilder();
        int countNo = 0;//聲明一個變量接受查詢數據
        try
        {
            //設定連接字串
            DBAccessProc.setConnStr(strCon);
            sbval.Append(carno).Append(",");

            //將查詢數據以DataTable形式取出
            dt = DBAccessProc.GetDataTable("GA_CarManagerment_PK0338_CheckDriver", 0, 1, "", sbval.ToString(), ",", out msg, out effectedRow);
            if (dt != null)
            {
                countNo = int.Parse(dt.Rows[0]["num"].ToString());
            }
        }
        catch (Exception ex)
        {
            Response.Write("GA_CarManagerment_PK0338_CheckDriver function error,Detail:" + ex.Message);
            sbval.Clear();
        }

        if ("" != msg)
        {
            Response.Write("GA_CarManagerment_PK0338_CheckDriver Failure:" + msg);

        }

        sbval.Clear();

        return countNo;

    }


    //GridView資料編輯
    protected void GridView_RowEditing(object sender, GridViewEditEventArgs e)
    {

        GridView.EditIndex = e.NewEditIndex;
        BindData();
    }

    /// <summary>

    protected void GridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        this.GridView.EditIndex = -1;//取消編輯狀態
        BindData();//綁定數據

    }


    //GridView資料更新
    protected void GridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

        //if (Session["car_IsSysmanager"].ToString().Trim() == "N")
        if (aa.IsSysmanager == "N") //20170515 Monica因Session["car_IsSysmanager"].ToString().Trim()會發生錯誤 故改由aa.IsSysmanager 
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", "window.alert('您無權編輯資料!');", true);
        }
        else
        {
            System.Data.DataTable dt = new DataTable();
            StringBuilder sbval = new StringBuilder();
            try
            {
                row_id = GridView.DataKeys[e.RowIndex].Value.ToString();
                string factoryarea = ((TextBox)GridView.Rows[e.RowIndex].FindControl("E_FactoryArea")).Text.Trim();
                string carno = ((TextBox)GridView.Rows[e.RowIndex].FindControl("E_CarNo")).Text;
                string drivername = ((TextBox)GridView.Rows[e.RowIndex].FindControl("E_DriverName")).Text;
                string driverphoneno = ((TextBox)GridView.Rows[e.RowIndex].FindControl("E_DriverPhoneNo")).Text;
                string remark = ((TextBox)GridView.Rows[e.RowIndex].FindControl("E_Remark")).Text;

                sbval.Append(row_id).Append(",");
                sbval.Append(factoryarea).Append(",");
                sbval.Append(carno).Append(",");
                sbval.Append(drivername).Append(",");
                sbval.Append(driverphoneno).Append(",");
                sbval.Append(remark).Append(",");
                sbval.Append(Session["UserID"].ToString().Trim()).Append(",");

                DBAccessProc.ExecSQL("GA_CarManagerment_PK0338_DriverGridviewUpdatedata", "", sbval.ToString(), ",", out msg, out rowcount, out effectRow);
                sbval.Clear();
                if ("" != msg)
                {
                    Response.Write("GA_CarManagerment_PK0338_DriverGridviewUpdatedata Failure:" + msg);
                }

            }
            catch (Exception ex)
            {
                Response.Write("GA_CarManagerment_PK0338_DriverGridviewUpdatedata function error,Detail:" + ex.Message);
            }
            GridView.EditIndex = -1;
        }
        BindData();

    }


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


                sbval.ToString();
                DBAccessProc.ExecuteSQL("GA_CarManagerment_PK0338_DeleteDriver", "", sbval.ToString(), ",", out msg, out rowcount, out effectedRow);
                sbval.Clear();

                if (msg != "")
                {
                    Response.Write("GA_CarManagerment_PK0338_DeleteDriver Error:" + msg);
                }
            }
            catch (Exception ex)
            {
                Response.Write("GA_CarManagerment_PK0338_DeleteDriver function error:" + ex.Message);
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