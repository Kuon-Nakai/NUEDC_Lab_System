#define USE_ASYNC

using MySql.Data.MySqlClient;
using System;
using System.Threading;
using System.Threading.Tasks;

public partial class AssetsPage : System.Web.UI.Page
{
    private DynamicControls dc = new DynamicControls();
    MySqlSvr svr;
    bool loggedIn = false;
    string userId;

    protected async void Page_Load(object sender, EventArgs e)
    {
        Page.MaintainScrollPositionOnPostBack = true;
        if (!loggedIn && Session["UserID"] != null)
        {
            loggedIn = true;
            userId = (string)Session["UserID"];
        }
        if (IsPostBack) { return; }
        svr = new MySqlSvr(new MySqlConnection("server=127.0.0.1; database=nuedc; user id=notRoot; password=1234"));
        //Update system usage public summary
#if USE_ASYNC
        svr.cn.Open();
        var tc = svr.Cmd("select sum(Amount) from assets group by com;").ExecuteScalarAsync();
        var te = svr.Cmd("select count(AssetCode) from assets group by com;").ExecuteScalarAsync();
        var tl = svr.Cmd("select sum(Qty) from lending group by com;").ExecuteScalarAsync();
        var tr = svr.Cmd("select sum(Qty) from lending group by TransactionCycleEnded having TransactionCycleEnded=1;").ExecuteScalarAsync();

        TotalComp_lb.Text = (await tc)?.ToString();
        TotalEntries_lb.Text = (await te)?.ToString();
        TotalLent_lb.Text = (await tl)?.ToString();
        TotalReg_lb.Text = (await tr)?.ToString();
#else // not USE_ASYNC
        TotalComp_lb.Text = (string)svr.QuerySingle("select sum(Amount) from assets group by com;");
        TotalEntries_lb.Text = (string)svr.QuerySingle("select count(AssetCode) from assets group by com;");
        TotalLent_lb.Text = (string)svr.QuerySingle("select sum(Qty) from lending group by com;");
        TotalReturned_lb.Text = (string)svr.QuerySingle("select sum(Qty) from lending group by TransactionCycleEnded having TransactionCycleEnded=1;");
        //TotalQueries_lb.Text = (string) svr.QuerySingle("select ")
#endif //USE_ASYNC
        InitiateSearch("select AssetCode, AssetName, MainValue, ValueUnit from assets left join assetclasses on assets.ClassCode = assetclasses.ClassCode;");
        dc.CreateAlert("This is a debug version of the website. Content may change at any time.", "notice", Alerts_pn);
        //DynamicControls.CreateAlert("Info", "info", Alerts_pn);
        LoadAssetData($"assets.AssetCode='{Asset_gv.Rows[0].Cells[1].Text}'");
    }

    private void InitiateSearch(string sql)
    {
        if (svr == null)
        {
            svr = new MySqlSvr("server=127.0.0.1; database=nuedc; user id=notRoot; password=1234");
        }
        Asset_gv.DataSource = svr.QueryDataset(sql);
        Asset_gv.DataBind();
    }

    protected bool Assert_Fields()
    {
        try
        {
            return int.Parse(BorrowQtySel_tb.Text).ToString().Length > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

    protected bool Assert_BorrowQty()
    {
        //if(int.Parse(BorrowQtySel_tb.Text) > int.Parse(Borrowable_lb.Text))
        if (int.Parse(BorrowQtySel_tb.Text) > 16)
        {
            Page.SetFocus(BorrowQtySel_tb);
            dc.CreateAlert($"申请数量无效: 超出最大可用量({Borrowable_lb.Text})", "error");
            return false;
        }
        if (int.Parse(BorrowQtySel_tb.Text) < 0)
        {
            Page.SetFocus(BorrowQtySel_tb);
            dc.CreateAlert("申请数量无效: 但是欢迎给实验室送元件()", "error");
            return false;
        }
        return true;
    }

    protected bool Assert_BorrowPerm()
    {
        try
        {
            return ((int?)Session["UserPerm"] ?? 256) <= (int)svr.QuerySingle($"select LendingPolicy from assets where AssetCode='{(Asset_gv.SelectedRow ?? Asset_gv.Rows[0]).Cells[1].Text}';");
        }
        catch (Exception)
        {
            return false;
        }
    }

    private void RequestBorrow()
    {
        if (Assert_Fields() &&
            Assert_BorrowQty())
        {
            // All web side checks pass
            try
            {
                if (svr.QuerySingle($"select TransactionCode from lending where AssetCode='{(Asset_gv.SelectedRow ?? Asset_gv.Rows[0]).Cells[1].Text}' and FullCode='{ItemID_tb.Text}' and not TransactionCycleEnded;").ToString().Length > 0)
                {
                    dc.CreateAlert("数据不一致: 该元件已经处于借出状态", "error", Alerts_pn);
                    return;
                }
                // Data consistency check pass
                if ((DateTime)Application["LastReqDate"] != DateTime.Today)
                {
                    Application["LastReqDate"] = DateTime.Today;
                    Application["TransactionCount"] = 0;
                }
                using (var transact = svr.cn.BeginTransaction())
                {
                    lock (this)
                    {
                        if (svr.Execute("insert into lending values(" +
                            $"'{DateTime.Today.ToString("yyyy-MM-dd").Replace("-", "") + string.Format("%4d", (int)Application["TransactionCount"])}'," + // TransactionCode
                            $"'{(Asset_gv.SelectedRow ?? Asset_gv.Rows[0]).Cells[1].Text}'," + // AssetCode
                            $"{(BorrowQtySel_tb.Enabled ? int.Parse(BorrowQtySel_tb.Text) : 1)}," + // Qty
                            $"{(ItemID_tb.Enabled ? ItemID_tb.Text : "null")}," + // FullCode
                            $"{userId}," + // MemberCode
                            "'Requested'," + // Status
                            "NOW()," + // DateRequested
                            "null," + // DateProcessed
                            "null," + // DateReturned
                            "0," + // TransactionCycleEnded
                            "null," + // Note
                            "1);", transact) != 1)
                        {
                            dc.CreateAlert("数据库操作异常/数据插入", "error", Alerts_pn);
                            transact.Rollback();
                            return;
                        }
                        if (Assert_BorrowPerm())
                        {
                            // Permission level reached, system will auto approve the request
                            if (svr.Execute("update lending set " +
                                $"DateProcessed=NOW()," +
                                $"Status='Taken'," +
                                $"Note='Automatically approved by system'" +
                                $"where TransactionCode={DateTime.Today.ToString("yyyy-MM-dd").Replace("-", "") + string.Format("%4d", (int)Application["TransactionCount"])};",
                                transact) != 1)
                            {
                                dc.CreateAlert("数据库操作异常/数据修改-1", "error", Alerts_pn);
                                transact.Rollback();
                                return;
                            }
                        }
                        if ((int)svr.QuerySingle($"select AutoCplt from assetclasses right join assets on assetclasses.ClassCode=assets.ClassCode where assets.AssetCode='{(Asset_gv.SelectedRow ?? Asset_gv.Rows[0]).Cells[1].Text}") == 1)
                        {
                            // Transaction cycle will be marked as complete
                            if (svr.Execute("update lending set " +
                                "TransactionCycleEnded = 1",
                                transact) != 1)
                            {
                                dc.CreateAlert("数据库操作异常/数据修改-2", "error", Alerts_pn);
                                transact.Rollback();
                                return;
                            }
                        }
                        var temp = (int)Application["TransactionCount"];
                        Application["TransactionCount"] = temp + 1;
                    }
                    transact.Commit();
                }

            }
            catch (Exception)
            {
                dc.CreateAlert("数据库操作错误", "error", Alerts_pn);
            }
        }
    }

    public void LoadAssetData(string sql_where_assets)
    {
        if (svr == null) { svr = new MySqlSvr("server=127.0.0.1; database=nuedc; user id=notRoot; password=1234"); }
        int i = -1;
        svr.QueryReader($"select AssetName, ClassName, MainValue, ValueUnit, Location, Characteristics, Amount-ReservationQty-(select count(TransactionCode), AutoCplt from lending right join assets on lending.AssetCode=assets.AssetCode where {sql_where_assets} and Status='Returned') from assets left join assetclasses on assets.ClassCode=assetclasses.ClassCode where {sql_where_assets};",
            AssignAssetData,
            /*() => Alert_DBQueryEmpty_pn.Visible = true*/
            () => dc.CreateAlert("元件信息查询失败: 返回信息为空", "error", Alerts_pn));
        svr.QueryReader($"select URL, Title from datasheets left join assets on datasheets.AssetCode=assets.AssetCode where {sql_where_assets}",
            (MySqlDataReader rd) =>
            {
                Datasheet_pn1.Controls.Remove(Datasheet_lk);
                dc.CreateHTMLElement("br", Datasheet_pn0);
                dc.CreateHTMLElement("div", Datasheet_pn1);
                dc.CreateHyperLink(rd.GetValue(1).ToString(), rd.GetValue(0).ToString(), Datasheet_pn1);
                //Datasheet_pn1.Controls.RemoveAt(Datasheet_pn1.Controls.Count - 1);
                ++i;
            },
            () =>
            {
                Datasheet_lk.Text = "暂无文档";
            });
    }

    private void AssignAssetData(MySqlDataReader rd)
    {
        AssetName_lb.Text = (string)rd[0];
        AssetClass_lb.Text = (string)rd[1];
        PrimValue_lb.Text = $"{rd[2]} {rd[3]}";
        Location_lb.Text = (string)rd[4];
        Property_lb.Text = rd[5].ToString().Length > 0 ? (string)rd[5] : "暂无数据";
        if ((bool)rd[6])
        {
            BorrowQtySel_tb.Enabled = true;
            ItemID_tb.Enabled = false;
        }
        else
        {
            BorrowQtySel_tb.Enabled = false;
            ItemID_tb.Enabled = true;
        }

        //Legacy code
        //if (((string)rd[6]).Length == 0)
        //{
        //    Datasheet_lk.Text = "暂无";
        //    Datasheet_lk.NavigateUrl = "#0";
        //}
        //else
        //{
        //    Datasheet_lk.Text = "点击访问";
        //    Datasheet_lk.NavigateUrl = (string)rd[6];
        //}

        Borrowable_lb.Text = rd[6]?.ToString();
        if ((UInt64)rd[6] > 0)
        {
            BorrowQtySel_tb.Enabled = true;
            //BorrowConfirm_pn.Visible = true;
            //BorrowNotAvailable_pn.Visible = false;
        }
        else
        {
            BorrowQtySel_tb.Enabled = false;
            //BorrowConfirm_pn.Visible = false;
            //BorrowNotAvailable_pn.Visible = true;
        }
    }

    protected void Search_bt_Click(object sender, EventArgs e)
    {
        string sear_str = sear_tb.Text;
        InitiateSearch($"select AssetCode as 元件代码, AssetName as 元件名称, MainValue as 值, ValueUnit as 单位 from assets left join assetclasses on assets.ClassCode = assetclasses.ClassCode where AssetCode like '%{sear_str}%' or AssetName like '%{sear_str}%';");
        LoadAssetData($"assets.AssetCode='{Asset_gv.Rows[0].Cells[1].Text}'");
    }

    protected void AwaitReturn_bt_Click(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            dc.CreateAlert("请登录", "error", Alerts_pn);
        }
        else
        {
            InitiateSearch($"select AssetCode, AssetName, MainValue, ValueUnit from assets left join assetclasses on assets.ClassCode = assetclasses.ClassCode where lending.MemberCode='{Session["UserID"]}' and lending.Status='taken'; ");
        }
    }

    protected async void LendReg_bt_Click(object sender, EventArgs e)
    {
        var tsk = new Task(RequestBorrow);
        var timeout = new Task(() =>
        {
            Thread.Sleep(5000);
            dc.CreateAlert("任务处理超时, 请重试或检查数据", "error", Alerts_pn);
        });
        tsk.Start();
        Borrow_tb.CssClass = "btn u-fullwidth";
        Borrow_tb.Enabled = false;
        Borrow_tb.Text = "正在处理...";
        await Task.WhenAny(tsk, timeout);
    }

    protected void Asset_gv_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadAssetData($"assets.AssetCode='{Asset_gv.SelectedRow.Cells[1].Text}'");
    }
}