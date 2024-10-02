using Serilog;
using StalKompParser.StalKompParser.Application.Services.StalKompParser;
using StalKompParser.StalKompParser.Common.PageLoader;
using StalKompParser.StalKompParser.Application.Configurations;

var builder = WebApplication.CreateBuilder(args);

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

// add http client
builder.Services.AddHttpClient();

// add scoped 
builder.Services.AddScoped<IPageLoader, PageLoader>();

// add parser
builder.Services.AddScoped<IProductParser, ProductParser>();

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
