using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
/// <summary>
/// CountAccount 的摘要描述
/// </summary>
public class CountAccount
{
    //＊＊＊＊＊＊＊＊DataSet＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    BTDataContext bt = new BTDataContext();
    BaseDataContext bc = new BaseDataContext();
    string baseCon = ConfigurationManager.ConnectionStrings["BASEConnectionString"].ConnectionString;
    //＊＊＊＊＊＊＊＊DataSet＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊
    StringBuilder sbval = new StringBuilder();
    StringBuilder sbrep = new StringBuilder();
    System.Data.DataTable dt = new DataTable();
    System.Data.DataTable dc = new DataTable();
    string msg = "";
    int effectRow = 0;
   
    public string countinfo(string formno, string currency, string exchange, string userid, string factory)
    {
        //string disrefundofall;//小計退款
        double RealityTWCost = 0;
        double PrePareTWCost = 0;
        double Social = 0;
        string draw="";
        //差旅核算
        //var DisPCost = from a in bt.List_CountAccount where a.AccFormNo == formno select a;
        //匯率 & 幣別
        //List_CountAccount→差旅核算
        //var table = from a in bt.FormBusinessAccount where a.FormNo == formno select a;
        try
        {
            //設定連接字串
            DBAccessProc.setConnStr(baseCon);
            sbval.Append(formno).Append(","); //定義變量用於獲取變更的參數
            //將查詢數據以DataTable形式取出
            dt = DBAccessProc.GetDataTable("HR_BusinessTrip_P1266_CountAccountView", 0, 1, "", sbval.ToString(), ",", out msg, out effectRow);
            foreach (DataRow dr in dt.Rows)
            {
                if (factory == "FSM" || factory == "大陸廠區")
                {
                    RealityTWCost = RealityTWCost + (System.Math.Round(Convert.ToDouble(dr["RealityTWCost"].ToString()), 2));
                    PrePareTWCost = PrePareTWCost + System.Math.Round(Convert.ToDouble(dr["PrePareTWCost"].ToString()), 2);
                }
                else {
                    RealityTWCost = RealityTWCost + (System.Math.Round(Convert.ToDouble(dr["RealityTWCost"].ToString()), 0));
                    PrePareTWCost = PrePareTWCost + System.Math.Round(Convert.ToDouble(dr["PrePareTWCost"].ToString()), 0);
                }
                draw = draw + dr["DisPreCurrency"].ToString();
            }
            
            
            var fund = from a in bt.FormBusinessAccount where a.FormNo == formno select a;
            //小計退款DisRefundofAll
            string disrefundofall = fund.First().DisRefundofAll.ToString().Trim();
            //小計預支DisPCostofAll
            double dispcostofall = PrePareTWCost;
            //小計實際DisRCostofAll
            double disrcostofall = RealityTWCost;
            string tolsocialdues = "";
            string tolothersdues = "";
            string cost = "";
            if (factory == "FSM" || factory == "大陸廠區")
            {
                //交際費
                tolsocialdues = Convert.ToString(System.Math.Round(Convert.ToDouble(Social), 2));
                //差旅費
                tolothersdues = Convert.ToString(System.Math.Round(Convert.ToDouble(disrcostofall) - Convert.ToDouble(tolsocialdues), 2));
                //付款計算=總預支-總支出-退款+匯兌損益
               cost = Convert.ToString(System.Math.Round(Convert.ToDouble(dispcostofall) - Convert.ToDouble(disrcostofall) - Convert.ToDouble(disrefundofall) + Convert.ToDouble(exchange), 2));
            }
            else {
                //交際費
                tolsocialdues = Convert.ToString(System.Math.Round(Convert.ToDouble(Social), 0));
                //差旅費
                tolothersdues = Convert.ToString(System.Math.Round(Convert.ToDouble(disrcostofall) - Convert.ToDouble(tolsocialdues), 0));
                //付款計算=總預支-總支出-退款+匯兌損益
                cost = Convert.ToString(System.Math.Round(Convert.ToDouble(dispcostofall) - Convert.ToDouble(disrcostofall) - Convert.ToDouble(disrefundofall) + Convert.ToDouble(exchange), 0));
            }
            //付款狀態(員工應退、公司支付)
            string othertype;
            string othercost;
            if (Convert.ToDouble(cost) > 0)
            {
                othertype = "員工應退";
                othercost = cost;
            }
            else
            {
                othertype = "公司支付";
                othercost = Convert.ToString(System.Math.Abs(Convert.ToDouble(cost)));
            }
            var update = from a in bt.FormBusinessAccount where a.FormNo == formno select a;
            foreach (var u in update)
            {
                u.DisPCostofAll = Convert.ToDouble(dispcostofall);
                u.DisRCostofAll = Convert.ToDouble(disrcostofall);
                u.DisRefundofAll = Convert.ToDouble(disrefundofall);
                u.TolSocialDues = Convert.ToDouble(tolsocialdues);
                u.TolOthersDues = Convert.ToDouble(tolothersdues);
                u.OtherType = othertype;
                u.OthetCost = Convert.ToDouble(othercost);
                u.ModifyMan = userid;
                u.ModifyDate = System.DateTime.Now;
            }
            try
            {
                bt.SubmitChanges();
                return dispcostofall + "|" + disrcostofall + "|" + tolsocialdues + "|" + tolothersdues + "|" + othertype + "|" + othercost;
            }
            catch
            {
                return "Error";
            }
        }
        catch {
            return "NO";
        }
           

    }
    public void approve(string formno, string userid, string exchangecost, string remittancedate)
    {
        string Remittance = "";
        string draw = "";
        double PrePareTWCost = 0;
        double DisPCostofAll = 0;
        double RealityTWCost = 0;
        double DisRCostofAll = 0;
        double Social = 0;
            try
            {
                //設定連接字串
                DBAccessProc.setConnStr(baseCon);
                var table = from a in bt.FormBusinessAccount where a.FormNo == formno select a;
                sbval.Append(formno).Append(","); //定義變量用於獲取變更的參數
                //將查詢數據以DataTable形式取出
                var item = from a in bt.List_CountAccount where a.AccFormNo == formno select a;
                var view = from a in bt.V_CountAccount where a.AccFormNo == formno select a;

                if (item.Count() == 0)
                {
                    dt = DBAccessProc.GetDataTable("HR_BusinessTrip_P1266_CountAccount", 0, 1, "", formno, ",", out msg, out effectRow);
                    foreach (DataRow dr in dt.Rows)
                    {
                        var detail = new List_CountAccount
                        {
                            ApplyManNo = table.First().ApplyManNo.ToString().Trim(),
                            CreateDate = Convert.ToDateTime(System.DateTime.Now),
                            Creator = "System",
                            AccFormNo = formno,
                            DisPreCurrency = dr["DisPreCurrency"].ToString(),
                            DisPreCost = Convert.ToDouble(dr["DisPreCost"].ToString()),
                            RealityRate = Convert.ToDouble(dr["RealityRate"].ToString()),
                            RealityCost = Convert.ToDouble(dr["RealityCost"].ToString()),
                            PrePareTWCost = System.Math.Round(Convert.ToDouble(dr["PrePareTWCost"].ToString()), 2),
                            RealityTWCost = System.Math.Round(Convert.ToDouble(dr["RealityTWCost"].ToString()), 2),
                        };
                        bt.List_CountAccount.InsertOnSubmit(detail);
                        try
                        {
                            bt.SubmitChanges();
                        }
                        catch
                        {
                        }
                        PrePareTWCost = PrePareTWCost + System.Math.Round(Convert.ToDouble(dr["PrePareTWCost"].ToString()), 2);
                        RealityTWCost = RealityTWCost + System.Math.Round(Convert.ToDouble(dr["RealityTWCost"].ToString()), 2);
                        draw = draw + dr["DisPreCurrency"].ToString();
                        Social = Social + System.Math.Round(Convert.ToDouble(dr["Social"].ToString()), 0);
                    }
                    //報銷與預支金額
                    var salary = from a in bt.List_DrawSalary where a.DeleteFlag == null && a.MainFormNo == table.First().MainFormNo.ToString().Trim() select a;
                    dc = DBAccessProc.GetDataTable("HR_BusinessTrip_P1266_DrawCurrency", 0, 1, "", formno, ",", out msg, out effectRow);
                    foreach (DataRow dr in dc.Rows)
                    {
                        string dcrows = dr["Currency"].ToString();
                        if (draw.Contains(Convert.ToString(dr["Currency"].ToString())))
                        {
                            string testaa = salary.First().Currency.ToString().Trim();
                        }
                        else
                        {
                            var detail = new List_CountAccount
                            {
                                ApplyManNo = table.First().ApplyManNo.ToString().Trim(),
                                CreateDate = Convert.ToDateTime(System.DateTime.Now),
                                Creator = "System",
                                AccFormNo = formno,
                                DisPreCurrency = dr["Currency"].ToString(),
                                DisPreCost = Convert.ToDouble(dr["CountCost"].ToString()),
                                RealityRate = Convert.ToDouble(dr["Rate"].ToString()),
                                PrePareTWCost = System.Math.Round(Convert.ToDouble(dr["Count"].ToString()), 2),
                                RealityCost = 0,
                                RealityTWCost = 0,
                            };
                            bt.List_CountAccount.InsertOnSubmit(detail);
                            PrePareTWCost = PrePareTWCost + System.Math.Round(Convert.ToDouble(dr["Count"].ToString()), 2);
                            try
                            {
                                bt.SubmitChanges();
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                else
                {
                    dt = DBAccessProc.GetDataTable("HR_BusinessTrip_P1266_CountAccount", 0, 1, "", formno, ",", out msg, out effectRow);
                    foreach (DataRow dr in dt.Rows)
                    {
                        PrePareTWCost = PrePareTWCost + System.Math.Round(Convert.ToDouble(dr["PrePareTWCost"].ToString()), 2);
                        RealityTWCost = RealityTWCost + System.Math.Round(Convert.ToDouble(dr["RealityTWCost"].ToString()), 2);
                        draw = draw + dr["DisPreCurrency"].ToString();
                        Social = Social + System.Math.Round(Convert.ToDouble(dr["Social"].ToString()), 0);
                    }

                }


                string DisRefundofAll = table.First().DisRefundofAll.ToString().Trim();//退款金額
                //付款計算=總預支-總支出-退款+匯兌損益
                DisPCostofAll = PrePareTWCost;
                DisRCostofAll = RealityTWCost;

                string cost = Convert.ToString(System.Math.Round(Convert.ToDouble(DisPCostofAll) - Convert.ToDouble(DisRCostofAll) - Convert.ToDouble(DisRefundofAll) + Convert.ToDouble(exchangecost), 2));

                string redate = remittancedate;
                if (Convert.ToString(remittancedate) == "")
                {
                    Remittance = null;
                }
                else
                {
                    Remittance = Convert.ToString(remittancedate);
                }
                //匯款日期
                string OtherType;
                string OtherCost;
                string TolSocialDues;
                string TolOthersDues;
                //付款狀態(員工應退、公司支付)
                if (Convert.ToDouble(cost) > 0)
                {
                    OtherType = "員工應退";
                    OtherCost = cost;
                }
                else
                {
                    OtherType = "公司支付";
                    OtherCost = Convert.ToString(System.Math.Abs(Convert.ToDouble(cost)));
                }
                //交際費
                TolSocialDues = Convert.ToString(System.Math.Round(Social, 0));
                //差旅費
                TolOthersDues = Convert.ToString(System.Math.Round(Convert.ToDouble(DisRCostofAll) - Convert.ToDouble(TolSocialDues), 2));

                foreach (var u in table)
                {
                    u.ExchangeCost = Convert.ToDouble(exchangecost);
                    u.DisPCostofAll = System.Math.Abs(Convert.ToDouble(DisPCostofAll));
                    u.DisRCostofAll = System.Math.Abs(Convert.ToDouble(DisRCostofAll));
                    u.OtherType = OtherType;
                    u.OthetCost = System.Math.Abs(Convert.ToDouble(cost));
                    u.RemittanceDate = Convert.ToDateTime(Remittance);
                    u.TolSocialDues = System.Math.Abs(Convert.ToDouble(TolSocialDues));
                    u.TolOthersDues = System.Math.Abs(Convert.ToDouble(TolOthersDues));
                    u.ModifyMan = "System";
                    u.ModifyDate = System.DateTime.Now;
                }
                try
                {
                    bt.SubmitChanges();
                }
                catch
                {
                }
            }
            catch
            {
            }
    }
}
