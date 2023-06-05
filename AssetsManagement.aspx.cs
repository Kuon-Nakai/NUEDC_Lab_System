#define USE_ASYNC

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AssetsManagement : System.Web.UI.Page
{
    private readonly DynamicControls dc = new DynamicControls();
    MySqlSvr svr;
    private bool loggedIn = false;
    private int expiredCnt = 0;
    private Stack<Task> expireChecks = new Stack<Task>();

    #region Cross boundary fields...
    public string userId;
    public Task<object> TgtMail_tk;
    public string TgtMail;
    public string MailSubject;
    public string MailBody;
    #endregion

    protected async void Page_Load(object sender, EventArgs e)
    {
        Page.MaintainScrollPositionOnPostBack = true;

        #region Disable client only buttons...
        Del_tb.Attributes["href"] = "#";
        FlagAnomaly_bt.Attributes["href"] = "#";
        MailNotif_bt.Attributes["href"] = "#";
        #endregion

        if (Session["UserPerm"] == null || (int)Session["UserPerm"] > 1)
        {
            dc.CreateAlert("用户权限不足, 拒绝访问", "info", Alerts_pn);
            Response.Redirect("index.aspx", false);
        }
        Page.MaintainScrollPositionOnPostBack = true;
        if (!loggedIn && Session["UserID"] != null)
        {
            loggedIn = true;
            Login_Jmp_bt.Text = "已登录";
            userId = (string)Session["UserID"];
            //Borrow_tb.OnClientClick = "showBorrowConfirmPopup();";
            //Borrow_tb.Attributes["href"] = "#";
        }
        svr = new MySqlSvr(new MySqlConnection("server=127.0.0.1; database=nuedc; user id=notRoot; password=1234"));

        //------------------------------- POSTBACK HANDLING ABOVE --------------------------------------//

        if (IsPostBack) { return; }

        // Load data into class filter dropdowns
        TypeSel0_ddl.Items.Add("");
        TypeSel1_ddl.Items.Add("");
        TypeSel2_ddl.Items.Add("");
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
        //LoadAssetData($"assets.AssetCode='{Asset_gv.Rows[0].Cells[0].Text}'");

        TotalComp_lb.Text = (await tc)?.ToString();
        TotalEntries_lb.Text = (await te)?.ToString();
        TotalLent_lb.Text = (await tl)?.ToString();
        TotalReg_lb.Text = (await tr)?.ToString();
#else // not USE_ASYNC
        // Legacy code
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
        if(Asset_gv.Rows.Count == 0) dc.CreateAlert("没有符合条件的元件", "info", Alerts_pn);
        else Asset_gv.SelectRow(0);
    }
    private void InitiateLendSearch(string sql)
    {
        expiredCnt = 0;
        expireChecks.Clear();
        if (svr == null)
        {
            svr = new MySqlSvr("server=127.0.0.1; database=nuedc; user id=notRoot; password=1234");
        }
        ViewState["dsl"] = svr.QueryDataset(sql);
        LendState_gv.DataSource = ViewState["dsl"];
        LendState_gv.DataBind();
        svr.cn.Close();
        if (svr == null) svr = new MySqlSvr("server=127.0.0.1; database=nuedc; user id=notRoot; password=1234");
        foreach (GridViewRow row in LendState_gv.Rows)
        {
            if(row.RowType == DataControlRowType.DataRow &&
                row.Cells[3].Text.Equals("Taken") &&
                DateTime.Now.AddMonths(-1).CompareTo(svr.QuerySingle($"select DateProcessed from lending where TransactionCode='{row.Cells[0].Text}'") as DateTime?) > 0)
            {
                ++expiredCnt;
            }
        }
        if (expiredCnt > 0)
            dc.CreateAlert($"注意: 该元件有{expiredCnt}个借用记录已过期未归还!", "notice", Alerts_pn);
        else
            dc.CreateAlert("当前元件未检测到过期记录", "success", Alerts_pn);
    }

    public void LoadAssetData(string sql_where_assets)
    {
        if (svr == null) { svr = new MySqlSvr("server=127.0.0.1; database=nuedc; user id=notRoot; password=1234"); }
        int i = -1;
        svr.QueryReader($"select AssetName, ClassName, MainValue, ValueUnit, Location, Characteristics, Amount-ReservationQty-(select count(TransactionCode) from lending right join assets on lending.AssetCode=assets.AssetCode where {sql_where_assets} and Status='Returned'), AutoCplt from assets left join assetclasses on assets.ClassCode=assetclasses.ClassCode where {sql_where_assets};",
            AssignAssetData,
            /*() => Alert_DBQueryEmpty_pn.Visible = true*/
            () => dc.CreateAlert("元件信息查询失败: 返回信息为空", "error", Alerts_pn));
        svr.QueryReader($"select URL, Title from datasheets left join assets on datasheets.AssetCode=assets.AssetCode where {sql_where_assets}",
            (MySqlDataReader rd) =>
            {
                //Datasheet_pn1.Controls.Remove(Datasheet_lk);
                dc.CreateHTMLElement("br", Datasheet_pn0);
                dc.CreateHTMLElement("div", Datasheet_pn1);
                dc.CreateHyperLink(rd.GetValue(1).ToString(), rd.GetValue(0).ToString(), Datasheet_pn1);

                //Datasheet_pn1.Controls.RemoveAt(Datasheet_pn1.Controls.Count - 1);
                ++i;
            },
            () =>
            {
                //Datasheet_lk.Text = "暂无文档";
            });
        //LendState_gv.DataSource = null; 
        //LendState_gv.DataBind();
        InitiateLendSearch($"select TransactionCode, MemberCode, {(byte.Parse(svr.QuerySingle($"select AutoCplt from assetclasses right join assets on assets.ClassCode=assetclasses.ClassCode where {sql_where_assets};").ToString()) == 0 ? "FullCode" : "Qty" )}, Status from lending left join assets on assets.AssetCode=lending.AssetCode where " +
            $"{sql_where_assets};");
        if(LendState_gv.Rows.Count > 0)
            LendState_gv.SelectedIndex = 0;
    }

    private void AssignAssetData(MySqlDataReader rd)
    {
        AssetName_lb.Text = rd[0].ToString();
        AssetClass_lb.Text = rd[1].ToString();
        PrimValue_lb.Text = rd[2].ToString();
        Location_lb.Text = rd[4].ToString();
        Property_lb.Text = rd[5].ToString().Length > 0 ? rd[5].ToString() : "暂无数据";
        
        #region Legacy code...
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

        //Borrowable_lb.Text = rd[6]?.ToString();
        //if ((UInt64)rd[6] > 0)
        //{
        //    BorrowQtySel_tb.Enabled = true;
        //    //BorrowConfirm_pn.Visible = true;
        //    //BorrowNotAvailable_pn.Visible = false;
        //}
        //else
        //{
        //    BorrowQtySel_tb.Enabled = false;
        //    //BorrowConfirm_pn.Visible = false;
        //    //BorrowNotAvailable_pn.Visible = true;
        //}
#endregion
    }

    protected void Search_bt_Click(object sender, EventArgs e)
    {
        string sear_str = sear_tb.Text;
        InitiateSearch("select AssetCode as 元件代码, AssetName as 元件名称, MainValue as 值, ClassName as 分类 from assets left join assetclasses on assets.ClassCode = assetclasses.ClassCode where " +
            $"AssetCode='{sear_str}' or AssetName like '%{sear_str}%' or MainValue like '%{sear_str}%' and " +
            $"assets.ClassCode like '{TypeSel0_ddl.SelectedValue.Substring(0, Math.Min(TypeSel0_ddl.SelectedValue.Length, 3))}" +
            $"{TypeSel1_ddl.SelectedValue.Substring(0, Math.Min(TypeSel1_ddl.SelectedValue.Length, 3))}" +
            $"{TypeSel2_ddl.SelectedValue.Substring(0, Math.Min(TypeSel2_ddl.SelectedValue.Length, 3))}%';");
        LoadAssetData($"assets.AssetCode='{Asset_gv.Rows[0].Cells[0].Text}'");
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
        Asset_gv.DataSource = (DataSet)ViewState["ds"];
        Asset_gv.PageIndex = e.NewPageIndex;
        Asset_gv.DataBind();
    }

    protected void LendSearch_bt_Click(object sender, EventArgs e)
    {
        InitiateLendSearch($"select TransactionCode, MemberCode, {(byte.Parse(svr.QuerySingle($"select AutoCplt from assetclasses right join assets on assets.ClassCode=assetclasses.ClassCode where AssetCode='{(Asset_gv.SelectedRow ?? Asset_gv.Rows[0]).Cells[0].Text}'").ToString()) == 0 ? "FullCode" : "Qty" )}, Status from lending where " +
            $"AssetCode='{(Asset_gv.SelectedRow ?? Asset_gv.Rows[0]).Cells[0].Text}' and (" +
            $"TransactionCode like'%{LendSearch_tb.Text}%' or " +
            $"MemberCode='{LendSearch_tb.Text}' or " +
            $"Status like '%{LendSearch_tb.Text}%');"); // TODOs
    }

    protected void Locate_bt_Click(object sender, EventArgs e)
    {
        svr.Execute($"UPDATE assets set AssetName='{AssetName_lb}',ClassCode=(SELECT ClassCode FROM assetclasses WHERE ClassName='{AssetClass_lb}'),MainValue='{PrimValue_lb}' ,Location='{Location_lb}' ,Characteristics='{Property_lb}' ,Amount={Qty_tb} ,ReservationQty={RsrvQty_tb} ,LendingPolicy={AutoApproveLvl_tb};", (Exception ex) =>
        {
            dc.CreateAlert("更新错误，无法删除", "error", Alerts_pn);
            InitiateSearch("select AssetCode, AssetName, MainValue, ValueUnit from assets left join assetclasses on assets.ClassCode = assetclasses.ClassCode;");
        });
    }

    protected void ConfirmDel_bt_Click(object sender, EventArgs e)
    {
        svr.Execute($"DELETE FROM assets WHERE AssetCode='{Asset_gv.SelectedRow.Cells[0]}';", (Exception ex) =>
        {
            dc.CreateAlert("删除错误，无法删除", "error", Alerts_pn);
            InitiateSearch("select AssetCode, AssetName, MainValue, ValueUnit from assets left join assetclasses on assets.ClassCode = assetclasses.ClassCode;");
        });
    }

    protected void ConfirmReturn_bt_Click(object sender, EventArgs e)
    {
        svr.cn.Open();
        using (var transact = svr.cn.BeginTransaction())
        {
            if (svr.Execute("update lending set Status='已归还', TransactionCycleEnded=1 where TransactionCode=" + LendState_gv.SelectedRow.Cells[0].Text, transact) != 1)
            {
                transact.Rollback();
                dc.CreateAlert("归还操作异常，已回退", "error", Alerts_pn);
                return;
            }
            transact.Commit();
            dc.CreateAlert("归还成功!", "success", Alerts_pn);
        }
    }
    
    protected void LendState_gv_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.background-color='#ffe5cc';";
            e.Row.Attributes["onmouseout"] = "this.style.background-color='none';";
            e.Row.ToolTip = "点击选择";
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(LendState_gv, "Select$" + e.Row.RowIndex);
        }
    }

    protected void LendState_gv_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region Operation data preload...
        TgtMail = svr.QuerySingle($"select Email from members where MemberCode='{int.Parse(LendState_gv.SelectedRow.Cells[1].Text):D9}'").ToString();
        MailSubject = "[NUEDC实验室]元件归还提醒";
        
        if(DateTime.Today.AddMonths(-1) > ((DateTime)svr.QuerySingle($"select DateProcessed from lending where TransactionCode='{LendState_gv.SelectedRow.Cells[0].Text}';")))
        {
            // Expired!
            dc.CreateAlert("注意: 检测到该借出已过期", "notice", Alerts_pn);
            MailBody = $"{svr.QuerySingle($"select MemberName from members where MemberCode={LendState_gv.SelectedRow.Cells[1].Text}")}({LendState_gv.SelectedRow.Cells[1].Text})同学:\n" +
            $"\t您借用的元件 {svr.QuerySingle($"select AssetName from assets right join lending on lending.AssetCode=assets.AssetCode where lending.TransactionCode='{LendState_gv.SelectedRow.Cells[0].Text}';")} " +
            $"已在 {((DateTime)svr.QuerySingle($"select DateProcessed from lending where TransactionCode='{LendState_gv.SelectedRow.Cells[0].Text}';")).AddMonths(1)} " +
            "达到借出时限。根据相关规定, 请尽快前往实验室归还或续借, 感谢理解!\n\n\n" +
            "该邮件由NUEDC实验室综合服务平台发送\n" +
            $"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}";
        }
        else
        {
            MailBody = $"{svr.QuerySingle($"select MemberName from members where MemberCode={LendState_gv.SelectedRow.Cells[1].Text}")}({LendState_gv.SelectedRow.Cells[1].Text})同学:\n" +
                $"\t您借用的元件 {svr.QuerySingle($"select AssetName from assets right join lending on lending.AssetCode=assets.AssetCode where lending.TransactionCode='{LendState_gv.SelectedRow.Cells[0].Text}';")} " +
                $"将在 {((DateTime)svr.QuerySingle($"select DateProcessed from lending where TransactionCode='{LendState_gv.SelectedRow.Cells[0].Text}';")).AddMonths(1)} " +
                "达到借出时限。请在时限内前往实验室归还或续借, 感谢理解!\n\n\n" +
                "该邮件由NUEDC实验室综合服务平台发送\n" +
                $"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}";
        }
        OpBtn_pn.DataBind();
        ViewState["tgtmail"] = TgtMail;
        ViewState["mailsubject"] = MailSubject;
        ViewState["mailbody"] = MailBody;
        #endregion
    }

    protected void FlagSubmit_bt_Click(object sender, EventArgs e)
    {
        // TODO
    }

    protected void SendMail_bt_Click(object sender, EventArgs e)
    {
        try
        {
            MailSvr.SendMail(ViewState["tgtmail"].ToString(), ViewState["mailsubject"].ToString(), ViewState["mailbody"].ToString());
        }
        catch(Exception ex)
        {
            dc.CreateAlert("邮件发送失败:\n" + ex.Message, "error", Alerts_pn);
            return;
        }
        dc.CreateAlert("邮件成功发送!", "success", Alerts_pn);
    }

    protected void TypeSel0_ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        InitiateSearch($"select AssetCode as 元件代码, AssetName as 元件名称, MainValue as 值 from assets where ClassCode Like '{TypeSel0_ddl.SelectedValue.Substring(0, Math.Min(TypeSel0_ddl.SelectedValue.Length, 3))}%'");
        TypeSel1_ddl.Items.Clear();
        TypeSel1_ddl.Items.Add("");
        if (TypeSel0_ddl.SelectedIndex == 0)
        {
            TypeSel1_ddl.Visible = false;
            TypeSel1_ddl.Enabled = false;
            TypeSel1_ddl.SelectedIndex = 0;
            TypeSel2_ddl.Visible = false;
            TypeSel2_ddl.Enabled = false;
            TypeSel2_ddl.SelectedIndex = 0;
            return;
        }
        svr.QueryReader($"select ClassCode, ClassName from assetclasses where Length(ClassCode)=6 and ClassCode Like '{TypeSel0_ddl.SelectedValue.Substring(0, Math.Min(TypeSel0_ddl.SelectedValue.Length, 3))}%';", (MySqlDataReader rd) =>
        {
            TypeSel1_ddl.Items.Add(rd.GetString(0).Substring(3) + rd.GetString(1));
            TypeSel1_ddl.Visible = true;
            TypeSel1_ddl.Enabled = true;
            TypeSel1_ddl.SelectedIndex = 0;
            TypeSel2_ddl.Enabled = false;
            TypeSel2_ddl.SelectedIndex = 0;
        }, () => TypeSel1_ddl.Visible = false);
    }

    protected void TypeSel1_ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        InitiateSearch("select AssetCode as 元件代码, AssetName as 元件名称, MainValue as 值 from assets where " +
            $"ClassCode Like '{TypeSel0_ddl.SelectedValue.Substring(0, Math.Min(TypeSel0_ddl.SelectedValue.Length, 3))}" +
            $"{TypeSel1_ddl.SelectedValue.Substring(0, Math.Min(TypeSel1_ddl.SelectedValue.Length, 3))}%'");
        TypeSel2_ddl.Items.Clear();
        TypeSel2_ddl.Items.Add("");
        if (TypeSel1_ddl.SelectedIndex == 0)
        {
            TypeSel2_ddl.Visible = false;
            TypeSel2_ddl.Enabled = false;
            TypeSel2_ddl.SelectedIndex = 0;
            return;
        }
        svr.QueryReader($"select ClassCode, ClassName from assetclasses where " +
            $"Length(ClassCode)=9 and ClassCode Like '{TypeSel0_ddl.SelectedValue.Substring(0, Math.Min(TypeSel0_ddl.SelectedValue.Length, 3))}" +
            $"{TypeSel1_ddl.SelectedValue.Substring(0, Math.Min(TypeSel1_ddl.SelectedValue.Length, 3))}%';", (MySqlDataReader rd) =>
            {
                TypeSel2_ddl.Items.Add(rd.GetString(0).Substring(6) + rd.GetString(1));
                TypeSel2_ddl.Visible = true;
                TypeSel2_ddl.Enabled = true;
                TypeSel2_ddl.SelectedIndex = 0;
            }, () => TypeSel2_ddl.Visible = false);
    }

    protected void TypeSel2_ddl_SelectedIndexChanged(object sender, EventArgs e) => InitiateSearch($"select AssetCode as 元件代码, AssetName as 元件名称, MainValue as 值 from assets where " +
        $"ClassCode='{TypeSel2_ddl.SelectedValue.Substring(0, Math.Min(TypeSel2_ddl.SelectedValue.Length, 3))}'");
}