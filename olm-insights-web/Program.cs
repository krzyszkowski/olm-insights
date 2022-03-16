using Azure.Storage.Blobs;
using Azure.Storage.Queues;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<BlobServiceClient>(
    c => new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=olminsightsstorage;AccountKey=AlmelybsNIBk7KhQdfraCqAqydKa229hqkVcD9JdrLUBhLf+8+swZrt7GGTgUxMtjl+oigkpy/X4jH/UG/rXLQ==;EndpointSuffix=core.windows.net"));
builder.Services.AddSingleton<QueueClient>(
    c => new QueueClient(
        "DefaultEndpointsProtocol=https;AccountName=olminsightsstorage;AccountKey=AlmelybsNIBk7KhQdfraCqAqydKa229hqkVcD9JdrLUBhLf+8+swZrt7GGTgUxMtjl+oigkpy/X4jH/UG/rXLQ==;EndpointSuffix=core.windows.net",
        queueName: "simple-queue"));

builder.Services.AddLogging();
builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
