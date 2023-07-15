using Epa.Engine.DB;
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

var connectionString = string.Format(builder.Configuration.GetConnectionString("DefaultConnection"), dbHost, dbName, dbPassword);

builder.Services.AddDbContext<EpaDbContext>(opt => opt.UseSqlServer(connectionString,
                                                                    b => b.MigrationsAssembly(
                                                                        typeof(Program).Assembly.GetName().Name
                                                                        )));

var app = builder.Build();

// Configure the HTTP request pipeline.





app.UseAuthorization();

app.MapControllers();

app.Run();
