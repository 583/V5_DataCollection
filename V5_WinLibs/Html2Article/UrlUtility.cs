using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace V5_WinLibs.Html2Article {
    /// <summary>
    /// Url��������
    /// </summary>
    public class UrlUtility {
        /// <summary>
        /// ����baseUrl����ȫhtml�����е�����
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="html"></param>
        public static string FixUrl(string baseUrl, string html) {
            html = Regex.Replace(html, "(?is)(href|src)=(\"|\')([^(\"|\')]+)(\"|\')", (match) => {
                string org = match.Value;
                string link = match.Groups[3].Value;
                if (link.StartsWith("http")) {
                    return org;
                }

                try {
                    Uri uri = new Uri(baseUrl);
                    Uri thisUri = new Uri(uri, link);
                    string fullUrl = String.Format("{0}=\"{1}\"", match.Groups[1].Value, thisUri.AbsoluteUri);
                    return fullUrl;
                }
                catch (Exception) {
                    return org;
                }
            });
            return html;
        }
    }
}
