using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace UMC.Web
{

    public class UIDataSource : UMC.Data.IJSON
    {
        Hashtable _data = new Hashtable();
        public UIDataSource(System.Data.DataTable data, String cell)
        {
            _data["data"] = data;
            if (String.IsNullOrEmpty(cell) == false)
            {
                _data["cell"] = cell;
            }
        }
        public UIDataSource(ICollection data, string cell)
        {
            _data["data"] = data;
            if (String.IsNullOrEmpty(cell) == false)
            {
                _data["cell"] = cell;
            }
        }
        public string Text
        {
            get
            {
                return _data["text"] as string;
            }
            set
            {
                _data["text"] = value;
            }
        }
        public UIDataSource(string model, string cmd)
            : this(model, cmd, "")
        {
        }
        public UIDataSource(string model, string cmd, string cell)
        {
            if (String.IsNullOrEmpty(model) == false && String.IsNullOrEmpty(cmd) == false)
            {
                _data["model"] = model;
                _data["cmd"] = cmd;
            }
            if (String.IsNullOrEmpty(cell) == false)
            {
                _data["cell"] = cell;
            }
        }
        public UIDataSource(string model, string cmd, WebMeta search, String cell)
        {
            if (String.IsNullOrEmpty(model) == false && String.IsNullOrEmpty(cmd) == false)
            {
                _data["model"] = model;
                _data["cmd"] = cmd;
            }
            if (String.IsNullOrEmpty(cell) == false)
            {
                _data["cell"] = cell;
            }
            if (search != null && search.Count > 0)
            {
                _data["search"] = search;
            }
        }

        public void BuildStyle(WebMeta style)
        {
            if (style != null && style.Count > 0)
            {
                _style.Copy(new UIStyle(style));
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
        public void BuildStyle(UIStyle style)
        {
            if (style != null)
            {
                _style.Copy(style);
            }
        }
        public void BuildFormat(WebMeta format)
        {
            if (format != null && format.Count > 0)
            {
                _data["format"] = format;
            }
        }
        public void BuildSubmit(string model, string cmd, string send)
        {
            var click = new UMC.Web.WebMeta().Put("model", model).Put("cmd", cmd);
            if (String.IsNullOrEmpty(send) == false)
            {
                click.Put("send", send); ;
            }

            _data["submit"] = click;

        }
        public void BuildSubmit(string model, string cmd, WebMeta send)
        {
            var click = new UMC.Web.WebMeta().Put("model", model).Put("cmd", cmd);
            if (send != null && send.Count > 0)
            {
                click.Put("send", send); ;
            }

            _data["submit"] = click;

        }
        public void BuildClick(string model, string cmd, string send)
        {
            var click = new UMC.Web.WebMeta().Put("model", model).Put("cmd", cmd);
            if (String.IsNullOrEmpty(send) == false)
            {
                click.Put("send", send); ;
            }

            _data["click"] = click;

        }
        public void BuildClick(string model, string cmd, WebMeta send)
        {
            var click = new UMC.Web.WebMeta().Put("model", model).Put("cmd", cmd);
            if (send != null && send.Count > 0)
            {
                click.Put("send", send); ;
            }

            _data["click"] = click;

        }

        void Data.IJSON.Write(System.IO.TextWriter writer)
        {

            _data["style"] = _style;
            UMC.Data.JSON.Serialize(_data, writer);
        }

        void Data.IJSON.Read(string key, object value)
        {

        }
    }
    public class UISectionBuilder
    {
        WebMeta _data = new UMC.Web.WebMeta();
        public UISectionBuilder(String model, String cmd)
        {
            _data.Put("model", model, "cmd", cmd);//.Put("search", search);

        }
        public UISectionBuilder(String model, String cmd, WebMeta search)
        {
            _data.Put("model", model, "cmd", cmd).Put("search", search);
        }
        public UISectionBuilder RefreshEvent(params String[] eventName)
        {

            _data.Put("RefreshEvent", String.Join(",", eventName));
            return this;
        }
        public UISectionBuilder DataEvent(params String[] eventName)
        {

            _data.Put("DataEvent", String.Join(",", eventName));
            return this;
        }
        public UISectionBuilder Scanning(Web.UIClick click)
        {

            _data.Put("Scanning", click);
            return this;
        }
        public UISectionBuilder CloseEvent(params String[] eventName)
        {

            _data.Put("CloseEvent", String.Join(",", eventName));
            return this;
        }
        public WebMeta Builder()
        {
            return new UMC.Web.WebMeta(_data.GetDictionary()).Put("type", "Pager");//.Put("DataSource", dataSources).Put("title", this.Title);//.Put("model");
        }
    }

    public class UIDataSourceBuilder
    {

        WebMeta _data = new UMC.Web.WebMeta();

        public UIDataSourceBuilder Menu(params Web.UIClick[] clicks)
        {
            _data.Put("menu", clicks);
            return this;
        }
        public string Title
        {
            get; set;
        }
        public UIDataSourceBuilder RefreshEvent(params String[] eventName)
        {

            _data.Put("RefreshEvent", String.Join(",", eventName));
            return this;
        }
        public UIDataSourceBuilder CloseEvent(params String[] eventName)
        {

            _data.Put("CloseEvent", String.Join(",", eventName));
            return this;
        }
        public UIDataSourceBuilder Header(UIHeader header)
        {
            _data.Put("Header", header);
            return this;
        }
        public UIDataSourceBuilder Footer(UIHeader footer)
        {
            _data.Put("Footer", footer);
            return this;
        }

        public WebMeta Builder(params UIDataSource[] dataSources)
        {
            return new UMC.Web.WebMeta(_data.GetDictionary()).Put("type", "DataSource").Put("DataSource", dataSources).Put("title", this.Title);//.Put("model");
        }
        public WebMeta BinderCells(params UIDataSource[] dataSources)
        {

            return new UMC.Web.WebMeta(_data.GetDictionary()).Put("type", "DataSource").Put("DataSource", dataSources).Put("model", "Cells").Put("title", this.Title);
        }
    }
}
