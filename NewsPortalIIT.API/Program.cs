using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using NewsPortalIIT.API;
using NewsPortalIIT.Business;
using NewsPortalIIT.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiLayer();
builder.Services.AddBusinessLayer();
builder.Services.AddPersistenceLayer();

builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "News Portal IIT", Version = "v1" });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDb")
                           ?? throw new InvalidOperationException("Connection string 'MongoDb' not found.");
    options.UseMongoDB(connectionString);
});

const string CorsPolicyName = "AllowFrontend";
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyName, policy =>
    {
        policy
            .SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

app.UseHttpsRedirection();

app.UseCors(CorsPolicyName);

app.UseAuthorization();

app.MapControllers();

app.Run();
