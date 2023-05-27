using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MarkdownSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DocView : System.Web.UI.Page
{
    DynamicControls dc = new DynamicControls();
    public string filePath;
    public string fileType;
    protected void Page_Load(object sender, EventArgs e)
    {
        filePath = $"PublicFiles/{Request.QueryString["filePath"]}";
        fileType = Request.QueryString["fileType"];
        DataBind();
        MasterPage.col = .5f;
        Page.MaintainScrollPositionOnPostBack = true;
        if (Session["UserID"] != null)
        {
            Login_Jmp_bt.Text = "已登录";
        }
        try
        {
            // Verify privilege level if config exists
            var cfgFilePath = Server.MapPath($"PublicFiles/{filePath.Substring(0, filePath.LastIndexOf('/'))}/Config.json");
            PublicFileConfig cfg = null;
            if (File.Exists(cfgFilePath))
            {
                cfg = JsonConvert.DeserializeObject<PublicFileConfig>(File.ReadAllText(cfgFilePath));
                if (!cfg.Whitelist.Contains(Session["UserID"]) && (cfg.Blacklist.Contains(Session["UserID"]) || cfg.ClassificationLevel < (int)Session["UserPerm"]))
                {
                    dc.CreateAlert("用户权限不足", "info", Alerts_pn);
                    return;
                }
            }
            // Render content
            var txt = File.ReadAllText(Server.MapPath(filePath));
            string t;
            switch(fileType)
            {
                case "md":
                    t = new Markdown()
                    {
                        EmptyElementSuffix = "/>"
                    }.Transform(txt);
                    if ((!cfg?.Config?.Contains("NoBrReplace")) ?? true)
                    {
                        t = t.Replace("\\", "<br />");
                    }
                    if((!cfg?.Config?.Contains("NoHReplace")) ?? true)
                    {
                        t = t.Replace("h5", "h6")
                            .Replace("h4", "h5")
                            .Replace("h3", "h4")
                            .Replace("h2", "h3")
                            .Replace("h1", "h2");
                    }
                    break;
                case "pdf":
                    t = $"<embed src=\"{filePath}\" type=\"application/pdf\" frameBorder=\"0\" scrolling=\"auto\" height=\"1000px\" width=\"100%\"></embed>";
                    break;
                case "jpg":
                case "jpeg":
                case "png":
                    t = $"<img src=\"{filePath}\" />";
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
            dc.CreateAlert("请求的文件不存在", "error", Alerts_pn).CssClass += " u-fullwidth";
        }
        //catch (DirectoryNotFoundException)
        //{
        //    dc.CreateAlert("请求的路径无效", "error", Alerts_pn).CssClass += " u-fullwidth";
        //}
        catch (UnauthorizedAccessException)
        {
            dc.CreateAlert("拒绝访问: 请检查文件路径", "error", Alerts_pn).CssClass += " u-fullwidth";
        }
    }

    protected void Login_Jmp_bt_Click(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            if (Session["jmpStack"] == null) Session["jmpStack"] = new Stack<string>();
            ((Stack<string>)Session["jmpStack"]).Push(Request.RawUrl);
            Response.Redirect("Login_Reg.aspx");
        }
    }
    

    

    private class PublicFileConfig
    {
        [JsonRequired]
        public ushort ClassificationLevel;
        [JsonRequired]
        public List<string> Whitelist;
        [JsonRequired]
        public List<string> Blacklist;
        public List<string> Config;
    }
}