using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace FMI_web.Pages.Shared
{
    public class _HeaderModel : PageModel
    {
        public HtmlString GenerateMenuMain(string parentName)
        {
            StringBuilder sbMenu = new StringBuilder();
            if (!parentName.Contains('&'))
            {
                sbMenu.Append("<li><form method=\"post\" action=\"/Maineditor\">");
                sbMenu.Append("<input type=\"hidden\" name=\"typeofedit\" value=\"add\" />");
                sbMenu.Append("<input type=\"hidden\" name=\"pageclass\" value=\"main\" />");
                sbMenu.Append("<input type=\"hidden\" name=\"parent\" value=\"" + parentName + "\" />");
                sbMenu.Append("<input type=\"submit\" value=\"Створити сторінку\" />");
                sbMenu.Append("</form></li>");
            }
            foreach (var item in Hashtables.MainPages)
            {
                string[] parents = item.Key.Split('&');
                string thisParentName = "";
                for (int i = 0; i < parents.Length - 1; i++)
                {
                    if (i == parents.Length - 2)
                        thisParentName += parents[i];
                    else
                        thisParentName += $"{parents[i]}&";
                }
                if (parentName != thisParentName)
                    continue;
                switch (item.Value["Type"])
                {
                    case "list":
                        if (parents.Length >= 2)
                        {
                            sbMenu.Append("<li class=\"parent\">");
                            sbMenu.Append("<form class=\"editForm\" method=\"post\" action=\"/Maineditor\">");
                            sbMenu.Append("<input type=\"hidden\" name=\"typeofedit\" value=\"edit\" />");
                            sbMenu.Append("<input type=\"hidden\" name=\"pageclass\" value=\"main\" />");
                            sbMenu.Append("<input type=\"hidden\" name=\"fullpagename\" value=\"" + item.Key + "\" />");
                            sbMenu.Append("<input class=\"editPage\" type=\"submit\" value=\"\" />");
                            sbMenu.Append("</form>");
                            sbMenu.Append("<span>" + item.Value["Name"] + "</span>");
                            sbMenu.Append("<img src=\"/icons/next.svg\" />");
                        }
                        sbMenu.Append("<div class=\"wrapper\">");
                        sbMenu.Append("<ul>");
                        if (parents.Length < 4)
                        {
                            sbMenu.Append("<li><form method=\"post\" action=\"/Maineditor\">");
                            sbMenu.Append("<input type=\"hidden\" name=\"typeofedit\" value=\"add\" />");
                            sbMenu.Append("<input type=\"hidden\" name=\"pageclass\" value=\"main\" />");
                            sbMenu.Append("<input type=\"hidden\" name=\"parent\" value=\"" + item.Key + "\" />");
                            sbMenu.Append("<input type=\"submit\" value=\"Створити сторінку\" />");
                            sbMenu.Append("</form></li>");
                        }
                        HtmlString a = GenerateMenuMain(item.Key);
                        sbMenu.Append(a);
                        sbMenu.Append("</ul>");
                        sbMenu.Append("</div>");
                        if (parents.Length > 2)
                            sbMenu.Append("</li>");
                        break;
                    case "page":
                        sbMenu.Append("<li>");
                        sbMenu.Append("<form class=\"editForm\" method=\"post\" action=\"/Maineditor\">");
                        sbMenu.Append("<input type=\"hidden\" name=\"typeofedit\" value=\"edit\" />");
                        sbMenu.Append("<input type=\"hidden\" name=\"pageclass\" value=\"main\" />");
                        sbMenu.Append("<input type=\"hidden\" name=\"fullpagename\" value=\"" + item.Key + "\" />");
                        sbMenu.Append("<input class=\"editPage\" type=\"submit\" value=\"\" />");
                        sbMenu.Append("</form>");
                        sbMenu.Append("<a href=\"/Mainpage/" + item.Key + "\">" + item.Value["Name"] + "</a></li>");
                        break;
                    case "link":
                        sbMenu.Append("<li>");
                        sbMenu.Append("<form class=\"editForm\" method=\"post\" action=\"/Maineditor\">");
                        sbMenu.Append("<input type=\"hidden\" name=\"typeofedit\" value=\"edit\" />");
                        sbMenu.Append("<input type=\"hidden\" name=\"pageclass\" value=\"main\" />");
                        sbMenu.Append("<input type=\"hidden\" name=\"fullpagename\" value=\"" + item.Key + "\" />");
                        sbMenu.Append("<input class=\"editPage\" type=\"submit\" value=\"\" />");
                        sbMenu.Append("</form>");
                        sbMenu.Append("<a href=\" + item.Value[\"Content\"] + \">" + item.Value["Name"] + "</a></li>");
                        break;
                    default:
                        break;
                }
            }
            return new HtmlString(sbMenu.ToString()) ;
        }
    }
}
