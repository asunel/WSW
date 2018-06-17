namespace WSW.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using WSW.Performance;

    public static class Extensions
    {
        public static List<TSource> ToListCasted<TSource>(this ConfigurationElementCollection source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            return source.Cast<TSource>().ToList();
        }

        public static string Bold(this string @this) => $"<b>{@this}</b>";
        public static string ToHtml(this Dictionary<string, string> dic, string title)
        {
            using (MetricTracker.Track(MethodBase.GetCurrentMethod()))
            {
                var table = new HtmlTable();
                var html = string.Format(Constants.TitleFormat, title);

                var flag = false;
                var isFirstItem = true;

                foreach (var item in dic)
                {
                    table.Rows.Add(new HtmlTableRow
                    {
                        Cells = { new HtmlTableCell { InnerHtml = item.Key }, new HtmlTableCell { InnerHtml = item.Value } },
                        BgColor = isFirstItem ? Constants.HeaderColor : (flag ? Constants.Grey : Constants.White)
                    });

                    isFirstItem = false;
                    flag = !flag;
                }

                using (var sw = new StringWriter())
                {
                    table.RenderControl(new HtmlTextWriter(sw));
                    html += sw.ToString();
                }

                return html;
            }
        }
    }
}
