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
                ErrorMessage = "������� � ����� ������ ��� ����!";
                return null;
            }
            Dictionary<string, string> temp = new()
            {
                { Defs.VALUE_TYPE, PageType },
                { Defs.VALUE_NAME, PageName }
            };
            switch (PageType)
            {
                case Defs.TYPE_PAGE:
                    if (string.IsNullOrEmpty(PageContent))
                    {
                        ErrorMessage = "������� ������� ��������!";
                        return null;
                    }
                    temp.Add(Defs.VALUE_CONTENT, PageContent);
                    break;
                case Defs.TYPE_LINK:
                    if (string.IsNullOrEmpty(PageContentLink))
                    {
                        ErrorMessage = "��������� �� ������� �������!";
                        return null;
                    }
                    temp.Add(Defs.VALUE_CONTENT, PageContentLink);
                    break;
                case Defs.TYPE_LIST:
                    string[] c = Parent.Split('&');
                    if (c.Length >= 3)
                    {
                        ErrorMessage = "������������ ����� ���� 3!";
                        return null;
                    }
                    temp.Add(Defs.VALUE_CONTENT, "");
                    break;
                default:
                    return RedirectToPage("Index");
            }
            Hashtables.MainPages.Add(fullPage, temp);
            Hashtables.HashtableToFile(Hashtables.MainPages,
                Defs.FILE_HASHTABLESDIRECTORY + '/' + Defs.FILE_MAINHASHTABLE);
            if (PageType == Defs.TYPE_PAGE)
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
                ErrorMessage = "������� � ����� ������ ��� ����!";
                return null;
            }
            if (newFullPage != fullPage)
            {
                Hashtables.MainPages.Remove(fullPage);
                Dictionary<string, string> temp = new()
                {
                    { Defs.VALUE_TYPE, PageType },
                    { Defs.VALUE_NAME, PageName }
                };
                switch (PageType)
                {
                    case Defs.TYPE_PAGE:
                        if (string.IsNullOrEmpty(PageContent))
                        {
                            ErrorMessage = "������� ������� ��������!";
                            return null;
                        }
                        temp.Add(Defs.VALUE_CONTENT, PageContent);
                        break;
                    case Defs.TYPE_LINK:
                        if (string.IsNullOrEmpty(PageContentLink))
                        {
                            ErrorMessage = "��������� �� ������� �������!";
                            return null;
                        }
                        temp.Add(Defs.VALUE_CONTENT, PageContentLink);
                        break;
                    case Defs.TYPE_LIST:
                        string[] c = newFullPage.Split('&');
                        if (c.Length >= 4)
                        {
                            ErrorMessage = "������������ ����� ���� 3!";
                            return null;
                        }
                        temp.Add(Defs.VALUE_CONTENT, "");
                        break;
                    default:
                        return RedirectToPage("Index");
                }
                Hashtables.MainPages.Add(newFullPage, temp);
                Hashtables.HashtableToFile(Hashtables.MainPages,
                Defs.FILE_HASHTABLESDIRECTORY + '/' + Defs.FILE_MAINHASHTABLE);
                if (PageType == Defs.TYPE_PAGE)
                    return Redirect($"Mainpage/{newFullPage}");
                return RedirectToPage("Index");
            }
            Hashtables.MainPages[fullPage][Defs.VALUE_TYPE] = PageType;
            switch (PageType)
            {
                case Defs.TYPE_PAGE:
                    if (string.IsNullOrEmpty(PageContent))
                    {
                        ErrorMessage = "������� ������� ��������!";
                        return null;
                    }
                    Hashtables.MainPages[fullPage][Defs.VALUE_CONTENT] = PageContent;
                    break;
                case Defs.TYPE_LINK:
                    if (string.IsNullOrEmpty(PageContentLink))
                    {
                        ErrorMessage = "��������� �� ������� �������!";
                        return null;
                    }
                    Hashtables.MainPages[fullPage][Defs.VALUE_CONTENT] = PageContentLink;
                    break;
                case Defs.TYPE_LIST:
                    string[] c = fullPage.Split('&');
                    if (c.Length >= 4)
                    {
                        ErrorMessage = "������������ ����� ���� 3!";
                        return null;
                    }
                    Hashtables.MainPages[fullPage][Defs.VALUE_CONTENT] = "";
                    break;
                default:
                    return null;
            }
            Hashtables.HashtableToFile(Hashtables.MainPages,
                Defs.FILE_HASHTABLESDIRECTORY + '/' + Defs.FILE_MAINHASHTABLE);
            if (PageType == Defs.TYPE_PAGE)
                return Redirect($"Mainpage/{fullPage}");
            return RedirectToPage("Index");
        }
        public IActionResult? OnPostRemove()
        {
            if (!string.IsNullOrEmpty(FullPageName))
            {
                Hashtables.MainPages.Remove(FullPageName);
            }
            Hashtables.HashtableToFile(Hashtables.MainPages,
                Defs.FILE_HASHTABLESDIRECTORY + '/' + Defs.FILE_MAINHASHTABLE);
            return RedirectToPage("Index");
        }
    }
}
