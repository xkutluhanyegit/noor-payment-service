using System.Text;
using Application.Interfaces;
using Application.Services;
using Infrastructure.Configurations;
using Infrastructure.ExternalServices.KizilbukSmsService;
using Infrastructure.Interfaces;
using Infrastructure.Payments.HashHelper;
using Infrastructure.Payments.Interfaces;
using Infrastructure.Payments.Providers.Halkbank;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories.OdooYildatOdeme;
using Infrastructure.Persistence.Repositories.YildatRepository;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


//CORS policy
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowPayten", builder =>
//     {
//         builder.WithOrigins("https://sanalpos.halkbank.com.tr")
//                .AllowAnyHeader()
//                .AllowAnyMethod()
//                .AllowCredentials(); // Gerekirse
//     });
// });



Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

//Dependency Injection
builder.Services.AddScoped<IYildatService,YildatService>();
builder.Services.AddScoped<IYildatRepository,YildatRepository>();
builder.Services.AddScoped<IOdooService, OdooService>();
builder.Services.AddScoped<IOdooAppService, OdooAppService>();
builder.Services.AddScoped<IHashService, HashService>();

builder.Services.AddScoped<IHashHelper, HashHelper>();

builder.Services.AddScoped<IYildatPaymentResponseService, YildatPaymentResponseService>();
builder.Services.AddScoped<IYildatPaymentResponseRepository, YildatPaymentResponseRepository>();


builder.Services.AddScoped<IVerificationSmsCodeService,VerificationSmsCodeService>();
builder.Services.AddScoped<IKizilbukSmsService,KizilbukSmsService>();
builder.Services.AddScoped<ITokenService,TokenService>();
builder.Services.AddScoped<IAuthRequestCodeService, AuthRequestCodeService>();

builder.Services.AddScoped<IPaymentProvider, HalkbankPaymentProvider>();


builder.Services.Configure<HalkbankPaymentProviderOptions>(builder.Configuration.GetSection("HalkbankSettings"));



builder.Services.AddHttpClient();

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

// app.UseCors("AllowPayten");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.Run();

