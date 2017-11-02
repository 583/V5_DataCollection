using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace V5_Utility.Core {
    /// <summary>
    /// ��֤��
    /// </summary>
    public class ValidatorHelper {
        private static readonly ValidatorHelper m_Instance = new ValidatorHelper();
        /// <summary>
        /// 
        /// </summary>
        public static ValidatorHelper Instance {
            get { return m_Instance; }
        }
        /// <summary>
        /// ��֤
        /// </summary>
        public ValidatorHelper() {

        }
        /// <summary>
        /// �Ƿ�Ϊip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public bool IsIP(string ip) {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        /// <summary>
        /// ����Ƿ���ȷ��IP
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public bool IsIPSect(string ip) {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){2}((2[0-4]\d|25[0-5]|[01]?\d\d?|\*)\.)(2[0-4]\d|25[0-5]|[01]?\d\d?|\*)$");
        }
        /// <summary>
        /// ����Ƿ���SqlΣ���ַ�
        /// </summary>
        /// <param name="str">Ҫ�ж��ַ���</param>
        /// <returns>�жϽ��</returns>
        public bool IsSafeSqlString(string str) {
            return Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }


        /// <summary>
        /// �õ��ַ������ȣ�һ�����ֳ���Ϊ2
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static int StrLength(string inputString) {
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            int tempLen = 0;
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++) {
                if ((int)s[i] == 63)
                    tempLen += 2;
                else
                    tempLen += 1;
            }
            return tempLen;
        }

        #region ��֤���ݵ�������ʽ
        /// <summary>
        /// ����Ƿ������� 
        /// </summary>
        /// <param name="InPut">Ҫ�����ִ�</param>
        /// <returns>bool</returns>
        public static bool IsNumeric(string InPut) {
            return IsNumeric(InPut, false, false);
        }
        /// <summary>
        /// ����Ƿ�������,֧�ָ�����С��
        /// </summary>
        /// <param name="InPut"></param>
        /// <param name="AllowNegative"></param>
        /// <param name="AllowDecimal"></param>
        /// <returns></returns>
        public static bool IsNumeric(string InPut, bool AllowNegative, bool AllowDecimal) {
            if (InPut == null) {
                return false;
            }
            else {
                string pattern;
                if (AllowNegative == true && AllowDecimal == true) {
                    pattern = "^(-?\\d+)(\\.\\d+)?$";
                }
                else {
                    if (AllowNegative == false && AllowDecimal == false) {
                        pattern = "^(\\d+)$";
                    }
                    else {
                        if (AllowNegative == false) {
                            pattern = "^(-?\\d+)$";
                        }
                        else {
                            pattern = "^(\\d+)(\\.\\d+)?$";
                        }
                    }
                }
                Regex reg = new Regex(pattern);
                return reg.IsMatch(InPut);
            }
        }

        /// <summary>
        /// ����Ƿ���ȷ��Email��ʽ
        /// </summary>
        /// <param name="InPut">Ҫ�����ִ�</param>
        /// <returns>bool</returns>
        public bool IsEmail(string InPut) {
            Regex reg = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.IgnoreCase);
            return reg.IsMatch(InPut);
        }

        /// <summary>
        /// ����Ƿ���ȷ�����ڸ�ʽ
        /// </summary>
        /// <param name="InPut">Ҫ�����ִ�</param>
        /// <returns>bool</returns>
        public bool IsDate(string InPut) {
            Regex reg = new Regex(@"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$");
            return reg.IsMatch(InPut);
        }

        /// <summary>
        /// �Ƿ���SQL���
        /// </summary>
        /// <param name="InPut">Ҫ�����ִ�</param>
        /// <returns>bool</returns>
        public bool IsSQL(string InPut) {
            Regex reg = new Regex(@"\?|select%20|select\s+|insert%20|insert\s+|delete%20|delete\s+|count\(|drop%20|drop\s+|update%20|update\s+", RegexOptions.IgnoreCase);

            return reg.IsMatch(InPut);
        }
        #endregion
    }
}
