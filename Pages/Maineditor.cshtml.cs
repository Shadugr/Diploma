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

        private readonly IWebHostEnvironment _environment;

        public MaineditorModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

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
            MainPageClass temp = new(PageName, PageType);
            switch (PageType)
            {
                case Defs.TYPE_PAGE:
                    if (string.IsNullOrEmpty(PageContent))
                    {
                        ErrorMessage = "Контент сторінки порожній!";
                        return null;
                    }
                    temp.Content = PageContent;
                    break;
                case Defs.TYPE_LINK:
                    if (string.IsNullOrEmpty(PageContentLink))
                    {
                        ErrorMessage = "Посилання на сторінку порожнє!";
                        return null;
                    }
                    if (!PageContentLink.StartsWith("https://") && !PageContentLink.StartsWith("http://"))
                        PageContentLink = "https://" + PageContentLink;
                    temp.Content = PageContentLink;
                    break;
                case Defs.TYPE_LIST:
                    string[] c = Parent.Split('&');
                    if (c.Length >= 3)
                    {
                        ErrorMessage = "Максимальний рівень меню 3!";
                        return null;
                    }
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
                ErrorMessage = "Сторінка з такою назвою вже існує!";
                return null;
            }
            if (newFullPage != fullPage)
            {
                Hashtables.MainPages.Remove(fullPage);
                MainPageClass temp = new(PageName, PageType);
                switch (PageType)
                {
                    case Defs.TYPE_PAGE:
                        if (string.IsNullOrEmpty(PageContent))
                        {
                            ErrorMessage = "Контент сторінки порожній!";
                            return null;
                        }
                        temp.Content = PageContent;
                        break;
                    case Defs.TYPE_LINK:
                        if (string.IsNullOrEmpty(PageContentLink))
                        {
                            ErrorMessage = "Посилання на сторінку порожнє!";
                            return null;
                        }
                        if (!PageContentLink.StartsWith("https://") && !PageContentLink.StartsWith("http://"))
                            PageContentLink = "https://" + PageContentLink;
                        temp.Content = PageContentLink;
                        break;
                    case Defs.TYPE_LIST:
                        string[] c = newFullPage.Split('&');
                        if (c.Length >= 4)
                        {
                            ErrorMessage = "Максимальний рівень меню 3!";
                            return null;
                        }
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
            Hashtables.MainPages[fullPage].Type = PageType;
            switch (PageType)
            {
                case Defs.TYPE_PAGE:
                    if (string.IsNullOrEmpty(PageContent))
                    {
                        ErrorMessage = "Контент сторінки порожній!";
                        return null;
                    }
                    Hashtables.MainPages[fullPage].Content = PageContent;
                    break;
                case Defs.TYPE_LINK:
                    if (string.IsNullOrEmpty(PageContentLink))
                    {
                        ErrorMessage = "Посилання на сторінку порожнє!";
                        return null;
                    }
                    if (!PageContentLink.StartsWith("https://") && !PageContentLink.StartsWith("http://"))
                        PageContentLink = "https://" + PageContentLink;
                    Hashtables.MainPages[fullPage].Content = PageContentLink;
                    break;
                case Defs.TYPE_LIST:
                    string[] c = fullPage.Split('&');
                    if (c.Length >= 4)
                    {
                        ErrorMessage = "Максимальний рівень меню 3!";
                        return null;
                    }
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
        public IActionResult OnPostUploader(IFormFile UploadImage)
        {
            if (UploadImage != null)
            {
                int imgCount = Directory.GetFiles(Defs.FILE_IMGDIRECTORY + '/' 
                    + Defs.FILE_FORMIMAGESDIRECTORY, "*", SearchOption.TopDirectoryOnly).Length;
                string uploadsFolder = _environment.WebRootPath + '/'
                    + Defs.FILE_IMGDIRECTORYSHORT + '/' + Defs.FILE_FORMIMAGESDIRECTORY;
                string newName;
                if (imgCount == 0)
                    newName = "1.";
                else
                    newName = (imgCount + 1).ToString() + '.';
                if (UploadImage.ContentType == "image/jpeg")
                    newName += "jpg";
                else if (UploadImage.ContentType == "image/png")
                    newName += "png";
                string filePath = uploadsFolder + '/' + newName;
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    UploadImage.CopyTo(fileStream);
                }
                return new ObjectResult(new { status = '/' + Defs.FILE_IMGDIRECTORYSHORT + '/' 
                    + Defs.FILE_FORMIMAGESDIRECTORY + '/' + newName });
            }
            return new ObjectResult(new { status = "fail" });
        }
    }
}
