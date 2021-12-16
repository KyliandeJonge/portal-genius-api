using PortalGenius.Core.Services;
//using PortalGenius.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Logging.ClearProviders();
builder.Logging.AddProvider(new Log4NetProvider());

builder.Services.AddControllers()
    .AddNewtonsoftJson();

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
