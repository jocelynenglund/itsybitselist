using ItsyBitseList.Infrastructure;
using ItsyBitseList.Core;
using System.Reflection;
using System.Text.Json.Serialization;

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
// Add services to the container.
builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .WithExposedHeaders("Location")
           .AllowAnyHeader();
}));

builder.Services.AddInfrastructureDependencies();
builder.Services.AddCoreDependencies();

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
