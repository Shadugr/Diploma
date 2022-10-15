using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FMI_web.Pages
{
    [IgnoreAntiforgeryToken]
    public class MaineditorModel : PageModel
    {
        [BindProperty]
        public string? TypeOfEdit { get; set; }
        [BindProperty]
        public string? PageClass { get; set; }
        [BindProperty]
        public string? PageType { get; set; }
        [BindProperty]
        public string? PageName { get; set; } = "";
        [BindProperty]
        public string? FullPageName { get; set; } = "";
        [BindProperty]
        public string? PageContent { get; set; }
        [BindProperty]
        public string? PageContentLink { get; set; }
        [BindProperty]
        public string? Parent { get; set; }
        public string ErrorMessage { get; set; } = "";
        public void OnPost(string? typeofedit, string? pageclass, string? pagetype, string? fullpagename, string? parent)
        {
            TypeOfEdit = typeofedit ?? "";
            PageClass = pageclass ?? "";
            PageType = pagetype ?? "";
            FullPageName = fullpagename ?? "";
            Parent = parent ?? "";
        }
        public IActionResult? OnPostAdd()
        {
            if (string.IsNullOrEmpty(PageName) || string.IsNullOrEmpty(PageType) || string.IsNullOrEmpty(Parent))
                return RedirectToPage("Index");
            string page = Hashtables.ConvertToLatin(PageName);
            string fullPage = Parent + "&" + page;
            if (Hashtables.MainPages.ContainsKey(fullPage))
            {
                ErrorMessage = "Сторінка з такою назвою вже існує!";
                return null;
            }
            Dictionary<string, string> temp = new()
            {
                { "Type", PageType },
                { "Name", PageName }
            };
            switch (PageType)
            {
                case "page":
                    if (string.IsNullOrEmpty(PageContent))
                    {
                        ErrorMessage = "Контент сторінки порожній!";
                        return null;
                    }
                    temp.Add("Content", PageContent);
                    break;
                case "link":
                    if (string.IsNullOrEmpty(PageContentLink))
                    {
                        ErrorMessage = "Посилання на сторінку порожнє!";
                        return null;
                    }
                    temp.Add("Content", PageContentLink);
                    break;
                case "list":
                    string[] c = Parent.Split('&');
                    if (c.Length >= 3)
                    {
                        ErrorMessage = "Максимальний рівень меню 3!";
                        return null;
                    }
                    temp.Add("Content", "");
                    break;
                default:
                    return null;
            }
            Hashtables.MainPages.Add(fullPage, temp);
            if (PageType == "page")
                return Redirect($"Mainpage/{fullPage}");
            return RedirectToPage("Index");
        }
        public IActionResult? OnPostEdit()
        {
            if (string.IsNullOrEmpty(PageName) || string.IsNullOrEmpty(PageType) || string.IsNullOrEmpty(FullPageName))
                return RedirectToPage("Index");
            if (!Hashtables.MainPages.ContainsKey(FullPageName))
                return RedirectToPage("Index");
            string fullPage = FullPageName;
            string newFullPage = "";
            string[] tempString = fullPage.Split('&');
            for (int i = 0; i < tempString.Length; i++)
            {
                if (i == tempString.Length - 1)
                    newFullPage += Hashtables.ConvertToLatin(PageName);
                else
                    newFullPage += $"{tempString[i]}&";
            }
            if (Hashtables.MainPages.ContainsKey(newFullPage) && !string.IsNullOrEmpty(newFullPage) &&
                 newFullPage != fullPage)
            {
                ErrorMessage = "Сторінка з такою назвою вже існує!";
                return null;
            }
            if (newFullPage != fullPage)
            {
                Hashtables.MainPages.Remove(fullPage);
                Dictionary<string, string> temp = new()
                {
                    { "Type", PageType },
                    { "Name", PageName }
                };
                switch (PageType)
                {
                    case "page":
                        if (string.IsNullOrEmpty(PageContent))
                        {
                            ErrorMessage = "Контент сторінки порожній!";
                            return null;
                        }
                        temp.Add("Content", PageContent);
                        break;
                    case "link":
                        if (string.IsNullOrEmpty(PageContentLink))
                        {
                            ErrorMessage = "Посилання на сторінку порожнє!";
                            return null;
                        }
                        temp.Add("Content", PageContentLink);
                        break;
                    case "list":
                        string[] c = newFullPage.Split('&');
                        if (c.Length >= 4)
                        {
                            ErrorMessage = "Максимальний рівень меню 3!";
                            return null;
                        }
                        temp.Add("Content", "");
                        break;
                    default:
                        return null;
                }
                Hashtables.MainPages.Add(newFullPage, temp);
            }
            else
            {
                Hashtables.MainPages[fullPage]["Type"] = PageType;
                switch (PageType)
                {
                    case "page":
                        if (string.IsNullOrEmpty(PageContent))
                        {
                            ErrorMessage = "Контент сторінки порожній!";
                            return null;
                        }
                        Hashtables.MainPages[fullPage]["Content"] = PageContent;
                        break;
                    case "link":
                        if (string.IsNullOrEmpty(PageContentLink))
                        {
                            ErrorMessage = "Посилання на сторінку порожнє!";
                            return null;
                        }
                        Hashtables.MainPages[fullPage]["Content"] = PageContentLink;
                        break;
                    case "list":
                        string[] c = fullPage.Split('&');
                        if (c.Length >= 4)
                        {
                            ErrorMessage = "Максимальний рівень меню 3!";
                            return null;
                        }
                        Hashtables.MainPages[fullPage]["Content"] = "";
                        break;
                    default:
                        return null;
                }
            }
            if (PageType == "page")
                return Redirect($"Mainpage/{fullPage}");
            return RedirectToPage("Index");
        }
        public IActionResult? OnPostRemove()
        {
            if (!string.IsNullOrEmpty(FullPageName))
            {
                Hashtables.MainPages.Remove(FullPageName);
            }
            return RedirectToPage("Index");
        }
    }
}
