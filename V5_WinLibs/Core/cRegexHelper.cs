using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace V5_WinLibs.Core {
    /// <summary>
    /// 
    /// </summary>
    public class cRegexHelper {
        /// <summary>
        /// href����
        /// </summary>
        public static string RegexATag = @"<a[^<>]*?hrefs*=s*[,""s]([^"",]*)[,""][^<>]*?>(.*?)</a>";
        /// <summary>
        /// ƥ���ƶ��ֻ���
        /// </summary>
        public const string PATTERN_CMCMOBILENUM = @" ^ 1(3[4-9]|5[012789]|8[78])\d{8}$";
        /// <summary>
        /// ƥ������ֻ���
        /// </summary>
        public const string PATTERN_CTCMOBILENUM = @"^18[09]\d{8}$";
        /// <summary>
        /// ƥ����ͨ�ֻ���
        /// </summary>
        public const string PATTERN_CUTMOBILENUM = @"^1(3[0-2]|5[56]|8[56])\d{8}$";
        /// <summary>
        /// �Ƿ�Ϊ�ֻ���
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsMobile(string val) {
            return Regex.IsMatch(val, @"^1[358]\d{9}$", RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// �ж��ֻ����� 0δ֪1�ƶ�2��ͨ3����
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int CheckMobileType(string val) {
            int type = 0;
            if (Regex.IsMatch(val, PATTERN_CMCMOBILENUM, RegexOptions.IgnoreCase))
                return 1;
            if (Regex.IsMatch(val, PATTERN_CUTMOBILENUM, RegexOptions.IgnoreCase))
                return 2;
            if (Regex.IsMatch(val, PATTERN_CTCMOBILENUM, RegexOptions.IgnoreCase))
                return 3;
            return type;
        }



        public static string ParseTags(string HTMLStr) {
            return Regex.Replace(HTMLStr, "<[^>]*>", "");
        }
    }
}
