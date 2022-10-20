using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FMI_web.Pages
{
    [IgnoreAntiforgeryToken]
    public class StaticeditorModel : PageModel
    {
        [BindProperty]
        public string? TypeOfEdit { get; set; }
        [BindProperty]
        public string? PageName { get; set; } = "";
        [BindProperty]
        public string? FullPageName { get; set; } = "";
        [BindProperty]
        public string? PageContent { get; set; }
        public string ErrorMessage { get; set; } = "";

        private readonly IWebHostEnvironment _environment;

        public StaticeditorModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public void OnPost(string? typeofedit, string? fullpagename)
        {
            TypeOfEdit = typeofedit ?? "";
            FullPageName = fullpagename ?? "";
        }
        public IActionResult? OnPostEdit()
        {
            if (string.IsNullOrEmpty(PageName) || string.IsNullOrEmpty(FullPageName))
                return RedirectToPage("Index");
            if (!Hashtables.StaticPages.ContainsKey(FullPageName))
                return RedirectToPage("Index");
            if (string.IsNullOrEmpty(PageContent))
            {
                ErrorMessage = "Контент сторінки порожній!";
                return null;
            }
            Hashtables.StaticPages[FullPageName].Name = PageName;
            Hashtables.StaticPages[FullPageName].Content = PageContent;
            Hashtables.HashtableToFile(Hashtables.StaticPages,
                Defs.FILE_HASHTABLESDIRECTORY + '/' + Defs.FILE_STATICHASHTABLE);
            return Redirect(FullPageName);
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
