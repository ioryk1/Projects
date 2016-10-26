using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace WebServiceForEFGP.Models {
    public class UtilitySvc {

        public static void writeLog(string str) {

            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".txt";

            string path = System.Web.HttpContext.Current.Server.MapPath("~/logs/");

            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
                        
            File.AppendAllText(path + fileName, str + "\n");

            
        }


        #region base64Convert
        /// <summary>
        /// 加密成 base64 string 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string base64Encode(string str) {
            string ret = Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
            return ret;
        }

        /// <summary>
        /// 反解 base64 string to 明文
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string base64Decode(string str) {
            string ret = "";
            try {
                ret = Encoding.UTF8.GetString(Convert.FromBase64String(str));
            } catch (Exception) {
                ret = str + "- decode fail";
            }
            return ret;
        }
        #endregion


        #region HttpRequest

        /// <summary>
        /// get request json
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string getRequest(string url, Dictionary<string, object> headers) {
            string ret = "";

            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/json";

            if (headers != null && headers.Keys.Count > 0) {
                /* add header */
                foreach (var h in headers) {
                    request.Headers.Add(h.Key.ToString(), h.Value.ToString());
                }
            }

            using (WebResponse response = request.GetResponse()) {
                StreamReader sr = new StreamReader(response.GetResponseStream());
                ret = sr.ReadToEnd();
                sr.Close();
            }



            return ret;
        }
        /// <summary>
        /// post request json
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postStr"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string postRequest(string url, string postStr, Dictionary<string, object> headers) {
            string ret = "";

            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json";

            byte[] bs = Encoding.UTF8.GetBytes(postStr);

            if (headers != null && headers.Keys.Count > 0) {
                foreach (var h in headers) {
                    request.Headers.Add(h.Key.ToString(), h.Value.ToString());
                }
            }

            using (Stream reqStream = request.GetRequestStream()) {
                reqStream.Write(bs, 0, bs.Length);
            }

            using (WebResponse response = request.GetResponse()) {
                StreamReader sr = new StreamReader(response.GetResponseStream());
                ret = sr.ReadToEnd();
                sr.Close();
            }




            return ret;
        }


        /// <summary>
        /// post form request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postStr"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string formPostRequest(string url, string postStr, Dictionary<string, object> headers) {
            string ret = "";

            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            byte[] bs = Encoding.UTF8.GetBytes(postStr);

            if (headers != null && headers.Keys.Count > 0) {
                /* add header */
            }

            using (Stream reqStream = request.GetRequestStream()) {
                reqStream.Write(bs, 0, bs.Length);
            }

            using (WebResponse response = request.GetResponse()) {
                StreamReader sr = new StreamReader(response.GetResponseStream());
                ret = sr.ReadToEnd();
                sr.Close();
            }



            return ret;
        }



        #endregion

        public static string alertMsg(string msg) {
            return @"<script type='text/javascript'>alert('" + msg + "')</script>";
        }

        public static string parseGridValue(string str) {
            return str.Replace("<#leftQuot>", "[").Replace("<#rightQuot>", "]").Replace("<#quot>", "'").Replace("<#slash>", "'")
                .Replace("<br />", Environment.NewLine).Replace("&lt;br /&gt;", Environment.NewLine);
        }

        public static bool trySetXmlDocInnerText(ref XmlDocument doc, string path, string value) {
            bool ret = false;
            try {
                XmlNode node = doc.SelectSingleNode(path);
                XmlElement ele = (XmlElement)node;
                ele.InnerText = value;
                ret = true;
            } catch (Exception) {
                ret = false;
            }
            return ret;
        }

        public static async Task<string> writeLogAsync(HttpRequestMessage req) {
            string str = await req.Content.ReadAsStringAsync();


            return str;
        }

    }
}
