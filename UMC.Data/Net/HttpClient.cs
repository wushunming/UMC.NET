using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace UMC.Net
{
    public class HttpClient : System.Net.Http.HttpClient
    {


        public HttpClient()
        {
            this.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");


        }
        public System.IO.Stream OpenRead(Uri url)
        {
            return this.GetAsync(url).Result.Content.ReadAsStreamAsync().Result;
        }
        public System.IO.Stream OpenRead(String url)
        {
            return this.GetAsync(url).Result.Content.ReadAsStreamAsync().Result;
        }
        public byte[] DownloadData(String url)
        {
            return this.GetAsync(url).Result.Content.ReadAsByteArrayAsync().Result;
        }
        public byte[] UploadData(String url, byte[] content)
        {
            return this.PostAsync(url, new System.Net.Http.ByteArrayContent(content)).Result.Content.ReadAsByteArrayAsync().Result;
        }
    }
}


