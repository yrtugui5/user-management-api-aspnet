using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using P2Api;
using P2Api.Data;
using P2Api.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var key = Encoding.ASCII.GetBytes(Settings.Secret);

// JWT Token Service Configuration
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
builder.Services.AddTransient<TokenService>();

// Policy Authorization Configuration
builder.Services.AddAuthorization(x => {
    x.AddPolicy("Adm", policy => policy.RequireRole("Adm"));
    x.AddPolicy("User", policy => policy.RequireRole("User"));
});

// Permite a utilização de Controllers
builder.Services.AddControllers();
// Configura o DbContext como Serviço
builder.Services.AddDbContext<AppDbContext>();

// Swagger Configuration
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo { Title = "Desafio P2", Version = "v1" });

    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Write a valid JWT Bearer Token to get authenticated."
    });
    x.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddEndpointsApiExplorer();



var app = builder.Build();
// Utiliza as Configurações de Policy & Token.
app.UseAuthentication();
app.UseAuthorization();
// Utiliza os Métodos Desenvolvidos do Controller.
app.MapControllers();
// Desenvolve a Documentação do Projeto
app.UseSwagger();
// Desenvolve uma interface grafica para o usuario manipular os dados
app.UseSwaggerUI();


app.Run();
