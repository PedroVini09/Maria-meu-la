using System.ComponentModel.DataAnnotations;
using MariaEmMeuLar.Models;

namespace MariaEmMeuLar.Tests.Models;

public class ContatosTests
{
    [Fact]
    public void Contato_Valido_DevePassarValidacao()
    {
        // Arrange
        var contato = new Contatos
        {
            Nome = "Maria",
            Email = "maria@example.com",
            Telefone = "1234567890",
            Assunto = "Assunto de Teste",
            Mensagem = "Mensagem de Teste"        
        };

        var contexto = new ValidationContext(contato);
        var resultados = new List<ValidationResult>();

        // Act
        var valido = Validator.TryValidateObject(contato, contexto, resultados, true);  

        // Assert
        Assert.True(valido);
        Assert.Empty(resultados);
    }

    [Fact]
    public void Contato_SemNome_DeveSerInvalido()
    {
        // Arrange
        var contato = new Contatos
        {
            Nome = "", // Nome inválido
            Email = "maria@example.com",
            Telefone = "1234567890",
            Assunto = "Assunto de Teste",
            Mensagem = "Mensagem de Teste"
        };

        var contexto = new ValidationContext(contato);
        var resultados = new List<ValidationResult>();

        // Act
        var valido = Validator.TryValidateObject(contato, contexto, resultados, true);

        // Assert
        Assert.False(valido);
        Assert.Contains(resultados, r => r.MemberNames.Contains(nameof(Contatos.Nome)));
    }
    [Fact]

    public void Contato_EmailInvalido_DeveSerInvalido()
    {
        // Arrange
        var contato = new Contatos
        {
            Nome = "Maria",
            Email = "emailinvalido", // Email inválido
            Telefone = "1234567890",
            Assunto = "Assunto de Teste",
            Mensagem = "Mensagem de Teste"
        };

        var contexto = new ValidationContext(contato);
        var resultados = new List<ValidationResult>();

        // Act
        var valido = Validator.TryValidateObject(contato, contexto, resultados, true);

        // Assert
        Assert.False(valido);
        Assert.Contains(resultados, r => r.MemberNames.Contains(nameof(Contatos.Email)));
    }
    [Fact]
    public void Contato_SemTelefone_DeveSerInvalido()
    {
        // Arrange
        var contato = new Contatos
        {
            Nome = "Maria",
            Email = "maria@example.com",
            Telefone = "", // Telefone inválido
            Assunto = "Assunto de Teste",
            Mensagem = "Mensagem de Teste"   
        };

        var contexto = new ValidationContext(contato);
        var resultados = new List<ValidationResult>();

        // Act
        var valido = Validator.TryValidateObject(contato, contexto, resultados, true);

        // Assert
        Assert.False(valido);
        Assert.Contains(resultados, r => r.MemberNames.Contains(nameof(Contatos.Telefone)));
    }
    [Fact]
    public void Contato_SemAssunto_DeveSerInvalido()
    {
        // Arrange
        var contato = new Contatos
        {
            Nome = "Maria",
            Email = "maria@example.com",
            Telefone = "1112544348", 
            Assunto = "",// Assunto inválido
            Mensagem = "Mensagem de Teste"
        };

        var contexto = new ValidationContext(contato);
        var resultados = new List<ValidationResult>();

        // Act
        var valido = Validator.TryValidateObject(contato, contexto, resultados, true);

        // Assert
        Assert.False(valido);
        Assert.Contains(resultados, r => r.MemberNames.Contains(nameof(Contatos.Assunto)));
    }

    [Fact]
    public void Contato_SemMensagem_DeveSerInvalido()
    {
        // Arrange
        var contato = new Contatos
        {
            Nome = "Maria",
            Email = "maria@example.com",
            Telefone = "1112544348",
            Assunto = "Assunto de teste",
            Mensagem = "" // Mensagem Invalido
        };

        var contexto = new ValidationContext(contato);
        var resultados = new List<ValidationResult>();

        // Act
        var valido = Validator.TryValidateObject(contato, contexto, resultados, true);

        // Assert
        Assert.False(valido);
        Assert.Contains(resultados, r => r.MemberNames.Contains(nameof(Contatos.Mensagem)));
    }
}