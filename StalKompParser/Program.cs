using StalKompParser.StalKompParser.Configurations;
using Serilog;
using StalKompParser.StalKompParser.PageLoader;
using StalKompParser.StalKompParser.StalKompParser;
using StalKompParser.StalKompParser.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// add configuration 
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(config)
    .CreateLogger();

builder.Services.AddCors();

builder.Host.UseSerilog(logger, dispose: true);

builder.Services.AddControllers();

// configure settings
builder.Services.Configure<ParserSettings>(config.GetSection(nameof(ParserSettings)));

// add http client factory with proxies 
builder.Services.AddHttpClient();

// add parser
builder.Services.AddScoped<IProductParser, ProductParser>();

// add scoped 
builder.Services.AddScoped<IPageLoader, PageLoader>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//app.UseMiddleware<TimeoutMiddleware>();
//app.UseMiddleware<ExceptionHandleMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(builder =>
{
    builder.AllowAnyOrigin();
    builder.AllowAnyMethod();
    builder.AllowAnyHeader();
});

app.MapControllers();

app.Run();
