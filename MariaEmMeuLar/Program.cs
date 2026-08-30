using DotNetEnv;
using MariaEmMeuLar.Models;
using MariaEmMeuLar.Services;
using MariaEmMeuLar.Data;
using Microsoft.EntityFrameworkCore;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddScoped<IEmailService, EmailService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

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
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();