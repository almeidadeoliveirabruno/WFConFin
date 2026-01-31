using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WFConFin.Data;
using WFConFin.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("ConnectionSqlServer"); // entre aspas duplas o nome da string de conexão
builder.Services.AddDbContext<WFConfinDbContext>(options =>
    options.UseSqlServer(connectionString)
);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var chave = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("Chave").Get<string>()); // transformando a chave em bytes

builder.Services.AddAuthentication(options =>
{
    // Define que o padrão de autenticação da API será JWT Bearer
    // Ou seja: a API vai esperar um token no header Authorization: Bearer TOKEN
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

    // Define o comportamento quando o usuário NÃO estiver autenticado
    // Normalmente retorna HTTP 401 (Unauthorized)
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
} 
).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey =  true, //para validar a chave de assinatura
        IssuerSigningKey = new SymmetricSecurityKey(chave), //definindo a chave de assinatura
        ValidateIssuer = false, //se vai validar quem está emitindo o token
        ValidateAudience = false //se vai validar quem está recebendo o token
    };
});

builder.Services.AddSingleton<TokenService>(); 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); //Utilizando os serviço que foi configurado anteriormente

app.UseAuthorization();

app.MapControllers();

app.Run();

//service configura e app utiliza