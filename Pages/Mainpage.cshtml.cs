using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting.Internal;
using System.Collections;
namespace FMI_web.Pages
{
    public class MainpageModel : PageModel
    {
        public string? Id { get; private set; }
        public void OnGet(string? id)
        {
            Id = id ?? "Error";
        }
    }
}
