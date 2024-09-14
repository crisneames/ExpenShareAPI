using ExpenShareAPI.Repositories;
using ExpenShareAPI.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Repositories and Services
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IEventRepository, EventRepository>();
builder.Services.AddSingleton<PasswordService>();
builder.Services.AddTransient<UserService>();

// JWT Configuration
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ClockSkew = TimeSpan.Zero
    };
});

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors(options =>
    {
        //options.WithOrigins("http://localhost:3000");
        options.AllowAnyOrigin();  // Adjust based on your needs (use AllowSpecificOrigins for production)
        options.AllowAnyMethod();
        options.AllowAnyHeader();
        //options.AllowCredentials();
    });
}

app.Use(async (context, next) =>
{
    Console.WriteLine($"Request {context.Request.Method} {context.Request.Path}");
    await next.Invoke();
});

app.UseHttpsRedirection();

// Enable authentication and authorization middleware
app.UseAuthentication(); // JWT Authentication middleware
app.UseAuthorization();

app.MapControllers();

app.Run();