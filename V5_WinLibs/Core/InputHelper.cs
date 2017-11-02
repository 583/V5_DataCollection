using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace V5_AllFrameWorkTools._Class
{
    public class InputHelper
    {
        public static string SetString(object o)
        {
            return Convert.ToString("" + o);
        }

        public static int SetNumber(object o)
        {
            return Convert.ToInt32("0" + o);
        }

        /// <summary>           
        /// �õ��ַ����ĳ��ȣ�һ��������3���ַ�           
        /// </summary>           
        /// <param name="str">�ַ���</param>           
        /// <returns>�����ַ�������</returns>           
        public static int GetLength(string str)
        {
            if (str.Length == 0) return 0;
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            byte[] s = ascii.GetBytes(str);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 3;
                }
                else
                {
                    tempLen += 1;
                }
            }
            return tempLen;
        }

        /// <summary>
        /// md5���ܷ�ʽ
        /// </summary>
        /// <param name="str">ԭ�ַ���</param>
        /// <param name="code">16��32 λ</param>
        /// <returns>���ܺ���ַ���</returns>
        public static string MD5(string str, int code)
        {
            if (code == 16)
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "md5").ToLower().Substring(8, 16);
            }
            if (code == 32)
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "md5").ToUpper();
            }
            return str;
        }

        /// <summary>
        ///SQLע�����
        /// </summary>
        /// <param name="InText">Ҫ���˵��ַ���</param>
        /// <returns>����������ڲ���ȫ�ַ����򷵻�true</returns>
        public static bool SqlFilter(string InText)
        {
            string word = "and|exec|insert|select|delete|update|chr|mid|master|or|truncate|char|declare|join";
            if (InText == null)
                return false;
            foreach (string i in word.Split('|'))
            {
                if ((InText.ToLower().IndexOf(i + " ") > -1) || (InText.ToLower().IndexOf(" " + i) > -1))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// SQLע�����
        /// </summary>
        /// <param name="InText"></param>
        /// <returns></returns>
        public static string SqlFilterString(string InText)
        {
            string word = "'|and |exec |insert |select |delete |update |count |% |chr |mid |master |truncate |char |declare ";
            string newWord = "|���䡡|�����㡡|���������|���������|�������塡|��������塡|���������|����|����|���䡡|��������|�����������塡|�����|�������塡";
            string[] wArr = word.Split('|');
            string[] nArr = newWord.Split('|');
            for (int i = 0; i < wArr.Length; i++)
            {
                InText = Regex.Replace(InText, wArr[i], nArr[i], RegexOptions.IgnoreCase);
            }
            InText = InText.Replace("*", "��");
            return InText;
        }
    }
}
