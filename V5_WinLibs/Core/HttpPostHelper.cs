using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Runtime.InteropServices;
using System.IO;

namespace V5_WinLibs.Core {
    public class HttpPostHelper {
        private string cookieHeader = "";
        /// <summary>
        /// Cookiesֵ
        /// </summary>
        public string CookieHeader {
            get { return cookieHeader; }
            set { cookieHeader = value; }
        }
        /// <summary>
        /// Get��ȡҳ���ֵ
        /// </summary>
        /// <param name="strURL">���ʵ�ַ</param>
        /// <param name="strReferer">��Դ��վ���ߵ�ַ</param>
        /// <param name="strPageCode">��վ����</param>
        /// <returns></returns>
        public string GetPageContent(
            string strURL,
            string strReferer,
            string strPageCode) {
            string strResult = "";
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(strURL);
            Request.ContentType = "text/html";
            Request.Method = "GET";
            Request.Referer = strReferer;
            Request.Headers.Add("cookie:" + CookieHeader);
            HttpWebResponse Response = null;
            System.IO.StreamReader sr = null;
            Response = (HttpWebResponse)Request.GetResponse();
            Encoding defaultEncoding;
            if (strPageCode.Trim() == "")
                defaultEncoding = Encoding.Default;
            else
                defaultEncoding = Encoding.GetEncoding(strPageCode);
            sr = new System.IO.StreamReader(Response.GetResponseStream(), defaultEncoding);
            strResult = sr.ReadToEnd();

            Request.Abort();
            Response.Close();
            return strResult;
        }
        /// <summary>
        /// Post��ȡ����
        /// </summary>
        /// <param name="strURL"></param>
        /// <param name="strArgs"></param>
        /// <param name="strReferer"></param>
        /// <param name="strPageCode"></param>
        /// <returns></returns>
        public string PostData(string strURL,
            string strArgs,
            string strReferer,
            string strPageCode) {
            string strResult = "";
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(strURL);
            Request.AllowAutoRedirect = true;
            Request.KeepAlive = true;
            Request.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/vnd.ms-excel, application/msword, application/x-shockwave-flash, */*";
            Request.Referer = strReferer;

            Request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Maxthon; .NET CLR 2.0.50727)";
            Request.ContentType = "application/x-www-form-urlencoded";
            Request.Method = "POST";
            if (CookieHeader.Trim() != "") {
                Request.Headers.Add("cookie:" + CookieHeader);
            }
            Stream MyRequestStrearm = Request.GetRequestStream();
            StreamWriter MyStreamWriter = new StreamWriter(MyRequestStrearm, Encoding.ASCII);
            MyStreamWriter.Write(strArgs);
            MyStreamWriter.Close();
            MyRequestStrearm.Close();
            HttpWebResponse Response = null;
            System.IO.StreamReader sr = null;
            Response = (HttpWebResponse)Request.GetResponse();
            sr = new System.IO.StreamReader(Response.GetResponseStream(), Encoding.GetEncoding(strPageCode));
            strResult = sr.ReadToEnd();
            Request.Abort();
            Response.Close();
            return strResult;
        }
        public string PostData(string strURL,
            string strArgs,
            string strReferer,
            string strPageCode,
            WebProxy wp) {
            string strResult = string.Empty;
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(strURL);
            Request.AllowAutoRedirect = true;
            Request.KeepAlive = true;
            Request.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/vnd.ms-excel, application/msword, application/x-shockwave-flash, */*";
            Request.Referer = strReferer;

            Request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Maxthon; .NET CLR 2.0.50727)";
            Request.ContentType = "application/x-www-form-urlencoded";
            Request.Method = "POST";
            if (CookieHeader.Trim() != "") {
                Request.Headers.Add("cookie:" + CookieHeader);
            }
            if (wp != null)
                Request.Proxy = wp;



            Stream MyRequestStrearm = Request.GetRequestStream();
            StreamWriter MyStreamWriter = new StreamWriter(MyRequestStrearm, Encoding.ASCII);
            MyStreamWriter.Write(strArgs);
            MyStreamWriter.Close();
            MyRequestStrearm.Close();
            HttpWebResponse Response = null;
            System.IO.StreamReader sr = null;
            Response = (HttpWebResponse)Request.GetResponse();
            sr = new System.IO.StreamReader(Response.GetResponseStream(), Encoding.GetEncoding(strPageCode));
            strResult = sr.ReadToEnd();
            Request.Abort();
            Response.Close();
            return strResult;
        }
        /// <summary>
        /// Post��½ ����¼Cookies
        /// </summary>
        /// <param name="strURL">��ַ</param>
        /// <param name="strArgs">����</param>
        /// <param name="strReferer">���õ�ַ</param>
        /// <param name="strPageCode">ҳ�����</param>
        /// <returns></returns>
        public string PostLogin(
            string strURL,
            string strArgs,
            string strReferer,
            string strPageCode) {
            string strResult = "";
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(strURL);
            Request.AllowAutoRedirect = true;
            Request.KeepAlive = true;
            Request.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/vnd.ms-excel, application/msword, application/x-shockwave-flash, */*";
            Request.Referer = strReferer;

            Request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Maxthon; .NET CLR 2.0.50727)";
            Request.ContentType = "application/x-www-form-urlencoded";
            Request.Method = "POST";

            CookieCollection myCookies = null;
            CookieContainer myCookieContainer = new CookieContainer();
            Request.CookieContainer = myCookieContainer;

            Stream RequestStrearm = Request.GetRequestStream();
            StreamWriter MyStreamWriter = new StreamWriter(RequestStrearm, Encoding.ASCII);
            MyStreamWriter.Write(strArgs);
            MyStreamWriter.Close();
            RequestStrearm.Close();
            HttpWebResponse Response = null;
            System.IO.StreamReader sr = null;
            Response = (HttpWebResponse)Request.GetResponse();
            if (CookieHeader.Trim() == "") {
                CookieHeader = Request.CookieContainer.GetCookieHeader(new Uri(strURL));
            }
            myCookies = Response.Cookies;
            sr = new System.IO.StreamReader(Response.GetResponseStream(), Encoding.GetEncoding(strPageCode));
            strResult = sr.ReadToEnd();
            Request.Abort();
            Response.Close();
            return strResult;
        }

        /// <summary>
        /// �ϴ�ģ�ⷢ������
        /// </summary>
        /// <param name="strURL"></param>
        /// <param name="strArgs"></param>
        /// <param name="strReferer"></param>
        /// <param name="strPageCode"></param>
        /// <returns></returns>
        public string Post_FormData(
            string strURL,
            string strArgs,
            string strReferer,
            string strPageCode) {
            string boundary = DateTime.Now.Ticks.ToString("x"); 
            WebRequest request = WebRequest.Create(strURL);
            request.Method = "POST";
            request.ContentType = "multipart/form-data; boundary=---------------------------" + boundary;
            if (CookieHeader.Trim() != "") {
                request.Headers.Add("cookie:" + CookieHeader);
            }

            if (string.IsNullOrEmpty(strPageCode))
                strPageCode = Encoding.Default.EncodingName;
            strArgs = strArgs.Replace("${boundary}", boundary);
            byte[] postFormData = Encoding.GetEncoding(strPageCode).GetBytes(strArgs);
            long length = postFormData.Length;
            request.ContentLength = length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(postFormData, 0, postFormData.Length);
            requestStream.Close();
            ////�ļ����� 
            WebResponse response = request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(strPageCode));
            string html = sr.ReadToEnd().Trim();
            sr.Close();
            if (response != null) {
                response.Close();
                response = null;
            }
            if (request != null) {
                request = null;
            }
            return html;
        }

        public string Post_FormData(
          string strURL,
          string strArgs,
          string strReferer,
          string strPageCode,
            string boundaryType,
            string boundary) {
            WebRequest request = WebRequest.Create(strURL);
            request.Method = "POST";
            request.ContentType = "multipart/form-data; boundary=" + boundaryType;
            if (CookieHeader.Trim() != "") {
                request.Headers.Add("cookie:" + CookieHeader);
            }

            if (string.IsNullOrEmpty(strPageCode))
                strPageCode = Encoding.Default.EncodingName;
            strArgs = strArgs.Replace("${boundary}", boundary);
            byte[] postFormData = Encoding.GetEncoding(strPageCode).GetBytes(strArgs);
            long length = postFormData.Length;
            request.ContentLength = length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(postFormData, 0, postFormData.Length);
            requestStream.Close();
            ////�ļ����� 
            WebResponse response = request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(strPageCode));
            string html = sr.ReadToEnd().Trim();
            sr.Close();
            if (response != null) {
                response.Close();
                response = null;
            }
            if (request != null) {
                request = null;
            }
            return html;
        }
        /// <summary>
        /// �ϴ�ģ���¼
        /// </summary>
        /// <param name="strURL"></param>
        /// <param name="strArgs"></param>
        /// <param name="strReferer"></param>
        /// <param name="strPageCode"></param>
        /// <returns></returns>
        public string Post_FormDataLogin(
            string strURL,
            string strArgs,
            string strReferer,
            string strPageCode) {
            string boundary = DateTime.Now.Ticks.ToString("x");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strURL);
            request.Method = "POST";
            request.ContentType = "multipart/form-data; boundary=---------------------------" + boundary;

            CookieContainer myCookieContainer = new CookieContainer();
            request.CookieContainer = myCookieContainer;

            if (string.IsNullOrEmpty(strPageCode))
                strPageCode = Encoding.Default.EncodingName;
            strArgs = strArgs.Replace("${boundary}", boundary);
            byte[] postFormData = Encoding.GetEncoding(strPageCode).GetBytes(strArgs);
            long length = postFormData.Length;
            request.ContentLength = length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(postFormData, 0, postFormData.Length);
            requestStream.Close();
            ////�ļ����� 
            WebResponse response = request.GetResponse();
            if (CookieHeader.Trim() == "") {
                CookieHeader = request.CookieContainer.GetCookieHeader(new Uri(strURL));
            }
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string html = sr.ReadToEnd().Trim();
            sr.Close();
            if (response != null) {
                response.Close();
                response = null;
            }
            if (request != null) {
                request = null;
            }
            return html;
        }

        ///// <summary>
        ///// ͨ��RPC��������
        ///// </summary>
        ///// <returns></returns>


        public void PostPage(
    string url,
    ref string cookieHeader) {
            Uri myUri = new Uri(url);
            WebRequest webRequest = WebRequest.Create(myUri);
            WebResponse webResponse = webRequest.GetResponse();
            cookieHeader = webResponse.Headers["Set-Cookie"];
            webResponse.GetResponseStream();
            return;
        }

        /// <summary>
        /// ͨ��COM����ȡCookie���ݡ�
        /// </summary>
        /// <param name="url">��ǰ��ַ��</param>
        /// <param name="cookieName">CookieName.</param>
        /// <param name="cookieData">���ڱ���Cookie Data��<see cref="StringBuilder"/>ʵ����</param>
        /// <param name="size">Cookie��С��</param>
        /// <returns>����ɹ��򷵻�<c>true</c>,���򷵻�<c>false</c>��</returns>
        [DllImport("wininet.dll", SetLastError = true)]
        public static extern bool InternetGetCookie(
          string url, string cookieName,
          StringBuilder cookieData, ref int size);
        /// <summary>
        /// ��ȡ��ǰ<see cref="Uri"/>��<see cref="CookieContainer"/>ʵ����
        /// </summary>
        /// <param name="uri">��ǰ<see cref="Uri"/>��ַ��</param>
        /// <returns>��ǰ<see cref="Uri"/>��<see cref="CookieContainer"/>ʵ����</returns>
        public CookieContainer GetUriCookieContainer(Uri uri) {
            CookieContainer cookies = null;

            int datasize = 256;
            StringBuilder cookieData = new StringBuilder(datasize);

            if (!InternetGetCookie(uri.ToString(), null, cookieData,
              ref datasize)) {
                if (datasize < 0)
                    return null;

                cookieData = new StringBuilder(datasize);
                if (!InternetGetCookie(uri.ToString(), null, cookieData,
                  ref datasize))
                    return null;
            }
            if (cookieData.Length > 0) {
                cookies = new CookieContainer();
                cookies.SetCookies(uri, cookieData.ToString().Replace(';', ','));
            }
            return cookies;
        }
    }
}
