using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    private Thread PathUpdThr;
    private bool RunPathBgUpd = true;
    public static float col = 0f;
    public static int txtOpa = 255;
    protected void Page_Load(object sender, EventArgs e)
    {
        PathUpdThr = new Thread(() =>
        {
            try
            {
                while (RunPathBgUpd)
                {
                    SiteMapPath_top.Parent.DataBind();
                    SiteMapPath_top.ForeColor = Color.FromArgb(txtOpa, 255, 255, 255);
                    Thread.Sleep(100);
                }
            }
            catch (ThreadAbortException) { }
        });
    }

    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {

    }

    protected void Page_Unload(object sender, EventArgs e)
    {
        PathUpdThr.Abort();
    }
}
