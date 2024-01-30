using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using API_DOC.Data;
using API_DOC.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders; // Add this namespace

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddAuthorization();
// Configure Entity Framework Core and add ApplicationDbContext to the services.

#region DataBase Context

var dataSource = builder.Configuration.GetSection("Database:ConnectionString:DataSource").Value ?? ".";
var initialCatalog = builder.Configuration.GetSection("Database:ConnectionString:InitialCatalog").Value
                     ?? "DadehfaTicketDB";
var userId = builder.Configuration.GetSection("Database:ConnectionString:UserId").Value ?? "sa";
var password = builder.Configuration.GetSection("Database:ConnectionString:Password").Value ?? "narenj@T";
var integratedSecurity =
    builder.Configuration.GetSection("Database:ConnectionString:IntegratedSecurity").Value ?? "True";
var encrypt = builder.Configuration.GetSection("Database:ConnectionString:Encrypt").Value ?? "True";
var trustServerCertificate =
    builder.Configuration.GetSection("Database:ConnectionString:TrustServerCertificate").Value ?? "True";
var trustedConnection =
    builder.Configuration.GetSection("Database:ConnectionString:Trusted_Connection").Value ?? "False";
var multipleActiveResultSets =
    builder.Configuration.GetSection("Database:ConnectionString:MultipleActiveResultSets").Value ?? "True";

var maxRetryCount = builder.Configuration.GetSection("Database:sqlServerOptionsAction:maxRetryCount").Value ?? "5";
var maxRetryDelay = builder.Configuration.GetSection("Database:sqlServerOptionsAction:maxRetryDelay").Value ?? "20";

var connectionString =
    $"Data Source = {dataSource}; Initial Catalog = {initialCatalog}; User Id = {userId}; " +
    $"Password={password}; Integrated Security = {integratedSecurity}; Encrypt = {encrypt}; " +
    $"TrustServerCertificate = {trustServerCertificate}; Trusted_Connection = {trustedConnection}; " +
    $"MultipleActiveResultSets = {multipleActiveResultSets};";

builder.Services.AddDbContext<AppDbContext>(
                                            options =>
                                            {
                                                options.UseSqlServer(connectionString,
                                                                     sqlServerOptionsAction: sqlOptions =>
                                                                     {
                                                                         sqlOptions.EnableRetryOnFailure(
                                                                          maxRetryCount: Convert.ToInt32(maxRetryCount),
                                                                          maxRetryDelay: TimeSpan.FromSeconds(Convert.ToInt32(maxRetryDelay)),
                                                                          errorNumbersToAdd: null);
                                                                     });
                                            });

#endregion

var app = builder.Build();

var development = builder.Configuration.GetSection("Development")?.Value.ToLower() ?? "false";

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment() || development is "true")
{
    app.UseExceptionHandler("/Error");
}

app.UseHsts();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

// Create and apply migrations, and seed the database.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    // Apply any pending migrations
    context.Database.Migrate();
    // Seed the database with initial data if needed
    // Your seeding logic here
}

app.Run();