using Chapter.WebApi.Contexts;
using Chapter.WebApi.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ChapterContext,ChapterContext>();
builder.Services.AddControllers();
builder.Services.AddTransient<LivroRepository, LivroRepository>();
builder.Services.AddTransient<UsuarioRepository, UsuarioRepository>();
// Add services to the container.
;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Forma de autenticação
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";
})

// Parametros de validação token
.AddJwtBearer("JwtBearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Valida quem esta solicitando.
        ValidateIssuer = true,
        // Valida quem está recebendo.
        ValidateAudience = true,
        // Define se o tempo de expiracao será validado
        ValidateLifetime = true,
        // Criptografia e validacao da chave de autenticacao
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("chapterapi-chave-autenticacao")),
        // Valida o tempo de expiracao do token
        ClockSkew = TimeSpan.FromMinutes(30),
        // Nome do issuer, da origem
        ValidIssuer = "chapterapi.webapi",
        // Nome do audience, para o destino
        ValidAudience = "chapterapi.webapi"
    };
});

builder.Services.AddTransient<LivroRepository,LivroRepository>();
builder.Services.AddTransient<UsuarioRepository,UsuarioRepository>();

var app = builder.Build();

app.UseDeveloperExceptionPage();

app.UseRouting();

// habilitar a autenticacao
app.UseAuthentication();

// habilitar a autorizacao
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
  //  app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();



// app.MapControllers();

app.Run();
