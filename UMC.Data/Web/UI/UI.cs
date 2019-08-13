
using System;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
namespace UMC.Web.UI
{




    public class UIDesc : UICell
    {
        public UIDesc(string desc)
        {
            this.data = new WebMeta().Put("desc", desc);
            this.Type = "Desc";
        }
        public UIDesc Click(UIClick click)
        {

            this.data.Put("click", click);
            return this;
        }
        public UIDesc(WebMeta desc)
        {
            this.data = desc;// new POSMeta().Put("desc", desc);
            this.Type = "Desc";

        }
        public UIDesc Desc(String desc)
        {
            this.Format.Put("desc", desc);
            return this;
        }
        WebMeta data;
        public override object Data => data;
    }
    public class UITitleMore : UICell
    {
        public static UITitleMore Create(String title)
        {
            var t = new UITitleMore(title);
            return t;

        }

        WebMeta data;
        public override object Data => data;
        private UITitleMore(string title)
        {
            this.data = new WebMeta().Put("title", title);
            this.Type = "TitleMore";
        }

        public UITitleMore Title(String title)
        {
            this.Format.Put("title", title);
            return this;
        }
        public UITitleMore More(String more)
        {
            this.Format.Put("more", more);
            return this;
        }
        public UITitleMore Click(UIClick click)
        {
            this.data.Put("click", click);
            this.data.Put("more", '\uE905');
            this.Style.Name("more", new UIStyle().Font("wdk").Size(12));
            return this;
        }
    }
    public class UIFooterButton : UICell
    {

        public static UIFooterButton Create(WebMeta data)
        {
            var t = new UIFooterButton(data);
            return t;

        }
        public static UIFooterButton Create()
        {
            var t = new UIFooterButton(new WebMeta());
            return t;

        }
        WebMeta data;
        public override object Data => data;
        private UIFooterButton(WebMeta data)
        {
            this.data = data;
            this.Type = "UIFooterButton";
        }
        public void Title(String title)
        {
            this.Format.Put("title", title);
        }
        //public void Button(params UIButton[] btns)
        //{
        //    data.Put("buttons", btns);
        //}
        public void Button(params UIEventText[] btns)
        {
            data.Put("buttons", btns);
        }

    }
    public class UIImageTitleDescBottom : UICell
    {
        private UIImageTitleDescBottom(WebMeta data)
        {
            this.data = data;
        }
        public static UIImageTitleDescBottom Create(WebMeta data, String src)
        {
            var t = new UIImageTitleDescBottom(data);
            t.data.Put("src", src);
            t.Type = "ImageTitleDescBottom";
            return t;

        }
        WebMeta data;
        public override object Data => data;

        public UIImageTitleDescBottom Desc(string desc)
        {
            Format.Put("desc", desc);
            return this;

        }
        public UIImageTitleDescBottom Click(UIClick click)
        {
            this.data.Put("click", click);
            return this;
        }
        public UIImageTitleDescBottom Title(string desc)
        {

            this.Format.Put("title", desc);
            return this;

        }
        public UIImageTitleDescBottom Left(string price)
        {
            this.Format.Put("left", price);
            return this;

        }
        public UIImageTitleDescBottom Right(string price)
        {
            this.Format.Put("right", price);
            return this;

        }
    }
    public class UIImageTextValue : UICell
    {
        private UIImageTextValue(WebMeta data)
        {
            this.data = data;
        }
        public static UIImageTextValue Create(String src, String text, String value)
        {
            var t = new UIImageTextValue(new WebMeta());
            t.data.Put("src", src);
            t.data.Put("text", text).Put("value", value);
            t.Type = "ImageTextValue";
            return t;

        }
        public UIImageTextValue Text(string text)
        {
            data.Put("text", text);
            return this;
        }
        public UIImageTextValue Value(string value)
        {
            data.Put("value", value);
            return this;
        }
        public UIImageTextValue Put(String name, String value)
        {
            data.Put(name, value);
            return this;

        }
        public UIImageTextValue Click(UIClick click)
        {
            data.Put("click", click);
            return this;

        }
        public
        WebMeta data;
        public override object Data => data;

    }

    public class UIImageTitleBottom : UICell
    {

        public static UIImageTitleBottom Create(String src)
        {
            var t = new UIImageTitleBottom(new WebMeta());
            t.data.Put("src", src);
            t.Type = "ImageTitleBottom";
            return t;

        }
        private UIImageTitleBottom(WebMeta data)
        {
            this.data = data;
        }
        public static UIImageTitleBottom Create(WebMeta data, String src)
        {
            var t = new UIImageTitleBottom(data);
            t.data.Put("src", src);
            t.Type = "ImageTitleBottom";
            return t;

        }
        public static UIImageTitleBottom Create(UIClick click, WebMeta data, String src)
        {
            var t = new UIImageTitleBottom(data);
            t.data.Put("src", src);
            t.data.Put("click", click);
            t.Type = "ImageTitleBottom";
            return t;

        }
        public UIImageTitleBottom Max(String src)
        {
            this.data.Put("max", src);
            return this;
        }
        public UIImageTitleBottom Left(string desc)
        {
            Format.Put("left", desc);
            return this;
        }
        public UIImageTitleBottom Title(string desc)
        {
            this.Format.Put("title", desc);
            return this;
        }
        public UIImageTitleBottom Right(string price)
        {
            this.Format.Put("right", price);
            return this;
        }
        /// <summary>
        /// 采用商品价格格式化
        /// </summary>
        /// <param name="price">价格</param>
        /// <param name="unit">单位</param>
        /// <returns></returns>
        public UIImageTitleBottom Left(String price, String unit)
        {
            this.data.Put("price", price, "unit", unit);
            this.Format.Put("left", "¥{1:price}/{1:unit}");
            this.Style.Name("price", new UIStyle().Size(20).Color(0xdb3652)).Name("unit", new UIStyle().Size(15).Color(0x999)).Name("orgin", new UIStyle().Color(0x999).Size(12).DelLine());
            return this;
        }
        /// <summary>
        /// 采用商品价格格式化
        /// </summary>
        /// <param name="price">价格</param>
        /// <param name="orgin">原价</param>
        /// <param name="unit">单位</param>
        public UIImageTitleBottom Left(String price, String orgin, String unit)
        {
            this.data.Put("price", price, "unit", unit, "orgin", orgin);
            this.Format.Put("left", "¥{1:price}/{1:unit} 原价:{3:orgin}");
            this.Style.Name("price", new UIStyle().Size(14).Color(0xdb3652)).Name("unit", new UIStyle().Size(12).Color(0x999)).Name("orgin", new UIStyle().Color(0x999).Size(12).DelLine());
            return this;
        }
        public UIImageTitleBottom Click(UIClick click)
        {
            this.data.Put("click", click);
            return this;
        }
        WebMeta data;
        public override object Data => data;
    }
    public class UICMSLook : UICell
    {
        public override object Data => data;
        public static UICMSLook Create(String src, UIClick click, WebMeta data)
        {
            var t = new UICMSLook(data);
            t.data.Put("src", src);
            t.data.Put("click", click);
            t.Type = "CMSLook";
            return t;

        }
        WebMeta data;
        private UICMSLook(WebMeta data)
        {
            this.data = data;
        }
        public UICMSLook Title(String title)
        {
            this.Format.Put("title", title);
            return this;
        }

        public UICMSLook Desc(String desc)
        {
            this.Format.Put("desc", desc);
            return this;
        }
    }
    public class UICMS : UICell
    {
        WebMeta data;
        public override object Data => data;

        private UICMS(WebMeta data)
        {
            this.data = data;
        }
        public static UICMS CreateMax(UIClick click, WebMeta data)
        {
            var t = new UICMS(data);
            t.data.Put("click", click);
            t.Type = "CMSMax";
            return t;

        }
        public static UICMS CreateMax(UIClick click, WebMeta data, String src)
        {
            var t = new UICMS(data);
            t.data.Put("src", src);
            t.data.Put("click", click);
            t.Type = "CMSMax";
            return t;
        }
        public static UICMS CreateOne(UIClick click, WebMeta data, String src)
        {
            var t = new UICMS(data);
            t.data.Put("src", src);
            t.data.Put("click", click);
            t.Type = "CMSOne";
            return t;

        }
        public static UICMS CreateThree(UIClick click, WebMeta data, params String[] src)
        {

            var t = new UICMS(data);
            t.data.Put("images", src);
            t.data.Put("click", click);
            t.Type = "CMSThree";
            return t;
        }
        public UICMS Desc(string desc)
        {
            data.Put("desc", desc);
            return this;
        }
        public UICMS Title(string title)
        {
            this.data.Put("title", title);
            return this;

        }
        public UICMS Right(string right)
        {
            data.Put("right", right);
            return this;

        }
        public UICMS Left(string left)
        {
            data.Put("left", left);
            return this;

        }
    }



    public class UIDiscount : UICell
    {

        WebMeta data;
        public override object Data => data;
        private UIDiscount(WebMeta data)
        {
            this.data = data;
        }

        public static UIDiscount Create()
        {

            var t = new UIDiscount(new WebMeta());
            t.Type = "Discount";
            return t;
        }
        public static UIDiscount Create(UIClick click)
        {
            var t = Create();
            t.data.Put("click", click);
            return t;
        }
        public UIDiscount Click(UIClick click)
        {

            data.Put("click", click);
            return this;
        }

        public UIDiscount Gradient(int startColor, int endColor)
        {

            if (startColor < 0x1000)
            {
                data.Put("startColor", String.Format("#{0:x3}", startColor));
            }
            else
            {
                data.Put("startColor", String.Format("#{0:x6}", startColor));
            }
            if (endColor < 0x1000)
            {
                data.Put("endColor", String.Format("#{0:x3}", endColor));
            }
            else
            {
                data.Put("endColor", String.Format("#{0:x6}", endColor));
            }
            return this;
        }
        public UIDiscount Desc(string desc)
        {
            data.Put("desc", desc);
            return this;
        }
        public UIDiscount Title(string title)
        {
            this.Format.Put("title", title);
            return this;

        }
        public UIDiscount End(string end)
        {
            data.Put("end", end);
            return this;

        }
        public UIDiscount Start(string start)
        {
            data.Put("start", start);
            return this;

        }
        public UIDiscount Value(string value)
        {
            this.data.Put("value", value);
            return this;

        }
        public UIDiscount State(string state)
        {
            this.data.Put("state", state);
            return this;

        }
    }
}