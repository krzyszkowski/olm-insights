using Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace olm_insights_web.Pages
{
    public class MessengerModel : PageModel
    {
        private readonly ILogger<MessengerModel> logger;
        private readonly QueueClient queueClient;

        [BindProperty]
        public MessengerVM Model { get; set; }

        public MessengerModel(ILogger<MessengerModel> logger, QueueClient queueClient)
        {
            this.logger = logger;
            this.queueClient = queueClient;
        }

        public void OnGet()
        {
        }

        public async Task OnPostAsync()
        {
            if (!ModelState.IsValid)
                return;

            logger.LogInformation( "Sending message to queue...");

            var payloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(Model.Message));

            await queueClient.SendMessageAsync(payloadBase64);
        }
    }

    public class MessengerVM
    {
        [Display(Name = "Message")]
        [Required(ErrorMessage = "Cant be empty")]
        [StringLength(maximumLength: 64, MinimumLength = 2)]
        public string Message { get; set; }
    }
}
