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
//連接database,需添加以下三行
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Configuration;
using FOX.DAL;
using System.Text;
using System.Web.Services;
using System.IO;
using FOX.COMMON;
using System.Net;
public partial class FormInFlow : System.Web.UI.Page
{
    string strCon = "";
    string msg = "";
    int effectRow = 0;
    int rowcount = 0;
    string UserID = "";
    string formType = "";
    string sysid = "003";
    string tableName = "";
    public class aa
    {
        public static string IsSysmanager = "N";
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        strCon = ConfigurationManager.ConnectionStrings["BASEConnectionString"].ToString();
        if (!IsPostBack)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("http://10.56.69.77/FitiGroup/");
            }
            else
            {
                UserID = Session["UserID"].ToString().Trim();//获取当前登录用户
                //aa.IsSysmanager = checkmanager(sysid, UserID);
               checksysman(UserID);
                if (aa.IsSysmanager == "N")
                {
                    Panel_Content.Visible = false;
                }
                else
                {
                    Panel_Content.Visible = true;
                    BindData();
                }
            }
            SqlDataAdapter ta = new SqlDataAdapter("select TableName from BASE.dbo.System_Index where SysID='003'", strCon);
            DataSet ts = new DataSet();
            ta.Fill(ts, "BASE.dbo.System_Index");     //執行SQL指令，取出資料
            txt_formType.DataValueField = "TableName";        //在此輸入的是資料表的欄位名稱
            txt_formType.DataTextField = "TableName";        //在此輸入的是資料表的欄位名稱
            txt_formType.DataSource = ts.Tables["Base.dbo.System_Index"].DefaultView;
            txt_formType.DataBind();
            txt_formType.Items.Insert(0, new ListItem("---Please Select---")); //下拉選單以---Please Select---為預設值
        }
    }

    //Tip+DropDownList綁定多個字段
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
            //1.Tip
            DataRowView dv = (DataRowView)e.Row.DataItem;
            e.Row.Cells[1].Attributes.Add("onmouseover", "this.oldcolor=this.style.backgroundColor;this.style.backgroundColor='#C8F7FF';this.style.cursor='hand';");
            e.Row.Cells[1].Attributes.Add("onmouseout", "this.style.backgroundColor=this.oldcolor;");
        }
    }

    //表單作廢
    protected void GridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string formID = (string)e.CommandArgument;//取得ID值
        //＊＊＊＊＊＊＊＊DataSet＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
        //＊＊＊＊＊＊＊＊DataSet＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
        if (e.CommandName == "updateDel")
        {
            System.Data.DataTable dt = new DataTable();
            StringBuilder sbval = new StringBuilder();
            StringBuilder sbrep = new StringBuilder();
            

            try
            {
                DBAccessProc.setConnStr(strCon);
                sbval.Append(formID).Append(",")
                   .Append(Session["UserID"].ToString().Trim()).Append(",");
                sbrep.Append(tableName).Append(",");
                DBAccessProc.ExecuteSQL("ENotes_PK0338_Delete", sbrep.ToString(), sbval.ToString(), ",", out msg, out rowcount, out effectRow);
                BindData();
            }
            catch (Exception ex)
            {
                Response.Write("ENotes_PK0338_Delete Form function error,Detail:" + ex.Message);
                sbval.Clear();
            }

            if ("" != msg)
            {
                Response.Write("ENotes_PK0338_Delete Form Failure:" + msg);
                return;
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
        DataView view = dt.AsDataView();

        //设置GridView控件的数据源为创建的数据集ds
        GridView.DataSource = view;
        //将数据库表中的主键字段放入GridView控件的DataKeyNames属性中
        GridView.DataKeyNames = new string[] { "FormNo" };
        //表頭內容不換行
        GridView.Style.Add("word-break", "keep-all");
        GridView.Style.Add("word-wrap", "normal");
        //绑定数据库表中数据
        GridView.DataBind();
    }

    //獲取數據，用於綁定GridView
    protected DataTable getResult()
    {
        UserID = Session["UserID"].ToString().Trim();//获取当前登录用户
        //UserID = "P1266";
        //select選擇框，選擇后跳轉頁面
        DataTable dt = new DataTable();
        StringBuilder sbrep = new StringBuilder();
        StringBuilder sbval = new StringBuilder();
        string txtFormID = this.txt_NoSendformId.Text.ToString().Trim();
        string beginDate1 = this.txt_applyDate.Text.ToString().Trim();
        string beginDate2 = this.txt_applyDate2.Text.ToString().Trim();
        string formType = this.txt_formType.Text.ToString().Trim();
        string applyMan = this.txt_applyMan.Text.ToString().Trim();
        if (beginDate1 != "" && beginDate2 != "")
        {
            if (Convert.ToDateTime(beginDate1) > Convert.ToDateTime(beginDate2))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "window.alert('起始日期不能大於終止日期');", true);
            }
        }
        if (formType == "---Please Select---") {
            formType = "";
        }
        if (txtFormID != "") sbrep.Append(" and  a.FormNo=@P2, ");
        else sbrep.Append(",");
        if (beginDate1 != "") sbrep.Append(" and  a.ApplyDate>=@P3, ");
        else sbrep.Append(",");
        if (beginDate2 != "") sbrep.Append(" and  a.ApplyDate<=@P4, ");
        else sbrep.Append(",");
        if (formType != "") sbrep.Append(" and  a.FormTypeName=@P5, ");
        else sbrep.Append(",");
        if (applyMan != "") sbrep.Append(" and  a.Register_Name like '%'+@P6+'%', ");
        else sbrep.Append(",");

        sbval.Append(UserID).Append(",")
            .Append(txtFormID).Append(",")
            .Append(beginDate1).Append(",")
            .Append(beginDate2).Append(",")
            .Append(formType).Append(",")
            .Append(applyMan).Append(",");
        sbrep.ToString();
        sbval.ToString();
        try
        {
            DBAccessProc.setConnStr(strCon);
            dt = DBAccessProc.GetDataTable("HR_BusinessTrip_P1266_ApplylistForManager", 0, 1, sbrep.ToString(), sbval.ToString(), ",", out msg, out effectRow);
            if ("" != msg)
            {
                Response.Write("HR_BusinessTrip_P1266_ApplylistForManager Failure:" + msg);
                return null;
            }
        }
        catch (Exception ex)
        {
            Response.Write("HR_BusinessTrip_P1266_ApplylistForManager function error,Detail:" + ex.Message);
        }
        return dt;
    }

    //查詢
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        dt = getResult();
        GridView.DataSource = dt;
        GridView.DataKeyNames = new string[] { "FormNo" };
        GridView.DataBind();
    }

    //匯出
    protected void btnExport_Click(object sender, EventArgs e)
    {
        /*
        DataTable dt = new DataTable();
        dt = getResult();
        try
        {

            DateTime datetime = DateTime.Now;
            string dts = string.Format("{0:yyyyMMddHHmmssffff}", datetime);
            string destination = Server.MapPath("~/Files/Temp/" + dts);

            //目標文件夾不存在就新增
            if (!DirFile.IsExistDirectory(destination))
            {
                DirFile.CreateDirectory(destination);
            }

            string filepath = HttpContext.Current.Server.MapPath("~/Files/Temp/" + dts + "/FormList.xls");

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbooks w = xlApp.Workbooks;
            Excel.Workbook workbook = w.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Worksheets[1];
            //写入字段 
            for (int i = 0; i < dt.Columns.Count - 13; i++)//將dt.Columns.Count改為5
            {
                switch (dt.Columns[i].ColumnName)
                {
                    case "FormID":
                        worksheet.Cells[1, i + 1] = "表單ID";
                        break;
                    case "FormNo":
                        worksheet.Cells[1, i + 1] = "表單單號";
                        break;
                    case "FormTypeName":
                        worksheet.Cells[1, i + 1] = "表單類型名稱";
                        break;
                    case "Form_Status":
                        worksheet.Cells[1, i + 1] = "表單狀態";
                        break;
                    case "Register_Name":
                        worksheet.Cells[1, i + 1] = "申請者";
                        break;
                    case "FormDate":
                        worksheet.Cells[1, i + 1] = "申請日期";
                        break;
                    default:
                        worksheet.Cells[1, i + 1] = dt.Columns[i].ColumnName;
                        break;
                }
            }
            //写入数值 
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                for (int i = 0; i < dt.Columns.Count - 13; i++)
                {
                    worksheet.Cells[r + 2, i + 1] = dt.Rows[r][i];
                }
            }
            worksheet.Columns.EntireColumn.AutoFit();//列宽自适应。
            workbook.Saved = true;
            workbook.SaveCopyAs(filepath);
            xlApp.Quit();
            GC.Collect();//强行销毁 
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(filepath));
            HttpContext.Current.Response.WriteFile(filepath);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        catch (Exception ex)
        {
            Response.Write("Transfer to excel error,Detail:" + ex.Message);
        }
        */
    }

    public string checkmanager(string sysid, string userid)
    {
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbrep = new StringBuilder();
        StringBuilder sbval = new StringBuilder();
        int countNo = 0;//聲明一個變量接受查詢數據
        try
        {
            DBAccessProc.setConnStr(strCon);//設定連接字串
            sbval.Append(sysid).Append(",")
                .Append(userid).Append(","); //定義變量用於獲取變更的參數

            //將查詢數據以DataTable形式取出
            dt = DBAccessProc.GetDataTable("CarApply_PK0338_IsManagerExist", 0, 1, "", sbval.ToString(), ",", out msg, out effectRow);
            countNo = int.Parse(dt.Rows[0]["num"].ToString());
        }
        catch (Exception ex)
        {
            Response.Write("CarApply_PK0338_IsManagerExist function error,Detail:" + ex.Message);
            sbrep.Clear();
            sbval.Clear();
        }

        if ("" != msg)
        {
            Response.Write("CarApply_PK0338_IsManagerExist Failure:" + msg);

        }

        sbrep.Clear();
        sbval.Clear();

        if (countNo > 0)
        {
            return "Y";
        }
        else
        {
            return "N";
        }
    }
    //判斷當前人員是否是當前系統管理員
    public void checksysman(string userid)
    {
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbrep = new StringBuilder();
        StringBuilder sbval = new StringBuilder();
        int countNo = 0;//聲明一個變量接受查詢數據
        try
        {
            DBAccessProc.setConnStr(strCon);//設定連接字串
            sbval.Append(userid).Append(","); //定義變量用於獲取變更的參數

            //將查詢數據以DataTable形式取出
            dt = DBAccessProc.GetDataTable("HR_BusinessTrip_P1266_CheckSysman", 0, 1, "", sbval.ToString(), ",", out msg, out effectRow);
            if (dt != null)
            {
                countNo = int.Parse(dt.Rows[0]["num"].ToString());
            }
        }
        catch (Exception ex)
        {
            Response.Write("HR_BusinessTrip_P1266_CheckSysman function error,Detail:" + ex.Message);
            sbrep.Clear();
            sbval.Clear();
        }

        if ("" != msg)
        {
            Response.Write("HR_BusinessTrip_P1266_CheckSysman Failure:" + msg);

        }

        sbrep.Clear();
        sbval.Clear();

        if (countNo > 0)
        {
            aa.IsSysmanager = "Y";
        }
    }    
}