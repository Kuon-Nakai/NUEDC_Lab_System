using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Security : System.Web.UI.Page
{
    //    Application["SessionData"] = new Queue<int>();
    //    Application["TotalQueriesData"] = new Queue<int>();
    //    Application["DBQueriesData"] = new Queue<int>();
    //    Application["DBExecData"] = new Queue<int>();
    //    Application["LendCntData"] = new Queue<int>();
    //    Application["UserCntData"] = new Queue<int>();

    private DynamicControls dc = new DynamicControls();
    private Queue<ChartData> SessionData = new Queue<ChartData>();
    private Queue<ChartData> TotalQueriesData = new Queue<ChartData>();
    private Queue<ChartData> DBQueriesData = new Queue<ChartData>();
    private Queue<ChartData> DBExecData = new Queue<ChartData>();
    private Queue<ChartData> LendCntData = new Queue<ChartData>();
    private Queue<ChartData> UserCntData = new Queue<ChartData>();

    #region Cross boundary variables...
    //public static Queue<int>    NodeCnt, TotalNodeCnt, WarnCnt, AnomalyCnt, SessionCnt, QueryCnt, UpdateCnt, AccCnt;
    //public static Queue<double> DataRxCnt, PowerCnt, PowerConsCnt;
    public string series = "[\'Main series\']";
    public string SessionResult;
    public string DBQueriesResult;
    public string DBExecResult;
    public string LendCntResult;
    public string TotalQueriesResult;
    public string UserCntResult;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        #region Disable postback...
        //SessionCnt_bt.Attributes["href"] = "#";
        #endregion

        if (Session["UserID"] != null)
        {
            Login_Jmp_bt.Text = "已登录";
        }
        dc.Restore();
        // Convert data to required JSON format
        int x = 0;
        SessionData.Clear();
        foreach (var s in Application["SessionData"] as Queue<int>)
            SessionData.Enqueue(new ChartData(x++.ToString(), s));
        SessionResult = JsonConvert.SerializeObject(SessionData, settings: new JsonSerializerSettings()).Replace("\"", "\'");
        foreach (var s in Application["TotalQueriesData"] as Queue<int>)
            TotalQueriesData.Enqueue(new ChartData(x++.ToString(), s));
        TotalQueriesResult = JsonConvert.SerializeObject(TotalQueriesData, settings: new JsonSerializerSettings()).Replace("\"", "\'");
        foreach (var s in Application["DBQueriesData"] as Queue<int>)
            DBQueriesData.Enqueue(new ChartData(x++.ToString(), s));
        DBQueriesResult = JsonConvert.SerializeObject(DBQueriesData, settings: new JsonSerializerSettings()).Replace("\"", "\'");
        foreach (var s in Application["DBExecData"] as Queue<int>)
            DBExecData.Enqueue(new ChartData(x++.ToString(), s));
        DBExecResult = JsonConvert.SerializeObject(DBExecData, settings: new JsonSerializerSettings()).Replace("\"", "\'");
        foreach (var s in Application["LendCntData"] as Queue<int>)
            LendCntData.Enqueue(new ChartData(x++.ToString(), s));
        LendCntResult = JsonConvert.SerializeObject(LendCntData, settings: new JsonSerializerSettings()).Replace("\"", "\'");
        foreach (var s in Application["UserCntData"] as Queue<int>)
            UserCntData.Enqueue(new ChartData(x++.ToString(), s));
        UserCntResult = JsonConvert.SerializeObject(UserCntData, settings: new JsonSerializerSettings()).Replace("\"", "\'");

        DataBind();
    }
    protected void Login_Jmp_bt_Click(object sender, EventArgs e)
    {
        if (Session["jmpStack"] == null) Session["jmpStack"] = new Stack<string>();
        ((Stack<string>)Session["jmpStack"]).Push("Events.aspx");
        Response.Redirect("Login_Reg.aspx");
    }

    [Serializable]
    public class ChartData
    {
        [JsonRequired]
        public string name;
        [JsonRequired]
        public string value;
        public ChartData(string name, object value)
        {
            this.name = name;
            this.value = value.ToString();
        }
    }
}