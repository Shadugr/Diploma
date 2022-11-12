using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FMI_web.Pages
{
    [IgnoreAntiforgeryToken]
    public class HandlersModel : PageModel
    {
        public static string? LoginErrorMessage { get; set; }

        public IActionResult? OnGet()
        {
            return RedirectToPage(Defs.PAGE_STATIC_INDEX);
        }
        public IActionResult? OnPost()
        {
            return RedirectToPage(Defs.PAGE_STATIC_INDEX);
        }

        public IActionResult? OnPostLogin(string? login, string? password)
        {
            if (!Hashtables.Users.Any(l => l.Login == login && l.Password == password))
            {
                LoginErrorMessage = "Неправильний логін або пароль!";
                return RedirectToPage(Defs.PAGE_STATIC_INDEX);
            }
            LoginErrorMessage = "";
            UsersClass temp = Hashtables.Users.Find(l => l.Login == login && l.Password == password);
            HttpContext.Session.SetString(Defs.SESSION_ACCESS, temp.Access);
            HttpContext.Session.SetString(Defs.SESSION_LOGIN, temp.Login);
            return RedirectToPage(Defs.PAGE_STATIC_INDEX);
        }
        public IActionResult? OnPostLogout()
        {
            HttpContext.Session.Remove(Defs.SESSION_ACCESS);
            HttpContext.Session.Remove(Defs.SESSION_LOGIN);
            return RedirectToPage(Defs.PAGE_STATIC_INDEX);
        }
    }
}
