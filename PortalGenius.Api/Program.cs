using PortalGenius.Core.Services;
using PortalGenius.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Logging.ClearProviders();
builder.Logging.AddProvider(new Log4NetProvider());

builder.Services.AddControllers()
    .AddNewtonsoftJson();

var configuration = builder.Configuration;
switch (configuration.GetValue<string>("DatabaseInUse"))
{
    case "Sqlite":
        builder.Services.AddDbContext<AppDbContext, SQLiteDbContext>();
        break;
    case "MSSQL":
        builder.Services.AddDbContext<AppDbContext, SQLServerDbContext>();
        break;
    case "PostgreSQL":
        break;
    case "Oracle":
        break;
    default:
        throw new ArgumentException("DatabaseInUse missing from appsettings.json");
}

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure the HTTP client endpoints
builder.Services.AddHttpClient("arcgis-api", options =>
{
    options.BaseAddress = new Uri("https://portalgenius.maps.arcgis.com/sharing/rest");
});
builder.Services.AddHttpService("arcgis-api");

builder.Services.AddTransient<ArcGISService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
