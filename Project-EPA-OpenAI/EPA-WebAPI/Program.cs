using Epa.Engine.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbPassword = Environment.GetEnvironmentVariable("SA_PASSWORD");

//my localhost sa password: P@ssw0rd121#
//container password: password@12345# 

var connectionString = $"Data Source={dbHost};Initial Catalog={dbName};User ID=sa;Password={dbPassword};TrustServerCertificate=True";

builder.Services.AddDbContext<EpaDbContext>(opt => opt.UseSqlServer(connectionString,
                                                                    b => b.MigrationsAssembly("Epa.Engine.Models")));

var app = builder.Build();

// Configure the HTTP request pipeline.





app.UseAuthorization();

app.MapControllers();

app.Run();
