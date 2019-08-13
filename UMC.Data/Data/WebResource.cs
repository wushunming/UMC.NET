using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Drawing;

namespace UMC.Data
{


    /// <summary>
    ///Web提供基础资源管理类
    /// </summary>
    public abstract class WebResource : UMC.Configuration.DataProvider
    {
        private class WebResourcer : WebResource
        {

        }
        /// <summary>
        /// 图片路径
        /// </summary>
        public const string ImageResource = "~/images/";
        /// <summary>
        /// 用户资源路径
        /// </summary>
        public const string UserResources = "~/UserResources/";
        static WebResource _Instance;

        public static WebResource Instance()
        {
            if (_Instance == null)
            {
                _Instance = UMC.Data.Reflection.CreateObject("WebResource") as WebResource;
                if (_Instance == null)
                {
                    _Instance = new WebResourcer();
                    UMC.Data.Reflection.SetProperty(_Instance, "Provider", Data.Provider.Create("WebResource", "UMC.Data.WebResource"));
                }
            }
            return _Instance;
        }

        public virtual string WebDomain()
        {
            return this.Provider["domain"] ?? "/";
        }
        public virtual string AccessToken(bool isRefresh = false)
        {
            return "";

        }

        public virtual bool SubmitCheck(string appid)
        {
            return true;
        }
        public virtual System.Collections.Hashtable Submit(String method, string json, string appId = "")
        {
            return null;
        }


        public virtual void Cache<T>(String key, T value)
        {

        }
        public virtual T Cache<T>(String key)
        {
            return default(T);
        }

        /// <summary>
        /// 转换网络地址
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public virtual string ResolveUrl(string path)
        {
            var vUrl = path;
            if (path.StartsWith("~/"))
            {
                vUrl = path.Substring(1);
            }
            else if (path.StartsWith("~"))
            {
                vUrl = "/" + path.Substring(1);
            }


            return (this.Provider["src"] ?? "http://image.365lu.cn") + vUrl;// base.ResolveUrl(path);


        }

        public virtual string ResolveUrl(Guid id, object seq, object size)
        {
            var kdey = "";
            switch (size.ToString())
            {
                case "0":
                    break;
                case "1":
                    kdey = "!350";
                    break;
                case "2":
                    kdey = "!200";
                    break;
                case "3":
                    kdey = "!150";
                    break;
                case "4":
                    kdey = "!100";
                    break;
                case "5":
                    kdey = "!50";
                    break;
            }
            return ResolveUrl(String.Format("{2}{0}/{1}/0.jpg{3}", id, seq, UMC.Data.WebResource.ImageResource, kdey));

        }
        public virtual string ImageResolve(Guid id, string key, int size)
        {
            int seq = UMC.Data.Utility.Parse(key, -1);
            if (seq < 0)
            {
                seq = key.GetHashCode();
                if (seq > 0)
                {
                    seq = -seq;
                }
            }
            var kdey = "";
            switch (size)
            {
                case 0:
                    break;
                case 1:
                    kdey = "!350";
                    break;
                case 2:
                    kdey = "!200";
                    break;
                case 3:
                    kdey = "!150";
                    break;
                case 4:
                    kdey = "!100";
                    break;
                case 5:
                    kdey = "!50";
                    break;
            }
            return ResolveUrl(String.Format("{2}{0}/{1}/0.jpg{3}", id, seq, UMC.Data.WebResource.ImageResource, kdey));

        }

        public virtual void CopyResolveUrl(String source, String target)
        {
            var sourcePath = Data.Utility.MapPath(this.ResolveUrl(source));
            var targetPath = Data.Utility.MapPath(this.ResolveUrl(target));
            if (System.IO.File.Exists(sourcePath))
            {
                File.Copy(sourcePath, targetPath);
            }

        }


        public virtual string BucketName()
        {
            return "wdk";
        }
        /// <summary>
        /// 获取站点
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>


        /// <summary>
        /// 转移资源目录下文件
        /// </summary>
        /// <param name="fileName"></param>
        public virtual String Transfer(string fileName, string targetKey)
        {
            return String.Empty;
        }
        public virtual String Transfer(Stream stream, string targetKey)
        {
            return String.Empty;
        }
        public virtual void Transfer(Uri soureUrl, string targetKey)
        {
        }


        public virtual void Transfer(Uri uri, Guid guid, int seq)
        {
        }

        public virtual void Transfer(Uri uri, Guid guid, int seq, string type)
        {

        }


        public virtual void Notice(Guid tid, string title, string desc, params object[] objs)
        {

        }

        public virtual void Push(Guid tid, params object[] objs)
        {
        }

        public virtual void Push(Guid[] tid, params object[] objs)
        {
        }
        public virtual void Push(string tag, params object[] objs)
        {

        }





    }
}
