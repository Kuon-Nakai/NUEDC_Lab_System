using MarkdownSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DocView : System.Web.UI.Page
{
    DynamicControls dc;
    protected void Page_Load(object sender, EventArgs e)
    {
        dc = new DynamicControls();
        var filePath = Request.QueryString["filePath"];
        var fileType = Request.QueryString["fileType"];
        try
        {
            var txt = File.ReadAllText(Server.MapPath(filePath));
            string t;
            switch(fileType)
            {
                case "md":
                    t = new Markdown()
                    {
                        EmptyElementSuffix = "/>"
                    }.Transform(txt);
                    break;
                // ...
                case "txt":
                default:
                    t = txt;
                    break;
            }
            DocContent_lt.Text = t;
        }
        catch (FileNotFoundException) 
        {
            dc.CreateAlert("请求的文件不存在", "error", Alerts_pn);
        }
    }

    protected void Login_Jmp_bt_Click(object sender, EventArgs e)
    {
        
    }
}