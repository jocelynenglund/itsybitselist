using ItsyBitseList.Core;
using ItsyBitseList.Infrastructure;
using System.Reflection;
using System.Text.Json.Serialization;
using ItsyBitseList.Api;
using ItsyBitseList.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "ItsyBitseList.Api",
        Version = "v1"
    });
    var xmlPath = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlFullPath = Path.Combine(AppContext.BaseDirectory, xmlPath);
    c.IncludeXmlComments(xmlFullPath);
});
// Setup cors
builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .WithExposedHeaders("Location")
           .AllowAnyHeader();
}));

builder.Services.AddInfrastructureDependencies();
builder.Services.AddCoreDependencies();

//bind settings
builder.Services.Configure<StorageSettings>(builder.Configuration.GetSection(nameof(StorageSettings)));
builder.Services.AddStorage();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

/// <summary>
/// Program class
/// </summary>
public partial class Program { }