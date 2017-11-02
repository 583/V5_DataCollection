using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace V5_WinLibs.Html2Article {
    public class TestHelper {

        public void Test1() {

            var html = "";

            Html2Article.AppendMode = false;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Article article = Html2Article.GetArticle(html);
            sw.Stop();

            StringBuilder sbContent = new StringBuilder();
            sbContent.AppendLine("��ȡ��ʱ��" + Environment.NewLine + sw.ElapsedMilliseconds + "����");
            sbContent.AppendLine(article.PublishDate.ToString());
            sbContent.AppendLine(article.Title);
            sbContent.AppendLine(article.Content);
            sbContent.AppendLine("����:");
            sbContent.AppendLine(UrlUtility.FixUrl("#", article.ContentWithTags));


        }
    }
}
