using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using UMC.Data;

namespace UMC.Demo
{
    /// <summary>
    /// DEMO使用的文件数据库，请在正式环境中使用服务性数据库
    /// </summary>
    public class SQLiteProvider : UMC.Data.Sql.DbCommonProvider
    {
        public override string AppendDbParameter(string key, object obj, DbCommand cmd)
        {
            if (obj is Guid)
            {
                obj = obj.ToString();
            }
            return base.AppendDbParameter(key, obj, cmd);
        }
        public override string ConntionString
        {
            get
            {

                System.Data.SQLite.SQLiteConnectionStringBuilder connstr = new System.Data.SQLite.SQLiteConnectionStringBuilder();
                connstr.DataSource = UMC.Data.Utility.MapPath("~App_Data/umc.db");
                return connstr.ToString();
            }
        }


        public override DbProviderFactory DbFactory => System.Data.SQLite.SQLiteFactory.Instance;

        public override string GetIdentityText(string tableName)
        {
            throw new NotImplementedException();
        }

        public override string GetPaginationText(int start, int limit, string selectText)
        {
            return String.Format("{0} limit {1} offset {2}", selectText, limit, start);
        }
    }

    public class WebResource : UMC.Data.WebResource
    {


        public override string ResolveUrl(string path)
        {
            String vUrl = path;
            if (path.StartsWith("~/"))
            {
                vUrl = path.Substring(1);
            }
            else if (path.StartsWith("~"))
            {
                vUrl = "/" + path.Substring(1);
            }
            String src = this.Provider["src"];
            if (String.IsNullOrEmpty(src))
            {

                String vpath = this.Provider["authkey"];

                if (String.IsNullOrEmpty(vpath) == false)
                {
                    String code = Utility.Parse36Encode(Utility.Guid(vpath).Value.GetHashCode());
                    vpath = code;// + "/";
                }

                return String.Format("http://image.365lu.cn/{0}{1}", vpath, vUrl);


            }
            return src + vUrl;
        }
        public override void Transfer(Uri src, Guid guid, int seq, String type)
        {

            String vpath = this.Provider["authkey"];
            if (String.IsNullOrEmpty(vpath) == false)
            {
                String code = Utility.Parse36Encode(Utility.Guid(vpath).Value.GetHashCode());
                vpath = code + "/";


                String key = String.Format("{0}images/{1}/{2}/{3}.{4}", vpath, guid, seq, 0, type.ToLower());


                String sts = String.Format("http://api.365lu.cn/OSS/Transfer/{0}", this.Provider["authkey"]);

                var webClient = new System.Net.Http.HttpClient();


                var t = webClient.PostAsync(sts, new System.Net.Http.StringContent(Data.JSON.Serialize(new Web.WebMeta().Put("src", src.AbsoluteUri, "key", key))))
                        .Result.Content.ReadAsStringAsync().Result;



            }

        }

    }
}