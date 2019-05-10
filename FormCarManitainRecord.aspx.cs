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

public partial class FormCarManitainRecord : System.Web.UI.Page
{
    string UserID = "";
    int effectedRow = 0;
    string msg = "";
    //＊＊＊＊＊＊＊＊DataSet＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    CarInfoDataContext CarInfo = new CarInfoDataContext();
    BaseDataContext UserInfo = new BaseDataContext();
    protected void Page_Load(object sender, EventArgs e)
    {   
        //分辨是否有登入
        if (Session["UserID"] == null)
        {
            Response.Redirect("http://10.56.69.77/FitiGroup/");
        }
        else
        {
            UserID = Session["UserID"].ToString().Trim().ToUpper();
        }
        //頁面是第一次加載時，所要執行的事件。
        if (!IsPostBack)
        {
            //從BRM_USER_INFO抓取申請者資料
            var from_var = from user in UserInfo.BRM_USER_INFO where UserID == user.UserID select new { user.UserID, user.Name, user.Dept, user.Phone, user.WorkStation };
            //將申請者取出並顯示
            if (from_var.FirstOrDefault() != null)
            {
                foreach (var doc in from_var) {
                    ApplyManNo.Text = doc.UserID;
                    ApplyMan.Text = doc.Name;
                    ApplyDept.Text = doc.Dept;
                    ApplyDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                    ApplyPhoneNo.Text = doc.Phone;
                    ApplyArea.Text = doc.WorkStation;
                }
            }
            //表單內容
            //從List_Dirver抓取車牌號碼
            var CarNo_var = (from car in CarInfo.List_Dirver where car.DeleteFlag==null select new { car.CarNo }).Distinct();
            CarNo.DataSource = CarNo_var;
            CarNo.DataTextField = "CarNo";
            CarNo.DataBind();
            //GridView綁定數據
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
        }
        //GridView綁定數據
        public void BindData()
        {
            //頁面載入時呈現的GridView
            if (CarNo.SelectedItem.Text == "" || CarNo.SelectedItem.Text == "請選擇")
            {
                System.Data.DataTable dt = new DataTable();

                dt = getResult();
                if (dt != null)
                {
                    DataView view = dt.AsDataView();
                    //將GridView的編輯和刪除欄隱藏
                    GRV1.Columns[5].Visible = false;
                    GRV1.DataSource = view;
                    ////将数据库表中的主键字段放入GridView控件的DataKeyNames属性中
                    GRV1.DataKeyNames = new string[] { "ID" };
                    //表頭內容不換行
                    GRV1.Style.Add("word-break", "keep-all");
                    GRV1.Style.Add("word-wrap", "normal");
                    //绑定数据库表中数据
                    GRV1.DataBind();
                }
            }
            else if (CarNo.SelectedItem.Text != "")
            {
                //CarNo點選項目後呈現的GridView
                var url = from a in CarInfo.List_CarManintainRecord
                          orderby a.MaintainDate descending
                          where a.CarNo == CarNo.SelectedItem.Text && a.DeleteFlag == null
                          select new { a.ID, a.CarNo, a.MaintainDate, a.MaintainCert, a.Notice };

                if (url.FirstOrDefault() != null)
                {
                    //设置GridView控件的数据源为创建的数据集ds
                    GRV1.Columns[5].Visible = true;//將GridView的編輯和刪除欄打開
                    GRV1.DataSource = url;
                    GRV1.DataBind();
                }

                //将数据库表中的主键字段放入GridView控件的DataKeyNames属性中
                GRV1.DataKeyNames = new string[] { "ID" };
                //表頭內容不換行
                GRV1.Style.Add("word-break", "keep-all");
                GRV1.Style.Add("word-wrap", "normal");
                //====================Fromat===============
                GRV1.Font.Size = 11;
                GRV1.GridLines = GridLines.Both;	//設定格線
                GRV1.PageSize = 15;    //10行
                GRV1.PagerSettings.Position = PagerPosition.Bottom; //分頁位置
                GRV1.Font.Name = "微軟正黑體";		//設定字型大小
                GRV1.AlternatingRowStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#e6e6e6");
                GRV1.PagerStyle.HorizontalAlign = HorizontalAlign.Center; //分頁對齊
                GRV1.AllowPaging = true;
                GRV1.PagerStyle.Font.Size = 12;
                //====================Fromat===============
                ////绑定数据库表中数据
                GRV1.DataBind();
            }
        }
        //GridView綁定數據
        public DataTable getResult()
        {   

            System.Data.DataTable dt = new DataTable();

            StringBuilder sbrep = new StringBuilder();
            StringBuilder sbval = new StringBuilder();

            //查詢功能需用字串
            //string txtdrivername = "%" + txt_DriverName.Text.ToString().Trim() + "%";
            string txt_carno = CarNo.SelectedItem.Value.ToString();
            string txt_MaintainDate = "%" + MaintainDate.Text.ToString().Trim() + "%";
            string txt_MaintainCert = "%" + MaintainCert.Text.ToString().Trim() + "%";
            string txt_Notice = "%" + Notice.Text.ToString().Trim() + "%";

            //if (txtcarno != "%%") sbrep.Append(" and CarNo like @P1{?,?} ");
            //else sbrep.Append("{?,?}");
            if (txt_carno != "") sbrep.Append(" and CarNo=@P1{?,?} ");
            else sbrep.Append("{?,?}");
            if (txt_MaintainDate != "") sbrep.Append(" and MaintainDate like @P2{?,?} ");
            else sbrep.Append("{?,?}");
            if (txt_MaintainCert != "") sbrep.Append(" and MaintainCert like @P3{?,?} ");
            else sbrep.Append("{?,?}");
            if (txt_Notice != "") sbrep.Append(" and Notice like @P4{?,?} ");
            else sbrep.Append("{?,?}");

            sbval.Append(txt_carno).Append("{?,?}")
                .Append(txt_MaintainDate).Append("{?,?}")
                .Append(txt_MaintainCert).Append("{?,?}")
                .Append(txt_Notice).Append("{?,?}");


            sbrep.ToString();
            sbval.ToString();


            try
            {
                    //將查詢數據以DataTable形式取出
                    //dt = DBAccessProc.GetDataTable("GA_CarManagerment_P1434_Manitain", 0, 1, sbrep.ToString(), sbval.ToString(), "{?,?}", out msg, out effectedRow);
                    //如果有欄位有值
                    //if (sbrep.ToString() != "" || sbval.ToString() != "")
                    //{   

                        //if (CarNo.SelectedItem.Text == "請選擇") 
                        //{ 
                            dt = DBAccessProc.GetDataTable("GA_CarManagerment_P1434_Manitain", 0, 1, sbrep.ToString(), sbval.ToString(), "{?,?}", out msg, out effectedRow);
                            //GRV1.Columns[5].Visible = false;
                        //}
                        //else
                        //dt = DBAccessProc.GetDataTable("GA_CarManagerment_P1434_Manitain_Search", 0, 1, sbrep.ToString(), sbval.ToString(), "{?,?}", out msg, out effectedRow);
                    //}
            }
            catch (Exception ex)
            {
                Response.Write("GA_CarManagerment_P1434_Manitain error,Detail:" + ex.Message);
                sbrep.Clear();
                sbval.Clear();
            }

            if ("" != msg)
            {
                Response.Write("GA_CarManagerment_P1434_Manitain Failure:" + msg);
                return null;
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
        //GridView delete功能
        protected void GridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //取得要刪除資料的引所
            string bid = GRV1.DataKeys[e.RowIndex].Values["ID"].ToString();//唯讀

            var update_var = from a in CarInfo.List_CarManintainRecord where a.CarNo == CarNo.SelectedItem.Text && a.ID == int.Parse(bid) select a;
            //// delete
            foreach (var u in update_var)
            {   

                u.DeleteFlag = "1";
                u.DeleteOn = DateTime.Today;
                u.ModifyDate = Convert.ToDateTime(System.DateTime.Now);
            }
            try
            {
                CarInfo.SubmitChanges();
                BindData();
            }
            //如果程式有誤或例外狀況,將執行這段
            catch
            {
                Console.WriteLine(e);
            }

            GRV1.EditIndex = -1;//退出編輯模式
            GRV1.DataBind();
        }
        //GridView編輯功能
        protected void GridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GRV1.EditIndex = e.NewEditIndex;
            BindData();
        }
        //GridView編輯取消
        protected void GridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            GRV1.EditIndex=-1;
            BindData();
        }
        //GridView update功能
        protected void GridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {   
        //取得要更新資料的索引
        string bid = GRV1.DataKeys[e.RowIndex].Values["ID"].ToString();//唯讀
        //var update_var = from a in gr.List_GoodsRequest where a.FormNo == formno && a.GoodsID == int.Parse(bid) select a;
        var update_var = from a in CarInfo.List_CarManintainRecord where a.CarNo == CarNo.SelectedItem.Text && a.ID == int.Parse(bid) select a;
        string dt1 = ((TextBox)GRV1.Rows[e.RowIndex].FindControl("MaintainDate")).Text;
        string gn = ((TextBox)GRV1.Rows[e.RowIndex].FindControl("MaintainCert")).Text;
        string st = ((TextBox)GRV1.Rows[e.RowIndex].FindControl("Notice")).Text;
        // update 到資料庫中
        foreach (var u in update_var)
        {
            
            u.MaintainDate = Convert.ToDateTime(dt1);
            u.MaintainCert = gn;
            u.Notice = st;

        }
        //執行
        try
        {
            CarInfo.SubmitChanges();
            
        }
        //如果程式有誤或例外狀況,將執行這段
        catch
        {
            Console.WriteLine("Update SQL Error! Please contact IT to fix issue!");
        }
        GRV1.DataBind();
        GRV1.EditIndex = -1;//退出編輯模式
        BindData();

    }
        //按下儲存按鈕
        protected void Save_Click(object sender, EventArgs e)
        {
            string carno;
            carno = CarNo.SelectedValue;
            //var url = from a in CarInfo.List_CarManintainRecord select a.ID;
            //內容是否為空
            if (CarNo.SelectedItem.Text == "" || CarNo.SelectedItem.Text == "請選擇")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "window.alert('車號不能為空!');", true);
                return;
            }
            if (MaintainCert.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "window.alert('憑證不能為空!');", true);
                return;
            }
            if (MaintainDate.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", "window.alert('維修日期不能為空!');", true);
                return;
            }
            //將欄位內的資料存入資料庫
            var docall = new List_CarManintainRecord
            {
                CarNo = CarNo.SelectedItem.Text,
                MaintainCert = MaintainCert.Text,
                MaintainDate = Convert.ToDateTime(MaintainDate.Text),
                Notice = Notice.Text,
                //CreateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/M/d hh:mm:ss"))
                CreateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/M/d"))
            };
            //insert into 
            CarInfo.List_CarManintainRecord.InsertOnSubmit(docall);
            //執行
            try
            {
                CarInfo.SubmitChanges();
            }
            //如果程式有誤或例外狀況,將執行這段
            catch
            {
                Console.WriteLine("Update SQL Error! Please contact IT to fix issue!");
            }
            BindData();
        }
        //按下查詢按鈕
        protected void Search_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dt = new DataTable();
            dt = getResult();

            if (dt != null)
            {
                //设置GridView控件的数据源为创建的数据集ds
                GRV1.DataSource = dt;
                //将数据库表中的主键字段放入GridView控件的DataKeyNames属性中
                GRV1.DataKeyNames = new string[] { "ID" };
                //绑定数据库表中数据
                GRV1.DataBind();
            }
        }
}