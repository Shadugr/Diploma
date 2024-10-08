using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace FMI_web.Pages.Shared
{
    public class _HeaderModel : PageModel
    {
        public string? Access { get; set; }
        public HtmlString GenerateMenuMain(string parentName)
        {
            StringBuilder sbMenu = new();
            if (!parentName.Contains('&') && (Access == Defs.USER_ADMIN_ACCESS 
                || Access == Defs.USER_MODERATOR_ACCESS))
            {
                sbMenu.Append("<li><form method=\"post\" action=\"/Maineditor\">");
                sbMenu.Append("<input type=\"hidden\" name=\"" + Defs.INPUT_TYPEOFEDIT + "\" value=\"" + Defs.TYPE_EDIT_ADD + "\" />");
                sbMenu.Append("<input type=\"hidden\" name=\"" + Defs.INPUT_PARENT + "\" value=\"" + parentName + "\" />");
                sbMenu.Append("<input type=\"submit\" value=\"�������� �������\" />");
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
                switch (item.Value.Type)
                {
                    case Defs.TYPE_LIST:
                        if (parents.Length >= 2)
                        {
                            sbMenu.Append("<li class=\"parent\">");
                            if (Access == Defs.USER_ADMIN_ACCESS || Access == Defs.USER_MODERATOR_ACCESS)
                            {
                                sbMenu.Append("<form class=\"editForm\" method=\"post\" action=\"/Maineditor\">");
                                sbMenu.Append("<input type=\"hidden\" name=\"" + Defs.INPUT_TYPEOFEDIT + "\" value=\"" + Defs.TYPE_EDIT_EDIT + "\" />");
                                sbMenu.Append("<input type=\"hidden\" name=\"" + Defs.INPUT_FULLPAGENAME + "\" value=\"" + item.Key + "\" />");
                                sbMenu.Append("<input class=\"editPage\" type=\"submit\" value=\"\" />");
                                sbMenu.Append("</form>");
                            }
                            sbMenu.Append("<span>" + item.Value.Name + "</span>");
                            sbMenu.Append("<img src=\"/icons/next.svg\" />");
                        }
                        sbMenu.Append("<div class=\"wrapper\">");
                        sbMenu.Append("<ul>");
                        if (parents.Length < 4 && (Access == Defs.USER_ADMIN_ACCESS
                        || Access == Defs.USER_MODERATOR_ACCESS))
                        {
                            sbMenu.Append("<li><form method=\"post\" action=\"/Maineditor\">");
                            sbMenu.Append("<input type=\"hidden\" name=\"" + Defs.INPUT_TYPEOFEDIT + "\" value=\"" + Defs.TYPE_EDIT_ADD + "\" />");
                            sbMenu.Append("<input type=\"hidden\" name=\"" + Defs.INPUT_PARENT + "\" value=\"" + item.Key + "\" />");
                            sbMenu.Append("<input type=\"submit\" value=\"�������� �������\" />");
                            sbMenu.Append("</form></li>");
                        }
                        HtmlString a = GenerateMenuMain(item.Key);
                        sbMenu.Append(a);
                        sbMenu.Append("</ul>");
                        sbMenu.Append("</div>");
                        if (parents.Length > 2)
                            sbMenu.Append("</li>");
                        break;
                    case Defs.TYPE_PAGE:
                        sbMenu.Append("<li>");
                        if (Access == Defs.USER_ADMIN_ACCESS || Access == Defs.USER_MODERATOR_ACCESS)
                        {
                            sbMenu.Append("<form class=\"editForm\" method=\"post\" action=\"/Maineditor\">");
                            sbMenu.Append("<input type=\"hidden\" name=\"" + Defs.INPUT_TYPEOFEDIT + "\" value=\"" + Defs.TYPE_EDIT_EDIT + "\" />");
                            sbMenu.Append("<input type=\"hidden\" name=\"" + Defs.INPUT_FULLPAGENAME + "\" value=\"" + item.Key + "\" />");
                            sbMenu.Append("<input class=\"editPage\" type=\"submit\" value=\"\" />");
                            sbMenu.Append("</form>");
                        }
                        sbMenu.Append("<a href=\"/Mainpage/" + item.Key + "\">" + item.Value.Name + "</a></li>");
                        break;
                    case Defs.TYPE_LINK:
                        sbMenu.Append("<li>");
                        if (Access == Defs.USER_ADMIN_ACCESS || Access == Defs.USER_MODERATOR_ACCESS)
                        {
                            sbMenu.Append("<form class=\"editForm\" method=\"post\" action=\"/Maineditor\">");
                            sbMenu.Append("<input type=\"hidden\" name=\"" + Defs.INPUT_TYPEOFEDIT + "\" value=\"" + Defs.TYPE_EDIT_EDIT + "\" />");
                            sbMenu.Append("<input type=\"hidden\" name=\"" + Defs.INPUT_FULLPAGENAME + "\" value=\"" + item.Key + "\" />");
                            sbMenu.Append("<input class=\"editPage\" type=\"submit\" value=\"\" />");
                            sbMenu.Append("</form>");
                        }
                        sbMenu.Append("<a href=\"" + item.Value.Content + "\">" + item.Value.Name + "</a></li>");
                        break;
                    default:
                        break;
                }
            }
            return new HtmlString(sbMenu.ToString()) ;
        }
    }
}
