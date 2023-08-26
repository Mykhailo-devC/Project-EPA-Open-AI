using EPA.Engine.DB;
using EPA.Engine.Models.Logic_Models;
using EPA.Engine.Repository;
using EPA.Engine.Repository.EntityRepositories;
using EPA.OpenAI.Core;
using EPA.OpenAI.Core.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

//my localhost sa password: P@ssw0rd121#
//container password: password@12345# 
builder.Services.AddDbContext<EpaDbContext>(opt => opt.UseSqlServer(GetConnectionString(ConnectionType.Local), b => b.MigrationsAssembly("EPA-WebAPI")));

builder.Services.AddSingleton<TransactionHandler>();
builder.Services.AddSingleton<OpenAIApiHandler>();
builder.Services.AddSingleton<SessionHandler>();
builder.Services.AddScoped<ConversationHandler>();
builder.Services.AddScoped<WordListRepository>();
builder.Services.AddScoped<WordPoolRepository>();
builder.Services.AddScoped<IOpenAISession, OpenAISession>();
builder.Services.AddScoped<IOpenAIService, OpenAIService>();
builder.Services.AddScoped<ServiceResolver.RepositoryResolver>(serviceProvider => type =>
{
    switch (type)
    {
        case RepositoryType.WordList:
            return serviceProvider.GetService<WordListRepository>();
        case RepositoryType.WordPool:
            return serviceProvider.GetService<WordPoolRepository>();
        default: return null;
    }
});

builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();

string GetConnectionString(ConnectionType connectionType)
{
    string dbHost;
    string dbName;
    string dbPassword;
    string connectionString = string.Empty;

    switch (connectionType)
    {
        case ConnectionType.Local:
            {
                dbHost = ".";
                dbName = "epa_db";
                dbPassword = "P@ssw0rd121#";
                connectionString = string.Format(builder.Configuration.GetConnectionString("DefaultConnection"), dbHost, dbName, dbPassword);
                break;
            }
        case ConnectionType.Docker:
            {
                dbHost = Environment.GetEnvironmentVariable("DB_HOST");
                dbName = Environment.GetEnvironmentVariable("DB_NAME");
                dbPassword = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD");
                connectionString = string.Format(builder.Configuration.GetConnectionString("DefaultConnection"), dbHost, dbName, dbPassword);
                break;
            }
        case ConnectionType.Azure:
            {
                connectionString = builder.Configuration.GetConnectionString("AzureDbConnection");
                break;
            }
    }
    return connectionString;
}