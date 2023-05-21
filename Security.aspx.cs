using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Security : System.Web.UI.Page
{
    public static DataSet NodeCnt, TotalNodeCnt, DataRxCnt, PowerCnt, PowerConsCnt, WarnCnt, AnomalyCnt;
    public static DataSet SessionCnt, QueryCnt, UpdateCnt, AccCnt;
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void Login_Jmp_bt_Click(object sender, EventArgs e)
    {

    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        if(SessionCnt == null)
        {
            DynamicControls.CreateAlert("数据不可用", "error", Alerts_pn);
            return;
        }
        if(SessionCnt.Tables.Count == 0)
        {
            DynamicControls.CreateAlert("暂无数据", "error", Alerts_pn);
            return;
        }
    }
}