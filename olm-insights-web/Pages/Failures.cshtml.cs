using Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text;

namespace olm_insights_web.Pages
{
    public class FailuresModel : PageModel
    {
        public void OnGet()
        {
        }
        public async Task OnPostAsync()
        {
            throw new NotImplementedException(":D");
        }
    }
}
