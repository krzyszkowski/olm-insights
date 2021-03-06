using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;

namespace olm_insights_web.Pages
{
    public class BlobMd5Model : PageModel
    {
        private readonly ILogger<BlobMd5Model> _logger;
        private const int Mb = 1048576;
        private readonly long _chunkSize;
        private readonly BlobServiceClient _blobServiceClient;

        public IList<BlobDescription> Blobs { get; private set; }
        [BindProperty]
        public Guid ResultMd5 { get; set; }

        public BlobMd5Model(
            ILogger<BlobMd5Model> logger,
            BlobServiceClient blobServiceClient)
        {
            Blobs = new List<BlobDescription>();
            _logger = logger;
            _blobServiceClient = blobServiceClient;
            _chunkSize = 5 * Mb;
        }

        public void OnGet()
        {
            var sourceContainer = _blobServiceClient.GetBlobContainerClient("source");
            var response = sourceContainer.GetBlobs();
            foreach (var blobItem in response)
            {
                Blobs.Add(new BlobDescription { Name = blobItem.Name, Size = blobItem.Properties.ContentLength });
            }
        }

        public async Task OnPostCalculateAsync(string blobName, long blobSize)
        {
            var sourceContainer = _blobServiceClient.GetBlobContainerClient("source");
            var blockBlob = sourceContainer.GetBlockBlobClient(blobName);
            ResultMd5 = await CalculateBlobMd5Async(blobSize, blockBlob);
        }

        private async Task<Guid> CalculateBlobMd5Async(long fileSize, BlockBlobClient blockBlobClient)
        {
            _logger.LogWarning($"Md5 calculation started...");
            
            using var totalMd5 = MD5.Create();

            await foreach (var dataChunk in EnumerateChunksAsync(blockBlobClient, fileSize))
            {
                _logger.LogError($"Batch {dataChunk.index} downloaded.");
                totalMd5.TransformBlock(dataChunk.data, 0, dataChunk.data.Length, null, 0);
            }

            totalMd5.TransformFinalBlock(new byte[128], 0, 0);
            var result = new Guid(totalMd5.Hash);
            _logger.LogWarning($"Missing md5 calculation completed :{result}");

            return result;
        }

        private async IAsyncEnumerable<(byte[] data, int index)> EnumerateChunksAsync(BlockBlobClient blockBlobClient, long fileSize)
        {
            var chunkCount = fileSize / _chunkSize;

            if (fileSize % _chunkSize != 0)
            {
                chunkCount += 1;
            }

            for (int i = 0; i < chunkCount; i++)
            {
                var offset = i * _chunkSize;
                var range = new HttpRange(offset, _chunkSize);

                await using var stream = new MemoryStream();
                var response = await blockBlobClient.DownloadStreamingAsync(range);
                await response.Value.Content.CopyToAsync(stream);

                yield return (stream.ToArray(), i);
            }
        }
    }

    public class BlobDescription
    {
        public string Name { get; set; }
        public long? Size { get; set; }
    }
}
