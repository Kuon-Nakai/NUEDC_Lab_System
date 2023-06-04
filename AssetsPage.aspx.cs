#define USE_ASYNC

using MySql.Data.MySqlClient;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

public partial class AssetsPage : System.Web.UI.Page
{
    private readonly DynamicControls dc = new DynamicControls();
    MySqlSvr svr = new MySqlSvr("server=127.0.0.1; database=nuedc; user id=notRoot; password=1234");
    bool loggedIn = false;
    public string userId;
    
    //private List<string> class0 = new List<string>();
    //private List<string> class1 = new List<string>();
    //private List<string> class2 = new List<string>();

    protected async void Page_Load(object sender, EventArgs e)
    {
        Page.MaintainScrollPositionOnPostBack = true;
        if (!loggedIn && Session["UserID"] != null)
        {
            loggedIn = true;
            Login_Jmp_bt.Text = "已登录";
            userId = (string)Session["UserID"];
            Borrow_tb.OnClientClick = "showBorrowConfirmPopup();";
            Borrow_tb.Attributes["href"] = "#";
        }
        if (IsPostBack) { return; }
        //if (class0.Count == 0)
        //{
            svr.QueryReader_Parallel(new MySqlSvr.QueryReaderTask
            {
                sql = "select ClassCode, ClassName from assetclasses where Length(ClassCode)=3;",
                ReaderDataHandler = (MySqlDataReader rd) =>
                {
                    TypeSel0_ddl.Items.Add($"{rd[0]}{rd[1]}");
                    //class0.Add($"{rd[0]} {rd[1]}");
                },
                NoDataHandler = () => TypeSel0_ddl.Visible = false
            }, new MySqlSvr.QueryReaderTask
            {
                sql = "select ClassCode, ClassName from assetclasses where Length(ClassCode)=6;",
                ReaderDataHandler = (MySqlDataReader rd) =>
                {
                    TypeSel1_ddl.Items.Add($"{((string)rd[0]).Substring(3)}{rd[1]}");
                    //class1.Add($"{((string)rd[0]).Substring(3)} {rd[1]}");
                },
                NoDataHandler = () => TypeSel1_ddl.Visible = false
            }, new MySqlSvr.QueryReaderTask
            {
                sql = "select ClassCode, ClassName from assetclasses where Length(ClassCode)=9;",
                ReaderDataHandler = (MySqlDataReader rd) =>
                {
                    TypeSel2_ddl.Items.Add($"{((string)rd[0]).Substring(6)}{rd[1]}");
                    //class2.Add($"{((string)rd[0]).Substring(6)} {rd[1]}");
                },
                NoDataHandler = () => TypeSel2_ddl.Visible = false
            });
        //}
        
        svr = new MySqlSvr(new MySqlConnection("server=127.0.0.1; database=nuedc; user id=notRoot; password=1234"));
        //Update system usage public summary
#if USE_ASYNC
        svr.cn.Open();
        var tc = svr.Cmd("select sum(Amount) from assets group by com;").ExecuteScalarAsync();
        var te = svr.Cmd("select count(AssetCode) from assets group by com;").ExecuteScalarAsync();
        var tl = svr.Cmd("select sum(Qty) from lending group by com;").ExecuteScalarAsync();
        var tr = svr.Cmd("select sum(Qty) from lending group by TransactionCycleEnded having TransactionCycleEnded=1;").ExecuteScalarAsync();

        InitiateSearch("select AssetCode, AssetName, MainValue, ValueUnit from assets left join assetclasses on assets.ClassCode = assetclasses.ClassCode;");
        dc.CreateAlert("This is a debug version of the website. Content may change at any time.", "notice", Alerts_pn);
        //DynamicControls.CreateAlert("Info", "info", Alerts_pn);
        LoadAssetData($"assets.AssetCode='{Asset_gv.Rows[0].Cells[0].Text}'");

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
    }

    private void InitiateSearch(string sql)
    {
        if (svr == null)
        {
            svr = new MySqlSvr("server=127.0.0.1; database=nuedc; user id=notRoot; password=1234");
        }
        ViewState["ds"] = svr.QueryDataset(sql);
        Asset_gv.DataSource = ViewState["ds"];
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
            return ((int?)Session["UserPerm"] ?? 256) <= (svr.QuerySingle($"select LendingPolicy from assets where AssetCode='{(Asset_gv.SelectedRow ?? Asset_gv.Rows[0]).Cells[0].Text}';") as int?);
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
                if (svr == null) svr = new MySqlSvr("server=127.0.0.1; database=nuedc; user id=notRoot; password=1234");
                if (svr.QuerySingle($"select TransactionCode from lending where AssetCode='{(Asset_gv.SelectedRow ?? Asset_gv.Rows[0]).Cells[0].Text}' and FullCode='{ItemID_tb.Text}' and not TransactionCycleEnded;") != null)
                {
                    dc.CreateAlert("数据不一致: 该元件已经处于借出状态", "error", Alerts_pn);
                    return;
                }
                // Data consistency check pass
                if (Application["LastReqDate"] == null || (DateTime)Application["LastReqDate"] != DateTime.Today)
                {
                    Application["LastReqDate"] = DateTime.Today;
                    Application["TransactionCount"] = 0;
                }
                svr.cn.Open();
                using (var transact = svr.cn.BeginTransaction())
                {
                    var permitted = false;
                    // Synchronization lock to prevent multiple requests conflicting
                    lock (this)
                    {
                        if (svr.Execute("insert into lending values(" +
                            $"'{DateTime.Today.ToString("yyyy-MM-dd").Replace("-", "") + ((int)Application["TransactionCount"]).ToString("0000")}'," + // TransactionCode
                            $"'{(Asset_gv.SelectedRow ?? Asset_gv.Rows[0]).Cells[0].Text}'," + // AssetCode
                            $"{(BorrowQtySel_tb.Enabled ? int.Parse(BorrowQtySel_tb.Text) : 1)}," + // Qty
                            $"{(ItemID_tb.Enabled ? (Asset_gv.SelectedRow ?? Asset_gv.Rows[0]).Cells[0].Text + ItemID_tb.Text : "null")}," + // FullCode
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
                            svr.cn.Close();
                            return;
                        }
                        var temp = (int)Application["TransactionCount"];
                        Application["TransactionCount"] = temp + 1;
                        //transact.Commit();
                        if (!Assert_BorrowPerm())
                        {
                            // Permission level reached, system will auto approve the request
                            if (svr.Execute("update lending set " +
                                "DateProcessed=NOW()," +
                                "Status='Taken'," +
                                "Note='Automatically approved by system'" +
                                $"where TransactionCode={DateTime.Today.ToString("yyyy-MM-dd").Replace("-", "") + ((int)Application["TransactionCount"] - 1).ToString("0000")};",
                                transact) != 1)
                            {
                                dc.CreateAlert("数据库操作异常/数据修改-1", "error", Alerts_pn);
                                transact.Rollback();
                                svr.cn.Close();
                                return;
                            }
                            else
                            {
                                permitted = true;
                            }
                        }
                        //transact.Commit();
                        if ((ulong)svr.QuerySingle($"select AutoCplt from assetclasses right join assets on assetclasses.ClassCode=assets.ClassCode where assets.AssetCode='{(Asset_gv.SelectedRow ?? Asset_gv.Rows[0]).Cells[0].Text}';") == 1)
                        {
                            // Transaction cycle will be marked as complete
                            if (svr.Execute("update lending set " +
                                "TransactionCycleEnded = 1 " +
                                $"where TransactionCode={DateTime.Today.ToString("yyyy-MM-dd").Replace("-", "") + ((int)Application["TransactionCount"] - 1).ToString("0000")};",
                                transact) != 1)
                            {
                                dc.CreateAlert("数据库操作异常/数据修改-2", "error", Alerts_pn);
                                transact.Rollback();
                                svr.cn.Close();
                                return;
                            }
                        }
                    }
                    transact.Commit();
                    svr.cn.Close();
                    Borrow_tb.Text = "登记成功!";
                    Borrow_tb.CssClass = "btn btn--primary u-fullwidth";
                    Borrow_tb.Enabled = true;
                    dc.CreateAlert("登记成功, 审核通过后即可取走元件!", "success", Alerts_pn);
                    if (permitted)
                        dc.CreateAlert("审核已通过, 请自行取走元件!", "success", Alerts_pn);
                }

            }
            catch (Exception)
            {
                //throw e;
                dc.CreateAlert("数据库操作错误", "error", Alerts_pn);
                svr.cn.CloseAsync();
            }
        }
    }

    public void LoadAssetData(string sql_where_assets)
    {
        if (svr == null) { svr = new MySqlSvr("server=127.0.0.1; database=nuedc; user id=notRoot; password=1234"); }
        int i = -1;
        // Load data to right side bar
        svr.QueryReader($"select AssetName, ClassName, MainValue, ValueUnit, Location, Characteristics, Amount-ReservationQty-(select count(TransactionCode) from lending right join assets on lending.AssetCode=assets.AssetCode where {sql_where_assets} and Status='Returned'), AutoCplt from assets left join assetclasses on assets.ClassCode=assetclasses.ClassCode where {sql_where_assets};",
            AssignAssetData,
            /*() => Alert_DBQueryEmpty_pn.Visible = true*/
            () => dc.CreateAlert("元件信息查询失败: 返回信息为空", "error", Alerts_pn));
        // Load all datasheets
        svr.QueryReader($"select URL, Title from datasheets left join assets on datasheets.AssetCode=assets.AssetCode where ({sql_where_assets})",
            (MySqlDataReader rd) =>
            {
                Datasheet_pn1.Controls.Remove(Datasheet_lk);
                dc.CreateHTMLElement("br", Datasheet_pn0);
                dc.CreateHTMLElement("div", Datasheet_pn1);
                dc.CreateHyperLink(rd.GetValue(1).ToString(), rd.GetValue(0).ToString(), Datasheet_pn1);
                
                //Datasheet_pn1.Controls.RemoveAt(Datasheet_pn1.Controls.Count - 1);
                ++i;
            },
            () => Datasheet_lk.Text = "暂无文档");
        // Check if return is possible
        if (Session["UserID"] != null)
        {
            var qty = 0;
            ReturnDeadline_lb.Text = "";
            ReturnCodeSel_ddl.Items.Clear();
            var requirePerm = byte.Parse(svr.QuerySingle($"select AutoCplt from assetclasses right join assets on assets.ClassCode=assetclasses.ClassCode where {sql_where_assets}").ToString()) == 0;
            if ((int)Session["UserPerm"] > 2 && requirePerm)
            {
                Return_bt.OnClientClick = "showReturnImpossiblePopup();";
                Return_bt.Attributes["href"] = "#";
            }
            svr.QueryReader($"select DateProcessed, TransactionCode, AssetName, FullCode from lending left join assets on assets.AssetCode=lending.AssetCode where ({sql_where_assets}) and MemberCode={Session["UserID"]} and Status='Taken'", (MySqlDataReader rd) =>
            {
                Return_pn.Visible = true;
                ReturnDeadline_lb.Text += $"{rd.GetDateTime(0).Date.AddMonths(1).ToLongDateString()}<br />";
                //if (requirePerm)
                ReturnCodeSel_ddl.Items.Add(requirePerm ? $"[{rd.GetString(1)}]{rd.GetString(2)}-{rd.GetString(3)} @ {rd.GetDateTime(0).Date.ToShortDateString()}" : $"[{rd.GetString(1)}]{rd.GetString(2)} @ {rd.GetDateTime(0).Date.ToShortDateString()}");
                //ReturnCodeSel_ddl.Visible = requirePerm;
                ++qty;
            }, () => Return_pn.Visible = false);
            if (qty > 0)
            {
                ToReturn_lb.Text = qty.ToString();
                ReturnCodeSel_ddl.ToolTip = ReturnCodeSel_ddl.SelectedValue;
            }
        }
        
    }

    private void AssignAssetData(MySqlDataReader rd)
    {
        AssetName_lb.Text = (string)rd[0];
        AssetClass_lb.Text = (string)rd[1];
        PrimValue_lb.Text = (string)rd[2];
        Location_lb.Text = (string)rd[4];
        Property_lb.Text = rd[5].ToString().Length > 0 ? (string)rd[5] : "暂无数据";
        if ((ulong)rd[7] == 1)
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
        InitiateSearch($"select AssetCode as 元件代码, AssetName as 元件名称, MainValue as 值, ValueUnit as 单位 from assets left join assetclasses on assets.ClassCode = assetclasses.ClassCode where AssetCode='{sear_str}' or AssetName like '%{sear_str}%' or MainValue like '%{sear_str}%';");
        LoadAssetData($"assets.AssetCode='{Asset_gv.Rows[0].Cells[0].Text}'");
    }

    protected void AwaitReturn_bt_Click(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            dc.CreateAlert("请登录", "error", Alerts_pn);
        }
        else
        {
            InitiateSearch($"select lending.AssetCode as 元件代码, AssetName as 元件名称, MainValue as 值, ValueUnit as 单位 from assets right join lending on assets.AssetCode=lending.AssetCode join assetclasses on assets.ClassCode=assetclasses.ClassCode where lending.MemberCode='{Session["UserID"]}' and lending.TransactionCycleEnded=0; ");
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

    protected void Asset_gv_SelectedIndexChanged(object sender, EventArgs e) => LoadAssetData($"assets.AssetCode='{Asset_gv.SelectedRow.Cells[0].Text}'");

    protected void Asset_gv_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.background-color='#ffe5cc';";
            e.Row.Attributes["onmouseout"] = "this.style.background-color='none';";
            e.Row.ToolTip = "点击选择";
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(Asset_gv, "Select$" + e.Row.RowIndex);
        }
    }

    protected void Login_Jmp_bt_Click(object sender, EventArgs e)
    {
        if (Session["jmpStack"] == null) Session["jmpStack"] = new Stack<string>();
        ((Stack<string>)Session["jmpStack"]).Push(Request.RawUrl);
        Response.Redirect("Login_Reg.aspx");
    }

    protected void Asset_gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataSet ds=new DataSet();
        ds = (DataSet)this.ViewState["ds"];
        Asset_gv.DataSource = ds;
        Asset_gv.PageIndex = e.NewPageIndex;
        Asset_gv.DataBind();
    }

    protected void Return_bt_Click(object sender, EventArgs e)
    {
        var requirePerm = byte.Parse(svr.QuerySingle($"select AutoCplt from assetclasses right join assets on assets.ClassCode=assetclasses.ClassCode where AssetCode='{(Asset_gv.SelectedRow ?? Asset_gv.Rows[0]).Cells[0].Text}'").ToString()) == 0;
        if(!requirePerm || (int)Session["UserPerm"] < 3)
        {
            svr.cn.Open();
            // Can return
            using (MySqlTransaction transact = svr.cn.BeginTransaction())
            {
                if (svr.Execute($"UPDATE lending SET `Status`='Returned',DateReturned=NOW(),TransactionCycleEnded=1 WHERE TransactionCode='{ReturnCodeSel_ddl.SelectedValue.Substring(1, 12)}'", transact) != 1)
                {
                    dc.CreateAlert("请重试或检查数据", "error", Alerts_pn);
                    transact.Rollback();
                    return;
                }
                transact.Commit();
            }
            dc.CreateAlert("归还成功!", "success", Alerts_pn);
        }
        else dc.CreateAlert("操作权限不足", "error", Alerts_pn);
    }

    protected void TypeSel0_ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        InitiateSearch($"select AssetCode as 元件代码, AssetName as 元件名称, MainValue as 值 from assets where ClassCode Like '{TypeSel0_ddl.SelectedValue.Substring(0, 3)}%'");
        TypeSel1_ddl.Items.Clear();
        svr.QueryReader($"select ClassCode, ClassName from assetclasses where Length(ClassCode)=6 and ClassCode Like '{TypeSel0_ddl.SelectedValue.Substring(0, 3)}%';", (MySqlDataReader rd) =>
        {
            TypeSel1_ddl.Items.Add(rd.GetString(0).Substring(3) + rd.GetString(1));
            TypeSel1_ddl.Visible = true;
        }, () => TypeSel1_ddl.Visible = false);
    }

    protected void TypeSel1_ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        InitiateSearch($"select AssetCode as 元件代码, AssetName as 元件名称, MainValue as 值 from assets where ClassCode Like '{TypeSel0_ddl.SelectedValue.Substring(0, 3)}{TypeSel1_ddl.SelectedValue.Substring(0, 3)}%'");
        TypeSel2_ddl.Items.Clear();
        svr.QueryReader($"select ClassCode, ClassName from assetclasses where Length(ClassCode)=9 and ClassCode Like '{TypeSel0_ddl.SelectedValue.Substring(0, 3)}{TypeSel1_ddl.SelectedValue.Substring(0, 3)}%';", (MySqlDataReader rd) =>
        {
            TypeSel2_ddl.Items.Add(rd.GetString(0).Substring(6) + rd.GetString(1));
            TypeSel2_ddl.Visible = true;
        }, () => TypeSel2_ddl.Visible = false);
    }

    protected void TypeSel2_ddl_SelectedIndexChanged(object sender, EventArgs e) => InitiateSearch($"select AssetCode as 元件代码, AssetName as 元件名称, MainValue as 值 from assets where ClassCode='{TypeSel2_ddl.SelectedValue.Substring(0, 3)}'");

    protected void Borrowed_bt_Click(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            dc.CreateAlert("请登录", "error", Alerts_pn);
        }
        else
        {
            InitiateSearch($"select lending.AssetCode as 元件代码, AssetName as 元件名称, MainValue as 值 from assets right join lending on assets.AssetCode=lending.AssetCode join assetclasses on assets.ClassCode=assetclasses.ClassCode where lending.MemberCode={Session["UserID"]} and lending.Status = 'taken';");
        }
    }

    protected void ReturnCodeSel_ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        ReturnCodeSel_ddl.ToolTip = ReturnCodeSel_ddl.SelectedValue;
    }
}