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
    private DynamicControls dc = new DynamicControls();
    MySqlSvr svr;
    bool loggedIn = false;
    public string userId;

    private bool Assert_Perm(byte limit)
    {
        if (Session["UserID"] == null) return true;
        Session["UserPerm"] = svr.QuerySingle($"select PermissionLevel from members where MemberCode={Session["UserID"]};");
        return (uint)Session["UserPerm"] > limit;
    }

    protected async void Page_Load(object sender, EventArgs e)
    {
        
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
        if (IsPostBack) { return; }
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
    }

    private void AssignAssetData(MySqlDataReader rd)
    {
        AssetName_lb.Text = (string)rd[0];
        AssetClass_lb.Text = (string)rd[1];
        PrimValue_lb.Text = (string)rd[2];
        Location_lb.Text = (string)rd[4];
        Property_lb.Text = rd[5].ToString().Length > 0 ? (string)rd[5] : "暂无数据";

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
        InitiateSearch($"select AssetCode as 元件代码, AssetName as 元件名称, MainValue as 值, ClassName as 分类 from assets left join assetclasses on assets.ClassCode = assetclasses.ClassCode where AssetCode='{sear_str}' or AssetName like '%{sear_str}%' or MainValue like '%{sear_str}%';");
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
        LendState_gv.DataSource = svr.QueryDataset($"select TransactionCode, MemberCode, {((byte)svr.QuerySingle($"select AutoCplt from assetclasses right join assets on assets.ClassCode=assetclasses=ClassCode where AssetCode='{(Asset_gv.SelectedRow ?? Asset_gv.Rows[0]).Cells[0].Text}'") == 0 ? "FullCode" : "Qty" )}, Status from lending where " +
            $"AssetCode='{(Asset_gv.SelectedRow ?? Asset_gv.Rows[0]).Cells[0].Text}' and (" +
            $"TransactionCode='{LendSearch_bt.Text}' or " +
            $"MemberCode='{LendSearch_bt.Text}' or " +
            $"Status like %'{LendSearch_bt.Text}'%;"); // TODOs
    }

    protected void Del_tb_Click(object sender, EventArgs e)
    {
        svr.Execute($"DELETE FROM assets WHERE AssetCode={Asset_gv.SelectedRow.Cells[0]}", (Exception ex) =>
        {
            dc.CreateAlert("删除错误，无法删除", "error", Alerts_pn);
        });
    }

    protected void Locate_bt_Click(object sender, EventArgs e)
    {
        svr.Execute($"UPDATE assets AssetName={AssetName_lb},ClassCode=(SELECT ClassCode FROM assetclasses WHERE ClassName={AssetClass_lb}),MainValue={PrimValue_lb} ,Location={Location_lb} ,Characteristics={Property_lb} ,Amount={Qty_tb} ,ReservationQty={RsrvQty_tb} ,LendingPolicy={AutoApproveLvl_tb};", (Exception ex) =>
        {
            dc.CreateAlert("删除错误，无法删除", "error", Alerts_pn);
        });
    }
}