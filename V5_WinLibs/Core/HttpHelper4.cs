using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.IO.Compression;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace V5_WinLibs.Core {
    /// <summary>
    /// Http���Ӳ���������
    /// </summary>
    public class HttpHelper4 {
        #region Ԥ���巽�����߱��
        private Encoding encoding = Encoding.Default;
        private Encoding postencoding = Encoding.Default;
        private HttpWebRequest request = null;
        private HttpWebResponse response = null;
        /// <summary>
        /// �����ഫ������ݣ��õ���Ӧҳ������
        /// </summary>
        /// <param name="item">���������</param>
        /// <returns>����HttpResult����</returns>
        public HttpResult GetHtml(HttpItem item) {
            HttpResult result = new HttpResult();
            try {
                SetRequest(item);
            }
            catch (Exception ex) {
                return new HttpResult() { Cookie = string.Empty, Header = null, Html = ex.Message, StatusDescription = "���ò���ʱ����" + ex.Message };
            }
            try {
                #region �õ������response
                using (response = (HttpWebResponse)request.GetResponse()) {
                    result.StatusCode = response.StatusCode;
                    result.StatusDescription = response.StatusDescription;
                    result.Header = response.Headers;
                    if (response.Cookies != null) result.CookieCollection = response.Cookies;
                    if (response.Headers["set-cookie"] != null) result.Cookie = response.Headers["set-cookie"];
                    byte[] ResponseByte = null;
                    using (MemoryStream _stream = new MemoryStream()) {
                        if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase)) {
                            new GZipStream(response.GetResponseStream(), CompressionMode.Decompress).CopyTo(_stream, 10240);
                        }
                        else {
                            response.GetResponseStream().CopyTo(_stream, 10240);
                        }
                        ResponseByte = _stream.ToArray();
                    }
                    if (ResponseByte != null & ResponseByte.Length > 0) {
                        if (item.ResultType == ResultType.Byte) result.ResultByte = ResponseByte;
                        if (encoding == null) {
                            Match meta = Regex.Match(Encoding.Default.GetString(ResponseByte), "<meta([^<]*)charset=([^<]*)[\"']", RegexOptions.IgnoreCase);
                            string c = (meta.Groups.Count > 1) ? meta.Groups[2].Value.ToLower().Trim() : string.Empty;
                            result.Encode = c;
                            if (c.Length > 2) {
                                try {
                                    if (c.IndexOf(" ") > 0) c = c.Substring(0, c.IndexOf(" "));
                                    encoding = Encoding.GetEncoding(c.Replace("\"", "").Replace("'", "").Replace(";", "").Replace("iso-8859-1", "gbk").Trim());
                                }
                                catch {
                                    if (string.IsNullOrEmpty(response.CharacterSet)) encoding = Encoding.UTF8;
                                    else encoding = Encoding.GetEncoding(response.CharacterSet);
                                }
                            }
                            else {
                                if (string.IsNullOrEmpty(response.CharacterSet)) encoding = Encoding.UTF8;
                                else encoding = Encoding.GetEncoding(response.CharacterSet);
                            }
                        }
                        result.Html = encoding.GetString(ResponseByte);
                    }
                    else {
                        result.Html = "��������δ�����κ�����";
                    }
                }
                #endregion
            }
            catch (WebException ex) {
                response = (HttpWebResponse)ex.Response;
                result.Html = ex.Message;
                if (response != null) {
                    result.StatusCode = response.StatusCode;
                    result.StatusDescription = response.StatusDescription;
                }
            }
            catch (Exception ex) {
                result.Html = ex.Message;
            }
            if (item.IsToLower) result.Html = result.Html.ToLower();
            return result;
        }
        /// <summary>
        /// Ϊ����׼������
        /// </summary>
        ///<param name="item">�����б�</param>
        private void SetRequest(HttpItem item) {
            SetCer(item);
            if (item.Header != null && item.Header.Count > 0) foreach (string key in item.Header.AllKeys) {
                    request.Headers.Add(key, item.Header[key]);
                }
            SetProxy(item);
            if (item.ProtocolVersion != null) request.ProtocolVersion = item.ProtocolVersion;
            request.ServicePoint.Expect100Continue = item.Expect100Continue;
            request.Method = item.Method;
            request.Timeout = item.Timeout;
            request.KeepAlive = item.KeepAlive;
            request.ReadWriteTimeout = item.ReadWriteTimeout;
            if (!string.IsNullOrWhiteSpace(item.Host)) {
                request.Host = item.Host;
            }
            request.Accept = item.Accept;
            request.ContentType = item.ContentType;
            request.UserAgent = item.UserAgent;
            encoding = item.Encoding;
            SetCookie(item);
            request.Referer = item.Referer;
            request.AllowAutoRedirect = item.Allowautoredirect;
            SetPostData(item);
            if (item.Connectionlimit > 0) request.ServicePoint.ConnectionLimit = item.Connectionlimit;
        }
        /// <summary>
        /// ����֤��
        /// </summary>
        /// <param name="item"></param>
        private void SetCer(HttpItem item) {
            if (!string.IsNullOrWhiteSpace(item.CerPath)) {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
                request = (HttpWebRequest)WebRequest.Create(item.URL);
                SetCerList(item);
                request.ClientCertificates.Add(new X509Certificate(item.CerPath));
            }
            else {
                request = (HttpWebRequest)WebRequest.Create(item.URL);
                SetCerList(item);
            }
        }
        /// <summary>
        /// ���ö��֤��
        /// </summary>
        /// <param name="item"></param>
        private void SetCerList(HttpItem item) {
            if (item.ClentCertificates != null && item.ClentCertificates.Count > 0) {
                foreach (X509Certificate c in item.ClentCertificates) {
                    request.ClientCertificates.Add(c);
                }
            }
        }
        /// <summary>
        /// ����Cookie
        /// </summary>
        /// <param name="item">Http����</param>
        private void SetCookie(HttpItem item) {
            if (item.ResultCookieType == ResultCookieType.String) {
                if (!string.IsNullOrEmpty(item.Cookie)) request.Headers[HttpRequestHeader.Cookie] = item.Cookie;
            }
            else if (item.ResultCookieType == ResultCookieType.CookieCollection) {
                request.CookieContainer = new CookieContainer();
                if (item.CookieCollection != null && item.CookieCollection.Count > 0)
                    request.CookieContainer.Add(item.CookieCollection);
            }
        }
        /// <summary>
        /// ����Post����
        /// </summary>
        /// <param name="item">Http����</param>
        private void SetPostData(HttpItem item) {
            if (request.Method.Trim().ToLower().Contains("post")) {
                if (item.PostEncoding != null) {
                    postencoding = item.PostEncoding;
                }
                byte[] buffer = null;
                if (item.PostDataType == PostDataType.Byte && item.PostdataByte != null && item.PostdataByte.Length > 0) {
                    buffer = item.PostdataByte;
                }
                else if (item.PostDataType == PostDataType.FilePath && !string.IsNullOrWhiteSpace(item.Postdata)) {
                    StreamReader r = new StreamReader(item.Postdata, postencoding);
                    buffer = postencoding.GetBytes(r.ReadToEnd());
                    r.Close();
                } 
                else if (!string.IsNullOrWhiteSpace(item.Postdata)) {
                    buffer = postencoding.GetBytes(item.Postdata);
                }
                if (buffer != null) {
                    request.ContentLength = buffer.Length;
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                }
            }
        }
        /// <summary>
        /// ���ô���
        /// </summary>
        /// <param name="item">��������</param>
        private void SetProxy(HttpItem item) {
            if (!string.IsNullOrWhiteSpace(item.ProxyIp)) {
                if (item.ProxyIp.Contains(":")) {
                    string[] plist = item.ProxyIp.Split(':');
                    WebProxy myProxy = new WebProxy(plist[0].Trim(), Convert.ToInt32(plist[1].Trim()));
                    myProxy.Credentials = new NetworkCredential(item.ProxyUserName, item.ProxyPwd);
                    request.Proxy = myProxy;
                }
                else {
                    WebProxy myProxy = new WebProxy(item.ProxyIp, false);
                    myProxy.Credentials = new NetworkCredential(item.ProxyUserName, item.ProxyPwd);
                    request.Proxy = myProxy;
                }
                request.Credentials = CredentialCache.DefaultCredentials;
            }
            else if (item.WebProxy != null) {
                request.Proxy = item.WebProxy;
            }
        }
        /// <summary>
        /// �ص���֤֤������
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="certificate">֤��</param>
        /// <param name="chain">X509Chain</param>
        /// <param name="errors">SslPolicyErrors</param>
        /// <returns>bool</returns>
        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) { return true; }
        #endregion
    }
    /// <summary>
    /// Http����ο���
    /// </summary>
    public class HttpItem {
        /// <summary>
        /// ����URL������д
        /// </summary>
        public string URL { get; set; }
        string _Method = "GET";
        /// <summary>
        /// ����ʽĬ��ΪGET��ʽ,��ΪPOST��ʽʱ��������Postdata��ֵ
        /// </summary>
        public string Method {
            get { return _Method; }
            set { _Method = value; }
        }
        int _Timeout = 100000;
        /// <summary>
        /// Ĭ������ʱʱ��
        /// </summary>
        public int Timeout {
            get { return _Timeout; }
            set { _Timeout = value; }
        }
        int _ReadWriteTimeout = 30000;
        /// <summary>
        /// Ĭ��д��Post���ݳ�ʱ��
        /// </summary>
        public int ReadWriteTimeout {
            get { return _ReadWriteTimeout; }
            set { _ReadWriteTimeout = value; }
        }
        /// <summary>
        /// ����Host�ı�ͷ��Ϣ
        /// </summary>
        public string Host { get; set; }
        Boolean _KeepAlive = true;
        /// <summary>
        ///  ��ȡ������һ��ֵ����ֵָʾ�Ƿ��� Internet ��Դ�����־�������Ĭ��Ϊtrue��
        /// </summary>
        public Boolean KeepAlive {
            get { return _KeepAlive; }
            set { _KeepAlive = value; }
        }
        string _Accept = "text/html, application/xhtml+xml, */*";
        /// <summary>
        /// �����ͷֵ Ĭ��Ϊtext/html, application/xhtml+xml, */*
        /// </summary>
        public string Accept {
            get { return _Accept; }
            set { _Accept = value; }
        }
        string _ContentType = "text/html";
        /// <summary>
        /// ���󷵻�����Ĭ�� text/html application/x-www-form-urlencoded;
        /// </summary>
        public string ContentType {
            get { return _ContentType; }
            set { _ContentType = value; }
        }
        string _UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
        /// <summary>
        /// �ͻ��˷�����ϢĬ��Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)
        /// </summary>
        public string UserAgent {
            get { return _UserAgent; }
            set { _UserAgent = value; }
        }
        /// <summary>
        /// �������ݱ���Ĭ��ΪNUll,�����Զ�ʶ��,һ��Ϊutf-8,gbk,gb2312
        /// </summary>
        public Encoding Encoding { get; set; }
        private PostDataType _PostDataType = PostDataType.String;
        /// <summary>
        /// Post����������
        /// </summary>
        public PostDataType PostDataType {
            get { return _PostDataType; }
            set { _PostDataType = value; }
        }
        /// <summary>
        /// Post����ʱҪ���͵��ַ���Post����
        /// </summary>
        public string Postdata { get; set; }
        /// <summary>
        /// Post����ʱҪ���͵�Byte���͵�Post����
        /// </summary>
        public byte[] PostdataByte { get; set; }
        /// <summary>
        /// Cookie���󼯺�
        /// </summary>
        public CookieCollection CookieCollection { get; set; }
        /// <summary>
        /// ����ʱ��Cookie
        /// </summary>
        public string Cookie { get; set; }
        /// <summary>
        /// ��Դ��ַ���ϴη��ʵ�ַ
        /// </summary>
        public string Referer { get; set; }
        /// <summary>
        /// ֤�����·��
        /// </summary>
        public string CerPath { get; set; }
        /// <summary>
        /// ���ô������
        /// </summary>
        public WebProxy WebProxy { get; set; }
        private Boolean isToLower = false;
        /// <summary>
        /// �Ƿ�����Ϊȫ��Сд��Ĭ��Ϊ��ת��
        /// </summary>
        public Boolean IsToLower {
            get { return isToLower; }
            set { isToLower = value; }
        }
        private Boolean allowautoredirect = false;
        /// <summary>
        /// ֧����תҳ�棬��ѯ���������ת���ҳ�棬Ĭ���ǲ���ת
        /// </summary>
        public Boolean Allowautoredirect {
            get { return allowautoredirect; }
            set { allowautoredirect = value; }
        }
        private int connectionlimit = 1024;
        /// <summary>
        /// ���������
        /// </summary>
        public int Connectionlimit {
            get { return connectionlimit; }
            set { connectionlimit = value; }
        }
        /// <summary>
        /// ����Proxy �������û���
        /// </summary>
        public string ProxyUserName { get; set; }
        /// <summary>
        /// ���� ����������
        /// </summary>
        public string ProxyPwd { get; set; }
        /// <summary>
        /// ���� ����IP
        /// </summary>
        public string ProxyIp { get; set; }
        private ResultType resulttype = ResultType.String;
        /// <summary>
        /// ���÷�������String��Byte
        /// </summary>
        public ResultType ResultType {
            get { return resulttype; }
            set { resulttype = value; }
        }
        private WebHeaderCollection header = new WebHeaderCollection();
        /// <summary>
        /// header����
        /// </summary>
        public WebHeaderCollection Header {
            get { return header; }
            set { header = value; }
        }
        /// <summary>
        /// </summary>
        public Version ProtocolVersion { get; set; }
        private Boolean _expect100continue = true;
        /// <summary>
        ///  ��ȡ������һ�� System.Boolean ֵ����ֵȷ���Ƿ�ʹ�� 100-Continue ��Ϊ����� POST ������Ҫ 100-Continue ��Ӧ����Ϊ true������Ϊ false��Ĭ��ֵΪ true��
        /// </summary>
        public Boolean Expect100Continue {
            get { return _expect100continue; }
            set { _expect100continue = value; }
        }
        /// <summary>
        /// ����509֤�鼯��
        /// </summary>
        public X509CertificateCollection ClentCertificates { get; set; }
        /// <summary>
        /// ���û��ȡPost��������,Ĭ�ϵ�ΪDefault����
        /// </summary>
        public Encoding PostEncoding { get; set; }
        private ResultCookieType _ResultCookieType = ResultCookieType.String;
        /// <summary>
        /// Cookie��������,Ĭ�ϵ���ֻ�����ַ�������
        /// </summary>
        public ResultCookieType ResultCookieType {
            get { return _ResultCookieType; }
            set { _ResultCookieType = value; }
        }
    }
    /// <summary>
    /// Http���ز�����
    /// </summary>
    public class HttpResult {
        /// <summary>
        /// Http���󷵻ص�Cookie
        /// </summary>
        public string Cookie { get; set; }
        /// <summary>
        /// Cookie���󼯺�
        /// </summary>
        public CookieCollection CookieCollection { get; set; }
        /// <summary>
        /// ���ص�String�������� ֻ��ResultType.Stringʱ�ŷ������ݣ��������Ϊ��
        /// </summary>
        public string Html { get; set; }
        /// <summary>
        /// ���ص�Byte���� ֻ��ResultType.Byteʱ�ŷ������ݣ��������Ϊ��
        /// </summary>
        public byte[] ResultByte { get; set; }
        /// <summary>
        /// header����
        /// </summary>
        public WebHeaderCollection Header { get; set; }
        /// <summary>
        /// ����״̬˵��
        /// </summary>
        public string StatusDescription { get; set; }
        /// <summary>
        /// ����״̬��,Ĭ��ΪOK
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
        /// <summary>
        /// ������ҳ����
        /// </summary>
        public string Encode { get; set; }
    }
    /// <summary>
    /// ��������
    /// </summary>
    public enum ResultType {
        /// <summary>
        /// ��ʾֻ�����ַ��� ֻ��Html������
        /// </summary>
        String,
        /// <summary>
        /// ��ʾ�����ַ������ֽ��� ResultByte��Html�������ݷ���
        /// </summary>
        Byte
    }
    /// <summary>
    /// Post�����ݸ�ʽĬ��Ϊstring
    /// </summary>
    public enum PostDataType {
        /// <summary>
        /// �ַ������ͣ���ʱ����Encoding�ɲ�����
        /// </summary>
        String,
        /// <summary>
        /// Byte���ͣ���Ҫ����PostdataByte������ֵ����Encoding������Ϊ��
        /// </summary>
        Byte,
        /// <summary>
        /// ���ļ���Postdata��������Ϊ�ļ��ľ���·������������Encoding��ֵ
        /// </summary>
        FilePath
    }
    /// <summary>
    /// Cookie��������
    /// </summary>
    public enum ResultCookieType {
        /// <summary>
        /// ֻ�����ַ������͵�Cookie
        /// </summary>
        String,
        /// <summary>
        /// CookieCollection��ʽ��Cookie����ͬʱҲ����String���͵�cookie
        /// </summary>
        CookieCollection
    }
}