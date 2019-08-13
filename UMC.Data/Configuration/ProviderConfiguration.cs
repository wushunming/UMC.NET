using System;
using System.Collections;
using System.Xml;
using UMC.Data;
namespace UMC.Configuration
{
    /// <summary>
    /// ��Ϣ���ü�����
    /// </summary>
    public class ProviderConfiguration
    {
        class ProviderHashtable : Hashtable
        {
            ProviderConfiguration pr;
            public override object this[object key]
            {
                get
                {
                    return base[key];
                }
                set
                {
                    if (this.ContainsKey(key))
                    {
                        base[key] = value;
                    }
                    else
                    {
                        this.Add(key, value);
                    }
                }
            }
            public ProviderHashtable(ProviderConfiguration pr)
            {
                this.pr = pr;
            }
            public override void Add(object key, object value)
            {
                base.Add(key, value);
                this.pr._KeyIndex.Add(key);
            }
            public override void Clear()
            {
                base.Clear();
                this.pr._KeyIndex.Clear();
            }
            public override void Remove(object key)
            {
                base.Remove(key);
                this.pr._KeyIndex.Remove(key);
            }
        }
        /// <summary>
        /// Ĭ�ϳ�ʼ��
        /// </summary>
        public ProviderConfiguration()
        {
            this._Providers = new ProviderHashtable(this);
        }
        /// <summary>
        /// Provider�ṩ�ĸ���
        /// </summary>
        public int Count
        {
            get
            {
                return this.Providers.Count;
            }
        }



        // Fields
        private string _ProviderType;
        private Hashtable _Providers;
        private ArrayList _KeyIndex = new ArrayList();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">ProviderKey</param>
        /// <returns></returns>
        public Provider this[string key]
        {
            get
            {
                return this._Providers[key] as Provider;
            }
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="index">����</param>
        /// <returns></returns>
        public Provider this[int index]
        {
            get
            {
                return this._Providers[this._KeyIndex[index]] as Provider;
            }
        }
        private static System.Collections.Generic.Dictionary<String, ProviderConfiguration> _cache = new System.Collections.Generic.Dictionary<string, ProviderConfiguration>();

        public static System.Collections.Generic.Dictionary<String, ProviderConfiguration> Cache
        {
            get
            {
                return _cache;
            }
        }

        public static ProviderConfiguration GetProvider(string filename)
        {
            if (_cache.ContainsKey(filename))
            {
                return _cache[filename];
            }
            ProviderConfiguration providers = null;

            if (System.IO.File.Exists(filename))
            {
                lock (_cache)
                {
                    providers = GetProvider(System.IO.File.OpenRead(filename));
                    _cache.Add(filename, providers);
                }
            }
            else
            {
                throw new System.IO.FileNotFoundException("û���ҵ��ļ�", filename);
            }
            //}
            return providers;
        }
        /// <summary>
        /// ��xml�ļ����еõ��ṩ��Ϣ
        /// </summary>
        /// <param name="reader">�ɶ���</param>
        public static ProviderConfiguration GetProvider(System.IO.TextReader reader)
        {
            ProviderConfiguration providers = new ProviderConfiguration();
            //providers._ProviderAttribute = filename;
            System.Xml.XmlDocument doc = new XmlDocument();
            doc.Load(reader);
            providers._ProviderType = doc.DocumentElement.Attributes["providerType"] == null ? "" : doc.DocumentElement.Attributes["providerType"].Value;
            foreach (XmlNode node2 in doc.DocumentElement.ChildNodes)
            {
                if (node2.Name == "providers")
                {
                    providers.GetProviders(node2);
                }
            }
            return providers;
        }

        public void WriteTo(System.Xml.XmlDocument doc, string group)
        {
            var provider = doc.CreateElement("providers");
            doc.DocumentElement.AppendChild(provider);
            if (!String.IsNullOrEmpty(this._ProviderType))
            {
                var providerType = doc.CreateAttribute("providerType");
                providerType.Value = this._ProviderType;
                provider.Attributes.Append(providerType);
            }
            if (String.IsNullOrEmpty(group) == false)
            {
                var groupAttr = doc.CreateAttribute("group");
                groupAttr.Value = group;
                provider.Attributes.Append(groupAttr);
            }

            for (int i = 0; i < this._KeyIndex.Count; i++)
            {
                Provider pro = (Provider)this._Providers[this._KeyIndex[i]];

                var add = doc.CreateElement("add");
                var name = doc.CreateAttribute("name");
                name.Value = pro.Name;
                add.Attributes.Append(name);
                var type = doc.CreateAttribute("type");
                type.Value = pro.Type;
                add.Attributes.Append(type);

                for (int a = 0; a < pro.Attributes.Count; a++)
                {
                    string str = pro.Attributes[a];
                    if (!String.IsNullOrEmpty(str))
                    {
                        if (str.IndexOf('\n') > -1)
                        {
                            var node = doc.CreateElement(pro.Attributes.GetKey(a));
                            node.AppendChild(doc.CreateCDataSection(str));
                            add.AppendChild(node);
                        }
                        else
                        {
                            var att = doc.CreateAttribute(pro.Attributes.GetKey(a));
                            att.Value = str;
                            add.Attributes.Append(att);
                        }
                    }
                }
                provider.AppendChild(add);

            }

        }
        public virtual void WriteTo(System.IO.Stream outStream)
        {
            var writer = new System.IO.StreamWriter(outStream);
            WriteTo(writer);
        }
        public virtual void WriteTo(string filename)
        {
            var file = System.IO.File.Open(filename, System.IO.FileMode.Create);
            try
            {
                WriteTo(file);
            }
            finally
            {
                file.Close();
            }

        }
        public virtual void WriteTo(System.IO.TextWriter writer)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.ConformanceLevel = ConformanceLevel.Auto;
            settings.IndentChars = "\t";
            settings.OmitXmlDeclaration = false;
            WriteTo(System.Xml.XmlWriter.Create(writer, settings));
        }
        public void WriteTo(System.Xml.XmlWriter writer)
        {
            System.Xml.XmlDocument doc = new XmlDocument();
            doc.LoadXml("<webadnuke/>");
            WriteTo(doc, String.Empty);
            doc.Save(writer);
        }
        /// <summary>
        /// ��xml�ļ����еõ��ṩ��Ϣ
        /// </summary>
        /// <param name="stream">�ɶ���</param>
        /// <param name="group">��</param>
        public static ProviderConfiguration GetProvider(System.IO.Stream stream, string group)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(stream);
            try
            {
                return GetProvider(reader, group);
            }
            finally
            {
                stream.Close();
                reader.Close();
            }
        }
        /// <summary>
        /// ��xml�ļ����еõ��ṩ��Ϣ
        /// </summary>
        /// <param name="stream">�ɶ���</param>
        public static ProviderConfiguration GetProvider(System.IO.Stream stream)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(stream);
            try
            {
                return GetProvider(reader);
            }
            finally
            {
                stream.Close();
                reader.Close();
            }
        }
        internal static ProviderConfiguration GetProvider(System.Xml.XmlNode node)
        {
            ProviderConfiguration providers = new ProviderConfiguration();
            providers.GetProviders(node);
            return providers;
        }
        /// <summary>
        /// ��xml�ļ����еõ��ṩ��Ϣ
        /// </summary>
        /// <param name="reader">�ɶ��ı���</param>
        /// <param name="group">��</param>
        /// <returns></returns>
        public static ProviderConfiguration GetProvider(System.IO.TextReader reader, string group)
        {
            ProviderConfiguration providers = new ProviderConfiguration();
            //providers._ProviderAttribute = filename;
            System.Xml.XmlDocument doc = new XmlDocument();
            doc.Load(reader);
            XmlNodeList list = doc.DocumentElement.SelectNodes(String.Format("providers[@group='{0}']", group));
            providers._ProviderType = doc.DocumentElement.Attributes["providerType"] == null ? "" : doc.DocumentElement.Attributes["providerType"].Value;
            foreach (XmlNode node2 in list)
            {
                if (node2.Name == "providers")
                {
                    if (node2.Attributes["providerType"] != null)
                    {
                        providers._ProviderType = node2.Attributes["providerType"].Value;
                    }
                    providers.GetProviders(node2);
                }
            }
            return providers;
        }
        /// <summary>
        /// ��xml�ļ��еõ��ṩ��Ϣ
        /// </summary>
        /// <param name="filename">�ļ���</param>
        /// <param name="group">��</param>
        /// <returns></returns>
        public static ProviderConfiguration GetProvider(string filename, string group)
        {
            if (System.IO.File.Exists(filename))
            {
                return GetProvider(System.IO.File.OpenRead(filename), group);
            }
            throw new System.IO.FileNotFoundException("û���ҵ��ļ�", filename);

        }
        public void GetProviders(XmlNode node)
        {

            try
            {
                foreach (XmlNode node2 in node.ChildNodes)
                {
                    switch (node2.Name)
                    {
                        case "add":
                            this.Providers[node2.Attributes["name"].Value] = new Provider(node2);
                            break;
                        case "remove":
                            this.Providers.Remove(node2.Attributes["name"].Value);
                            break;
                        case "clear":
                            this.Providers.Clear();
                            break;
                    }
                }
            }
            finally
            {
            }
        }
        public static ProviderConfiguration GetProvider(System.Xml.XPath.XPathNavigator nav)
        {
            ProviderConfiguration providers = new ProviderConfiguration();
            providers.GetProviders(nav);
            return providers;
        }
        void GetProviders(System.Xml.XPath.XPathNavigator nav1)
        {
            var navProviders = nav1.Select("providers");

            while (navProviders.MoveNext())
            {
                var navs = navProviders.Current.SelectChildren(System.Xml.XPath.XPathNodeType.Element);

                while (navs.MoveNext())
                {
                    var cur = navs.Current.Clone();

                    string name = cur.GetAttribute("name", string.Empty);
                    switch (cur.Name.ToLower())
                    {
                        case "add":
                            this.Providers[name] = new Provider(cur);
                            break;
                        case "remove":
                            this.Providers.Remove(name);
                            break;
                        case "clear":
                            this.Providers.Clear();
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// �ṩ�����ԣ�����Ǵ��ļ��мӲã���Щ����Ϊ�Ӳõ��ļ���
        /// </summary>
        public string ProviderType
        {
            get
            {
                return this._ProviderType;
            }
        }
        /// <summary>
        /// �����ṩ�߽ڵ���Ϣ
        /// </summary>
        public Hashtable Providers
        {
            get
            {
                return this._Providers;
            }
        }
    }


}