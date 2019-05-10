using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using FOX.DAL;
using Excel = Microsoft.Office.Interop.Excel;
using System.Text;
using FOX.COMMON;

public partial class FormAssist : System.Web.UI.Page
{
    string strCon = "";
    string msg = "";
    int effectRow = 0;
    string UserID = "";

    string OwnerInfo = "";//簽核人欄位.
    string txtFormID = "";
    string sysid = "003";
    string tableid = "";
    string FormStatus = "";
    string FormApplier = "";
    string beginDate1 = "";
    string beginDate2 = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        strCon = ConfigurationManager.ConnectionStrings["BASEConnectionString"].ToString();
        GetInitValue();

        if (!IsPostBack)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("http://10.56.69.77/GA_CarManagement");
            }
            else
            {
                UserID = Session["UserID"].ToString().Trim();//获取当前登录用户
                //UserID = "P1266";
                DataTable dt = new DataTable();
                dt = getResult();
                GridView.DataSource = dt;
                GridView.DataKeyNames = new string[] { "FormNo" };
                GridView.DataBind();
            }

            DBAccessProc.setConnStr(strCon);

            BindData();

        }
    }



    //每行編號
    //Tip
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
            //tooltip的值
            DataRowView dv = (DataRowView)e.Row.DataItem;
            string S1 = dv["FormNo"].ToString().Trim();
            string S2 = dv["Register_Name"].ToString().Trim();
            string S3 = dv["ApplyDept"].ToString().Trim();
            string S4 = dv["ApplyType"].ToString().Trim();
            string S5 = dv["Pickup_SendAdd"].ToString().Trim();
            string S6 = dv["Pickup_BackAdd"].ToString().Trim();
            string S7 = dv["IsAgentStart"].ToString().Trim();
            string S8 = dv["IsDrawSalary"].ToString().Trim();
            string S9 = dv["IsReimburse"].ToString().Trim();
            string S10 = dv["Form_Status"].ToString().Trim();

            //如果要使鼠标移到每行的每个单元格都显示就把e.Row.Cells[0].Attributes.Add换成e.Row.Attributes.Add
            e.Row.Cells[1].Attributes.Add("onmouseover", "this.oldcolor=this.style.backgroundColor;this.style.backgroundColor='#C8F7FF';this.style.cursor='hand';");
            e.Row.Cells[1].Attributes.Add("onmousemove", "Show('" + S1 + "','" + S2 + "','" + S3 + "','" + S4 + "','" + S5 + "','" + S6 + "','" + S7 + "','" + S8 + "','" + S9 + "','" + S10 + "');");
            //e.Row.Cells[1].Attributes.Add("onmousemove", "Show('Test')");
            e.Row.Cells[1].Attributes.Add("onmouseout", "this.style.backgroundColor=this.oldcolor;Hide();");

        }
    }

    //翻頁時進行處理
    protected void GridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //GridView.PageIndex = e.NewPageIndex;//獲取當前分頁索引值
        //BindData();

        // 得到該控制項
        GridView theGrid = sender as GridView;
        int newPageIndex = 0;
        if (e.NewPageIndex == -3)
        {
            //點擊了Go按鈕
            TextBox txtNewPageIndex = null;

            //GridView較DataGrid提供了更多的API，獲取分頁塊可以使用BottomPagerRow 或者TopPagerRow，當然還增加了HeaderRow和FooterRow
            GridViewRow pagerRow = theGrid.BottomPagerRow;

            if (pagerRow != null)
            {
                //得到text控制項
                txtNewPageIndex = pagerRow.FindControl("txtNewPageIndex") as TextBox;
            }
            if (txtNewPageIndex != null)
            {
                //得到索引
                newPageIndex = int.Parse(txtNewPageIndex.Text) - 1;
            }
        }
        else
        {
            //點擊了其他的按鈕
            newPageIndex = e.NewPageIndex;
        }
        //防止新索引溢出
        newPageIndex = newPageIndex < 0 ? 0 : newPageIndex;
        newPageIndex = newPageIndex >= theGrid.PageCount ? theGrid.PageCount - 1 : newPageIndex;

        //得到新的值
        theGrid.PageIndex = newPageIndex;//獲取當前分頁索引值

        //重新綁定
        BindData();
    }

    //查询
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
            for (int i = 0; i < dt.Columns.Count - 1; i++)//將dt.Columns.Count改為5
            {
                switch (dt.Columns[i].ColumnName)
                {
                    case "FormID":
                        worksheet.Cells[1, i + 1] = "表單ID";
                        break;
                    case "MainFormNo":
                        worksheet.Cells[1, i + 1] = "表單單號";
                        break;
                    case "FormTypeName":
                        worksheet.Cells[1, i + 1] = "表單類別";
                        break;
                    case "Form_Status":
                        worksheet.Cells[1, i + 1] = "表單狀態";
                        break;
                    case "Register":
                        worksheet.Cells[1, i + 1] = "申請人工號";
                        break;
                    case "Register_Name":
                        worksheet.Cells[1, i + 1] = "申請人姓名";
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
            for (int r = 0; r < dt.Rows.Count; r++)//行
            {
                for (int i = 0; i < dt.Columns.Count - 1; i++)//列
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
    }

    //GridView綁定數據
    protected void BindData()
    {
        DataTable dt = new DataTable();

        //if (OwnerInfo != "") dt = getQueryResult();
        //else 
        dt = getResult();
        DataView view = dt.AsDataView();
        //设置GridView控件的数据源为创建的数据集ds
        this.GridView.DataSource = view;
        //将数据库表中的主键字段放入GridView控件的DataKeyNames属性中
        this.GridView.DataKeyNames = new string[] { "FormNo" };
        //表頭內容不換行
        GridView.Style.Add("word-break", "keep-all");
        GridView.Style.Add("word-wrap", "normal");
        //绑定数据库表中数据
        this.GridView.DataBind();
    }

    //獲取數據，用於綁定GridView
    protected DataTable getResult()
    {
        UserID = Session["UserID"].ToString().Trim();
        //UserID = "P1266";
        //string FormType = "";
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbrep = new StringBuilder();
        StringBuilder sbval = new StringBuilder();

        if (beginDate1 != "" && beginDate2 != "")
        {
            if (Convert.ToDateTime(beginDate1) > Convert.ToDateTime(beginDate2))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "window.alert('起始日期不能大於終止日期');", true);
            }
        }
        beginDate1 = Convert.ToDateTime(beginDate1).ToString("yyyy-MM-dd").Trim();
        beginDate2 = Convert.ToDateTime(beginDate2).ToString("yyyy-MM-dd").Trim();

        sbval.Append(UserID).Append("{?,?}");

        sbval.ToString();

        try
        {
            //EPS_PK2764_GetFormInFlow
            dt = DBAccessProc.GetDataTable("HR_BusinessTrip_P1266_Assist", 0, 1, sbrep.ToString(), sbval.ToString(), "{?,?}", out msg, out effectRow);

            if ("" != msg)
            {
                Response.Write("HR_BusinessTrip_P1266_Assist Failure:" + msg);
                return null;
            }
        }
        catch (Exception ex)
        {
            Response.Write("HR_BusinessTrip_P1266_Assist function error,Detail:" + ex.Message);
        }
        return dt;
    }

    //獲取查詢數據
    //protected DataTable getQueryResult()
    //{

    //    System.Data.DataTable dt = new DataTable();
    //    StringBuilder sbrep = new StringBuilder();
    //    StringBuilder sbval = new StringBuilder();


    //    if (beginDate1 != "" && beginDate2 != "")
    //    {
    //        if (Convert.ToDateTime(beginDate1) > Convert.ToDateTime(beginDate2))
    //        {
    //            Page.ClientScript.RegisterStartupScript(this.GetType(), "", "window.alert('起始日期不能大於終止日期');", true);
    //        }
    //    }
    //    if (txtFormID != "") sbrep.Append(" and FormNo=@P1, ");
    //    else sbrep.Append(",");
    //    if (FormTypeName != "") sbrep.Append(" and  FormTypeName=@P2, ");
    //    else sbrep.Append(",");
    //    if (OwnerInfo != "") sbrep.Append(" and  OwnerInfo like @P3, ");
    //    else sbrep.Append(",");
    //    if (FormApplier != "") sbrep.Append(" and  Register_Name like @P4, ");
    //    else sbrep.Append(",");
    //    if (beginDate1 != "") sbrep.Append(" and  FormDate>=@P5, ");
    //    else sbrep.Append(",");
    //    if (beginDate2 != "") sbrep.Append(" and  FormDate<=@P6, ");
    //    else sbrep.Append(",");

    //    sbval.Append(txtFormID).Append(",")
    //        .Append(FormTypeName).Append(",")
    //        .Append("%").Append(OwnerInfo).Append("%").Append(",")
    //        .Append("%").Append(FormApplier).Append("%").Append(",")
    //        .Append(beginDate1).Append(",")
    //        .Append(beginDate2).Append(",");

    //    sbrep.ToString();
    //    sbval.ToString();

    //    try
    //    {
    //        dt = DBAccessProc.GetDataTable("EPS_PK2764_GetFormInFlowByTheOwner", 0, 1, sbrep.ToString(), sbval.ToString(), ",", out msg, out effectRow);

    //        if ("" != msg)
    //        {
    //            Response.Write("BindData Failure:" + msg);
    //            return null;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Response.Write("BindData function error,Detail:" + ex.Message);
    //    }
    //    return dt;
    //}

    /*
    //表單類別
    protected void setDDLFormType()
    {
        System.Data.DataTable dt = new DataTable();
        try
        {
            dt = DBAccessProc.GetDataTable("ENotes_PK0338_GetSysName", 0, 1, "", "", "", out msg, out effectRow);
        }
        catch (Exception ex)
        {
            Response.Write("ENotes_PK0338_GetSysName function error,Detail:" + ex.Message);
        }

        if ("" != msg)
        {
            Response.Write("ENotes_PK0338_GetSysName Failure:" + msg);
            return;
        }
        ddl_type.DataSource = dt.DefaultView;
        //ddl_type.DataValueField = "FormType";
        ddl_type.DataValueField = "FormType";
        ddl_type.DataTextField = "TableName";
        ddl_type.DataBind();
        ddl_type.SelectedIndex = 0;
        ddl_type.Items.Insert(0, new ListItem("------請選擇------", ""));
        ddl_type.SelectedIndex = 0;
    }
    */

    //取基本數據值
    protected void GetInitValue()
    {
        //OwnerInfo = this.txt_TheOwnerInFlow.Text.ToString().Trim();//根據‘簽核人’進行查詢時，有區別于其他欄位 
        //txtFormID = this.txt_Flowformid.Text.ToString().Trim(); //表單資訊
        //FormTypeName = this.ddl_type.SelectedValue.ToString().Trim();
        FormApplier = this.txt_Flowapplier.Text.ToString().Trim();
        beginDate1 = this.txt_applyDate.Text.ToString().Trim();
        beginDate2 = this.txt_applyDate2.Text.ToString().Trim();
        if (beginDate1 == "")   //為避免資料量過多時,頁面負載過重,default顯示三個月以內的文件
        {
            beginDate1 = Convert.ToDateTime(DateTime.Today.AddMonths(-3)).ToString("yyyy-MM-dd").Trim();
            beginDate2 = Convert.ToDateTime(DateTime.Today).ToString("yyyy-MM-dd").Trim();
        }
    }


}