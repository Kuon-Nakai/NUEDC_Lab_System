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
    private DynamicControls dc = new DynamicControls();

    public static Queue<int>    NodeCnt, TotalNodeCnt, WarnCnt, AnomalyCnt, SessionCnt, QueryCnt, UpdateCnt, AccCnt;
    public static Queue<double> DataRxCnt, PowerCnt, PowerConsCnt;
    public string series = "[\'Main series\']";
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
        //test data
    }
    protected void Login_Jmp_bt_Click(object sender, EventArgs e)
    {
        if (Session["jmpStack"] == null) Session["jmpStack"] = new Stack<string>();
        ((Stack<string>)Session["jmpStack"]).Push("Events.aspx");
        Response.Redirect("Login_Reg.aspx");
    }

}