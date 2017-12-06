using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace V5_WinLibs.Core {

    public class StringHelper {
        private static readonly StringHelper m_Instance = new StringHelper();
        /// <summary>
        /// 
        /// </summary>
        public static StringHelper Instance {
            get { return m_Instance; }
        }
        /// <summary>
        /// ת����int����
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int SetNumber(object str) {
            return int.Parse("0" + str);
        }

        /// <summary>
        /// ת����string����
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string SetString(object str) {
            return Convert.ToString("" + str);
        }

        /// <summary>
        /// ����A��ǩ�� ָ���ַ��Ĺ���
        /// </summary>
        /// <param name="htmlStr">Ҫ���˵�html����</param>
        /// <param name="noReplaceStr">���˸�ʽ</param>
        /// <returns></returns>
        public string[] GetRegAValue(string htmlStr, string noReplaceStr) {
            Regex regObj = new Regex("<a.+?>(.+?)</a>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            string[] strAry = new string[regObj.Matches(htmlStr).Count];
            int i = 0;
            string matStr = "";
            string reStr = noReplaceStr;
            string[] arrReStr = reStr.Split(new string[] { "||" }, StringSplitOptions.None);
            foreach (Match matchItem in regObj.Matches(htmlStr)) {
                matStr = matchItem.Value;
                foreach (string s in arrReStr) {
                    if (matStr.IndexOf(s) > -1) {
                        strAry[i] = "";
                        break;
                    }
                    else if (matStr.IndexOf("href") < 0) {
                        strAry[i] = "";
                        break;
                    }
                    else {
                        strAry[i] = matchItem.Value;
                    }
                }
                i++;
            }
            return strAry;
        }

        /// <summary>
        /// md5���ܷ�ʽ
        /// </summary>
        /// <param name="str">ԭ�ַ���</param>
        /// <param name="code">16��32 λ</param>
        /// <returns>���ܺ���ַ���</returns>
        public string MD5(string str, int code) {
            if (code == 16) {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "md5").ToLower().Substring(8, 16);
            }
            if (code == 32) {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "md5").ToUpper();
            }
            return str;
        }

        /// <summary>
        /// ʹ��vb���滻�ַ���
        /// </summary>
        /// <param name="source"></param>
        /// <param name="oldstring"></param>
        /// <param name="newstring"></param>
        /// <returns></returns>
        public string ReplaceString(string source, string oldstring, string newstring) {
            return Strings.Replace(source, oldstring, newstring, 1, -1, CompareMethod.Text);
        }

        public string Replace(string source, string oldstring, string newstring) {
            return Strings.Replace(source, oldstring, newstring, 1, -1, CompareMethod.Text);
        }
        /// <summary>
        /// ������ʽ��ѯ�ַ���������
        /// </summary>
        /// <param name="Tag">��ǩ�ַ���</param>
        /// <param name="TagName">��ǩ����</param>
        /// <returns></returns>
        public string TagVal(string Tag, string TagName) {
            string[] strArray = Tag.Split(new string[] { "||" }, StringSplitOptions.None);
            for (int i = 0; i < strArray.Length; i++) {
                Regex regex = new Regex(@"(?<Keyword>\w+)\s*=\s*(?<Value>.*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                for (Match match = regex.Match(strArray[i]); match.Success; match = match.NextMatch()) {
                    if (match.Groups["Keyword"].ToString().ToLower().IndexOf(TagName.ToLower()) != -1) {
                        return match.Groups["Value"].ToString().ToLower();
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Http�������ݴ��ݱ���
        /// </summary>
        /// <param name="sourcetxt">�ַ�������</param>
        /// <param name="encode">����  gb2312 utf-8 gbk ��</param>
        /// <returns></returns>
        public string UrlEncode(string sourcetxt, string encode) {
            if (sourcetxt != null) {
                sourcetxt = System.Web.HttpUtility.UrlEncode(sourcetxt, Encoding.GetEncoding(encode));
                return sourcetxt;
            }
            else {
                return string.Empty;
            }
        }
    }
}
