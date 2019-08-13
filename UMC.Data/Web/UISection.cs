using System;
using System.Collections.Generic;
using System.IO;
using UMC.Data;

namespace UMC.Web
{
    public class UITitle : UMC.Data.IJSON
    {
        public UITitle(String title)
        {
            meta.Put("text", title);
        }
        public UITitle Name(String name, string value)
        {
            meta.Put(name, value);
            return this;
        }
        private UITitle()
        {

        }
        public static UITitle Create()
        {
            return new UITitle();
        }
        public String Title
        {
            get
            {
                return meta.Get("text");
            }
            set
            {

                meta.Put("text", value);
            }
        }
        public UITitle Float()
        {
            meta.Put("float", true);
            return this;
        }
        public UITitle Left(char icon, UIClick click)
        {
            this.Left(new UIEventText().Icon(icon).Click(click));
            return this;

        }
        public UITitle Left(UIEventText text)
        {
            meta.Put("left", text);
            return this;
        }
        public UITitle Left(char icon, String click)
        {
            this.Left(new UIEventText().Icon(icon).Click(click));
            return this;
        }
        public UITitle Right(char icon, String click)
        {
            this.Right(new UIEventText().Icon(icon).Click(click));
            return this;
        }
        public UITitle Right(UIEventText text)
        {
            meta.Put("right", text);
            return this;
        }

        public UITitle Right(char icon, UIClick click)
        {
            this.Right(new UIEventText().Icon(icon).Click(click));
            return this;
        }
        public static UITitle TabTitle()
        {
            var t = new UITitle();
            t.meta.Put("type", "Tab");
            return t;
        }
        WebMeta meta = new WebMeta();
        void IJSON.Read(string key, object value)
        {

        }
        void IJSON.Write(System.IO.TextWriter writer)
        {
            UMC.Data.JSON.Serialize(this.meta, writer);

        }

    }
    public class UIItem : UMC.Data.IJSON
    {
        public static UIItem Create(WebMeta meta)
        {

            var item = new UIItem("", "");
            item.meta = meta;
            return item;
        }

        public UIItem Gradient(int startColor, int endColor)
        {

            if (startColor < 0x1000)
            {
                meta.Put("startColor", String.Format("#{0:x3}", startColor));
            }
            else
            {
                meta.Put("startColor", String.Format("#{0:x6}", startColor));
            }
            if (endColor < 0x1000)
            {
                meta.Put("endColor", String.Format("#{0:x3}", endColor));
            }
            else
            {
                meta.Put("endColor", String.Format("#{0:x6}", endColor));
            }
            return this;
        }
        public UIItem Style(UIStyle style)
        {

            meta.Put("style", style);
            return this;
        }
        public UIItem Click(UIClick click)
        {

            meta.Put("click", click);
            return this;
        }
        WebMeta meta = new WebMeta();
        public UIItem(string title, String desc)
        {
            meta.Put("title", title);
            meta.Put("desc", desc);

        }
        public UIItem Src(String url)
        {

            meta.Put("src", url);
            return this;
        }
        void IJSON.Read(string key, object value)
        {

        }
        void IJSON.Write(System.IO.TextWriter writer)
        {
            UMC.Data.JSON.Serialize(this.meta, writer);

        }

    }

    public class UIEventText : UMC.Data.IJSON
    {
        WebMeta meta = new WebMeta();
        public UIEventText Key(String key)
        {
            meta.Put("key", key);
            return this;
        }
        public UIEventText() { }
        public UIEventText Style(UIStyle style)
        {
            meta.Put("style", style);
            return this;

        }
        public UIEventText flex(float flex)
        {

            meta.Put("flex", flex);
            return this;
        }
        public UIEventText(String text)
        {
            meta.Put("text", text);
            meta.Put("format", "{text}");
        }
        public UIEventText Src(string src)
        {
            meta.Put("src", src);
            return this;

        }
        public UIEventText Icon(String icon, string color)
        {
            meta.Put("icon", icon);
            meta.Put("color", color);
            return this;
        }

        public UIEventText Icon(char icon, string color)
        {
            meta.Put("icon", icon);
            meta.Put("color", color);
            return this;

        }
        public UIEventText Icon(char icon, int color)
        {

            meta.Put("icon", icon);
            if (color < 0x1000)
            {
                meta.Put("color", String.Format("#{0:x3}", color));
            }
            else
            {
                meta.Put("color", String.Format("#{0:x6}", color));
            }
            return this;

        }
        public UIEventText Icon(char icon)
        {
            meta.Put("icon", icon);
            meta.Put("format", "{icon}");
            this.Style(new UIStyle().Name("icon", new UIStyle().Font("wdk").Size(20)));
            return this;
        }
        public UIEventText(char icon, String text)
        {
            meta.Put("icon", icon);
            meta.Put("text", text);
            meta.Put("format", "{icon}\n{text}");
            this.Style(new UIStyle().Size(8).Name("icon", new UIStyle().Font("wdk").Size(20)).Color(0x666));
        }
        public UIEventText Init(UIClick init)
        {
            meta.Put("init", init);
            return this;
        }
        public UIEventText Format(String format)
        {
            meta.Put("format", format);
            return this;

        }
        void IJSON.Read(string key, object value)
        {

        }
        void IJSON.Write(System.IO.TextWriter writer)
        {
            UMC.Data.JSON.Serialize(this.meta, writer);

        }


        public UIEventText Click(UIClick click)
        {
            meta.Put("click", click);
            return this;
        }
        public UIEventText Click(String click)
        {
            meta.Put("click", click);
            return this;
        }
    }
    public class UIHeader : UMC.Data.IJSON
    {


        /// <summary>
        /// 肖像模式
        /// </summary>
        public class Portrait : UMC.Data.IJSON
        {
            WebMeta meta = new WebMeta();
            public Portrait(String src)
            {
                meta.Put("src", src);

            }

            public Portrait()
            {

            }
            public Portrait Key(String Key)
            {
                meta.Put("key", Key);
                return this;

            }
            public Portrait Click(UIClick click)
            {

                meta.Put("click", click);
                return this;
            }
            public Portrait Title(String title)
            {
                meta.Put("title", title);
                return this;
            }
            public Portrait Time(String time)
            {
                meta.Put("time", time);
                return this;
            }

            public Portrait Value(String value)
            {
                meta.Put("value", value);
                return this;
            }
            public Portrait Desc(String desc)
            {
                meta.Put("desc", desc);
                return this;
            }
            void IJSON.Read(string key, object value)
            {

            }
            void IJSON.Write(System.IO.TextWriter writer)
            {
                UMC.Data.JSON.Serialize(this.meta, writer);

            }
            public Portrait Gradient(int startColor, int endColor)
            {

                if (startColor < 0x1000)
                {
                    meta.Put("startColor", String.Format("#{0:x3}", startColor));
                }
                else
                {
                    meta.Put("startColor", String.Format("#{0:x6}", startColor));
                }
                if (endColor < 0x1000)
                {
                    meta.Put("endColor", String.Format("#{0:x3}", endColor));
                }
                else
                {
                    meta.Put("endColor", String.Format("#{0:x6}", endColor));
                }
                return this;
            }
        }
        /// <summary>
        /// 小头像模式
        /// </summary>
        public class Profile : UMC.Data.IJSON
        {
            WebMeta meta = new WebMeta();
            public Profile(string name, String number, String src)
            {
                meta.Put("name", name);
                meta.Put("number", number);
                meta.Put("src", src);

            }
            public Profile(string name, String src)
            {
                meta.Put("name", name);
                meta.Put("src", src);

            }
            public Profile Account(string amount, String tip, String tag)
            {
                return Account(amount, tip, tag, null);
            }
            public Profile Account(UIClick click)
            {
                return Account(null, null, null, click);
            }
            public Profile Account(string amount, String tip, String tag, UIClick click)
            {
                var acount = new WebMeta().Put("amount", amount);
                if (String.IsNullOrEmpty(tip) == false)
                {
                    acount.Put("tag", tip);
                }

                if (String.IsNullOrEmpty(tip) == false)
                {
                    acount.Put("tag", tip);
                }

                if (click != null)
                {
                    acount.Put("click", click);
                }
                meta.Put("account", acount);
                return this;
            }
            public Profile Click(UIClick click)
            {

                this.meta.Put("click", click);
                return this;
            }
            void IJSON.Read(string key, object value)
            {

            }
            void IJSON.Write(System.IO.TextWriter writer)
            {
                UMC.Data.JSON.Serialize(this.meta, writer);

            }
            public Profile Gradient(int startColor, int endColor)
            {

                if (startColor < 0x1000)
                {
                    meta.Put("startColor", String.Format("#{0:x3}", startColor));
                }
                else
                {
                    meta.Put("startColor", String.Format("#{0:x6}", startColor));
                }
                if (endColor < 0x1000)
                {
                    meta.Put("endColor", String.Format("#{0:x3}", endColor));
                }
                else
                {
                    meta.Put("endColor", String.Format("#{0:x6}", endColor));
                }
                return this;
            }
            public void AddKey(params UIClick[] clicks)
            {
                if (clicks.Length > 0)
                    meta.Put("Keys", clicks);
            }
            public void AddKey(params string[] keys)
            {
                var Keys = new List<WebMeta>();
                for (int i = 0; i < keys.Length; i = i + 2)
                {
                    if (i + 1 < keys.Length)
                    {
                        Keys.Add(new WebMeta().Put("text", keys[i]).Put("value", keys[i + 1]));
                    }
                }
                meta.Put("Keys", Keys.ToArray());

            }
        }
        protected WebMeta meta = new WebMeta();
        public UIHeader Slider(System.Collections.IList sliders)
        {
            meta.Put("type", "Slider").Put("data", new WebMeta().Put("data", sliders));
            return this;
        }
        public UIHeader Slider(System.Data.DataTable sliders)
        {
            meta.Put("type", "Slider").Put("data", new WebMeta().Put("data", sliders));
            return this;
        }
        public UIHeader Slider(params UISlider[] sliders)
        {
            meta.Put("type", "Slider").Put("data", new WebMeta().Put("data", sliders));
            return this;
        }
        public UIHeader AddProfile(Profile profile, string numberFormat, string amountFormat)
        {

            meta.Put("type", "Profile").Put("data", profile).Put("format", new WebMeta().Put("number", numberFormat).Put("amount", amountFormat));
            return this;
        }
        public UIHeader AddProfile(Profile profile)
        {

            meta.Put("type", "Profile").Put("data", profile).Put("format", new WebMeta().Put("number", "{number}").Put("amount", "{amount}"));
            return this;
        }
        public UIHeader SliderSquare(params UISlider[] sliders)
        {
            meta.Put("type", "SliderSquare").Put("data", new WebMeta().Put("data", sliders));
            return this;
        }
        public UIHeader AddPortrait(Portrait discount)
        {
            meta.Put("type", "Portrait").Put("data", discount);
            return this;
        }

        public UIHeader Desc(WebMeta data, String format, UIStyle style)
        {

            this.meta.Put("type", "Desc").Put("style", style).Put("format", new WebMeta().Put("desc", format)).Put("data", data);
            return this;

        }
        public UIHeader Put(String key, object value)
        {
            meta.Put(key, value);
            return this;
        }
        public UIHeader Search(String placeholder)
        {

            meta.Put("type", "Search").Put("data", new WebMeta().Put("placeholder", placeholder));

            return this;
        }
        public UIHeader Search(String placeholder, String Keyword)
        {

            meta.Put("type", "Search").Put("data", new WebMeta().Put("Keyword", Keyword)).PlaceHolder(placeholder);

            return this;
        }

        void IJSON.Read(string key, object value)
        {

        }
        void IJSON.Write(System.IO.TextWriter writer)
        {
            UMC.Data.JSON.Serialize(this.meta, writer);

        }
    }
    public class UIFooter : UIHeader
    {
        List<object> _icons = new List<object>();
        List<object> _btons = new List<object>();
        public UIFooter AddCart()
        {
            _icons.Add("-");
            if (this.meta.ContainsKey("icons") == false)
            {
                this.meta.Put("icons", _icons);
            }
            return this;
        }
        public UIFooter AddText(params UIEventText[] text)
        {
            _btons.AddRange(text);
            if (this.meta.ContainsKey("buttons") == false)
            {
                this.meta.Put("buttons", _btons);
            }
            return this;

        }
        public UIFooter AddIcon(params UIEventText[] icons)
        {
            _icons.AddRange(icons);
            if (this.meta.ContainsKey("icons") == false)
            {
                this.meta.Put("icons", _icons);
            }
            return this;

        }

        public bool IsFixed
        {
            get
            {
                return this.meta.ContainsKey("fixed");
            }
            set
            {
                if (value)
                {
                    this.meta.Put("fixed", true);
                }
                else
                {
                    this.meta.Remove("fixed");

                }
            }
        }
    }
    //public class UIButton : UMC.Data.IJSON
    //{

    //    public UIButton(String text)
    //    {

    //        meta.Put("text", text);
    //    }
    //    private UIButton() { }
    //    public static UIButton Create(WebMeta icon)
    //    {
    //        var c = new UIButton();
    //        c.meta = icon;
    //        return c;
    //    }
    //    WebMeta meta = new WebMeta();

    //    public UIButton Click(UIClick click)
    //    {
    //        meta.Put("click", click);
    //        return this;

    //    }

    //    void IJSON.Read(string key, object value)
    //    {

    //    }
    //    void IJSON.Write(System.IO.TextWriter writer)
    //    {
    //        UMC.Data.JSON.Serialize(this.meta, writer);

    //    }
    //}

    public class UIPrice : UMC.Data.IJSON
    {
        public UIPrice(object id, String src)
        {

            meta.Put("id", id);
            meta.Put("src", src);
        }
        WebMeta meta = new WebMeta();
        public UIPrice Price(decimal? price)
        {
            meta.Put("price", price);
            return this;

        }

        public UIPrice Click(UIClick click)
        {
            meta.Put("click", click);
            return this;
        }
        public UIPrice Origin(decimal? price)
        {
            meta.Put("origin", price);
            return this;

        }
        public UIPrice Name(String name)
        {
            meta.Put("name", name);
            return this;

        }
        void IJSON.Read(string key, object value)
        {

        }
        void IJSON.Write(System.IO.TextWriter writer)
        {
            UMC.Data.JSON.Serialize(this.meta, writer);

        }

    }
    public class UIStyle : UMC.Data.IJSON
    {
        public static int[] Padding(WebMeta meta)
        {
            return Padding(meta["Padding"]);
        }
        public static int[] Padding(String padding)
        {
            if (String.IsNullOrEmpty(padding) == false)
            {
                var ids = new List<int>();
                var ps = padding.Split(' ');
                switch (ps.Length)
                {
                    case 1:
                        var t = Data.Utility.IntParse(ps[0], 0);
                        ids.Add(t);
                        ids.Add(t);
                        ids.Add(t);
                        ids.Add(t);
                        break;
                    case 2:

                        var t21 = Data.Utility.IntParse(ps[0], 0);
                        var t22 = Data.Utility.IntParse(ps[1], 0);
                        ids.Add(t21);
                        ids.Add(t22);
                        ids.Add(t21);
                        ids.Add(t22);
                        break;
                    case 3:
                        var t31 = Data.Utility.IntParse(ps[0], 0);
                        var t32 = Data.Utility.IntParse(ps[1], 0);
                        var t33 = Data.Utility.IntParse(ps[1], 0);
                        ids.Add(t31);
                        ids.Add(t32);
                        ids.Add(t33);
                        ids.Add(0);
                        break;
                    default:
                        ids.Add(Data.Utility.IntParse(ps[0], 0));
                        ids.Add(Data.Utility.IntParse(ps[1], 0));
                        ids.Add(Data.Utility.IntParse(ps[2], 0));
                        ids.Add(Data.Utility.IntParse(ps[3], 0));
                        break;
                }
                return ids.ToArray();
            }
            return new int[0] { };
        }
        WebMeta meta = new WebMeta();

        public UIStyle AlignLeft()
        {
            return Align(0);

        }
        public UIStyle()
        {

        }
        public UIStyle(WebMeta meta)
        {
            if (meta != null)
            {
                this.meta = meta;
            }
        }
        public void Copy(UIStyle style)
        {
            var dic = this.meta.GetDictionary();

            var em = style.meta.GetDictionary().GetEnumerator();
            while (em.MoveNext())
            {
                dic[em.Key] = em.Value;
            }

        }
        public UIStyle AlignCenter()
        {
            return Align(1);

        }
        public UIStyle Radius(int radius)
        {
            meta.Put("border-radius", radius);
            return this;
        }
        public UIStyle AlignRight()
        {
            return Align(2);

        }
        /// <summary>
        /// 文本对齐
        /// </summary>
        /// <param name="c">0为left,2为center,3为right</param>
        /// <returns></returns>
        public UIStyle Align(int c)
        {
            switch (c % 3)
            {
                default:
                    meta.Put("text-align", "left");
                    break;
                case 1:
                    meta.Put("text-align", "center");
                    break;
                case 2:
                    meta.Put("text-align", "right");
                    break;
            }
            return this;
        }
        public UIStyle Name(String key, string value)
        {
            meta.Put(key, value);
            return this;

        }
        public UIStyle Name(String key, int value)
        {

            meta.Put(key, value);
            return this;
        }
        public UIStyle Bold()
        {

            meta.Put("font-weight", "bold");
            return this;
        }
        //public UIStyle Weight(int weight)
        //{
        //    meta.Put("font-weight", weight);
        //    return this;

        //}
        public UIStyle Height(int height)
        {

            meta.Put("height", height);
            return this;
        }
        public UIStyle Padding(params int[] padding)
        {

            switch (padding.Length)
            {
                case 0:
                    break;
                case 1:
                    meta.Put("padding", String.Format("{0} {0} {0} {0}", padding[0]));
                    break;
                case 2:
                case 3:
                    meta.Put("padding", String.Format("{0} {1} {0} {1}", padding[0], padding[1]));
                    break;
                case 4:
                    meta.Put("padding", String.Format("{0} {1} {2} {3}", padding[0], padding[1], padding[2], padding[3]));
                    break;
            }

            return this;
        }
        public UIStyle Font(string c)
        {
            meta.Put("font", c);
            return this;
        }
        public UIStyle Name(String key, UIStyle style)
        {
            meta.Put(key, style);
            return this;

        }
        public UIStyle Name(String key)
        {
            var style = new UIStyle();
            meta.Put(key, style);
            return style;

        }
        public UIStyle BgColor()
        {
            return BgColor(0xef4f4f);
        }
        public UIStyle BgColor(int color)
        {

            meta.Put("background-color", ToColor(color));

            return this;
        }
        public static string ToColor(int color)
        {

            if (color < 0x1000)
            {
                return String.Format("#{0:x3}", color);
            }
            else
            {
                return String.Format("#{0:x6}", color);
            }
        }


        public UIStyle Color(int color)
        {

            meta.Put("color", ToColor(color));

            return this;
        }
        public UIStyle BorderColor(int color)
        {

            meta.Put("border-color", ToColor(color));

            return this;
        }


        public UIStyle UnderLine()
        {

            meta.Put("text-decoration", "underline");
            return this;
        }
        public UIStyle DelLine()
        {

            meta.Put("text-decoration", "line-through");
            return this;
        }
        public UIStyle Size(int size)
        {
            meta.Put("font-size", size.ToString());
            return this;
        }
        public UIStyle Click(UIClick click)
        {
            meta.Put("click", click);
            return this;

        }
        void IJSON.Read(string key, object value)
        {

        }
        void IJSON.Write(System.IO.TextWriter writer)
        {
            UMC.Data.JSON.Serialize(this.meta, writer);

        }
    }
    public class UISlider : IJSON
    {
        public static UICell Create(params UISlider[] sliders)
        {
            return UICell.Create("Slider", new WebMeta().Put("data", sliders));
        }
        public String Src
        {
            get; set;
        }
        public UIClick Click { get; set; }
        void IJSON.Read(string key, object value)
        {

        }
        void IJSON.Write(System.IO.TextWriter writer)
        {
            writer.Write("{");
            if (String.IsNullOrEmpty(this.Src) == false)
            {
                UMC.Data.JSON.Serialize("src", writer); writer.Write(":");
                UMC.Data.JSON.Serialize(this.Src, writer);
                if (Click != null)
                    writer.Write(",");
            }
            if (Click != null)
            {
                UMC.Data.JSON.Serialize("click", writer); writer.Write(":");
                UMC.Data.JSON.Serialize(Click, writer);

            }
            writer.Write("}");
        }
    }



    public abstract class UICell
    {
        private class UICeller : UICell
        {
            WebMeta data;
            public override object Data => data;
            public UICeller(WebMeta data)
            {
                this.data = data;
            }
        }

        public static UICell Create(String type, WebMeta data)
        {
            var celler = new UICeller(data);
            celler.Type = type;
            return celler;
        }
        String _type;
        public string Type
        {
            get
            {
                return _type;

            }
            protected set
            {
                _type = value;
            }
        }
        WebMeta _format = new WebMeta();
        public WebMeta Format
        {
            get
            {
                return _format;

            }
        }

        UIStyle _style = new UIStyle();


        public UIStyle Style
        {
            get
            {
                return _style;

            }
        }
        public abstract object Data
        {
            get;
        }
    }

    public class UISection : UMC.Data.IJSON
    {

        public class Editer
        {
            UMC.Web.WebMeta webMeta = new WebMeta();
            public Editer(int section, int row)
            {

                webMeta.Put("section", section).Put("row", row);

            }
            public Editer(String section, String row)
            {

                webMeta.Put("section", Utility.IntParse(section, 0)).Put("row", Utility.IntParse(row, 0));

            }
            public Editer Put(UICell value, bool reloadSinge)
            {
                if (reloadSinge)
                {
                    webMeta.Put("value", new WebMeta().Cell(value)).Put("method", "PUT").Put("reloadSinle", true);
                }
                else
                {
                    webMeta.Put("value", new WebMeta().Cell(value)).Put("method", "PUT");//.Put("reloadSinle", true);

                }
                return this;
            }
            public Editer Delete()
            {
                webMeta.Put("method", "DEL");
                return this;
            }
            public Editer Append(UICell value)
            {
                webMeta.Put("value", new WebMeta().Cell(value)).Put("method", "APPEND");
                return this;
            }
            public Editer Insert(UICell value)
            {
                webMeta.Put("value", new WebMeta().Cell(value)).Put("method", "INSERT");
                return this;
            }
            public void Builder(WebContext context, String ui, bool endResponse)
            {
                context.Send(new UMC.Web.WebMeta().UIEvent("UI.Edit", ui, webMeta), true);

            }
        }
        WebMeta _header = new WebMeta();
        private UISection()
        {

        }
        public WebMeta Header
        {
            get
            {
                return _header;
            }
        }
        UIHeader _uiheaders;
        UIFooter _uifooter;
        UITitle _title;
        public static UISection Create(UIHeader header, UIFooter footer)
        {
            var t = new UISection();
            t.Sections = new List<UISection>();
            t.Sections.Add(t);
            WebMeta meta = new WebMeta();
            t._uiheaders = header;
            t._uifooter = footer;
            return t;
        }


        public UISection AddNumberCell(string text, String value, Web.UIClick submit)
        {
            var cell = UICell.Create("NumberCell", new UMC.Web.WebMeta().Put("text", text, "value", value).Put("submit", submit));
            this.Add(cell);
            return this;
        }
        public UISection AddNumberCell(string text, String value, string title, Web.UIClick submit)
        {
            var cell = UICell.Create("NumberCell", new UMC.Web.WebMeta().Put("text", text, "value", value, "title", title).Put("submit", submit));
            this.Add(cell);
            return this;
        }
        public UISection AddImageTextValue(string src, String value, Web.UIClick click)
        {
            var cell = UICell.Create("ImageTextValue", new UMC.Web.WebMeta().Put("src", src, "value", value).Put("click", click));
            this.Add(cell);
            return this;
        }
        public UISection AddImageTextValue(string src, string text, String value, Web.UIClick click)
        {
            var cell = UICell.Create("ImageTextValue", new UMC.Web.WebMeta().Put("src", src, "value", value, "text", text).Put("click", click));
            this.Add(cell);
            return this;
        }
        public UISection AddImageTextValue(string src, String value, int imageWidth, Web.UIClick click)
        {
            var cell = UICell.Create("ImageTextValue", new UMC.Web.WebMeta().Put("src", src, "value", value).Put("click", click));
            cell.Style.Name("image-width", imageWidth);
            this.Add(cell);
            return this;
        }
        public UISection AddImageTextValue(string src, string text, String value, int imageWidth, Web.UIClick click)
        {
            var cell = UICell.Create("ImageTextValue", new UMC.Web.WebMeta().Put("src", src, "value", value, "text", text).Put("click", click));
            cell.Style.Name("image-width", imageWidth);
            this.Add(cell);
            return this;
        }

        public static UISection Create(UIHeader header)
        {
            return Create(header, null, null);
        }
        public UITitle Title
        {
            set
            {
                _title = value;
            }
            get
            {
                return _title;
            }
        }
        public UIHeader UIHeader
        {
            set
            {
                _uiheaders = value;
            }
            get
            {
                return _uiheaders;
            }

        }
        public UIFooter UIFooter
        {
            set
            {
                _uifooter = value;
            }
            get
            {
                return _uifooter;
            }
        }
        public static UISection Create(UIHeader header, UIFooter footer, UITitle title)
        {
            var t = new UISection();
            t.Sections = new List<UISection>();
            t.Sections.Add(t);
            WebMeta meta = new WebMeta();
            t._uiheaders = header;
            t._uifooter = footer;
            t._title = title;
            return t;
        }
        public static UISection Create(UITitle title, UIFooter footer)
        {
            var t = new UISection();
            t.Sections = new List<UISection>();
            t.Sections.Add(t);
            WebMeta meta = new WebMeta();
            t._uifooter = footer;
            t._title = title;
            return t;
        }
        public static UISection Create(UITitle title)
        {
            var t = new UISection();
            t.Sections = new List<UISection>();
            t.Sections.Add(t);
            t._title = title;
            WebMeta meta = new WebMeta();
            return t;


        }
        public static UISection Create(UIHeader header, UITitle title)
        {
            var t = new UISection();
            t.Sections = new List<UISection>();
            t.Sections.Add(t);
            t._title = title;
            WebMeta meta = new WebMeta();
            t._uiheaders = header;
            return t;


        }
        public int Total
        {
            get; set;
        }
        public static UISection Create()
        {
            var t = new UISection();
            t.Sections = new List<UISection>();
            t.Sections.Add(t);
            return t;

        }
        public String Key
        {
            get;
            set;
        }

        public bool IsEditer
        {
            get;
            set;
        }
        List<UISection> Sections;
        Object _data;
        List<WebMeta> data = new List<WebMeta>();

        public UISection NewSection()
        {
            var t = new UISection();
            t.Sections = this.Sections;
            this.Sections.Add(t);
            return t;
        }
        public int Count
        {
            get
            {
                return this.Sections.Count;
            }
        }
        public UISection NewSection(Array data)
        {
            var t = new UISection();
            t.Sections = this.Sections;
            this.Sections.Add(t);
            t._data = data;
            return t;
        }
        public UISection AddCells(params WebMeta[] data)
        {

            this.data.AddRange(data);
            return this;
        }
        public UISection AddCells(params UICell[] cells)
        {
            foreach (var e in cells)
                this.Add(e);
            return this;
        }
        public int Length
        {
            get
            {
                return data.Count;
            }
        }
        public UISection AddCell(String text, String value, UIClick click)
        {
            return this.Add(UICell.Create("Cell", new WebMeta().Put("value", value, "text", text).Put("click", click)));

        }
        public UISection AddCell(String text, String value, string click)
        {
            return this.Add(UICell.Create("Cell", new WebMeta().Put("value", value, "text", text).Put("click", click)));

        }
        public UISection AddCell(String text, UIClick click)
        {
            return this.Add(UICell.Create("Cell", new WebMeta().Put("text", text).Put("click", click)));

        }
        public UISection AddCell(String text, String value)
        {
            return this.Add(UICell.Create("Cell", new WebMeta().Put("value", value, "text", text)));

        }
        public UISection AddCell(char icon, String text, String value)
        {
            return this.Add(UICell.Create("UI", new WebMeta().Put("value", value, "text", text).Put("Icon", icon)));

        }
        public UISection AddCell(char icon, String text, String value, UIClick click)
        {
            return this.Add(UICell.Create("UI", new WebMeta().Put("value", value, "text", text).Put("Icon", icon).Put("click", click)));

        }
        public UISection Add(UICell cell)
        {

            data.Add(new WebMeta().Put("_CellName", cell.Type).Put("value", cell.Data).Put("format", cell.Format).Put("style", cell.Style));
            return this;
        }
        public UISection Delete(UICell cell, UIEventText eventText)
        {
            data.Add(new WebMeta().Put("del", eventText).Put("_CellName", cell.Type).Put("value", cell.Data).Put("format", cell.Format).Put("style", cell.Style));
            return this;
        }


        public UISection Add(String type, WebMeta value, WebMeta format, UIStyle style)
        {
            data.Add(new WebMeta().Put("_CellName", type).Put("value", value).Put("format", format).Put("style", style));
            return this;
        }
        public UISection AddPro(params UIPrice[] pros)
        {
            data.Add(new WebMeta().Put("_CellName", "Products").Put("value", new WebMeta().Put("data", pros)));
            return this;

        }
        public UISection AddPro(UIStyle style, params UIPrice[] pros)
        {
            data.Add(new WebMeta().Put("_CellName", "Products").Put("value", new WebMeta().Put("data", pros)).Put("style", style));
            return this;
        }
        public UISection AddItems(String model, params UIItem[] items)
        {
            data.Add(new WebMeta().Put("_CellName", "UIItems").Put("value", new WebMeta().Put("items", items).Put("model", model)));//.Put("format", format).Put("style", style));
            return this;
        }
        public UISection AddItems(params UIItem[] items)
        {
            data.Add(new WebMeta().Put("_CellName", "UIItems").Put("value", new WebMeta().Put("items", items)));//.Put("format", format).Put("style", style));
            return this;
        }
        public UISection AddItems(UIStyle style, params UIItem[] items)
        {
            data.Add(new WebMeta().Put("_CellName", "UIItems").Put("value", new WebMeta().Put("items", items)).Put("style", style));
            return this;
        }
        public UISection AddIcon(params UIEventText[] icons)
        {
            data.Add(new WebMeta().Put("_CellName", "Icons").Put("value", new WebMeta().Put("icons", icons)));//.Put("format", format).Put("style", style));
            return this;
        }
        public UISection AddIcon(UIStyle style, params UIEventText[] icons)
        {
            data.Add(new WebMeta().Put("_CellName", "Icons").Put("value", new WebMeta().Put("icons", icons)).Put("style", style));
            return this;
        }
        public UISection Add(String type, WebMeta value)
        {
            data.Add(new WebMeta().Put("_CellName", type).Put("value", value));
            return this;
        }
        public UISection AddSlider(params UISlider[] sliders)
        {
            data.Add(new WebMeta().Put("_CellName", "Slider").Put("value", new WebMeta().Put("data", sliders)));
            return this;
        }

        void IJSON.Read(string key, object value)
        {

        }
        public WebMeta ToMeta()
        {
            var meta = new WebMeta();
            if (_uiheaders != null)
            {
                meta.Put("Header", this._uiheaders);
            }
            if (this._title != null)
            {
                meta.Put("Title", this._title);
            }
            if (this._uifooter != null)
            {
                meta.Put("Footer", this._uifooter);
            }
            if (Total > 0)
                meta.Put("total", this.Total);
            var metas = new List<WebMeta>();
            foreach (var sec in this.Sections)
            {
                var s = new WebMeta();


                if (sec.Key != null)
                {
                    s.Put("key", sec.Key);
                }
                if (sec._data != null)
                {
                    s.Put("data", sec._data);

                }
                else
                {
                    s.Put("data", sec.data);
                }
                if (sec._header.Count > 0)
                {
                    s.Put("header", sec._header);
                }
                metas.Add(s);
            }
            meta.Put("DataSource", metas);
            return meta;
        }

        public bool? IsNext
        {
            get; set;

        }
        public int? StartIndex
        {

            get; set;
        }
        void IJSON.Write(System.IO.TextWriter writer)
        {
            writer.Write("{");
            if (_uiheaders != null)
            {
                UMC.Data.JSON.Serialize("Header", writer); writer.Write(":");
                UMC.Data.JSON.Serialize(this._uiheaders, writer);
                writer.Write(",");
            }
            if (this._title != null)
            {
                UMC.Data.JSON.Serialize("Title", writer);
                writer.Write(":");
                UMC.Data.JSON.Serialize(this._title, writer);

                writer.Write(",");

            }
            if (_uifooter != null)
            {
                UMC.Data.JSON.Serialize("Footer", writer); writer.Write(":");
                UMC.Data.JSON.Serialize(this._uifooter, writer);

                writer.Write(",");
            }
            if (Total > 0)
            {
                UMC.Data.JSON.Serialize("total", writer);
                writer.Write(":");
                UMC.Data.JSON.Serialize(Total, writer);
                writer.Write(",");

            }
            if (StartIndex.HasValue && StartIndex.Value > -1)
            {
                UMC.Data.JSON.Serialize("start", writer);
                writer.Write(":");
                UMC.Data.JSON.Serialize(this.StartIndex.Value, writer);
                writer.Write(",");

            }
            if (this.IsNext.HasValue)
            {

                UMC.Data.JSON.Serialize("next", writer);
                writer.Write(":");
                UMC.Data.JSON.Serialize(this.IsNext.Value, writer);
                writer.Write(",");
            }
            UMC.Data.JSON.Serialize("DataSource", writer);
            writer.Write(":[");
            bool b = false;
            foreach (var sec in this.Sections)
            {
                if (b)
                {
                    writer.Write(",");
                }
                else
                {
                    b = true;
                }
                writer.Write("{");
                if (String.IsNullOrEmpty(sec.Key) == false)
                {
                    UMC.Data.JSON.Serialize("key", writer);
                    writer.Write(":");
                    UMC.Data.JSON.Serialize(sec.Key, writer);
                    writer.Write(",");
                }
                if (sec.IsEditer)
                {
                    UMC.Data.JSON.Serialize("isEditer", writer);
                    writer.Write(":");
                    UMC.Data.JSON.Serialize(sec.IsEditer, writer);
                    writer.Write(",");

                }
                UMC.Data.JSON.Serialize("data", writer);
                writer.Write(":");
                if (sec._data != null)
                {

                    UMC.Data.JSON.Serialize(sec._data, writer);
                }
                else
                {
                    UMC.Data.JSON.Serialize(sec.data, writer);
                }
                if (sec._header.Count > 0)
                {
                    writer.Write(",");
                    UMC.Data.JSON.Serialize("header", writer); writer.Write(":");
                    UMC.Data.JSON.Serialize(sec._header, writer);
                }
                writer.Write("}");

            }

            writer.Write("]}");

        }
    }
}