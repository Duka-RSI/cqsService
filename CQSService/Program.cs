using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using CQSService.Models;

var builder = WebApplication.CreateBuilder(args);

//Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.SetIsOriginAllowed(_ => true);
            //policy.WithOrigins("http://example.com",
            //                    "http://www.contoso.com");
            policy.AllowAnyMethod();
            policy.AllowAnyHeader();
            policy.AllowCredentials();
        });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAuthentication(IISDefaults.AuthenticationScheme);
builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<QuotaionContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefualtDatabase")));
var app = builder.Build();
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

app.Run();
