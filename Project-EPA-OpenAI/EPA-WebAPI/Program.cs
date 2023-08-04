using Epa.Engine.DB;
using Epa.Engine.Models.Logic_Models;
using Epa.Engine.Repository;
using Epa.Engine.Repository.EntityRepositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

bool usingDocker = false;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});


string dbHost;
string dbName;
string dbPassword;
string connectionString;

if (usingDocker)
{
    dbHost = Environment.GetEnvironmentVariable("DB_HOST");
    dbName = Environment.GetEnvironmentVariable("DB_NAME");
    dbPassword = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD");
}
else
{
    dbHost = ".";
    dbName = "epa_db";
    dbPassword = "P@ssw0rd121#";
    
}


//my localhost sa password: P@ssw0rd121#
//container password: password@12345# 

connectionString = string.Format(builder.Configuration.GetConnectionString("DefaultConnection"), dbHost, dbName, dbPassword);

builder.Services.AddDbContext<EpaDbContext>(opt => opt.UseSqlServer(connectionString, b => b.MigrationsAssembly("EPA-WebAPI")));

builder.Services.AddScoped<WordListRepository>();
builder.Services.AddScoped<WordPoolRepository>();
builder.Services.AddTransient<ServiceResolver.RepositoryResolver>(serviceProvider => type =>
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

var app = builder.Build();

// Configure the HTTP request pipeline.





app.UseAuthorization();

app.MapControllers();

app.Run();
