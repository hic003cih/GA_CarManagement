using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
//以下是自己加的
using System.Data;
using FOX.DAL;
using System.Configuration;
using Excel = Microsoft.Office.Interop.Excel;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using FOX.COMMON;
//使用NPOI匯出EXCEL
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public partial class V_FormReportSearch : System.Web.UI.Page
{
    string UserID = "";
    //搜尋系統用
    string msg = "";
    int effectRow = 0;
    string beginDate1 = "";
    string beginDate2 = "";
    //匯出excel用
    int[] array = new int[] { };

    protected void Page_Load(object sender, EventArgs e)
    {   
        //頁面是第一次加載時，所要執行的事件。
        if (!IsPostBack) {
        //分辨是否有登入
            //UserID = "P0090";
            //Session.Add("UserID", "P0090"); 
        if (Session["UserID"] == null)
        {
           
            Response.Redirect("http://10.56.69.77/FitiGroup/");
        }
        else
        {
            UserID = Session["UserID"].ToString().Trim().ToUpper();
        }
            //將資料庫內的資料塞入gridview
            DataTable dt= new DataTable();
            dt=getResult();
            if(dt!=null){
            GRV1.DataSource=dt;
            GRV1.DataBind();
            }
            BindData();
        }
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
            this.GRV1.DataSource = view;
            //将数据库表中的主键字段放入GridView控件的DataKeyNames属性中
            this.GRV1.DataKeyNames = new string[] { "FormNo" };
            //表頭內容不換行
            //GRV1.Style.Add("word-break", "keep-all");
            //GRV1.Style.Add("word-wrap", "normal");
            ////====================Fromat===============
            //GRV1.Font.Size = 11;
            //GRV1.GridLines = GridLines.Both;	//設定格線
            //GRV1.PageSize = 15;    //10行
            //GRV1.PagerSettings.Position = PagerPosition.Bottom; //分頁位置
            //GRV1.Font.Name = "微軟正黑體";		//設定字型大小
            //GRV1.AlternatingRowStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#e6e6e6");
            //GRV1.PagerStyle.HorizontalAlign = HorizontalAlign.Center; //分頁對齊
            //GRV1.AllowPaging = true;
            //GRV1.PagerStyle.Font.Size = 12;
            //====================Fromat===============
            //绑定数据库表中数据
            
            this.GRV1.DataBind();
        }
    }
    //獲取數據，用於綁定GridView
    protected DataTable getResult() {
        
        System.Data.DataTable dt = new DataTable();
        StringBuilder sbrep = new StringBuilder();
        StringBuilder sbval = new StringBuilder();
        //查詢用的東西
        beginDate1 = txt_Departuretime.Text.ToString().Trim();
        beginDate2 = txt_Departuretime2.Text.ToString().Trim();


        if (beginDate1 != "") sbrep.Append(" and Departuretime>=@P1{?,?} ");
        else sbrep.Append("{?,?}");
        if (beginDate2 != "") sbrep.Append(" and Departuretime<=@P2{?,?} ");
        else sbrep.Append("{?,?}");

        sbval.Append(beginDate1).Append("{?,?}")
            .Append(beginDate2).Append("{?,?}");

        sbrep.ToString();
        sbval.ToString();

        try
        {
            //從BASEConnectionString資料庫抓取GA_CarManagerment_P1434_GetDataAll的SQL指令來抓取需要的資料
            dt = DBAccessProc.GetDataTable("GA_CarManagerment_P1434_GetDataAll", 0, 1, sbrep.ToString(), sbval.ToString(), "{?,?}", out msg, out effectRow);

            if ("" != msg)
            {
                Response.Write("GA_CarManagerment_P1434_GetDataAll Failure:" + msg);
                return null;
            }
        }
        catch (Exception ex)
        {
            Response.Write("GA_CarManagerment_P1434_GetDataAll error,Detail:" + ex.Message);
        }

        sbrep.Clear();
        sbval.Clear();
        return dt;
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
    //每行編號
    //Tip
    protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataRowView dv = (DataRowView)e.Row.DataItem;
            //string S1 = dv["ApplyMan"].ToString().Trim();  //
            //e.Row.Cells[1].Text = S1;
            //e.Row.Cells[3].Width = 150;
            //e.Row.Cells[6].Width = 120;
            //e.Row.Cells[7].Width = 100;
            if (e.Row.RowIndex != -1)
            {
                int id = e.Row.RowIndex + 1;
                e.Row.Cells[0].Text = id.ToString();

            }
            

        }
    }
    //按下查詢按鈕
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        string a1 = txt_Departuretime.Text.ToString();
        string a2 = txt_Departuretime2.Text.ToString();
       
            if (a1 == "" && a2 == "")
            {
                ScriptManager.RegisterStartupScript((System.Web.UI.Page)HttpContext.Current.CurrentHandler, typeof(System.Web.UI.Page), "ShowWindows", "window.alert('請選擇日期範圍!');", true);
                return;
            }
                DataTable dt = new DataTable();
                dt = getResult();
                if (dt != null)
                {
                    GRV1.DataSource = dt;
                    GRV1.DataKeyNames = new string[] { "FormNo" };
                    GRV1.DataBind();
                }
    }
    //使用NPOI匯出EXCEL
    //按下輸出EXCEL按鈕
    protected void Button1_Click(object sender, EventArgs e)
    {

        //Response.Clear();
        //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //XSSFWorkbook worbook = new XSSFWorkbook();
        //ISheet u_sheet = worbook.CreateSheet("NPOI20_工作表_Sheet1");

        ////*** 每一列的「第一格」才能用這種寫法！！ ******************************************
        //u_sheet.CreateRow(0).CreateCell(0).SetCellValue("中文測試。This is a Sample。中文測試");

        ////*** 每一列的第二格「起」，必須使用下面寫法才行！！********************************
        //int x = 1;
        //for (int i = 1; i <= 15; i++)
        //{
        //    IRow u_row = u_sheet.CreateRow(i);    // 在工作表裡面，產生一列。
        //    for (int j = 0; j < 15; j++)
        //    {
        //        u_row.CreateCell(j).SetCellValue(x++);     // 在這一列裡面，產生格子（儲存格）並寫入資料。
        //    }
        //}
       
        //MemoryStream MS = new MemoryStream();
        //Response.AddHeader("Content-Disposition", "attachment;filename=test_1.xlsx");
        //Response.BinaryWrite(MS.ToArray());

        //worbook = null;
        //MS.Close();
        //MS.Dispose();

        //Response.Flush();
        //Response.End();

        //將資料庫內的資料取出並放在dt中
        DataTable dt = new DataTable();
        dt = getResult();
        //下面這兩行程式碼很重要
        Response.Clear();
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //產生新版Excel 2007的.xlsx檔
        XSSFWorkbook workbook = new XSSFWorkbook();

        // 新增試算表。 
        //workbook.CreateSheet("試算表 A");
        //workbook.CreateSheet("試算表 B");
        //workbook.CreateSheet("試算表 C");

        //== 產生工作表
        ISheet u_sheet = workbook.CreateSheet("Sheet1");
        IRow headerrow = u_sheet.CreateRow(0);
        //設定Excel樣式
        headerrow.Height = 30 * 20;
        ICellStyle style = workbook.CreateCellStyle();
        style.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
        style.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
        style.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        style.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
        style.WrapText = true;
        //文字置中
        style.Alignment = HorizontalAlignment.Center;
        style.VerticalAlignment = VerticalAlignment.Center;

        ///*标题*/
        //for (int i = 0; i < dt.Columns.Count; i++)
        //{
        //    ICell cell = headerrow.CreateCell(i);
        //    cell.CellStyle = style;
        //    cell.SetCellValue(dt.Columns[i].ColumnName);
        //    /*设置列宽*/
        //    if (array.Length > 0)
        //    {
        //        for (int c = 0; c < array.Length; c++)
        //        {
        //            sheet.SetColumnWidth(c, array[c] * 256);
        //        }
        //    }
        //}

        ///*内容*/
        //for (int j = 1; j <= dt.Rows.Count; j++)
        //{
        //    NPOI.SS.UserModel.IRow contentrow = sheet.CreateRow(j);
        //    contentrow.Height = 100 * 20;
        //    for (int k = 0; k < dt.Columns.Count; k++)
        //    {
        //        ICell cell = contentrow.CreateCell(k);
        //        cell.CellStyle = style;
        //        if (k == 0)
        //        {
        //            string picurl = dt.Rows[j - 1][k].ToString();
        //            AddCellPicture(sheet, book, picurl, j, k);
        //        }
        //        else
        //        {
        //            cell.CellStyle = style;
        //            cell.SetCellValue(dt.Rows[j - 1][k].ToString());
        //        }
        //    }
        //}
        //ICell x = headerrow.CreateCell(0);
        //x.CellStyle = style;
        //x.SetCellValue("行號");
        
        /*标题*/
        //取gridview欄位的HeaderText的值放到excel中
        //如果不要抓行號到excel,設i=1
            for ( int i = 0; i < GRV1.Columns.Count; i++)
            {
                ICell cell = headerrow.CreateCell(i);
                cell.CellStyle = style;
                cell.SetCellValue(GRV1.Columns[i].HeaderText);

                ///*设置列宽*/
                //if (array.Length > 0)
                //{
                //    for (int c = 0; c < array.Length; c++)
                //    {
                //        u_sheet.SetColumnWidth(c, array[c] * 256);
                //    }
                //}
            }
            int z = 1;
        /*内容*/
        //設定行(直的)的資料
        for (int j = 1; j <= dt.Rows.Count; j++)
        {   
            //創造j行
            NPOI.SS.UserModel.IRow contentrow = u_sheet.CreateRow(j);
            //contentrow.Height = 100 * 20;
            //自動欄位大小
            u_sheet.AutoSizeColumn(j);
            //設定第一行的行號
            contentrow.CreateCell(0).SetCellValue(z++);
            
            //設定欄位(橫的)的資料
            for (int k = 1; k < dt.Columns.Count; k++)
            {   
                //在第j行中建造單元格
                ICell cell = contentrow.CreateCell(k);
                cell.CellStyle = style;
                //循環往第j行的單元格中添加數據
                cell.SetCellValue(dt.Rows[j - 1][k].ToString());
            }
        }
        

        //== 輸出Excel 2007檔案。==============================
        MemoryStream ms = new MemoryStream();
        workbook.Write(ms);
        //== Excel檔名，請寫在最後面 filename的地方
        Response.AddHeader("Content-Disposition", string.Format("attachment; filename=Workbook.xlsx"));
        Response.BinaryWrite(ms.ToArray());
        //== 釋放資源
        workbook = null;
        ms.Close();
        ms.Dispose();
        //== 不寫這兩段程式，輸出Excel檔並開啟以後，會出現檔案內容混損，需要修復的字眼。
        Response.Flush();
        Response.End();
    }
    
}