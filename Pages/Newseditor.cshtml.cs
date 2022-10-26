using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FMI_web.Pages
{
    [IgnoreAntiforgeryToken]
    public class NewseditorModel : PageModel
    {
        [BindProperty]
        public string? TypeOfEdit { get; set; }
        [BindProperty]
        public string? PageTag { get; set; }
        [BindProperty]
        public string? PageName { get; set; } = "";
        [BindProperty]
        public int? Id { get; set; }
        [BindProperty]
        public string? PageContent { get; set; }
        [BindProperty]
        public IFormFile? PageLogo { get; set; }
        public string ErrorMessage { get; set; } = "";

        private readonly IWebHostEnvironment _environment;

        public NewseditorModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public void OnPost(string? typeofedit, int? id)
        {
            TypeOfEdit = typeofedit ?? "";
            Id = id ?? -1;
        }
        public IActionResult? OnPostAdd()
        {
            if (string.IsNullOrEmpty(PageName) || PageLogo == null)
                return RedirectToPage("Index");
            if (string.IsNullOrEmpty(PageContent))
            {
                ErrorMessage = "Контент сторінки порожній!";
                return null;
            }
            if (Hashtables.NewsPages.Any(v => v.Name == PageName))
            {
                ErrorMessage = "Сторінка з такою назвою вже існує!";
                return null;
            }
            string latinName = Hashtables.ConvertToLatin(PageName);
            if (PageLogo.ContentType == "image/jpeg")
                latinName += ".jpg";
            else if (PageLogo.ContentType == "image/png")
                latinName += ".png";
            string uploadsFolder = _environment.WebRootPath + '/'
                    + Defs.FILE_IMGDIRECTORYSHORT + '/' + Defs.FILE_NEWSIMAGESDIRECTORY;
            string filePath = uploadsFolder + '/' + latinName;
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                PageLogo.CopyTo(fileStream);
            }
            string filePathImg = '/' + Defs.FILE_IMGDIRECTORYSHORT + '/'
                + Defs.FILE_NEWSIMAGESDIRECTORY + '/' + latinName;
            DateTime now = DateTime.Now;
            NewsPageClass temp = new(PageName, PageTag, PageContent, $"{now:d}", filePathImg);
            Hashtables.NewsPages.Add(temp);
            Hashtables.HashtableToFile(Hashtables.NewsPages,
                Defs.FILE_HASHTABLESDIRECTORY + '/' + Defs.FILE_NEWSHASHTABLE);
            return Redirect($"/Newspage/{Hashtables.NewsPages.Count - 1}");
        }
        public IActionResult? OnPostEdit()
        {
            if (string.IsNullOrEmpty(PageName) || Id == null || Id < 0 || Id >= Hashtables.NewsPages.Count)
                return RedirectToPage("Index");
            if (string.IsNullOrEmpty(PageContent))
            {
                ErrorMessage = "Контент сторінки порожній!";
                return null;
            }
            if (Hashtables.NewsPages.Any(v => v.Name == PageName) 
                && Hashtables.NewsPages[(int)Id].Name != PageName)
            {
                ErrorMessage = "Сторінка з такою назвою вже існує!";
                return null;
            }
            if (PageLogo != null)
            {
                string latinName = Hashtables.ConvertToLatin(PageName);
                if (PageLogo.ContentType == "image/jpeg")
                    latinName += ".jpg";
                else if (PageLogo.ContentType == "image/png")
                    latinName += ".png";
                string uploadsFolder = _environment.WebRootPath + '/'
                        + Defs.FILE_IMGDIRECTORYSHORT + '/' + Defs.FILE_NEWSIMAGESDIRECTORY;
                string filePath = uploadsFolder + '/' + latinName;
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    PageLogo.CopyTo(fileStream);
                }
                string filePathImg = '/' + Defs.FILE_IMGDIRECTORYSHORT + '/'
                    + Defs.FILE_NEWSIMAGESDIRECTORY + '/' + latinName;
                Hashtables.NewsPages[(int)Id].Logo = filePathImg;
            }
            if (PageName != Hashtables.NewsPages[(int)Id].Name)
            {
                string latinName = Hashtables.ConvertToLatin(PageName) + '.'
                    + Hashtables.NewsPages[(int)Id].Logo.Split('.').Last();
                string uploadsFolder = _environment.WebRootPath + '/'
                        + Defs.FILE_IMGDIRECTORYSHORT + '/' + Defs.FILE_NEWSIMAGESDIRECTORY;
                string oldPath = uploadsFolder + 
                    '/' + Hashtables.NewsPages[(int)Id].Logo.Split('/').Last();
                string newPath = uploadsFolder + '/' + latinName;
                System.IO.File.Move(oldPath, newPath);
                string filePathImg = '/' + Defs.FILE_IMGDIRECTORYSHORT + '/'
                + Defs.FILE_NEWSIMAGESDIRECTORY + '/' + latinName;
                Hashtables.NewsPages[(int)Id].Name = PageName;
                Hashtables.NewsPages[(int)Id].Logo = filePathImg;
            }
            Hashtables.NewsPages[(int)Id].Content = PageContent;
            Hashtables.NewsPages[(int)Id].Tag = PageTag;
            Hashtables.HashtableToFile(Hashtables.NewsPages,
                Defs.FILE_HASHTABLESDIRECTORY + '/' + Defs.FILE_NEWSHASHTABLE);
            return Redirect($"Newspage/{(int)Id}");
        }
        public IActionResult? OnPostRemove()
        {
            if (Id < 0 || Id >= Hashtables.NewsPages.Count || Id == null )
                return RedirectToPage("Index");
            string uploadsFolder = _environment.WebRootPath + '/'
                        + Defs.FILE_IMGDIRECTORYSHORT + '/' + Defs.FILE_NEWSIMAGESDIRECTORY;
            string oldPath = uploadsFolder +
                '/' + Hashtables.NewsPages[(int)Id].Logo.Split('/').Last();
            System.IO.File.Delete(oldPath);
            Hashtables.NewsPages.RemoveAt((int)Id);
            Hashtables.HashtableToFile(Hashtables.NewsPages,
                Defs.FILE_HASHTABLESDIRECTORY + '/' + Defs.FILE_NEWSHASHTABLE);
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
                return new ObjectResult(new
                {
                    status = '/' + Defs.FILE_IMGDIRECTORYSHORT + '/'
                    + Defs.FILE_FORMIMAGESDIRECTORY + '/' + newName
                });
            }
            return new ObjectResult(new { status = "fail" });
        }
    }
}
