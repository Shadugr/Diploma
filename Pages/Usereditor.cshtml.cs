using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FMI_web.Pages
{
    [IgnoreAntiforgeryToken]
    public class UsereditorModel : PageModel
    {
        [BindProperty]
        public string? TypeOfEdit { get; set; }
        [BindProperty]
        public string? Login { get; set; } = "";
        [BindProperty]
        public string? Password { get; set; }
        [BindProperty]
        public string? Access { get; set; }
        [BindProperty]
        public string? OldLogin { get; set; }
        public string ErrorMessage { get; set; } = "";

        public void OnPost(string? typeofedit)
        {
            TypeOfEdit = typeofedit ?? "";
        }
        public IActionResult? OnPostAdd()
        {
            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Access))
                return RedirectToPage(Defs.PAGE_STATIC_INDEX);
            if (Hashtables.Users.Any(l => l.Login == Login))
            {
                ErrorMessage = "Користувач з таким логіном вже існує!";
                return null;
            }
            UsersClass temp = new(Login, Password, Access);
            Hashtables.Users.Add(temp);
            Hashtables.HashtableToFile(Hashtables.Users,
                Defs.FILE_HASHTABLESDIRECTORY + '/' + Defs.FILE_USERSHASHTABLE);
            return RedirectToPage(Defs.PAGE_STATIC_INDEX);
        }
        public IActionResult? OnPostEdit()
        {
            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password)
                 || string.IsNullOrEmpty(OldLogin))
                return RedirectToPage(Defs.PAGE_STATIC_INDEX);
            if (!Hashtables.Users.Any(l => l.Login == OldLogin))
                return RedirectToPage(Defs.PAGE_STATIC_INDEX);
            if (Hashtables.Users.Any(l => l.Login == Login) && Login != OldLogin)
            {
                Login = OldLogin;
                ErrorMessage = "Користувач з таким логіном вже існує!";
                return null;
            }
            int ind = Hashtables.Users.FindIndex(l => l.Login == OldLogin);
            Hashtables.Users[ind].Login = Login;
            Hashtables.Users[ind].Password = Password;
            if (!string.IsNullOrEmpty(Access))
                Hashtables.Users[ind].Access = Access;
            Hashtables.HashtableToFile(Hashtables.Users,
                Defs.FILE_HASHTABLESDIRECTORY + '/' + Defs.FILE_USERSHASHTABLE);
            if (HttpContext.Session.GetString(Defs.SESSION_LOGIN) == OldLogin)
            {
                HttpContext.Session.Remove(Defs.SESSION_ACCESS);
                HttpContext.Session.Remove(Defs.SESSION_LOGIN);
            }
            return RedirectToPage(Defs.PAGE_STATIC_INDEX);
        }
        public IActionResult? OnPostRemove()
        {
            if (string.IsNullOrEmpty(Login) || HttpContext.Session.GetString(Defs.SESSION_LOGIN) == Login)
                return RedirectToPage(Defs.PAGE_STATIC_INDEX);
            Hashtables.Users.RemoveAt(Hashtables.Users.FindIndex(l => l.Login == Login));
            Hashtables.HashtableToFile(Hashtables.Users,
                Defs.FILE_HASHTABLESDIRECTORY + '/' + Defs.FILE_USERSHASHTABLE);
            if (HttpContext.Session.GetString(Defs.SESSION_LOGIN) == Login)
            {
                HttpContext.Session.Remove(Defs.SESSION_ACCESS);
                HttpContext.Session.Remove(Defs.SESSION_LOGIN);
            }
            return RedirectToPage(Defs.PAGE_STATIC_INDEX);
        }
    }
}
