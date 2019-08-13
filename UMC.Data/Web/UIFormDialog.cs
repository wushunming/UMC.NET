using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Specialized;

namespace UMC.Web
{
    /// <summary>
    /// 表单对话框
    /// </summary> 
    public class UIFormDialog : UIDialog
    {
        List<WebMeta> dataSrouce = new List<WebMeta>();// new POSMeta();
        /// <summary>
        /// 类型
        /// </summary>
        protected override string DialogType
        {
            get { return "Form"; }
        }
        /// <summary>
        /// 增加地址输入框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="Code"></param>
        /// <param name="defaultValue"></param>
        public WebMeta AddReceiver(string Code, string title, string defaultValue)
        {
            WebMeta v = new WebMeta();
            v["Title"] = title;
            //if (String.IsNullOrEmpty(address) == false)
            //    v["Address"] = address;
            //if (String.IsNullOrEmpty(receiver) == false)
            //    v["Receiver"] = receiver;
            v["DefaultValue"] = defaultValue;
            v["Type"] = "Receiver";
            v["Name"] = Code;
            this.dataSrouce.Add(v);
            return v;
        }
        public WebMeta CreateMenu(string text, string model, string cmd, string value)
        {
            var p = new WebMeta();
            p["model"] = model;
            if (String.IsNullOrEmpty(value) == false)
            {
                p["send"] = value;
            }
            p["text"] = text;
            p["cmd"] = cmd;
            return p;
        }
        public void Menu(string text, string model, string cmd, string value)
        {
            this.Menu(this.CreateMenu(text, model, cmd, value));
        }
        public void Menu(string text, string model, string cmd)
        {
            this.Menu(this.CreateMenu(text, model, cmd, ""));
        }
        public void Menu(params WebMeta[] menus)
        {
            this.Config.Put("menu", menus);
        }
        public WebMeta CreateMenu(string text, string model, string cmd, WebMeta param)
        {

            var p = new WebMeta();
            if (param != null)
            {
                p.Set("send", param);
            }
            p["model"] = model;
            p["text"] = text;
            p["cmd"] = cmd;
            return p;
        }
        public void Menu(string text, string model, string cmd, WebMeta param)
        {
            this.Menu(this.CreateMenu(text, model, cmd, param));
        }
        /// <summary>
        /// 增加非输入型组件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <param name="style"></param>
        public void Add(String type, WebMeta value, WebMeta format, UIStyle style)
        {
            this.dataSrouce.Add(new WebMeta().Put("Type", type).Put("value", value).Put("format", format).Put("style", style));

        }
        public void Add(UICell cell)
        {
            this.dataSrouce.Add(new WebMeta().Put("Type", cell.Type).Put("value", cell.Data).Put("format", cell.Format).Put("style", cell.Style));

        }
        /// <summary>
        /// 获取异步对话框的值
        /// </summary>
        /// <param name="asyncId">异步值Id</param>
        /// <param name="callback">对话框回调方法</param>
        /// <returns></returns>
        public static new WebMeta AsyncDialog(string asyncId, AsyncDialogCallback callback)
        {
            return GetAsyncValue(asyncId, false, callback, false) as WebMeta;
        }
        /// <summary>
        /// 增加地址输入框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="Code"></param>
        /// <param name="defaultValue"></param>
        public void AddArea(string title, string Code, string defaultValue)
        {
            //POSMeta v = new POSMeta();
            //v["Title"] = title;
            //v["DefaultValue"] = defaultValue;
            //v["Type"] = "Area";
            //v["Name"] = Code;
            //this.dataSrouce.Add(v);
            this.AddOption(title, Code, defaultValue, defaultValue).Put("Model", "Schedule", "Command", "Area");
        }
        public void AddSlider(string title, string Code, int defaultValue, int min, int max)
        {
            WebMeta v = new WebMeta();
            v["Title"] = title;
            v["DefaultValue"] = defaultValue.ToString();
            v["Type"] = "FieldSlider";
            v["Name"] = Code;
            v["Max"] = max.ToString();
            v["Min"] = min.ToString(); ;
            this.dataSrouce.Add(v);
        }
        public void AddSlider(string title, string Code, int defaultValue)
        {
            AddSlider(title, Code, defaultValue, 0, 100);
        }
        /// <summary>
        /// 增加地址输入框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="Code"></param>
        /// <param name="defaultValue"></param>
        public void AddAddress(string title, string Code, string defaultValue)
        {
            WebMeta v = new WebMeta();
            v["Title"] = title;
            v["DefaultValue"] = defaultValue;
            v["Type"] = "Address";
            v["Name"] = Code;
            this.dataSrouce.Add(v);
        }
        /// <summary>
        /// 增加电话输入框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="Code"></param>
        /// <param name="defaultValue"></param>
        public void AddPhone(string title, string Code, string defaultValue)
        {
            WebMeta v = new WebMeta();
            v["Title"] = title;
            v["DefaultValue"] = defaultValue;
            v["Type"] = "Number";
            v["Vtype"] = "Phone";
            v["Name"] = Code;
            this.dataSrouce.Add(v);
        }
        /// <summary>
        /// 增加数字输入框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="Code"></param>
        /// <param name="defaultValue"></param>
        public WebMeta AddNumber(string title, string Code, int? defaultValue)
        {
            WebMeta v = new WebMeta();
            v["Title"] = title;
            v["DefaultValue"] = defaultValue.ToString();
            v["Type"] = "Number";
            v["Name"] = Code;
            this.dataSrouce.Add(v);
            return v;
        }

        public WebMeta Add(String type, String Code, String title, String defaultValue)
        {
            WebMeta v = new WebMeta();
            v["Title"] = title;
            v["DefaultValue"] = defaultValue;//.ToString();
            v["Type"] = type;// "Number";
            v["Name"] = Code;
            this.dataSrouce.Add(v);
            return v;

        }
        /// <summary>
        /// 增加数字输入框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="Code"></param>
        /// <param name="defaultValue"></param>
        public void AddNumber(string title, string Code, decimal? defaultValue)
        {
            WebMeta v = new WebMeta();
            v["Title"] = title;
            v["DefaultValue"] = defaultValue.ToString();
            v["Type"] = "Number";
            v["Name"] = Code;
            this.dataSrouce.Add(v);
        }
        /// <summary>
        /// 增加数字输入框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="Code"></param>
        /// <param name="defaultValue"></param>
        public void AddNumber(string title, string Code, float? defaultValue)
        {
            WebMeta v = new WebMeta();
            v["Title"] = title;
            v["DefaultValue"] = defaultValue.ToString();
            v["Type"] = "Number";
            v["Name"] = Code;
            this.dataSrouce.Add(v);
        }
        /// <summary>
        /// 增加数字输入框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="Code"></param>
        /// <param name="defaultValue"></param>
        public WebMeta AddNumber(string title, string Code, string defaultValue = "")
        {
            WebMeta v = new WebMeta();
            v["Title"] = title;
            v["DefaultValue"] = defaultValue;
            v["Type"] = "Number";
            v["Name"] = Code;
            this.dataSrouce.Add(v);
            return v;
        }
        /// <summary>
        /// 增加确认提示
        /// </summary>
        /// <param name="caption"></param>
        public void AddConfirm(string caption)
        {
            this.AddConfirm(caption, null, null);
        }
        /// <summary>
        /// 增加确认提示
        /// </summary>
        /// <param name="caption"></param>
        public void AddConfirm(string caption, string name, string defaultValue)
        {
            WebMeta v = new WebMeta();
            //v["Title"] = title;
            v["Text"] = caption;
            v["Type"] = "Confirm";
            v["DefaultValue"] = defaultValue ?? "YES";
            v["Name"] = name ?? "CONFIRM_NAME";// WebADNuke.Utils.Utility.Parse62Encode(Guid.NewGuid().GetHashCode());
            this.dataSrouce.Add(v);
        }
        /// <summary>
        /// 增加提示，
        /// </summary>
        /// <param name="caption"></param>
        public void AddPrompt(string caption)
        {
            WebMeta v = new WebMeta();
            //v["Title"] = title;
            v["Text"] = caption;
            v["Type"] = "Prompt";
            v["Name"] = UMC.Data.Utility.Parse62Encode(Guid.NewGuid().GetHashCode());
            this.dataSrouce.Add(v);
        }
        /// <summary>
        /// 条码输入框，
        /// </summary>
        /// <param name="title"></param>
        /// <param name="Code"></param>
        /// <param name="defaultValue"></param>
        public WebMeta AddBarCode(string title, string Code, string defaultValue)
        {
            WebMeta v = new WebMeta();
            v["Title"] = title;
            v["DefaultValue"] = defaultValue;
            v["Type"] = "BarCode";
            v["Name"] = Code;
            this.dataSrouce.Add(v);
            return v;
            //this.dataSrouce.Set(Code, v);
        }
        /// <summary>
        /// 条码输入框，
        /// </summary>
        /// <param name="title"></param>
        /// <param name="code"></param>
        /// <param name="value"></param>
        public WebMeta AddOption(string title, string code, string value, String text)
        {
            WebMeta v = new WebMeta();
            v["Title"] = title;
            v.Put("Text", text).Put("DefaultValue", value);
            v["Type"] = "Option";
            v["Name"] = code;
            this.dataSrouce.Add(v);
            return v;
        }
        /// <summary>
        /// 文件上传，
        /// </summary>
        /// <param name="title"></param>
        /// <param name="Code"></param>
        /// <param name="defaultValue"></param>
        public WebMeta AddFile(string title, string Code, string defaultValue)
        {
            WebMeta v = new WebMeta();
            v["Title"] = title;
            v["Type"] = "File";
            v["Name"] = Code;
            if (String.IsNullOrEmpty(defaultValue) == false)
            {
                v["DefaultValue"] = defaultValue;
            }
            this.dataSrouce.Add(v);
            return v;
        }
        /// <summary>
        /// 文件上传，
        /// </summary>
        /// <param name="title"></param>
        /// <param name="Code"></param>
        /// <param name="defaultValue"></param>
        public WebMeta AddFiles(string title, string Code)
        {
            WebMeta v = new WebMeta();
            v["Title"] = title;
            v["Type"] = "Files";
            v["Name"] = Code;
            this.dataSrouce.Add(v);
            return v;
        }
        /// <summary>
        /// 文件上传，
        /// </summary>
        /// <param name="title"></param>
        /// <param name="Code"></param>
        /// <param name="defaultValue"></param>
        public WebMeta AddTextarea(string title, string Code, string defaultValue)
        {
            WebMeta v = new WebMeta();
            v["Title"] = title;
            v["Type"] = "Textarea";
            if (String.IsNullOrEmpty(defaultValue) == false)
            {
                v["DefaultValue"] = defaultValue;
            }
            v["Name"] = Code;
            this.dataSrouce.Add(v);
            return v;
        }
        /// <summary>
        /// 时间输入框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="code"></param>
        /// <param name="defaultValue"></param>
        public WebMeta AddDate(string title, string code, DateTime? defaultValue)
        {

            WebMeta v = new WebMeta();
            v["Title"] = title;
            v["Name"] = code;
            if (defaultValue.HasValue)
            {
                v["DefaultValue"] = defaultValue.Value.ToString("yyyy-MM-dd");
            }
            v["Type"] = "Date";
            this.dataSrouce.Add(v);
            return v;
        }
        /// <summary>
        /// 增加文本输入框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="Code"></param>
        /// <param name="defaultValue"></param>
        public WebMeta AddText(string title, string Code, string defaultValue = "")
        {
            WebMeta v = new WebMeta();
            v["Title"] = title;
            v["DefaultValue"] = defaultValue;
            v["Type"] = "Text";
            v["Name"] = Code;
            this.dataSrouce.Add(v);
            return v;
            //this.dataSrouce.Set(Code, v);
        }
        /// <summary>
        /// 增加字典对说明
        /// </summary>
        /// <param name="items"></param>
        public WebMeta AddTextValue(string title, ListItemCollection items)
        {
            WebMeta v = new WebMeta();
            if (String.IsNullOrEmpty(title) == false)
                v["Title"] = title;
            v.GetDictionary()["DataSource"] = items;

            v["Type"] = "TextValue";
            this.dataSrouce.Add(v);
            return v;
        }
        /// <summary>
        /// 增加字典对说明
        /// </summary>
        /// <param name="items"></param>
        public WebMeta AddTextValue(ListItemCollection items)
        {
            return this.AddTextValue(String.Empty, items);
        }
        /// <summary>
        /// 增加字典对说明
        /// </summary>
        /// <param name="items"></param>
        public WebMeta AddTextNameValue(ListItemCollection items)
        {
            return this.AddTextNameValue(String.Empty, items);
        }
        /// <summary>
        /// 增加字典对说明
        /// </summary>
        /// <param name="items"></param>
        public WebMeta AddTextNameValue(string title, ListItemCollection items)
        {
            WebMeta v = new WebMeta();
            if (String.IsNullOrEmpty(title) == false)
                v["Title"] = title;
            v.GetDictionary()["DataSource"] = items;

            v["Type"] = "TextNameValue";
            this.dataSrouce.Add(v);
            return v;
        }
        /// <summary>
        /// 增加字典对说明
        /// </summary>
        /// <param name="items"></param>
        public ListItemCollection AddTextNameValue(string title)
        {
            var t = new ListItemCollection();
            this.AddTextNameValue(title, t);
            return t;
        }
        /// <summary>
        /// 增加字典对说明
        /// </summary>
        /// <param name="items"></param>
        public ListItemCollection AddTextNameValue()
        {
            var t = new ListItemCollection();
            this.AddTextNameValue(t);
            return t;
        }
        /// <summary>
        /// 增加字典对说明
        /// </summary>
        /// <param name="items"></param>
        public ListItemCollection AddTextValue(string title)
        {
            var t = new ListItemCollection();
            this.AddTextValue(title, t);
            return t;
        }
        /// <summary>
        /// 增加字典对说明
        /// </summary>
        /// <param name="items"></param>
        public ListItemCollection AddTextValue()
        {
            var t = new ListItemCollection();
            this.AddTextValue(t);
            return t;
        }
        /// <summary>
        /// 增加一个图片
        /// </summary>
        /// <param name="src"></param>
        public void AddImage(Uri src)
        {
            WebMeta v = new WebMeta();
            v["Src"] = src.AbsoluteUri;//.ToString();
            v["Type"] = "Image";
            v["Name"] = UMC.Data.Utility.Parse62Encode(Guid.NewGuid().GetHashCode());
            this.dataSrouce.Add(v);
        }


        /// <summary>
        /// 增加密码输入框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="Code"></param>
        /// <param name="defaultValue"></param>
        public WebMeta AddPassword(string title, string Code, bool IsDisabledMD5)
        {
            WebMeta v = new WebMeta();
            v["Title"] = title;
            if (IsDisabledMD5)
            {
                v["IsDisabledMD5"] = "true";
            }
            else
            {
                v["Time"] = Data.Utility.TimeSpan().ToString();
            }
            v["Type"] = "Password";
            v["Name"] = Code;
            this.dataSrouce.Add(v);
            return v;
        }
        /// <summary>
        /// 增加密码输入框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="Code"></param>
        /// <param name="defaultValue"></param>
        public WebMeta AddPassword(string title, string Code, string defaultValue)
        {
            WebMeta v = new WebMeta();
            v["Title"] = title;
            v["DefaultValue"] = defaultValue;
            v["Time"] = Data.Utility.TimeSpan().ToString();
            v["Type"] = "Password";
            v["Name"] = Code;
            this.dataSrouce.Add(v);
            return v;
        }
        public WebMeta AddUI(String title, String name, string desc)
        {
            WebMeta v = new WebMeta();
            v["Title"] = title;
            v["DefaultValue"] = desc;
            v["Type"] = "UI";
            v["Name"] = name;// WebADNuke.Utils.Utility.Parse62Encode(Guid.NewGuid().GetHashCode());
            this.dataSrouce.Add(v);
            return v;

        }

        public WebMeta AddUI(String title, string desc)
        {
            return AddUI(title, UMC.Data.Utility.Parse62Encode(Guid.NewGuid().GetHashCode()), desc);
        }
        public WebMeta AddUIIcon(String icon, string title)
        {
            return AddUIIcon(icon, title, String.Empty, 0);
        }
        public WebMeta AddUIIcon(String icon, string title, int color)
        {
            return AddUIIcon(icon, title, String.Empty, color);
        }
        public WebMeta AddUIIcon(String icon, string title, string desc, int color)
        {
            WebMeta v = new WebMeta();
            v["Icon"] = icon;
            if (color != 0)
            {
                if (color < 0x1000)
                {
                    v["Color"] = String.Format("#{0:x3}", color);
                }
                else
                {
                    v["Color"] = String.Format("#{0:x6}", color);
                }
            }
            if (String.IsNullOrEmpty(title) == false)
                v["Title"] = title;
            if (String.IsNullOrEmpty(desc) == false)
                v["DefaultValue"] = desc;
            v["Type"] = "UI";
            v["Name"] = UMC.Data.Utility.Parse62Encode(Guid.NewGuid().GetHashCode());
            this.dataSrouce.Add(v);
            return v;
        }
        public WebMeta AddUIIcon(UIIcon icon, string title)
        {
            return AddUIIcon(icon, title, String.Empty, 0);
        }
        public WebMeta AddUIIcon(UIIcon icon, string title, string desc)
        {
            return AddUIIcon(icon, title, desc, 0);
        }
        public WebMeta AddUIIcon(UIIcon icon, string title, string desc, int color)
        {
            int index = (int)icon;
            if (icons.Length > index)
            {
                return AddUIIcon(icons[index], title, desc, color);
            }
            else
            {
                return AddUI(title, desc);
            }
        }

        /// <summary>
        /// 增加时间选择框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="code"></param>
        public WebMeta AddTime(string title, string code, int hour, int minute)
        {
            WebMeta v = new WebMeta();//"DataSource", ds.ToArray());
            v["Title"] = title;
            v["Type"] = "Time";
            v["DefaultValue"] = String.Format("{0:00}:{1:00}", hour, minute);
            v["Name"] = code;
            this.dataSrouce.Add(v);
            return v;

        }
        /// <summary>
        /// 增加时间选择框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="code"></param>
        public WebMeta AddTime(string title, string code, DateTime? defaultValue)
        {
            WebMeta v = new WebMeta();//"DataSource", ds.ToArray());
            v["Title"] = title;
            v["Type"] = "Time";
            if (defaultValue.HasValue)
            {
                v["DefaultValue"] = defaultValue.Value.ToString("HH:mm");
            }
            v["Name"] = code;
            this.dataSrouce.Add(v);
            return v;
        }

        /// <summary>
        /// 增加选择框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="Code"></param>
        /// <param name="items"></param>
        public ListItemCollection AddSelect(string title, string code)
        {
            ListItemCollection t = new ListItemCollection();
            AddSelect(title, code, t);
            return t;
        }
        /// <summary>
        /// 增加选择框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="code"></param>
        /// <param name="items"></param>
        public void AddSelect(string title, string code, ListItemCollection items)
        {
            WebMeta v = new WebMeta();
            v.GetDictionary()["DataSource"] = items;
            v["Title"] = title;
            v["Type"] = "Select";
            v["Name"] = code;
            this.dataSrouce.Add(v);
        }
        /// <summary>
        /// 增加复选框
        /// </summary>
        public ListItemCollection AddCheckBox(string title, string code, string defaultValue)
        {
            ListItemCollection t = new ListItemCollection();
            AddCheckBox(title, code, t, defaultValue);
            return t;
        }
        /// <summary>
        /// 增加复选框
        /// </summary>
        public ListItemCollection AddCheckBox(string title, string code)
        {
            ListItemCollection t = new ListItemCollection();
            AddCheckBox(title, code, t);
            return t;
        }
        public void AddCheckBox(string title, string code, ListItemCollection items, string defaultValue)
        {

            WebMeta v = new WebMeta();
            v.GetDictionary()["DataSource"] = items;
            v["Title"] = title;
            if (String.IsNullOrEmpty(defaultValue) == false)
            {
                v["DefaultValue"] = defaultValue;
            }
            v["Type"] = "CheckboxGroup";
            v["Name"] = code;
            this.dataSrouce.Add(v);
        }
        /// <summary>
        /// 增加复选框
        /// </summary>
        public void AddCheckBox(string title, string code, ListItemCollection items)
        {
            AddCheckBox(title, code, items, null);
        }
        public ListItemCollection AddRadio(string title, string code)
        {
            ListItemCollection t = new ListItemCollection();
            AddRadio(title, code, t);
            return t;
        }
        /// <summary>
        /// 增加单选择框
        /// </summary>
        public void AddRadio(string title, string code, ListItemCollection items)
        {
            WebMeta v = new WebMeta();
            v.GetDictionary()["DataSource"] = items;
            v["Title"] = title;
            v["Type"] = "RadioGroup";
            v["Name"] = code;
            this.dataSrouce.Add(v);

        }
        public void Submit(String btnName)
        {
            this.Config.Put("submit", btnName);
            this.dataSrouce[this.dataSrouce.Count - 1].Put("Submit", "YES");
        }
        public void Submit(String btnName, string model, string cmd, WebMeta param)
        {
            var p = new WebMeta();
            if (param != null && param.Count > 0)
            {
                p.Set("send", param);
            }
            p["model"] = model;
            p["cmd"] = cmd;
            if (String.IsNullOrEmpty(btnName) == false)
            {
                p["text"] = btnName;
            }

            this.Config.Set("submit", p);
            this.dataSrouce[this.dataSrouce.Count - 1].Put("Submit", "YES");

        }
        WebMeta submit;
        public void Submit(String btnName, string model, string cmd, params string[] colseEvent)
        {
            var p = new WebMeta();

            p["model"] = model;
            p["cmd"] = cmd;
            if (String.IsNullOrEmpty(btnName) == false)
            {
                p["text"] = btnName;
            }
            if (colseEvent.Length > 0)
            {
                this.Config.Put("CloseEvent", String.Join(",", colseEvent));
            }
            this.Config.Set("submit", p);

        }
        /// <summary>
        /// 不使用提交按钮
        /// </summary>
        public void HideSubmit()
        {
            this.Config.Put("submit", false);
        }
        /// <summary>
        /// 在最后一个控件下添加提交按钮
        /// </summary>
        public void Submit()
        {
            this.dataSrouce[this.dataSrouce.Count - 1].Put("Submit", "YES");
        }
        public WebMeta AddVerify(String title, String code, String placeholder)
        {

            WebMeta v = new WebMeta();

            v["Title"] = title;
            v["Type"] = "Verify";
            v["Name"] = code;
            v["placeholder"] = placeholder;
            this.dataSrouce.Add(v);
            return v;

        }
        public void Submit(string btnName, WebRequest request, params string[] colseEvent)
        {
            if (colseEvent.Length > 0)
            {
                this.Config.Put("CloseEvent", String.Join(",", colseEvent));
            }
            var pa = new WebMeta(request.Arguments.GetDictionary());

            submit = new WebMeta("model", request.Model, "cmd", request.Command, "text", btnName).Put("send", pa);
            Submit(btnName);
        }
        protected override void Initialization()
        {
            if (submit != null)
            {
                var send = submit.GetDictionary()["send"] as WebMeta;
                send.Put(UIDialog.KEY_DIALOG_ID, this.AsyncId);
                //submit._send !=
                this.Config.Put("submit", submit);
            }
            this.Config.Put("DataSource", dataSrouce);
        }
        public List<WebMeta> DataSource
        {
            get
            {
                return dataSrouce;
            }

        }
        static string[] icons = new String[]{
"\uf000",
"\uf001",
"\uf002",
"\uf003",
"\uf004",
"\uf005",
"\uf006",
"\uf007",
"\uf008",
"\uf009",
"\uf00a",
"\uf00b",
"\uf00c",
"\uf00d",
"\uf00e",
"\uf010",
"\uf011",
"\uf012",
"\uf013",
"\uf014",
"\uf015",
"\uf016",
"\uf017",
"\uf018",
"\uf019",
"\uf01a",
"\uf01b",
"\uf01c",
"\uf01d",
"\uf01e",
"\uf021",
"\uf022",
"\uf023",
"\uf024",
"\uf025",
"\uf026",
"\uf027",
"\uf028",
"\uf029",
"\uf02a",
"\uf02b",
"\uf02c",
"\uf02d",
"\uf02e",
"\uf02f",
"\uf030",
"\uf031",
"\uf032",
"\uf033",
"\uf034",
"\uf035",
"\uf036",
"\uf037",
"\uf038",
"\uf039",
"\uf03a",
"\uf03b",
"\uf03c",
"\uf03d",
"\uf03e",
"\uf040",
"\uf041",
"\uf042",
"\uf043",
"\uf044",
"\uf045",
"\uf046",
"\uf047",
"\uf048",
"\uf049",
"\uf04a",
"\uf04b",
"\uf04c",
"\uf04d",
"\uf04e",
"\uf050",
"\uf051",
"\uf052",
"\uf053",
"\uf054",
"\uf055",
"\uf056",
"\uf057",
"\uf058",
"\uf059",
"\uf05a",
"\uf05b",
"\uf05c",
"\uf05d",
"\uf05e",
"\uf060",
"\uf061",
"\uf062",
"\uf063",
"\uf064",
"\uf065",
"\uf066",
"\uf067",
"\uf068",
"\uf069",
"\uf06a",
"\uf06b",
"\uf06c",
"\uf06d",
"\uf06e",
"\uf070",
"\uf071",
"\uf072",
"\uf073",
"\uf074",
"\uf075",
"\uf076",
"\uf077",
"\uf078",
"\uf079",
"\uf07a",
"\uf07b",
"\uf07c",
"\uf07d",
"\uf07e",
"\uf080",
"\uf081",
"\uf082",
"\uf083",
"\uf084",
"\uf085",
"\uf086",
"\uf087",
"\uf088",
"\uf089",
"\uf08a",
"\uf08b",
"\uf08c",
"\uf08d",
"\uf08e",
"\uf090",
"\uf091",
"\uf092",
"\uf093",
"\uf094",
"\uf095",
"\uf096",
"\uf097",
"\uf098",
"\uf099",
"\uf09a",
"\uf09b",
"\uf09c",
"\uf09d",
"\uf09e",
"\uf0a0",
"\uf0a1",
"\uf0f3",
"\uf0a3",
"\uf0a4",
"\uf0a5",
"\uf0a6",
"\uf0a7",
"\uf0a8",
"\uf0a9",
"\uf0aa",
"\uf0ab",
"\uf0ac",
"\uf0ad",
"\uf0ae",
"\uf0b0",
"\uf0b1",
"\uf0b2",
"\uf0c0",
"\uf0c1",
"\uf0c2",
"\uf0c3",
"\uf0c4",
"\uf0c5",
"\uf0c6",
"\uf0c7",
"\uf0c8",
"\uf0c9",
"\uf0ca",
"\uf0cb",
"\uf0cc",
"\uf0cd",
"\uf0ce",
"\uf0d0",
"\uf0d1",
"\uf0d2",
"\uf0d3",
"\uf0d4",
"\uf0d5",
"\uf0d6",
"\uf0d7",
"\uf0d8",
"\uf0d9",
"\uf0da",
"\uf0db",
"\uf0dc",
"\uf0dd",
"\uf0de",
"\uf0e0",
"\uf0e1",
"\uf0e2",
"\uf0e3",
"\uf0e4",
"\uf0e5",
"\uf0e6",
"\uf0e7",
"\uf0e8",
"\uf0e9",
"\uf0ea",
"\uf0eb",
"\uf0ec",
"\uf0ed",
"\uf0ee",
"\uf0f0",
"\uf0f1",
"\uf0f2",
"\uf0a2",
"\uf0f4",
"\uf0f5",
"\uf0f6",
"\uf0f7",
"\uf0f8",
"\uf0f9",
"\uf0fa",
"\uf0fb",
"\uf0fc",
"\uf0fd",
"\uf0fe",
"\uf100",
"\uf101",
"\uf102",
"\uf103",
"\uf104",
"\uf105",
"\uf106",
"\uf107",
"\uf108",
"\uf109",
"\uf10a",
"\uf10b",
"\uf10c",
"\uf10d",
"\uf10e",
"\uf110",
"\uf111",
"\uf112",
"\uf113",
"\uf114",
"\uf115",
"\uf118",
"\uf119",
"\uf11a",
"\uf11b",
"\uf11c",
"\uf11d",
"\uf11e",
"\uf120",
"\uf121",
"\uf122",
"\uf123",
"\uf124",
"\uf125",
"\uf126",
"\uf127",
"\uf128",
"\uf129",
"\uf12a",
"\uf12b",
"\uf12c",
"\uf12d",
"\uf12e",
"\uf130",
"\uf131",
"\uf132",
"\uf133",
"\uf134",
"\uf135",
"\uf136",
"\uf137",
"\uf138",
"\uf139",
"\uf13a",
"\uf13b",
"\uf13c",
"\uf13d",
"\uf13e",
"\uf140",
"\uf141",
"\uf142",
"\uf143",
"\uf144",
"\uf145",
"\uf146",
"\uf147",
"\uf148",
"\uf149",
"\uf14a",
"\uf14b",
"\uf14c",
"\uf14d",
"\uf14e",
"\uf150",
"\uf151",
"\uf152",
"\uf153",
"\uf154",
"\uf155",
"\uf156",
"\uf157",
"\uf158",
"\uf159",
"\uf15a",
"\uf15b",
"\uf15c",
"\uf15d",
"\uf15e",
"\uf160",
"\uf161",
"\uf162",
"\uf163",
"\uf164",
"\uf165",
"\uf166",
"\uf167",
"\uf168",
"\uf169",
"\uf16a",
"\uf16b",
"\uf16c",
"\uf16d",
"\uf16e",
"\uf170",
"\uf171",
"\uf172",
"\uf173",
"\uf174",
"\uf175",
"\uf176",
"\uf177",
"\uf178",
"\uf179",
"\uf17a",
"\uf17b",
"\uf17c",
"\uf17d",
"\uf17e",
"\uf180",
"\uf181",
"\uf182",
"\uf183",
"\uf184",
"\uf185",
"\uf186",
"\uf187",
"\uf188",
"\uf189",
"\uf18a",
"\uf18b",
"\uf18c",
"\uf18d",
"\uf18e",
"\uf190",
"\uf191",
"\uf192",
"\uf193",
"\uf194",
"\uf195",
"\uf196",
"\uf197",
"\uf198",
"\uf199",
"\uf19a",
"\uf19b",
"\uf19c",
"\uf19d",
"\uf19e",
"\uf1a0",
"\uf1a1",
"\uf1a2",
"\uf1a3",
"\uf1a4",
"\uf1a5",
"\uf1a6",
"\uf1a7",
"\uf1a8",
"\uf1a9",
"\uf1aa",
"\uf1ab",
"\uf1ac",
"\uf1ad",
"\uf1ae",
"\uf1b0",
"\uf1b1",
"\uf1b2",
"\uf1b3",
"\uf1b4",
"\uf1b5",
"\uf1b6",
"\uf1b7",
"\uf1b8",
"\uf1b9",
"\uf1ba",
"\uf1bb",
"\uf1bc",
"\uf1bd",
"\uf1be",
"\uf1c0",
"\uf1c1",
"\uf1c2",
"\uf1c3",
"\uf1c4",
"\uf1c5",
"\uf1c6",
"\uf1c7",
"\uf1c8",
"\uf1c9",
"\uf1ca",
"\uf1cb",
"\uf1cc",
"\uf1cd",
"\uf1ce",
"\uf1d0",
"\uf1d1",
"\uf1d2",
"\uf1d3",
"\uf1d4",
"\uf1d5",
"\uf1d6",
"\uf1d7",
"\uf1d8",
"\uf1d9",
"\uf1da",
"\uf1db",
"\uf1dc",
"\uf1dd",
"\uf1de",
"\uf1e0",
"\uf1e1",
"\uf1e2",
"\uf1e3",
"\uf1e4",
"\uf1e5",
"\uf1e6",
"\uf1e7",
"\uf1e8",
"\uf1e9",
"\uf1ea",
"\uf1eb",
"\uf1ec",
"\uf1ed",
"\uf1ee",
"\uf1f0",
"\uf1f1",
"\uf1f2",
"\uf1f3",
"\uf1f4",
"\uf1f5",
"\uf1f6",
"\uf1f7",
"\uf1f8",
"\uf1f9",
"\uf1fa",
"\uf1fb",
"\uf1fc",
"\uf1fd",
"\uf1fe",
"\uf200",
"\uf201",
"\uf202",
"\uf203",
"\uf204",
"\uf205",
"\uf206",
"\uf207",
"\uf208",
"\uf209",
"\uf20a",
"\uf20b",
"\uf20c",
"\uf20d",
"\uf20e",
"\uf210",
"\uf211",
"\uf212",
"\uf213",
"\uf214",
"\uf215",
"\uf216",
"\uf217",
"\uf218",
"\uf219",
"\uf21a",
"\uf21b",
"\uf21c",
"\uf21d",
"\uf21e",
"\uf221",
"\uf222",
"\uf223",
"\uf224",
"\uf225",
"\uf226",
"\uf227",
"\uf228",
"\uf229",
"\uf22a",
"\uf22b",
"\uf22c",
"\uf22d",
"\uf230",
"\uf231",
"\uf232",
"\uf233",
"\uf234",
"\uf235",
"\uf236",
"\uf237",
"\uf238",
"\uf239",
"\uf23a",
"\uf23b",
"\uf23c",
"\uf23d",
"\uf23e",
"\uf240",
"\uf241",
"\uf242",
"\uf243",
"\uf244",
"\uf245",
"\uf246",
"\uf247",
"\uf248",
"\uf249",
"\uf24a",
"\uf24b",
"\uf24c",
"\uf24d",
"\uf24e",
"\uf250",
"\uf251",
"\uf252",
"\uf253",
"\uf254",
"\uf255",
"\uf256",
"\uf257",
"\uf258",
"\uf259",
"\uf25a",
"\uf25b",
"\uf25c",
"\uf25d",
"\uf25e",
"\uf260",
"\uf261",
"\uf262",
"\uf263",
"\uf264",
"\uf265",
"\uf266",
"\uf267",
"\uf268",
"\uf269",
"\uf26a",
"\uf26b",
"\uf26c",
"\uf26d",
"\uf26e",
"\uf270",
"\uf271",
"\uf272",
"\uf273",
"\uf274",
"\uf275",
"\uf276",
"\uf277",
"\uf278",
"\uf279",
"\uf27a",
"\uf27b",
"\uf27c",
"\uf27d",
"\uf27e",
"\uf280",
"\uf281",
"\uf282",
"\uf283",
"\uf284",
"\uf285",
"\uf286",
"\uf287",
"\uf288",
"\uf289",
"\uf28a",
"\uf28b",
"\uf28c",
"\uf28d",
"\uf28e",
"\uf290",
"\uf291",
"\uf292",
"\uf293",
"\uf294",
"\uf295",
"\uf296",
"\uf297",
"\uf298",
"\uf299",
"\uf29a",
"\uf29b",
"\uf29c",
"\uf29d",
"\uf29e",
"\uf2a0",
"\uf2a1",
"\uf2a2",
"\uf2a3",
"\uf2a4",
"\uf2a5",
"\uf2a6",
"\uf2a7",
"\uf2a8",
"\uf2a9",
"\uf2aa",
"\uf2ab",
"\uf2ac",
"\uf2ad",
"\uf2ae",
"\uf2b0",
"\uf2b1",
"\uf2b2",
"\uf2b3",
"\uf2b4",
"\uf2b5",
"\uf2b6",
"\uf2b7",
"\uf2b8",
"\uf2b9",
"\uf2ba",
"\uf2bb",
"\uf2bc",
"\uf2bd",
"\uf2be",
"\uf2c0",
"\uf2c1",
"\uf2c2",
"\uf2c3",
"\uf2c4",
"\uf2c5",
"\uf2c6",
"\uf2c7",
"\uf2c8",
"\uf2c9",
"\uf2ca",
"\uf2cb",
"\uf2cc",
"\uf2cd",
"\uf2ce",
"\uf2d0",
"\uf2d1",
"\uf2d2",
"\uf2d3",
"\uf2d4",
"\uf2d5",
"\uf2d6",
"\uf2d7",
"\uf2d8",
"\uf2d9",
"\uf2da",
"\uf2db",
"\uf2dc",
"\uf2dd",
"\uf2de",
"\uf2e0"};

    }
    public enum UIIcon
    {

        fa_glass = 0,
        fa_music,
        fa_search,
        fa_envelope_o,
        fa_heart,
        fa_star,
        fa_star_o,
        fa_user,
        fa_film,
        fa_th_large,
        fa_th,
        fa_th_list,
        fa_check,
        fa_times,
        fa_search_plus,
        fa_search_minus,
        fa_power_off,
        fa_signal,
        fa_cog,
        fa_trash_o,
        fa_home,
        fa_file_o,
        fa_clock_o,
        fa_road,
        fa_download,
        fa_arrow_circle_o_down,
        fa_arrow_circle_o_up,
        fa_inbox,
        fa_play_circle_o,
        fa_repeat,
        fa_refresh,
        fa_list_alt,
        fa_lock,
        fa_flag,
        fa_headphones,
        fa_volume_off,
        fa_volume_down,
        fa_volume_up,
        fa_qrcode,
        fa_barcode,
        fa_tag,
        fa_tags,
        fa_book,
        fa_bookmark,
        fa_print,
        fa_camera,
        fa_font,
        fa_bold,
        fa_italic,
        fa_text_height,
        fa_text_width,
        fa_align_left,
        fa_align_center,
        fa_align_right,
        fa_align_justify,
        fa_list,
        fa_outdent,
        fa_indent,
        fa_video_camera,
        fa_picture_o,
        fa_pencil,
        fa_map_marker,
        fa_adjust,
        fa_tint,
        fa_pencil_square_o,
        fa_share_square_o,
        fa_check_square_o,
        fa_arrows,
        fa_step_backward,
        fa_fast_backward,
        fa_backward,
        fa_play,
        fa_pause,
        fa_stop,
        fa_forward,
        fa_fast_forward,
        fa_step_forward,
        fa_eject,
        fa_chevron_left,
        fa_chevron_right,
        fa_plus_circle,
        fa_minus_circle,
        fa_times_circle,
        fa_check_circle,
        fa_question_circle,
        fa_info_circle,
        fa_crosshairs,
        fa_times_circle_o,
        fa_check_circle_o,
        fa_ban,
        fa_arrow_left,
        fa_arrow_right,
        fa_arrow_up,
        fa_arrow_down,
        fa_share,
        fa_expand,
        fa_compress,
        fa_plus,
        fa_minus,
        fa_asterisk,
        fa_exclamation_circle,
        fa_gift,
        fa_leaf,
        fa_fire,
        fa_eye,
        fa_eye_slash,
        fa_exclamation_triangle,
        fa_plane,
        fa_calendar,
        fa_random,
        fa_comment,
        fa_magnet,
        fa_chevron_up,
        fa_chevron_down,
        fa_retweet,
        fa_shopping_cart,
        fa_folder,
        fa_folder_open,
        fa_arrows_v,
        fa_arrows_h,
        fa_bar_chart,
        fa_twitter_square,
        fa_facebook_square,
        fa_camera_retro,
        fa_key,
        fa_cogs,
        fa_comments,
        fa_thumbs_o_up,
        fa_thumbs_o_down,
        fa_star_half,
        fa_heart_o,
        fa_sign_out,
        fa_linkedin_square,
        fa_thumb_tack,
        fa_external_link,
        fa_sign_in,
        fa_trophy,
        fa_github_square,
        fa_upload,
        fa_lemon_o,
        fa_phone,
        fa_square_o,
        fa_bookmark_o,
        fa_phone_square,
        fa_twitter,
        fa_facebook,
        fa_github,
        fa_unlock,
        fa_credit_card,
        fa_rss,
        fa_hdd_o,
        fa_bullhorn,
        fa_bell,
        fa_certificate,
        fa_hand_o_right,
        fa_hand_o_left,
        fa_hand_o_up,
        fa_hand_o_down,
        fa_arrow_circle_left,
        fa_arrow_circle_right,
        fa_arrow_circle_up,
        fa_arrow_circle_down,
        fa_globe,
        fa_wrench,
        fa_tasks,
        fa_filter,
        fa_briefcase,
        fa_arrows_alt,
        fa_users,
        fa_link,
        fa_cloud,
        fa_flask,
        fa_scissors,
        fa_files_o,
        fa_paperclip,
        fa_floppy_o,
        fa_square,
        fa_bars,
        fa_list_ul,
        fa_list_ol,
        fa_strikethrough,
        fa_underline,
        fa_table,
        fa_magic,
        fa_truck,
        fa_pinterest,
        fa_pinterest_square,
        fa_google_plus_square,
        fa_google_plus,
        fa_money,
        fa_caret_down,
        fa_caret_up,
        fa_caret_left,
        fa_caret_right,
        fa_columns,
        fa_sort,
        fa_sort_desc,
        fa_sort_asc,
        fa_envelope,
        fa_linkedin,
        fa_undo,
        fa_gavel,
        fa_tachometer,
        fa_comment_o,
        fa_comments_o,
        fa_bolt,
        fa_sitemap,
        fa_umbrella,
        fa_clipboard,
        fa_lightbulb_o,
        fa_exchange,
        fa_cloud_download,
        fa_cloud_upload,
        fa_user_md,
        fa_stethoscope,
        fa_suitcase,
        fa_bell_o,
        fa_coffee,
        fa_cutlery,
        fa_file_text_o,
        fa_building_o,
        fa_hospital_o,
        fa_ambulance,
        fa_medkit,
        fa_fighter_jet,
        fa_beer,
        fa_h_square,
        fa_plus_square,
        fa_angle_double_left,
        fa_angle_double_right,
        fa_angle_double_up,
        fa_angle_double_down,
        fa_angle_left,
        fa_angle_right,
        fa_angle_up,
        fa_angle_down,
        fa_desktop,
        fa_laptop,
        fa_tablet,
        fa_mobile,
        fa_circle_o,
        fa_quote_left,
        fa_quote_right,
        fa_spinner,
        fa_circle,
        fa_reply,
        fa_github_alt,
        fa_folder_o,
        fa_folder_open_o,
        fa_smile_o,
        fa_frown_o,
        fa_meh_o,
        fa_gamepad,
        fa_keyboard_o,
        fa_flag_o,
        fa_flag_checkered,
        fa_terminal,
        fa_code,
        fa_reply_all,
        fa_star_half_o,
        fa_location_arrow,
        fa_crop,
        fa_code_fork,
        fa_chain_broken,
        fa_question,
        fa_info,
        fa_exclamation,
        fa_superscript,
        fa_subscript,
        fa_eraser,
        fa_puzzle_piece,
        fa_microphone,
        fa_microphone_slash,
        fa_shield,
        fa_calendar_o,
        fa_fire_extinguisher,
        fa_rocket,
        fa_maxcdn,
        fa_chevron_circle_left,
        fa_chevron_circle_right,
        fa_chevron_circle_up,
        fa_chevron_circle_down,
        fa_html5,
        fa_css3,
        fa_anchor,
        fa_unlock_alt,
        fa_bullseye,
        fa_ellipsis_h,
        fa_ellipsis_v,
        fa_rss_square,
        fa_play_circle,
        fa_ticket,
        fa_minus_square,
        fa_minus_square_o,
        fa_level_up,
        fa_level_down,
        fa_check_square,
        fa_pencil_square,
        fa_external_link_square,
        fa_share_square,
        fa_compass,
        fa_caret_square_o_down,
        fa_caret_square_o_up,
        fa_caret_square_o_right,
        fa_eur,
        fa_gbp,
        fa_usd,
        fa_inr,
        fa_jpy,
        fa_rub,
        fa_krw,
        fa_btc,
        fa_file,
        fa_file_text,
        fa_sort_alpha_asc,
        fa_sort_alpha_desc,
        fa_sort_amount_asc,
        fa_sort_amount_desc,
        fa_sort_numeric_asc,
        fa_sort_numeric_desc,
        fa_thumbs_up,
        fa_thumbs_down,
        fa_youtube_square,
        fa_youtube,
        fa_xing,
        fa_xing_square,
        fa_youtube_play,
        fa_dropbox,
        fa_stack_overflow,
        fa_instagram,
        fa_flickr,
        fa_adn,
        fa_bitbucket,
        fa_bitbucket_square,
        fa_tumblr,
        fa_tumblr_square,
        fa_long_arrow_down,
        fa_long_arrow_up,
        fa_long_arrow_left,
        fa_long_arrow_right,
        fa_apple,
        fa_windows,
        fa_android,
        fa_linux,
        fa_dribbble,
        fa_skype,
        fa_foursquare,
        fa_trello,
        fa_female,
        fa_male,
        fa_gratipay,
        fa_sun_o,
        fa_moon_o,
        fa_archive,
        fa_bug,
        fa_vk,
        fa_weibo,
        fa_renren,
        fa_pagelines,
        fa_stack_exchange,
        fa_arrow_circle_o_right,
        fa_arrow_circle_o_left,
        fa_caret_square_o_left,
        fa_dot_circle_o,
        fa_wheelchair,
        fa_vimeo_square,
        fa_try,
        fa_plus_square_o,
        fa_space_shuttle,
        fa_slack,
        fa_envelope_square,
        fa_wordpress,
        fa_openid,
        fa_university,
        fa_graduation_cap,
        fa_yahoo,
        fa_google,
        fa_reddit,
        fa_reddit_square,
        fa_stumbleupon_circle,
        fa_stumbleupon,
        fa_delicious,
        fa_digg,
        fa_pied_piper_pp,
        fa_pied_piper_alt,
        fa_drupal,
        fa_joomla,
        fa_language,
        fa_fax,
        fa_building,
        fa_child,
        fa_paw,
        fa_spoon,
        fa_cube,
        fa_cubes,
        fa_behance,
        fa_behance_square,
        fa_steam,
        fa_steam_square,
        fa_recycle,
        fa_car,
        fa_taxi,
        fa_tree,
        fa_spotify,
        fa_deviantart,
        fa_soundcloud,
        fa_database,
        fa_file_pdf_o,
        fa_file_word_o,
        fa_file_excel_o,
        fa_file_powerpoint_o,
        fa_file_image_o,
        fa_file_archive_o,
        fa_file_audio_o,
        fa_file_video_o,
        fa_file_code_o,
        fa_vine,
        fa_codepen,
        fa_jsfiddle,
        fa_life_ring,
        fa_circle_o_notch,
        fa_rebel,
        fa_empire,
        fa_git_square,
        fa_git,
        fa_hacker_news,
        fa_tencent_weibo,
        fa_qq,
        fa_weixin,
        fa_paper_plane,
        fa_paper_plane_o,
        fa_history,
        fa_circle_thin,
        fa_header,
        fa_paragraph,
        fa_sliders,
        fa_share_alt,
        fa_share_alt_square,
        fa_bomb,
        fa_futbol_o,
        fa_tty,
        fa_binoculars,
        fa_plug,
        fa_slideshare,
        fa_twitch,
        fa_yelp,
        fa_newspaper_o,
        fa_wifi,
        fa_calculator,
        fa_paypal,
        fa_google_wallet,
        fa_cc_visa,
        fa_cc_mastercard,
        fa_cc_discover,
        fa_cc_amex,
        fa_cc_paypal,
        fa_cc_stripe,
        fa_bell_slash,
        fa_bell_slash_o,
        fa_trash,
        fa_copyright,
        fa_at,
        fa_eyedropper,
        fa_paint_brush,
        fa_birthday_cake,
        fa_area_chart,
        fa_pie_chart,
        fa_line_chart,
        fa_lastfm,
        fa_lastfm_square,
        fa_toggle_off,
        fa_toggle_on,
        fa_bicycle,
        fa_bus,
        fa_ioxhost,
        fa_angellist,
        fa_cc,
        fa_ils,
        fa_meanpath,
        fa_buysellads,
        fa_connectdevelop,
        fa_dashcube,
        fa_forumbee,
        fa_leanpub,
        fa_sellsy,
        fa_shirtsinbulk,
        fa_simplybuilt,
        fa_skyatlas,
        fa_cart_plus,
        fa_cart_arrow_down,
        fa_diamond,
        fa_ship,
        fa_user_secret,
        fa_motorcycle,
        fa_street_view,
        fa_heartbeat,
        fa_venus,
        fa_mars,
        fa_mercury,
        fa_transgender,
        fa_transgender_alt,
        fa_venus_double,
        fa_mars_double,
        fa_venus_mars,
        fa_mars_stroke,
        fa_mars_stroke_v,
        fa_mars_stroke_h,
        fa_neuter,
        fa_genderless,
        fa_facebook_official,
        fa_pinterest_p,
        fa_whatsapp,
        fa_server,
        fa_user_plus,
        fa_user_times,
        fa_bed,
        fa_viacoin,
        fa_train,
        fa_subway,
        fa_medium,
        fa_y_combinator,
        fa_optin_monster,
        fa_opencart,
        fa_expeditedssl,
        fa_battery_full,
        fa_battery_three_quarters,
        fa_battery_half,
        fa_battery_quarter,
        fa_battery_empty,
        fa_mouse_pointer,
        fa_i_cursor,
        fa_object_group,
        fa_object_ungroup,
        fa_sticky_note,
        fa_sticky_note_o,
        fa_cc_jcb,
        fa_cc_diners_club,
        fa_clone,
        fa_balance_scale,
        fa_hourglass_o,
        fa_hourglass_start,
        fa_hourglass_half,
        fa_hourglass_end,
        fa_hourglass,
        fa_hand_rock_o,
        fa_hand_paper_o,
        fa_hand_scissors_o,
        fa_hand_lizard_o,
        fa_hand_spock_o,
        fa_hand_pointer_o,
        fa_hand_peace_o,
        fa_trademark,
        fa_registered,
        fa_creative_commons,
        fa_gg,
        fa_gg_circle,
        fa_tripadvisor,
        fa_odnoklassniki,
        fa_odnoklassniki_square,
        fa_get_pocket,
        fa_wikipedia_w,
        fa_safari,
        fa_chrome,
        fa_firefox,
        fa_opera,
        fa_internet_explorer,
        fa_television,
        fa_contao,
        fa_500px,
        fa_amazon,
        fa_calendar_plus_o,
        fa_calendar_minus_o,
        fa_calendar_times_o,
        fa_calendar_check_o,
        fa_industry,
        fa_map_pin,
        fa_map_signs,
        fa_map_o,
        fa_map,
        fa_commenting,
        fa_commenting_o,
        fa_houzz,
        fa_vimeo,
        fa_black_tie,
        fa_fonticons,
        fa_reddit_alien,
        fa_edge,
        fa_credit_card_alt,
        fa_codiepie,
        fa_modx,
        fa_fort_awesome,
        fa_usb,
        fa_product_hunt,
        fa_mixcloud,
        fa_scribd,
        fa_pause_circle,
        fa_pause_circle_o,
        fa_stop_circle,
        fa_stop_circle_o,
        fa_shopping_bag,
        fa_shopping_basket,
        fa_hashtag,
        fa_bluetooth,
        fa_bluetooth_b,
        fa_percent,
        fa_gitlab,
        fa_wpbeginner,
        fa_wpforms,
        fa_envira,
        fa_universal_access,
        fa_wheelchair_alt,
        fa_question_circle_o,
        fa_blind,
        fa_audio_description,
        fa_volume_control_phone,
        fa_braille,
        fa_assistive_listening_systems,
        fa_american_sign_language_interpreting,
        fa_deaf,
        fa_glide,
        fa_glide_g,
        fa_sign_language,
        fa_low_vision,
        fa_viadeo,
        fa_viadeo_square,
        fa_snapchat,
        fa_snapchat_ghost,
        fa_snapchat_square,
        fa_pied_piper,
        fa_first_order,
        fa_yoast,
        fa_themeisle,
        fa_google_plus_official,
        fa_font_awesome,
        fa_handshake_o,
        fa_envelope_open,
        fa_envelope_open_o,
        fa_linode,
        fa_address_book,
        fa_address_book_o,
        fa_address_card,
        fa_address_card_o,
        fa_user_circle,
        fa_user_circle_o,
        fa_user_o,
        fa_id_badge,
        fa_id_card,
        fa_id_card_o,
        fa_quora,
        fa_free_code_camp,
        fa_telegram,
        fa_thermometer_full,
        fa_thermometer_three_quarters,
        fa_thermometer_half,
        fa_thermometer_quarter,
        fa_thermometer_empty,
        fa_shower,
        fa_bath,
        fa_podcast,
        fa_window_maximize,
        fa_window_minimize,
        fa_window_restore,
        fa_window_close,
        fa_window_close_o,
        fa_bandcamp,
        fa_grav,
        fa_etsy,
        fa_imdb,
        fa_ravelry,
        fa_eercast,
        fa_microchip,
        fa_snowflake_o,
        fa_superpowers,
        fa_wpexplorer,
        fa_meetup

    }
}
