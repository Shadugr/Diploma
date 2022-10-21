using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FMI_web.Pages
{
    public class NewslistModel : PageModel
    {
        public int? Id { get; private set; }
        public void OnGet(string? id)
        {
            int temp;
            if (int.TryParse(id, out temp))
                Id = temp;
            else
                Id = -1;
        }
    }
}
