using System.ComponentModel.DataAnnotations;
using MariaEmMeuLar.Models;

namespace MariaEmMeuLar.Tests.Models;

public class ProgramacaoTests
{
    [Fact]
    public void Programacao_Valida_DevePassarValidacao()
    {
        // Arrange
        var programacao = new Programacao
        {
            Titulo = "Programação de Teste",
            Local = "Local de Teste",
            HoraInicial = new TimeSpan(10, 0, 0),
            HoraFinal = new TimeSpan(12, 0, 0),
            MissaoId = 1
            
        };

        var contexto = new ValidationContext(programacao);
        var resultados = new List<ValidationResult>();

        // Act
        var valido = Validator.TryValidateObject(programacao, contexto, resultados, true);

        // Assert
        Assert.True(valido);
        Assert.Empty(resultados);
    }

    [Fact]
    public void Programacao_SemTitulo_DeveSerInvalida()
    {
        // Arrange
        var programacao = new Programacao
        {
            Titulo = "", // Título inválido
            Local = "Local de Teste",
            HoraInicial = new TimeSpan(10, 0, 0),
            HoraFinal = new TimeSpan(12, 0, 0),
            MissaoId = 1
        };

        var contexto = new ValidationContext(programacao);
        var resultados = new List<ValidationResult>();

        // Act
        var valido = Validator.TryValidateObject(programacao, contexto, resultados, true);

        // Assert
        Assert.False(valido);
        Assert.Contains(resultados, r => r.MemberNames.Contains(nameof(Programacao.Titulo)));
    }
    [Fact]
    public void Programacao_SemLocal_DeveSerInvalida()
    {
        // Arrange
        var programacao = new Programacao
        {
            Titulo = "Programação de Teste",
            Local = "", // Local inválido
            HoraInicial = new TimeSpan(10, 0, 0),
            HoraFinal = new TimeSpan(12, 0, 0),
            MissaoId = 1
        };

        var contexto = new ValidationContext(programacao);
        var resultados = new List<ValidationResult>();

        // Act
        var valido = Validator.TryValidateObject(programacao, contexto, resultados, true);

        // Assert
        Assert.False(valido);
        Assert.Contains(resultados, r => r.MemberNames.Contains(nameof(Programacao.Local)));
    }

    [Fact]
    public void Programacao_SemHoraInicial_DeveSerInvalida()
    {
        // Arrange
        var programacao = new Programacao
        {
            Titulo = "Programação de Teste",
            Local = "Local de Teste",
            HoraInicial = TimeSpan.Zero, // Hora inicial inválida
            HoraFinal = new TimeSpan(12, 0, 0),
            MissaoId = 1
        };

        var contexto = new ValidationContext(programacao);
        var resultados = new List<ValidationResult>();

        // Act
        var valido = Validator.TryValidateObject(programacao, contexto, resultados, true);

        // Assert
        Assert.False(valido);
        Assert.Contains(resultados, r => r.MemberNames.Contains(nameof(Programacao.HoraInicial)));
    }

    [Fact]
    public void Programacao_HoraFinalMenorQueInicial_DeveSerInvalida()
    {
        // Arrange
        var programacao = new Programacao
        {
            Titulo = "Programação de Teste",
            Local = "Local de Teste",
            HoraInicial = new TimeSpan(10, 0, 0),
            HoraFinal = new TimeSpan(8, 0, 0), // Hora final inválida
            MissaoId = 1
        };

        var contexto = new ValidationContext(programacao);
        var resultados = new List<ValidationResult>();

        // Act
        var valido = Validator.TryValidateObject(programacao, contexto, resultados, true);

        // Assert
        Assert.False(valido);
        Assert.Contains(resultados, r => r.MemberNames.Contains(nameof(Programacao.HoraFinal)));
    }

    [Fact]
    public void Programacao_SemHoraFinal_DeveSerInvalida()
    {
        // Arrange
        var programacao = new Programacao
        {
            Titulo = "Programação de Teste",
            Local = "Local de Teste",
            HoraInicial = new TimeSpan(10, 0, 0),
            HoraFinal = TimeSpan.Zero, // Hora final inválida
            MissaoId = 1
        };

        var contexto = new ValidationContext(programacao);
        var resultados = new List<ValidationResult>();

        // Act
        var valido = Validator.TryValidateObject(programacao, contexto, resultados, true);

        // Assert
        Assert.False(valido);
        Assert.Contains(resultados, r => r.MemberNames.Contains(nameof(Programacao.HoraFinal)));
    }
    
    [Fact]
    public void Programacao_SemMissao_DeveSerInvalida()
    {
        // Arrange
        var programacao = new Programacao
        {
            Titulo = "Programação de Teste",
            Local = "Local de Teste",
            HoraInicial = new TimeSpan(10, 0, 0),
            HoraFinal = new TimeSpan(12, 0, 0),
            MissaoId = 0// Missão inválida
        };

        var contexto = new ValidationContext(programacao);
        var resultados = new List<ValidationResult>();

        // Act
        var valido = Validator.TryValidateObject(programacao, contexto, resultados, true);

        // Assert
        Assert.False(valido);
        Assert.Contains(resultados, r => r.MemberNames.Contains(nameof(Programacao.MissaoId)));
    }
}
