using System.Text;
using Application.Interfaces;
using Application.Services;
using Infrastructure.Configurations;
using Infrastructure.Interfaces;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories.YildatRepository;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();


//Dependency Injection
builder.Services.AddScoped<IYildatService,YildatService>();
builder.Services.AddScoped<IYildatRepository,YildatRepository>();

builder.Services.AddScoped<IVerificationSmsCodeService,VerificationSmsCodeService>();
builder.Services.AddScoped<ITokenService,TokenService>();

builder.Services.AddMemoryCache();

builder.Services.AddDbContext<Noor17Context>();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));


builder.Services.AddAuthorization(opt=> {
    opt.AddPolicy("SmsVerificationOnly", policy => {
        policy.RequireClaim("purpose","sms-verification");
    });
});


builder.Services.AddAuthorization(opt=> {
    opt.AddPolicy("PaymentOnly", policy => {
        policy.RequireClaim("purpose","payment");
    });
});


builder.Services.AddAuthentication(opt=> {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.Run();

