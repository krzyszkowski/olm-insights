using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Messaging.EventGrid;
using Microsoft.Azure.Storage.Queue.Protocol;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace olm_inights_functions
{
    public class Function1
    {
        [FunctionName("simple-queue-function")]
        public async Task Run([QueueTrigger("simple-queue", Connection = "QueueConnection")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"simple-queue-function processed: {myQueueItem}");

            if (myQueueItem.Contains("eventgrid"))
            {
                EventGridPublisherClient eventsClient = new EventGridPublisherClient(
                    new Uri("https://olm-insights-evt.westeurope-1.eventgrid.azure.net/api/events"),
                    new DefaultAzureCredential()
                    );

                eventsClient.SendEvent(new EventGridEvent("subject", "typeDefinition1", "1.0",
                    new SimpleEventMessage { Message = myQueueItem }));
            }

            if (myQueueItem.Contains("webhook"))
            {
                var httpClient = new HttpClient();
                var resp = await httpClient.GetAsync("https://olm-insights-web.azurewebsites.net/WebHook");
                var cont = await resp.Content.ReadAsStringAsync();

                log.LogInformation(cont);
            }
        }

        [FunctionName("events-handler-1")]
        public void Run1([QueueTrigger("events-1-queue", Connection = "QueueConnection")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"events-handler-1 processed: {myQueueItem}");

        }

        [FunctionName("events-handler-2")]
        public void Run2([QueueTrigger("events-2-queue", Connection = "QueueConnection")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"events-handler-2 processed: {myQueueItem}");

        }
    }
}
