using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Events : System.Web.UI.Page
{
    private DynamicControls dc = new DynamicControls();
    MySqlSvr svr;
    protected async void Page_Load(object sender, EventArgs e)
    {
        MasterPage.col = .5f;
        if (Session["UserID"] != null)
        {
            Login_Jmp_bt.Text = "已登录";
        }
        InitiateSearch("select EventCode as 活动代码, EventName as 活动名称, DateStart as 活动时间, DateReg as 报名时间, MaxParticipants as 最大组数 from event");
        svr.SafeOpen();
        var t_e = svr.Cmd("SELECT COUNT(*) FROM `event`;").ExecuteScalarAsync();
        var t_o = svr.Cmd("SELECT COUNT(*) FROM `event` WHERE DateReg<=NOW() AND DateRegEnd>=NOW();").ExecuteScalarAsync();
        var t_l = svr.Cmd("SELECT COUNT(*) FROM `event` WHERE DateEnd<=NOW();").ExecuteScalarAsync();
        TotalEntries_lb.Text = (await t_e)?.ToString();
        TotalEntries_lb.Text = (await t_e)?.ToString();
        TotalLent_lb.Text = (await t_l)?.ToString();

    }
    private void InitiateSearch(string sql)
    {
        if (svr == null)
        {
            svr = new MySqlSvr("server=127.0.0.1; database=nuedc; user id=notRoot; password=1234");
        }
        ViewState["ds"] = svr.QueryDataset(sql);
        Event_gv.DataSource = ViewState["ds"];
        Event_gv.DataBind();
        Event_gv.SelectRow(0);
    }
    protected void Login_Jmp_bt_Click(object sender, EventArgs e)
    {
        if (Session["jmpStack"] == null) Session["jmpStack"] = new Stack<string>();
        ((Stack<string>)Session["jmpStack"]).Push("Events.aspx");
        Response.Redirect("Login_Reg.aspx");
    }

    protected void Event_gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataSet ds = new DataSet();
        ds = (DataSet)this.ViewState["ds"];
        Event_gv.DataSource = ds;
        Event_gv.PageIndex = e.NewPageIndex;
        Event_gv.DataBind();
    }

    protected void Search_bt_Click(object sender, EventArgs e)
    {
        InitiateSearch($"select EventCode as 活动代码, EventName as 活动名称, DateStart as 活动时间, DateReg as 报名时间, MaxParticipants as 最大组数 from event where (EventName like '%{event_sea.Text}%' or EventCode like '%{event_sea.Text}%') and JoinLevel>={Session["USerPerm"]}");
    }

    public void LoadAssetData(string sql_where_assets)
    {
        if (svr == null) { svr = new MySqlSvr("server=127.0.0.1; database=nuedc; user id=notRoot; password=1234"); }
        svr.QueryReader($"select * from event where EventCode='{Event_gv.SelectedRow.Cells[0].Text}';",
            AssignAssetData,
            /*() => Alert_DBQueryEmpty_pn.Visible = true*/
            () => dc.CreateAlert("活动信息查询失败: 返回信息为空", "error", Alerts_pn));
    }

    private void AssignAssetData(MySqlDataReader rd)
    {
        EventName_lb.Text = (string)rd[1];
        EventTime_lb.Text = $"{(DateTime)rd[6]:yyyy-MM-dd HH:mm} --- {(DateTime)rd[7]:yyyy-MM-dd HH:mm}";
        Location_lb.Text =  rd[10].ToString().Length > 0 ? (string)rd[10] : "暂无数据";
        SignupTime_lb.Text = $"{(DateTime)rd[4]:yyyy-MM-dd HH:mm} --- {(DateTime)rd[5]:yyyy-MM-dd HH:mm}";
        MaxCount_lb.Text = $"{rd[8]}";
        EventDesc_lb.Text = rd[2].ToString().Length > 0 ? (string)rd[2] : "暂无数据";
        

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

        
    }
    protected void Event_gv_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.background-color='#ffe5cc';";
            e.Row.Attributes["onmouseout"] = "this.style.background-color='none';";
            e.Row.ToolTip = "点击选择";
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(Event_gv, "Select$" + e.Row.RowIndex);
        }
    }
    protected void Event_gv_SelectedIndexChanged(object sender, EventArgs e) => LoadAssetData($"assets.AssetCode='{Event_gv.SelectedRow.Cells[0].Text}'");

    protected void Signup_bt_Click(object sender, EventArgs e)
    {
        // Assert login state
        if (Session["UserID"] == null)
        {
            dc.CreateAlert("请登录", "notice", Alerts_pn);
            return;
        }

        // Assert form fillout state
        // FIXME: Potentially a critical issue with sync lock
        // Either use another method (work queue?) or move all synced code into a single location
        var EventCode = Event_gv.SelectedRow.Cells[0].Text;
        lock (this)
        {
            var obj = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(File.ReadAllText(Server.MapPath($"EventForms/{EventCode}_data.json")));
            if (obj.ContainsKey(Session["UserID"].ToString()))
            {
                dc.CreateAlert("已经填写过报名信息", "notice", Alerts_pn);
                return;
            }
        }

        Session["EventCode"] = Event_gv.SelectedRow.Cells[0].Text;
        Response.Redirect("EventForm.aspx");
    }

    protected void EditForm_bt_Click(object sender, EventArgs e)
    {
        // FIXME: Potentially a critical issue with sync lock
        // Either use another method (work queue?) or move all synced code into a single location
        var EventCode = Event_gv.SelectedRow.Cells[0].Text;
        lock (this)
        {
            var obj = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(File.ReadAllText(Server.MapPath($"EventForms/{EventCode}_data.json")));
            if (!obj.ContainsKey(Session["UserID"].ToString()))
            {
                dc.CreateAlert("没有填写过报名信息", "notice", Alerts_pn);
                return;
            }
        }

        Session["EventCode"] = EventCode;
        Response.Redirect("EventForm.aspx");
    }

    protected void Cancel_bt_Click(object sender, EventArgs e)
    {
        // FIXME: Potentially a critical issue with sync lock
        // Either use another method (work queue?) or move all synced code into a single location
        var EventCode = Event_gv.SelectedRow.Cells[0].Text;
        lock (this)
        {
            var obj = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(File.ReadAllText(Server.MapPath($"EventForms/{EventCode}_data.json")));
            if (!obj.ContainsKey(Session["UserID"].ToString()))
            {
                dc.CreateAlert("没有填写过报名信息", "notice", Alerts_pn);
                return;
            }
            // Execute delection & write back
            obj.Remove(Session["UserID"].ToString());
            File.WriteAllText(Server.MapPath($"EventForms/{EventCode}_data.json"), JsonConvert.SerializeObject(obj));
        }

    }
}