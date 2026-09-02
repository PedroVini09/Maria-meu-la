using DotNetEnv;
using MariaEmMeuLar.Models;
using MariaEmMeuLar.Services;
using MariaEmMeuLar.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

//Email Service registrado
builder.Services.AddScoped<IEmailService, EmailService>();

// registrar os servicos 
builder.Services.AddControllersWithViews();

//JWT Service registrado
builder.Services.AddScoped<IJwtService, JwtService>();

//Hash 
builder.Services.AddScoped<IPasswordHasher<UsuarioAdmin>,PasswordHasher<UsuarioAdmin>>();

//configuração jwt e caso falte para o sistema imediatamente
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("A chave JWT não foi configurada.");

var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("O Issuer JWT não foi configurado.");
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("A Audience JWT não foi configurada.");

builder.Services
   .AddAuthentication(Options =>
   {
       Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;//Descobrir autenticação

       Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;//Sem Autenticação
   })
   //Validar o Jwt
   .AddJwtBearer(Options =>
   {
       Options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuer = true,
           ValidateAudience = true,
           ValidateLifetime = true,
           ValidateIssuerSigningKey = true,

           ValidIssuer = jwtIssuer,
           ValidAudience =jwtAudience,

           IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),

           ClockSkew = TimeSpan.Zero
       };
       Options.Events = new JwtBearerEvents
       {
           OnMessageReceived= context =>
           {
               if(context.Request.Cookies.TryGetValue("AdminToken", out var token))
               {
                   context.Token = token;
               }

               return Task.CompletedTask;
           },

           OnChallenge = context =>
           {
               context.HandleResponse();

               context.Response.Redirect("/Auth/Login");

               return Task.CompletedTask;
           }
       };
   });

   builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await AdminSeeder.CriarAdminInicialAsync(app.Services, app.Configuration);
}

app.MapGet("/teste-banco", async (AppDbContext db) =>
{
    try
    {
        var missoes = await db.Missoes.CountAsync();
        var inscricoes = await db.Inscricoes.CountAsync();
        var programacoes = await db.Programacoes.CountAsync();
        var galerias = await db.Galerias.CountAsync();
        var contatos = await db.Contatos.CountAsync();
        var noticias = await db.Noticias.CountAsync();
        var usuariosAdmin = await db.UsuariosAdmin.CountAsync();
        return Results.Ok(new
        {
            Mensagem = "Conexão com o banco de dados bem-sucedida.",
            Missoes = missoes,
            Inscricoes = inscricoes,
            Programacoes = programacoes,
            Galeria = galerias,
            Contatos = contatos,
            Noticias = noticias,
            UsuariosAdmin = usuariosAdmin
        });
    }
    catch (Exception ex)
    {
        return Results.Problem("Erro ao acessar o banco de dados.", ex.Message);
    }
});
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();