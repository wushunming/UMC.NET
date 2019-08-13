using System;
using System.Collections.Generic;
using System.Text;
using UMC.Web;

namespace UMC.Data
{
    /// <summary>
    /// Markdown格式
    /// </summary>
    public class Markdown
    {
        WebMeta webRel = new WebMeta();
        List<UICell> cells = new List<UICell>();
        WebMeta data = new WebMeta();
        UIStyle style = new UIStyle();
        StringBuilder dataText = new StringBuilder();
        void Header(string text)
        {
            int i = 0;
            while (i < text.Length && text[i] == '#')
            {
                i++;
                if (i == 6)
                {
                    break;
                }
            }
            int size = 26 - (i - 1) * 2;
            dataText.Append(text.Substring(i).Trim());
            style.Name("m" + data.Count.ToString(), new UIStyle().Bold().Size(size));


            this.Append();
        }
        private Markdown() { }
        class Highlighter
        {
            static string[] keys = new string[] { "abstract", "+", "-", "*", "/", "%", "var", "instanceof", "extern", "private", "protected", "public", "namespace", "class", "for", "if", "else", "while", "switch", "case", "using", "get", "return", "null", "void", "int", "string", "float", "char", "this", "set", "new", "true", "false", "const", "static", "internal", "extends", "super", "import", "default", "break", "try", "catch", "finally", "implements", "package", "final" };

            WebMeta data = new WebMeta();
            UIStyle style = new UIStyle();
            StringBuilder dataText = new StringBuilder();


            public UICell Transform(String text)
            {
                Check(text);
                Append();
                if (dataText.Length > 0)
                {
                    data.Put("h" + data.Count.ToString(), dataText.ToString());
                }
                dataText = new StringBuilder();
                var sb = new StringBuilder();
                for (int i = 0; i < data.Count; i++)
                {
                    sb.Append("{h");
                    sb.Append(i);
                    sb.Append("}");
                }
                var cell = UICell.Create("CMSCode", data);
                cell.Format.Put("text", sb.ToString());
                cell.Style.Copy(style);

                return cell;

            }
            public void CheckWork()
            {
                var value = dataText.ToString();
                foreach (var k in keys)
                {
                    if (value.EndsWith(k))
                    {
                        if (value.Length > k.Length)
                        {
                            switch (value[value.Length - k.Length - 1])
                            {
                                case '.':
                                case ' ':
                                case '\n':
                                case '(':
                                case '[':
                                case '{':
                                case '\t':
                                    dataText.Remove(value.Length - k.Length, k.Length);
                                    data.Put("h" + data.Count, dataText.ToString());

                                    style.Name("h" + data.Count).Color(0x00f);
                                    data.Put("h" + data.Count, k);
                                    dataText.Clear();
                                    break;
                            }
                        }
                        else
                        {

                            style.Name("h" + data.Count).Color(0x00f);
                            data.Put("h" + data.Count, dataText.ToString());
                            dataText.Clear();
                        }
                    }
                }
            }
            public void CheckMethod()
            {
                var value = dataText.ToString();
                int t = value.Length - 1;
                while (t > -1)
                {
                    switch (value[t])
                    {
                        case '.':
                        case ' ':
                        case '\n':
                        case '(':
                        case '[':
                        case '{':
                        case '\t':
                            if (t + 1 < value.Length)
                            {

                                var fm = value.Substring(t + 1);

                                data.Put("h" + data.Count, value.Substring(0, t + 1));

                                style.Name("h" + data.Count).Color(0x2196f3);
                                data.Put("h" + data.Count, fm);
                                dataText.Clear();

                            }
                            return;
                    }
                    t--;
                }
            }
            public void Append()
            {
                CheckWork();
                if (dataText.Length > 0)
                {

                    data.Put("h" + data.Count, dataText.ToString());
                    dataText.Clear();
                }
            }
            void Check(string code)
            {
                int index = 0;
                while (index < code.Length)
                {
                    switch (code[index])
                    {
                        case '"':
                            {
                                Append();
                                var end = code.IndexOf('"', index + 1);

                                style.Name("h" + data.Count).Color(0xc00);
                                if (end == -1)
                                {
                                    end = code.IndexOf('\n', index);
                                    if (end == -1)
                                    {
                                        data.Put("h" + data.Count, code.Substring(index));
                                        return;
                                    }
                                    else
                                    {

                                        data.Put("h" + data.Count, code.Substring(end - index + 1));
                                        index = end + 1;

                                        continue;
                                    }
                                }
                                else
                                {
                                    while (code[end - 1] == '\\')
                                    {

                                        end = code.IndexOf('"', end + 1);
                                        if (end == -1)
                                        {
                                            data.Put("h" + data.Count, code.Substring(index));
                                            return;
                                        }
                                    }

                                    data.Put("h" + data.Count, code.Substring(index, end - index + 1));
                                }
                                index = end + 1;

                                continue;
                            }
                        case '\'':
                            {
                                Append();
                                var end = code.IndexOf('\'', index + 1);

                                if (end > 0)
                                {
                                    style.Name("h" + data.Count).Color(0xc00);

                                    data.Put("h" + data.Count, code.Substring(index, end - index + 1));

                                    index = end + 1;
                                    continue;

                                }
                            }
                            break;
                        case '/':
                            switch (code[index + 1])
                            {
                                case '/':
                                    {
                                        Append();
                                        if (dataText.Length > 0)
                                        {
                                            data.Put("h" + data.Count, dataText.ToString());
                                        }
                                        dataText.Clear();
                                        style.Name("h" + data.Count).Color(0x008000);
                                        var end = code.IndexOf('\n', index);
                                        if (end == -1)
                                        {

                                            data.Put("h" + data.Count, code.Substring(index));
                                            return;
                                        }
                                        else
                                        {

                                            data.Put("h" + data.Count, code.Substring(index, end - index));
                                        }
                                        index = end;
                                    }
                                    continue;
                                case '*':
                                    {
                                        Append();
                                        if (dataText.Length > 0)
                                        {
                                            data.Put("h" + data.Count, dataText.ToString());
                                        }
                                        dataText.Clear();
                                        style.Name("h" + data.Count).Color(0x008000);
                                        var end = code.IndexOf("*/", index);
                                        if (end == -1)
                                        {

                                            data.Put("h" + data.Count, code.Substring(index));
                                            return;
                                        }
                                        else
                                        {

                                            data.Put("h" + data.Count, code.Substring(index, end - index + 2));
                                        }
                                        index = end + 2;
                                        continue;
                                    }
                            }
                            break;
                        case ' ':
                        case '.':
                            CheckWork();
                            break;
                        case '(':
                            CheckMethod();
                            break;
                    }
                    dataText.Append(code[index]);
                    index++;
                }

            }
        }
        public static UICell[] Transform(String text)
        {
            var mk = new Markdown();
            mk.Check(text, 0);
            foreach (UIClick click in mk.links)
            {
                click.Send(mk.webRel[(String)click.Send()]);
            }
            foreach (WebMeta meta in mk.webRels)
            {
                meta.Put("src", mk.webRel.Get(meta.Get("src")));
            }
            return mk.cells.ToArray();

        }
        void Append()
        {
            if (data.Count > 0 || dataText.Length > 0)
            {
                if (dataText.Length > 0)
                {
                    data.Put("m" + data.Count.ToString(), dataText.ToString());
                }
                dataText = new StringBuilder();
                var sb = new StringBuilder();
                for (int i = 0; i < data.Count; i++)
                {
                    sb.Append("{m");
                    sb.Append(i);
                    sb.Append("}");
                }
                var cell = UICell.Create(data["type"] ?? "CMSText", data);
                data.Remove("type");
                cell.Format.Put("text", sb.ToString());
                cell.Style.Copy(style);
                cells.Add(cell);
                data = new WebMeta();
                style = new UIStyle();
            }
        }
        void CheckRow(string text, int index)
        {
            CheckRow(text, index, true);
        }
        void AppendData()
        {
            if (dataText.Length > 0)
            {
                //if (data.Count == 0)
                //{
                //    data.Put("m" + data.Count.ToString(), dataText.ToString().TrimStart());
                //}
                //else
                //{
                data.Put("m" + data.Count.ToString(), dataText.ToString());
                //}
                dataText = new StringBuilder();
            }
        }
        private List<WebMeta> webRels = new List<WebMeta>();
        private List<UIClick> links = new List<UIClick>();
        void CheckRow(string text, int index, bool isNextLIne)
        {
            while (index + 1 < text.Length && text[index] != '\n')
            {

                switch (text[index])
                {
                    case '\r':
                        index++;
                        continue;
                    case '!':
                        if (text[index + 1] == '[' && isNextLIne)
                        {
                            int end = text.IndexOf("]", index + 1);
                            if (end > index)
                            {

                                String content = text.Substring(index + 1, end - index - 1).Trim('[', ']');
                                if (content.IndexOf('\n') == -1)
                                {
                                    if (text[end + 1] == '(')
                                    {
                                        Append();
                                        int end2 = text.IndexOf(")", end + 1);
                                        if (end2 > end)
                                        {
                                            var url = text.Substring(end + 1, end2 - end - 1).Trim(' ', '(', ')').Split(' ')[0];
                                            var cell = UICell.Create("CMSImage", new WebMeta().Put("src", url));
                                            cell.Style.Padding(0, 10);
                                            cells.Add(cell);
                                            index = end2 + 1;

                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        Append();

                                        if (webRel.ContainsKey(content))
                                        {
                                            var cell = UICell.Create("CMSImage", new WebMeta().Put("src", webRel[content]));
                                            cell.Style.Padding(0, 10);
                                            cells.Add(cell);
                                        }
                                        else
                                        {
                                            var src = new WebMeta().Put("src", content);
                                            this.webRels.Add(src);
                                            var cell = UICell.Create("CMSImage", src);
                                            cell.Style.Padding(0, 10);
                                            cells.Add(cell);

                                        }
                                        index = end + 1;

                                        continue;
                                    }

                                }

                            }
                        }
                        break;
                    case '[':
                        {
                            int end = text.IndexOf("]", index + 1);
                            if (end > index)
                            {
                                String content = text.Substring(index, end - index).Trim('[', ']');
                                if (content.IndexOf('\n') == -1)
                                {
                                    if (text[end + 1] == '(')
                                    {

                                        int end2 = text.IndexOf(")", end + 1);
                                        if (end2 > end)
                                        {
                                            var url = text.Substring(end + 1, end2 - end - 1).Trim(' ', '(', ')').Split(' ')[0];


                                            AppendData();
                                            style.Name("m" + data.Count.ToString(), new UIStyle().Click(new UIClick(url) { Key = "Url" }));
                                            data.Put("m" + data.Count.ToString(), content);

                                            index = end2 + 1;

                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        AppendData();
                                        if (webRel.ContainsKey(content))
                                        {
                                            style.Name("m" + data.Count.ToString(), new UIStyle().Click(new UIClick(webRel[content]) { Key = "Url" }));
                                        }
                                        else
                                        {
                                            var click = new UIClick(content) { Key = "Url" };
                                            this.links.Add(click);
                                            //click.Value
                                            style.Name("m" + data.Count.ToString(), new UIStyle().Click(click));

                                        }
                                        data.Put("m" + data.Count.ToString(), content);


                                        index = end + 1;

                                        continue;
                                    }

                                }
                            }
                        }
                        break;
                    case '`':
                        {

                            int end = text.IndexOf("`", index + 1);
                            if (end > index)
                            {
                                String content = text.Substring(index, end - index);
                                if (content.IndexOf('\n') == -1)
                                {

                                    AppendData();
                                    style.Name("m" + data.Count.ToString(), new UIStyle().Color(0xCC6600));
                                    data.Put("m" + data.Count.ToString(), content.Trim('`'));


                                    index = end + 1;

                                    continue;
                                }
                            }
                        }
                        break;
                    case '~':
                        if (text[index + 1] == '~')
                        {

                            int end = text.IndexOf("~~", index + 1);
                            if (end > index)
                            {

                                String content = text.Substring(index, end - index);
                                if (content.IndexOf('\n') == -1)
                                {

                                    AppendData();
                                    style.Name("m" + data.Count.ToString(), new UIStyle().DelLine());
                                    data.Put("m" + data.Count.ToString(), content.Trim('~'));

                                    index = end + 2;

                                    continue;

                                }
                            }

                        }
                        else
                        {
                            int end = text.IndexOf("~", index + 1);
                            if (end > index)
                            {
                                String content = text.Substring(index, end - index);
                                if (content.IndexOf('\n') == -1)
                                {
                                    AppendData();
                                    style.Name("m" + data.Count.ToString(), new UIStyle().UnderLine());
                                    data.Put("m" + data.Count.ToString(), content.Trim('*'));
                                    index = end + 1;

                                    continue;


                                }
                            }

                        }
                        break;
                    case '*':
                        if (text[index + 1] == '*')
                        {

                            int end = text.IndexOf("**", index + 1);

                            if (end > index)
                            {
                                String content = text.Substring(index, end - index);
                                if (content.IndexOf('\n') == -1)
                                {

                                    AppendData();
                                    style.Name("m" + data.Count.ToString(), new UIStyle().Bold());
                                    data.Put("m" + data.Count.ToString(), content.Trim('*'));

                                    index = end + 2;

                                    continue;

                                }
                            }

                        }
                        else
                        {
                            int end = text.IndexOf("*", index + 1);
                            if (end > index)
                            {
                                String content = text.Substring(index + 1, end - index);
                                if (content.IndexOf('\n') == -1)
                                {

                                    AppendData();
                                    style.Name("m" + data.Count.ToString(), new UIStyle().UnderLine());
                                    data.Put("m" + data.Count.ToString(), content.Trim('*'));
                                    index = end + 1;

                                    continue; ;

                                }
                            }

                        }
                        break;
                }
                dataText.Append(text[index]);
                index++;
            }
            if (index + 1 == text.Length)
            {
                dataText.Append(text[index]);
            }
            if (isNextLIne)
            {
                Append();
                Check(text, index + 1);
            }

        }
        void Grid(List<String> rows)
        {
            var hStyle = new List<UIStyle>();
            var header = rows[1].Trim('|').Split('|');
            var grid = new List<List<WebMeta>>();
            int flexs = 0;
            foreach (var h in header)
            {
                var st = new UIStyle();
                var s = h.Trim();
                if (s.StartsWith(":") && s.EndsWith(":"))
                {
                    st.AlignCenter();
                }
                else if (s.EndsWith(":"))
                {
                    st.AlignRight();
                }
                else
                {
                    st.AlignLeft();
                }
                int flex = s.Split('-').Length - 1;
                st.Name("flex", flex);
                flexs += flex;
                hStyle.Add(st);
            }
            rows.RemoveAt(1);
            foreach (var row in rows)
            {
                var cells = row.Trim('|').Split('|');
                var cdata = new List<WebMeta>();
                for (int i = 0; i < hStyle.Count; i++)
                {
                    var cstyle = new UIStyle();
                    cstyle.Copy(hStyle[i]);
                    this.style = cstyle;
                    this.data = new WebMeta();
                    this.dataText = new StringBuilder();
                    if (i < cells.Length)
                    {
                        CheckRow(cells[i].Trim(), 0, false);
                    }
                    if (dataText.Length > 0)
                    {
                        this.data.Put("m" + data.Count.ToString(), dataText.ToString().Trim());
                    }
                    var sb = new StringBuilder();
                    for (int c = 0; c < data.Count; c++)
                    {
                        sb.Append("{m");
                        sb.Append(c);
                        sb.Append("}");
                    }
                    cdata.Add(new WebMeta().Put("format", sb.ToString()).Put("data", this.data).Put("style", this.style));

                }
                grid.Add(cdata);
            }

            var cell = UICell.Create("CMSGrid", new WebMeta().Put("grid", grid).Put("flex", flexs));
            cells.Add(cell);
            data = new WebMeta();
            style = new UIStyle();
            dataText = new StringBuilder();

        }
        void Check(string text, int index)
        {
            if (index + 1 >= text.Length)
            {
                Append();
                return;
            }
            switch (text[index])
            {
                case '#':
                    {
                        int end = text.IndexOf('\n', index);
                        if (end > index)
                        {
                            Header(text.Substring(index, end - index));
                            Check(text, end + 1);
                        }
                        else
                        {
                            Header(text.Substring(index));
                        }
                        return;
                    }
                case '`':
                    {
                        if (text.Substring(index, 3) == "```")
                        {
                            int end = text.IndexOf("\n```", index + 1);
                            if (end > index)
                            {
                                String content = text.Substring(index, end - index);

                                content = content.Substring(content.IndexOf('\n') + 1);

                                cells.Add(new Highlighter().Transform(content));
                                index = text.IndexOf('\n', end + 1) + 1;
                                Check(text, index);
                                return;
                            }
                        }
                    }
                    break;
                case '>':
                    {
                        if (cells.Count > 0)
                        {
                            var cell = cells[cells.Count - 1];
                            if (cell.Type == "CMSRel")
                            {
                                var d = cell.Data as WebMeta;

                                cells.RemoveAt(cells.Count - 1);
                                this.data = d;
                                this.dataText.Append("\r\n");
                                this.style = cell.Style;
                                this.data.Put("type", "CMSRel");
                                CheckRow(text, index + 1);
                                return;

                            }
                        }
                        this.data.Put("type", "CMSRel");
                        CheckRow(text, index + 1);
                        return;
                    }
                case '|':
                    {

                        int end = text.IndexOf('\n', index);
                        if (end > index)
                        {
                            var grids = new List<String>();
                            String conent = text.Substring(index, end - index).Trim().Replace(" ", "");
                            if (conent[conent.Length - 1] == '|')
                            {
                                int end2 = text.IndexOf('\n', end + 1);//.Trim();
                                String conent2 = text.Substring(end + 1, end2 - end - 1).Trim();
                                grids.Add(conent);
                                conent2 = conent2.Replace(" ", "");

                                if (System.Text.RegularExpressions.Regex.IsMatch(conent2, "^[\\|:-]+$"))
                                {
                                    grids.Add(conent2);
                                    if (conent2.Split('|').Length == conent.Split('|').Length)
                                    {
                                        bool isGO = true;
                                        while (isGO)
                                        {
                                            isGO = false;
                                            int end3 = text.IndexOf('\n', end2 + 1);//.Trim();

                                            String conent3 = end3 > 0 ? text.Substring(end2 + 1, end3 - end2 - 1).Trim() : text.Substring(end2 + 1).Trim();
                                            if (conent3.StartsWith("|") && conent3.EndsWith("|"))
                                            {
                                                isGO = true;
                                                grids.Add(conent3);
                                                end2 = end3;
                                            }
                                        }
                                        this.Grid(grids);
                                        Check(text, end2 + 1);
                                        return;

                                    }
                                }

                            }
                        }
                    }
                    break;
                case '[':
                    {
                        int end = text.IndexOf("]", index + 1);
                        if (end > index && end + 1 < text.Length)
                        {
                            if (text[end + 1] == ':')
                            {
                                String content = text.Substring(index, end - index).Trim('[', ']');
                                if (content.IndexOf('\n') == -1)
                                {
                                    int end2 = text.IndexOf("\n", end + 1);
                                    if (end2 == -1)
                                    {
                                        var url = text.Substring(end + 2).Trim().Trim(' ', '(', ')').Split(' ')[0];
                                        webRel.Put(content, url); 
                                    }
                                    else
                                    {

                                        var url = text.Substring(end + 2, end2 - end - 2).Trim().Trim(' ', '(', ')').Split(' ')[0];
                                        webRel.Put(content, url);
                                        Check(text, end2 + 1);
                                    }
                                    return;

                                }


                            }
                        }
                    }
                    break;
                case ' ':
                    while (text.Length > index && text[index] == ' ')
                    {
                        index++;
                    }
                    break;

            }
            //  while(text.Length>index&&text[in])

            this.CheckRow(text, index);
        }
    }
}