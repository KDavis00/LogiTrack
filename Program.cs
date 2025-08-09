using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LogiTrack.Models;

var builder = WebApplication.CreateBuilder(args);

// ✅ Configure EF Core with SQLite
builder.Services.AddDbContext<LogiTrackContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<LogiTrackContext>()
    .AddDefaultTokenProviders();

// ✅ Configure JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? "super_secret_key";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "LogiTrackAPI";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "LogiTrackUsers";

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience
        };
    });

// ✅ Add memory cache
builder.Services.AddMemoryCache();

// ✅ Add authorization and controllers
builder.Services.AddAuthorization();
builder.Services.AddControllers();

// ✅ Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// ✅ Enable Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// ✅ Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

// ✅ Map controllers
app.MapControllers();

// ✅ Apply migrations and seed roles
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LogiTrackContext>();
    db.Database.Migrate();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await SeedRoles(roleManager);
}

// ✅ Run the app
app.Run();

// ✅ Role seeding helper
static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
{
    var roles = new[] { "User", "Admin" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}
