using Microsoft.EntityFrameworkCore;
using olm_insights_API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(
        options => options.UseSqlServer("Server=tcp:olm-insights-database.database.windows.net,1433;Initial Catalog=olm-insights-database;Persist Security Info=False;User ID=servadm;Password=bC5MUcp3gPhP87HDEB1q;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
