using Assiginment.Services;
using ContactsApi.Middleware;
using ContactsApi.Services;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Net.Mail;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddSingleton<IContactService, ContactService>();


builder.Services.AddTransient<SmtpClient>(provider => new SmtpClient("smtp.gmail.com")
{
    Port = 587,
    Credentials = new NetworkCredential("your email ID ", "Password"),
    EnableSsl = true,
});

// Register IEmailService
builder.Services.AddScoped<IEmailService, EmailService>();

// Register Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Contacts API", Version = "v1" });
});

// Register CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policyBuilder => policyBuilder
            .WithOrigins("http://localhost:4200") // Replace with your frontend URL
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

// Middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
