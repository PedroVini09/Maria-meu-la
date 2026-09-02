using System.Data;
using MariaEmMeuLar.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MariaEmMeuLar.Data
{
    public static class AdminSeeder
    {
        public static async Task CriarAdminInicialAsync(IServiceProvider service, IConfiguration configuration)
        {
            using var scope = service.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<UsuarioAdmin>>();

            var nome = configuration["AdminSeed:Nome"];
            var email = configuration["AdminSeed:Email"];
            var password = configuration["AdminSeed:Password"];

            if(string.IsNullOrWhiteSpace(nome)||
               string.IsNullOrWhiteSpace(email)||
               string.IsNullOrWhiteSpace(password))
            {
                return;
            }

            var adminExiste = await context.UsuariosAdmin
                .AnyAsync(u => u.Email == email);

            if (adminExiste)
            {
                return;
            }

            var admin = new UsuarioAdmin
            {
                Nome = nome,
                Email = email,
                Perfil = "Admin",
                Ativo = true,
                DataCriacao = DateTime.Now
            };

            admin.Password = passwordHasher.HashPassword(admin, password);

            context.UsuariosAdmin.Add(admin);

            await context.SaveChangesAsync();
        }
    }
}