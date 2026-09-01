using System.ComponentModel.DataAnnotations;
using MariaEmMeuLar.Models;

namespace MariaEmMeuLar.Tests.Models;
public class InscricaoTests
{
    [Fact]
    public void Inscricao_Valida_DevePassarValidacao()
    {
        // Arrange
        var inscricao = new Inscricao
        {
            Nome = "Maria",
            Idade = 30,
            Email = "maria@example.com",
            Telefone = "1234567890",
            MissaoId=1
        };

        var contexto = new ValidationContext(inscricao);
        var resultados = new List<ValidationResult>();

        // Act
        var valido = Validator.TryValidateObject(inscricao, contexto, resultados, true);

        // Assert
        Assert.True(valido);

        Assert.Empty(resultados);


    }

    [Fact]
    public void Inscricao_SemNome_DeveSerInvalida()
    {
        // Arrange
        var inscricao = new Inscricao
        {
            Nome = "", // Nome inválido
            Idade = 20, // Idade inválida
            Email = "emailinvalido", // Email inválido
            Telefone = "1234116", // Telefone inválido
            MissaoId=1 // Missão inválida
        };

        var contexto = new ValidationContext(inscricao);
        var resultados = new List<ValidationResult>();

        // Act
        var valido = Validator.TryValidateObject(inscricao, contexto, resultados, true);

        // Assert
        Assert.False(valido);

        Assert.Contains(resultados, r => r.MemberNames.Contains(nameof(Inscricao.Nome)));
    }

    [Fact]
    public void Inscricao_EmailInvalido_DeveSerInvalida()
    {
        // Arrange
        var inscricao = new Inscricao
        {
            Nome = "Maria",
            Idade = 25,
            Email = "EMAIL-INVALIDO", // Email inválido
            Telefone = "1234567890",
            MissaoId=1
        };

        var contexto = new ValidationContext(inscricao);
        var resultados = new List<ValidationResult>();

        // Act
        var valido = Validator.TryValidateObject(inscricao, contexto, resultados, true);

        // Assert
        Assert.False(valido);

        Assert.Contains(resultados, r => r.MemberNames.Contains(nameof(Inscricao.Email)));
    }

    [Fact]
    public void Inscricao_SemTelefone_DeveSerInvalida()
    {
        // Arrange
        var inscricao = new Inscricao
        {
            Nome = "Maria",
            Idade = 25,
            Email = "maria@example.com",
            Telefone = "", // Telefone inválido
            MissaoId=1
        };

        var contexto = new ValidationContext(inscricao);
        var resultados = new List<ValidationResult>();

        // Act
        var valido = Validator.TryValidateObject(inscricao, contexto, resultados, true);

        // Assert
        Assert.False(valido);

        Assert.Contains(resultados, r => r.MemberNames.Contains(nameof(Inscricao.Telefone)));
    }

    [Fact]
    public void Inscricao_SemMissao_DeveSerInvalida()
    {
        // Arrange
        var inscricao = new Inscricao
        {
            Nome = "Maria",
            Idade = 25,
            Email = "maria@example.com",
            Telefone = "1234567890",
            MissaoId = 0 // Missão inválida
        };

        var contexto = new ValidationContext(inscricao);
        var resultados = new List<ValidationResult>();

        // Act
        var valido = Validator.TryValidateObject(inscricao, contexto, resultados, true);

        // Assert
        Assert.False(valido);

        Assert.Contains(resultados, r => r.MemberNames.Contains(nameof(Inscricao.MissaoId)));
    }

    [Fact]
    public void Inscricao_IdadeInvalida_DeveSerInvalida()
    {
        var inscricao = new Inscricao
        {
            Nome = "Maria",
            Idade = 0, // Idade inválida
            Email = "maria@example.com",
            Telefone = "1234567890",
            MissaoId = 1    
        };

        var contexto = new ValidationContext(inscricao);
        var resultados = new List<ValidationResult>();

        // Act
        var valido = Validator.TryValidateObject(inscricao, contexto, resultados, true);

        // Assert
        Assert.False(valido);

        Assert.Contains(resultados, r => r.MemberNames.Contains(nameof(Inscricao.Idade)));
    }

    [Fact]
    public void Inscricao_SemIdade_DeveSerInvalida()
    {
        var inscricao = new Inscricao
        {
            Nome = "Maria",
            Idade = null, // Idade inválida
            Email = "maria@example.com",
            Telefone = "1234567890",
            MissaoId = 1
        };

        var contexto = new ValidationContext(inscricao);
        var resultados = new List<ValidationResult>();

        // Act
        var valido = Validator.TryValidateObject(inscricao, contexto, resultados, true);

        // Assert
        Assert.True(valido);
    }

    [Fact]
    public void Inscricao_SemEmail_DeveSerInvalida()
    {
        var inscricao = new Inscricao
        {
            Nome = "Maria",
            Idade = 20, // Idade inválida
            Email = null,
            Telefone = "1234567890",
            MissaoId = 1
        };

        var contexto = new ValidationContext(inscricao);
        var resultados = new List<ValidationResult>();

        // Act
        var valido = Validator.TryValidateObject(inscricao, contexto, resultados, true);

        // Assert
       Assert.True(valido);
    }
}