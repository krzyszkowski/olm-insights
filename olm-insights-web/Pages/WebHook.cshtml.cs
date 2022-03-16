using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace olm_insights_web.Pages
{
    public class WebHookModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }
    }
}
