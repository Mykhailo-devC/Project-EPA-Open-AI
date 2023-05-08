using Epa.Engine.DB;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});



var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbPassword = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD");

//my localhost sa password: P@ssw0rd121#
//container password: password@12345# 
//Hello vovan
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
