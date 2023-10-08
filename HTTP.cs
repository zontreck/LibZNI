using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace LibAC
{
    public class HTTP
    {
#pragma warning disable IDE1006 // Naming Styles
        public static HTTPReplyData performRequest(string url, string sJson)
#pragma warning restore IDE1006 // Naming Styles
        {
            HttpRequestMessage hrm = new HttpRequestMessage();
            if (sJson == "") hrm.Method = HttpMethod.Get;
            else
                hrm.Method = HttpMethod.Post;
            hrm.RequestUri = new Uri(url);
            hrm.Content = new StringContent(sJson, Encoding.UTF8, "application/json");
            return HTTP.Request(hrm);
        }
#pragma warning disable IDE1006 // Naming Styles
        public static HTTPReplyData performRequest(string url, string sJson, string xSLOwner)
#pragma warning restore IDE1006 // Naming Styles
        {
            HttpRequestMessage hrm = new HttpRequestMessage();
            if (sJson == "") hrm.Method = HttpMethod.Get;
            else
                hrm.Method = HttpMethod.Post;
            hrm.RequestUri = new Uri(url);
            hrm.Headers.Add("X-SecondLife-Owner-Key", xSLOwner);
            hrm.Content = new StringContent(sJson, Encoding.UTF8, "application/json");
            return HTTP.Request(hrm);
        }
        public static HTTPReplyData Request(HttpRequestMessage request)
        {
            HTTPReplyData rd = new HTTPReplyData();
            HttpClient client = new HttpClient();
            Task<HttpResponseMessage> t_hrm = client.SendAsync(request);
            t_hrm.Wait();

            HttpResponseMessage response = t_hrm.Result;
            

            try
            {

                rd.Code = (int)response.StatusCode;
                HttpContent cnt = response.Content;
                HttpResponseHeaders hrh = response.Headers;

                foreach (KeyValuePair<string, IEnumerable<string>> kvp in hrh)
                {
                    foreach(string key in kvp.Value)
                    {
                        rd.Headers[kvp.Key] = key;
                    }
                }
                HttpContentHeaders hch = cnt.Headers;

                foreach (KeyValuePair<string, IEnumerable<string>> kvp in hch)
                {
                    foreach (string key in kvp.Value)
                    {
                        rd.Headers[kvp.Key] = key;
                    }
                }
                rd.ContentType = rd.Headers["Content-Type"];
                Task<byte[]> bf = cnt.ReadAsByteArrayAsync();
                bf.Wait();
                rd.MessageAsBytes = bf.Result;

                rd.MessageAsString = Encoding.UTF8.GetString(bf.Result);
                


                return rd;
            }catch(Exception ex)
            {
                return rd;
            }
        }
    }
    public class HTTPReplyData
    {
        public int Code;
        public string ContentType;
        public string MessageAsString;
        public byte[] MessageAsBytes;
        public Dictionary<string, string> Headers=new Dictionary<string, string>();
    }
}
