using System.Text;
using ApiEcommerce.Constants;
using ApiEcommerce.Repository;
using ApiEcommerce.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var dbConnectionString = builder.Configuration.GetConnectionString("ConexionSql");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(dbConnectionString));

builder.Services.AddResponseCaching(options =>
{
    options.MaximumBodySize = 1024 * 1024; // 1 MB
    options.UseCaseSensitivePaths = true;
});


builder.Services.AddScoped<ICategoryRepository, CategoryRepository>(); // Dependency Injection
builder.Services.AddScoped<IProductRepository, ProductRepository>(); // Dependency Injection
builder.Services.AddScoped<IUserRepository, UserRepository>(); // Dependency Injection
builder.Services.AddAutoMapper(cfg => { // AutoMapper configuration
    cfg.AddMaps(typeof(Program).Assembly);
});

var secretKey = builder.Configuration.GetValue<string>("ApiSettings:SecretKey"); // Get secret key from configuration
if (string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException("Secret key not found in configuration.");
}

builder.Services.AddAuthentication(options => // Authentication configuration
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Set default authentication scheme
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Set default challenge scheme
}).AddJwtBearer(options => // JWT Bearer configuration
{
    options.RequireHttpsMetadata = false; // Disable HTTPS metadata requirement
    options.SaveToken = true; // Save token
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, // Validate issuer signing key
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), // Set issuer signing key
        ValidateIssuer = false, // Disable issuer validation
        ValidateAudience = false, // Disable audience validation
    };
    options.Authority = builder.Configuration["Auth0:Domain"]; // Set authority from configuration
    options.Audience = builder.Configuration["Auth0:Audience"]; // Set audience from configuration
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => // CORS configuration
{
    options.AddPolicy(PolicyNames.AllowSpecificOrigin, builder => // CORS policy
    {
        builder.WithOrigins("*") // Allow any origin
               .AllowAnyMethod() // Allow any method
               .AllowAnyHeader(); // Allow any header
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi().AllowAnonymous();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseCors(PolicyNames.AllowSpecificOrigin); // Use CORS policy

app.UseResponseCaching(); // Use response caching

app.UseAuthentication(); // Use authentication

app.UseAuthorization();

app.MapControllers();

app.Run();
