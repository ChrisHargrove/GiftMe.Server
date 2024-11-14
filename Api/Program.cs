using Api.Authorization;
using Api.ErrorHandling;
using DataLayer.Database;
using DataLayer.Repositories.Abstract;
using Helpers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Models.Identity;
using ServiceLayer.Services.Abstract;
using SwaggerThemes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(opt => {
    opt.Filters.Add<HttpResponseExceptionFilter>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
    .AddEndpointsApiExplorer()
    .AddAuthSwaggerGen()
    .AddLogging(logs => {
        logs.AddConsole();
        logs.AddDebug();
    });

builder.Services
    .AddDbContext<DatabaseContext>()
    .AddAllScoped<IService>()
    .AddAllScoped<IRepository>()
    .AddFirebaseApp(builder.Configuration)
    .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
    .AddScoped<IAuthorizationHandler, RefreshTokenHandler>();

builder.Services
    .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies().ToList());
    
builder.Services
    .AddAuthentication()
    .AddJwtBearer("Firebase", opt => {
        opt.SaveToken = true;
        opt.Authority = builder.Configuration["Jwt:Firebase:ValidIssuer"];
        opt.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Firebase:ValidIssuer"],
            ValidAudience = builder.Configuration["Jwt:Firebase:ValidAudience"]
        };
    });
builder.Services
    .AddAuthorization(opt => {
        opt.AddPolicy("Security", policy => {
            policy.AddAuthenticationSchemes("Firebase");
            policy.Requirements.Add(new NonExpiredIdTokenRequirement());
        });
        opt.AddPolicy("Admin", policy => {
            policy.Requirements.Add(new AccountRoleRequirement(AccountRole.Admin, AccountRole.Owner));
        });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerThemes(Theme.NordDark);
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
