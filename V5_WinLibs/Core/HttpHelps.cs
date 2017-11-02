/// <summary>
/// ��˵����HttpHelps�࣬����ʵ��Http���ʣ�Post����Get��ʽ�ģ�ֱ�ӷ��ʣ���Cookie�ģ���֤��ĵȷ�ʽ���������ô���
/// �������ڣ�2011-08-20
/// �� �� �ˣ�  �շ�
/// ��ϵ��ʽ��361983679  Email��sufei.1013@163.com  Blogs:http://sufei.cnblogs.com
/// �޸����ڣ�2011-12-30
/// </summary>
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.IO.Compression;
namespace V5_WinLibs.Core {
    public class HttpHelps {
        #region Ԥ���巽�����߱��

        public Encoding encoding = Encoding.Default;
        public HttpWebRequest request = null;
        private HttpWebResponse response = null;
        public Boolean isToLower = true;
        private StreamReader reader = null;
        private string returnData = "String Error";

        /// <summary>
        /// �����ഫ������ݣ��õ���Ӧҳ������
        /// </summary>
        /// <param name="strPostdata">���������Post��ʽ,get��ʽ��NUll���߿��ַ���������</param>
        /// <returns>string���͵���Ӧ����</returns>
        private string GetHttpRequestData(string strPostdata) {
            try {
                request.AllowAutoRedirect = true;

                if (!string.IsNullOrEmpty(strPostdata) && request.Method.Trim().ToLower().Contains("post")) {
                    byte[] buffer = encoding.GetBytes(strPostdata);
                    request.ContentLength = buffer.Length;
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                }

                ////���������
                #region �õ������response

                using (response = (HttpWebResponse)request.GetResponse()) {
                    if (encoding == null) {
                        MemoryStream _stream = new MemoryStream();
                        if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase)) {
                            _stream = GetMemoryStream(response.GetResponseStream());
                        }
                        else {
                            _stream = GetMemoryStream(response.GetResponseStream());
                        }
                        byte[] RawResponse = _stream.ToArray();
                        string temp = Encoding.Default.GetString(RawResponse, 0, RawResponse.Length);
                        Match meta = Regex.Match(temp, "<meta([^<]*)charset=([^<]*)[\"']", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string charter = (meta.Groups.Count > 2) ? meta.Groups[2].Value : string.Empty;
                        charter = charter.Replace("\"", string.Empty).Replace("'", string.Empty).Replace(";", string.Empty);
                        if (charter.Length > 0) {
                            charter = charter.ToLower().Replace("iso-8859-1", "gbk");
                            encoding = Encoding.GetEncoding(charter);
                        }
                        else {
                            if (response.CharacterSet.ToLower().Trim() == "iso-8859-1") {
                                encoding = Encoding.GetEncoding("gbk");
                            }
                            else {
                                if (string.IsNullOrEmpty(response.CharacterSet.Trim())) {
                                    encoding = Encoding.UTF8;
                                }
                                else {
                                    encoding = Encoding.GetEncoding(response.CharacterSet);
                                }
                            }
                        }
                        returnData = encoding.GetString(RawResponse);
                    }
                    else {
                        if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase)) {
                            using (reader = new StreamReader(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress), encoding)) {
                                returnData = reader.ReadToEnd();
                            }
                        }
                        else {
                            using (reader = new StreamReader(response.GetResponseStream(), encoding)) {
                                returnData = reader.ReadToEnd();
                            }
                        }
                    }
                }

                #endregion
            }
            catch (WebException ex) {
                returnData = "String Error";
                response = (HttpWebResponse)ex.Response;
            }
            if (isToLower) {
                returnData = returnData.ToLower();
            }
            return returnData;
        }

        /// <summary>
        /// 4.0����.net�汾ȡˮ��
        /// </summary>
        /// <param name="streamResponse"></param>
        private static MemoryStream GetMemoryStream(Stream streamResponse) {
            MemoryStream _stream = new MemoryStream();
            int Length = 256;
            Byte[] buffer = new Byte[Length];
            int bytesRead = streamResponse.Read(buffer, 0, Length);
            while (bytesRead > 0) {
                _stream.Write(buffer, 0, bytesRead);
                bytesRead = streamResponse.Read(buffer, 0, Length);
            }
            return _stream;
        }

        /// <summary>
        /// Ϊ����׼������
        /// </summary>
        /// <param name="_URL">�����URL��ַ</param>
        /// <param name="_Method">����ʽGet����Post</param>
        /// <param name="_Accept">Accept</param>
        /// <param name="_ContentType">ContentType��������</param>
        /// <param name="_UserAgent">UserAgent�ͻ��˵ķ������ͣ�����������汾�Ͳ���ϵͳ��Ϣ</param>
        /// <param name="_Encoding">��ȡ����ʱ�ı��뷽ʽ</param>
        private void SetRequest(string _URL, string _Method, string _Accept, string _ContentType, string _UserAgent, Encoding _Encoding) {
            request = (HttpWebRequest)WebRequest.Create(GetUrl(_URL));
            request.Method = _Method;
            request.Accept = _Accept;
            request.ContentType = _ContentType;
            request.UserAgent = _UserAgent;
            encoding = _Encoding;
        }

        /// <summary>
        /// ���õ�ǰ����ʹ�õĴ���
        /// </summary>
        /// <param name="userName">���� �������û���</param>
        /// <param name="passWord">���� ����������</param>
        /// <param name="ip">���� ��������ַ</param>
        public void SetWebProxy(string userName, string passWord, string ip) {
            WebProxy myProxy = new WebProxy(ip, false);
            myProxy.Credentials = new NetworkCredential(userName, passWord);
            request.Proxy = myProxy;
            request.Credentials = CredentialCache.DefaultNetworkCredentials;
        }

        #endregion

        #region ��ͨ����
        /// <summary>    
        /// ����һ����ȷ����ȷ��URl��������ȷ��URL
        /// </summary>    
        /// <param name="URL">url</param>   
        /// <returns>
        /// </returns>    
        public static string GetUrl(string URL) {
            if (!(URL.Contains("http://") || URL.Contains("https://"))) {
                URL = "http://" + URL;
            }
            return URL;
        }

        /// <summary>
        /// ����httpsЭ��GET|POST��ʽ��������,���ݴ����URl��ַ���õ���Ӧ�������ַ�����
        /// </summary>
        /// <param name="_URL"></param>
        /// <param name="_Method">����ʽGet����Post</param>
        /// <param name="_Accept">Accept</param>
        /// <param name="_ContentType">ContentType��������</param>
        /// <param name="_UserAgent">UserAgent�ͻ��˵ķ������ͣ�����������汾�Ͳ���ϵͳ��Ϣ</param>
        /// <param name="_Encoding">��ȡ����ʱ�ı��뷽ʽ</param>
        /// <param name="_Postdata">ֻ��_MethodΪPost��ʽʱ����Ҫ����ֵ</param>
        /// <returns>����HtmlԴ����</returns>
        public string GetHttpRequestString(string _URL, string _Method, string _Accept, string _ContentType, string _UserAgent, Encoding _Encoding, string _Postdata) {
            SetRequest(_URL, _Method, _Accept, _ContentType, _UserAgent, _Encoding);
            return GetHttpRequestData(_Postdata);
        }

        ///<summary>
        ///����httpsЭ��GET��ʽ��������,���ݴ����URl��ַ���õ���Ӧ�������ַ�����
        ///</summary>
        ///<param name="URL">url��ַ</param>
        ///<param name="objencoding">���뷽ʽ���磺System.Text.Encoding.UTF8;</param>
        ///<returns>String���͵�����</returns>
        public string GetHttpRequestStringByNUll_Get(string URL, Encoding objencoding) {
            SetRequest(URL, "GET", "text/html, application/xhtml+xml, */*", "text/html", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)", objencoding);
            return GetHttpRequestData("");
        }

        ///<summary>
        ///����httpsЭ��GET��ʽ��������,���ݴ����URl��ַ���õ���Ӧ�������ַ�����
        ///</summary>
        ///<param name="URL">url��ַ</param>
        ///<param name="objencoding">���뷽ʽ���磺System.Text.Encoding.UTF8;</param>
        ///<param name="stgrcookie">Cookie�ַ���</param>
        ///<returns>String���͵�����</returns>
        public string GetHttpRequestStringByNUll_GetBycookie(string URL, Encoding objencoding, string stgrcookie) {
            SetRequest(URL, "GET", "text/html, application/xhtml+xml, */*", "text/html", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)", objencoding);
            request.Headers[HttpRequestHeader.Cookie] = stgrcookie;
            return GetHttpRequestData("");
        }

        ///<summary>
        ///����httpsЭ��GET��ʽ��������,���ݴ����URl��ַ���õ���Ӧ�������ַ�����
        ///</summary>
        ///<param name="URL">url��ַ</param>
        ///<param name="objencoding">���뷽ʽ���磺System.Text.Encoding.UTF8;</param>
        ///<returns>String���͵�����</returns>
        public string GetHttpRequestStringByNUll_Get(string URL, Encoding objencoding, string _Accept, string useragent) {
            SetRequest(URL, "GET", _Accept, "text/html", useragent, objencoding);
            return GetHttpRequestData("");
        }

        ///<summary>
        ///����httpsЭ��Post��ʽ��������,���ݴ����URl��ַ���õ���Ӧ�������ַ�����
        ///</summary>
        ///<param name="URL">url��ַ</param>
        ///<param name="strPostdata">Post���͵�����</param>
        ///<param name="objencoding">���뷽ʽ���磺System.Text.Encoding.UTF8;</param>
        ///<returns>String���͵�����</returns>
        public string GetHttpRequestStringByNUll_Post(string URL, string strPostdata, Encoding objencoding) {
            SetRequest(URL, "post", "text/html, application/xhtml+xml, */*,zh-CN", "text/html", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)", objencoding);
            return GetHttpRequestData(strPostdata);
        }

        #endregion
    }
}
