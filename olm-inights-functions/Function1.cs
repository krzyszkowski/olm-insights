using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Messaging.EventGrid;
using Azure.Storage.Queues;
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

                Entropy(log);
            }

            if (myQueueItem.Contains("webhook"))
            {
                var httpClient = new HttpClient();
                var resp = await httpClient.GetAsync("https://olm-insights-web.azurewebsites.net/WebHook");
                var cont = await resp.Content.ReadAsStringAsync();

                log.LogInformation(cont);
            }

            Entropy(log);
        }

        [FunctionName("events-handler-1")]
        public void Run1([QueueTrigger("events-1-queue", Connection = "QueueConnection")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"events-handler-1 processed: {myQueueItem}");
            Entropy(log);
        }

        [FunctionName("events-handler-2")]
        public void Run2([QueueTrigger("events-2-queue", Connection = "QueueConnection")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"events-handler-2 processed: {myQueueItem}");
            
            Entropy(log);
            Entropy(log);

            log.LogWarning($"events-handler-2 entropy PASSED!!!");

            EventGridPublisherClient eventsClient = new EventGridPublisherClient(
                    new Uri("https://olm-insights-evt.westeurope-1.eventgrid.azure.net/api/events"),
                    new DefaultAzureCredential()
                    );

            eventsClient.SendEvent(new EventGridEvent("subject", "typeDefinition2", "1.0",
                new SimpleEventMessage { Message = myQueueItem }));
        }

        [FunctionName("events-handler-3")]
        public async Task Run3([QueueTrigger("events-3-queue", Connection = "QueueConnection")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"events-handler-3 processed: {myQueueItem}");

            var httpClient = new HttpClient();
            var random = Guid.NewGuid().ToString();
            var jsonRequest = new { Title = $"events-handler-3 blog - {random}" };

            var result = await httpClient.PostAsync("https://olm-insights-api.azurewebsites.net/Olmug", jsonRequest.AsUtf8Json());
            if (result.IsSuccessStatusCode)
            {
                log.LogInformation($"events-handler-3 created blog with random guid: {random}");
            }
            else
            {
                throw new WebException($"events-handler-3 Error details: {await result.Content.ReadAsStringAsync()}");
            }
        }

        private void Entropy(ILogger log)
        {
            var rand = new Random();

            var badLuck = rand.Next(0, 10);

            log.LogWarning("Entropy is ... " + badLuck);

            if (badLuck == 5)
            {
                throw new Exception("shit happens ...");
            }
        }
    }
}
