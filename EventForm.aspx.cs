using DocumentFormat.OpenXml.Packaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EventForm : System.Web.UI.Page
{
    private DynamicControls dc;
    private MySqlSvr svr = new MySqlSvr("server=127.0.0.1; database=nuedc; user id=notRoot; password=1234");
    private string EventCode;
    private Dictionary<string, FormFieldConfig> fields;

    protected void Page_Load(object sender, EventArgs e)
    {
        dc = new DynamicControls(Alerts_pn);
        EventCode = Session["EventCode"] as string;
        // Build dynamic form
        if (Session["UserID"] == null)
            dc.CreateAlert("报名表需登录填写", "notice");

        //-------------------- POSTBACK HANDLING ABOVE -----------------------//

        if(IsPostBack) return;
        // Verify if event signup is open
        if (DateTime.Now < (DateTime)svr.QuerySingle($"select DateReg from event where EventCode='{EventCode}'") ||
            DateTime.Now > (DateTime)svr.QuerySingle($"select DateRegEnd from event where EventCode='{EventCode}'"))
        {
            // Event not open
            dc.CreateAlert("当前不在报名允许时段", "error");
            FormSubmit_bt.Enabled = false;
            FormSubmit_bt.CssClass = "btn u-fullwidth";
            return;
        }
        FormSubmit_bt.Enabled = true;
        FormSubmit_bt.CssClass = "btn btn--primary u-fullwidth";

        // Fetch & render all fields
        fields = JsonConvert.DeserializeObject<Dictionary<string, FormFieldConfig>>(File.ReadAllText(Server.MapPath($"EventForms/{EventCode}.json")));
        foreach (var field in fields.Values)
            field.Render(Form_pn, (Control ctrl) =>
            {
                ctrl.Focus();
                dc.CreateAlert("该字段为必填字段", "notice");
            }, int.Parse(Session["UserID"] as string));
        Session["fields"] = fields;
        
    }

    protected void FormSubmit_bt_Click(object sender, EventArgs e)
    {
        // Verify if event signup is open
        if (DateTime.Now < new DateTime(2019, 5, 1))
        {
            // Event not open
            dc.CreateAlert("活动报名尚未开始", "error");
            return;
        }

        // Save all data into json
        // Verification is done by the dynamically generated components
        var storage = new Dictionary<string, Dictionary<string, string>>();
        var dataEntry = new Dictionary<string, string>();
        foreach(var field in (Session["fields"] as Dictionary<string, FormFieldConfig>).Values)
            dataEntry[field.Prompt] = field.GetValue();
        // Sync lock to prevent write conflict
        lock(this)
        {
            File.WriteAllText(Server.MapPath($"EventForms/{EventCode}_data.json"), JsonConvert.SerializeObject(
                JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(File.ReadAllText(Server.MapPath($"EventForms/{EventCode}_data.json")))
                [Session["UserID"].ToString()] = dataEntry));
        }
        dc.CreateAlert("信息已保存", "success");
    }

    protected void Login_Jmp_bt_Click(object sender, EventArgs e)
    {
        if (Session["jmpStack"] == null) Session["jmpStack"] = new Stack<string>();
        ((Stack<string>)Session["jmpStack"]).Push("Events.aspx");
        Response.Redirect("Login_Reg.aspx");
    }
    [Serializable]
    private class FormFieldConfig
    {
        [JsonRequired]
        public string Prompt;
        [JsonRequired]
        public string Input;
        [JsonRequired]
        public bool Required;
        public string Autofill;
        public List<string> Options;
        public int? Mode;
        [JsonIgnore]
        [NonSerialized]
        public Func<string> GetValue;
        public Panel Render(Control parent, Action<Control> VerifFailHandler, int mc)
        {
            var panel = new Panel();
            var prompt = new Label()
            {
                Text = Prompt
            };
            panel.Controls.Add(prompt);
            dynamic input;
            switch (Input)
            {
                case "textbox":
                    input = new TextBox()
                    {
                        Text = (Autofill?.StartsWith("=") ?? false) ? new MySqlSvr("server=127.0.0.1; database=nuedc; user id=notRoot; password=1234")
                            .QuerySingle($"select {Autofill.Substring(1)} from members where MemberCode={mc}").ToString()
                            : (Autofill ?? ""),
                        CssClass = "u-fullwidth",
                        TextMode = (TextBoxMode)Mode
                    };
                    if (Autofill == "=MemberCode")
                        input.Text = mc.ToString("D9");
                    GetValue = () =>
                    {
                        if(Required && input.Text.Trim().Length == 0)
                        {
                            VerifFailHandler?.Invoke(input);
                            return null;
                        }
                        return input.Text.Trim();
                    };
                    break;
                case "dropdown":
                    input = new DropDownList()
                    {
                        CssClass = "u-fullwidth"
                    };
                    foreach(var option in Options)
                        input.Items.Add(option);
                    GetValue = () =>
                    {
                        if (Required && input.SelectedValue == null)
                        {
                            VerifFailHandler?.Invoke(input);
                            return null;
                        }
                        return input.Text.Trim();
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Input field is invalid.");
            }
            panel.Controls.Add(input);
            parent.Controls.Add(panel);
            return panel;
        }

    }

    protected void Download_bt_Click(object sender, EventArgs e)
    {
        // TODO
    }
}
