using System.ComponentModel.DataAnnotations;
using MariaEmMeuLar.Models;

namespace MariaEmMeuLar.Tests.Models;

public class UsuarioAdminTests
{
    [Fact]
    public void UsuarioAdmin_Valido_DevePassarValidacao()
    {
        var usuario = new UsuarioAdmin
        {
            Nome = "Administrador",
            Email = "admin@email.com",
            Password = "SenhaDeTeste123",
            Perfil = "Admin",
            Ativo = true
        };

        var contexto = new ValidationContext(usuario);
        var resultados = new List<ValidationResult>();

        var valido = Validator.TryValidateObject(
            usuario,
            contexto,
            resultados,
            true
        );

        Assert.True(valido);
        Assert.Empty(resultados);
    }

    [Fact]
    public void UsuarioAdmin_SemNome_DeveSerInvalido()
    {
        var usuario = new UsuarioAdmin
        {
            Nome = "",
            Email = "admin@email.com",
            Password = "SenhaDeTeste123",
            Perfil = "Admin"
        };

        var contexto = new ValidationContext(usuario);
        var resultados = new List<ValidationResult>();

        var valido = Validator.TryValidateObject(
            usuario,
            contexto,
            resultados,
            true
        );

        Assert.False(valido);

        Assert.Contains(
            resultados,
            r => r.MemberNames.Contains(nameof(UsuarioAdmin.Nome))
        );
    }

    [Fact]
    public void UsuarioAdmin_EmailInvalido_DeveSerInvalido()
    {
        var usuario = new UsuarioAdmin
        {
            Nome = "Administrador",
            Email = "email-invalido",
            Password = "SenhaDeTeste123",
            Perfil = "Admin"
        };

        var contexto = new ValidationContext(usuario);
        var resultados = new List<ValidationResult>();

        var valido = Validator.TryValidateObject(
            usuario,
            contexto,
            resultados,
            true
        );

        Assert.False(valido);

        Assert.Contains(
            resultados,
            r => r.MemberNames.Contains(nameof(UsuarioAdmin.Email))
        );
    }

    [Fact]
    public void UsuarioAdmin_SemEmail_DeveSerInvalido()
    {
        var usuario = new UsuarioAdmin
        {
            Nome = "Administrador",
            Email = "",
            Password = "SenhaDeTeste123",
            Perfil = "Admin"
        };

        var contexto = new ValidationContext(usuario);
        var resultados = new List<ValidationResult>();

        var valido = Validator.TryValidateObject(
            usuario,
            contexto,
            resultados,
            true
        );

        Assert.False(valido);

        Assert.Contains(
            resultados,
            r => r.MemberNames.Contains(nameof(UsuarioAdmin.Email))
        );
    }

    [Fact]
    public void UsuarioAdmin_SemPassword_DeveSerInvalido()
    {
        var usuario = new UsuarioAdmin
        {
            Nome = "Administrador",
            Email = "admin@email.com",
            Password = "",
            Perfil = "Admin"
        };

        var contexto = new ValidationContext(usuario);
        var resultados = new List<ValidationResult>();

        var valido = Validator.TryValidateObject(
            usuario,
            contexto,
            resultados,
            true
        );

        Assert.False(valido);

        Assert.Contains(
            resultados,
            r => r.MemberNames.Contains(nameof(UsuarioAdmin.Password))
        );
    }
}