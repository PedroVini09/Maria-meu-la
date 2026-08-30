using System.ComponentModel.DataAnnotations;
using MariaEmMeuLar.Models;

namespace MariaEmMeuLar.Tests.Models;

public class MissaoTests
{
    [Fact]
    public void Missao_Valida_DevePassarValidacao()
    {
        var missao = new Missao
        {
            Nome = "Maria em Meu Lar",
            Descricao = "Missão de evangelização junto às famílias.",
            Ativa = true
        };

        var contexto = new ValidationContext(missao);
        var resultados = new List<ValidationResult>();

        var valido = Validator.TryValidateObject(
            missao,
            contexto,
            resultados,
            true
        );

        Assert.True(valido);
        Assert.Empty(resultados);
    }
    [Fact]
    public void Missao_SemNome_DeveserInvalido()
    {
        var missao = new Missao
        {
            Nome = "", //Nome invalido
            Descricao = "Missão de evangelização junto às famílias.",
            Ativa = true
        };

        var contexto = new ValidationContext(missao);
        var resultados = new List<ValidationResult>();

        var valido = Validator.TryValidateObject(
            missao,
            contexto,
            resultados,
            true
        );

        Assert.False(valido);
        Assert.Contains(resultados, r=> r.MemberNames.Contains(nameof(Missao.Nome)));
    }
    [Fact]
    public void Missao_NomeMaiorQueLimite_DeveserInvalido()
    {
        var missao = new Missao
        {
            Nome = new string('A',101),
            Descricao = "Missão de evangelização junto às famílias.",
            Ativa = true
        };

        var contexto = new ValidationContext(missao);
        var resultados = new List<ValidationResult>();

        var valido = Validator.TryValidateObject(
            missao,
            contexto,
            resultados,
            true
        );

        Assert.False(valido);
        Assert.Contains(resultados, r => r.MemberNames.Contains(nameof(Missao.Nome)));
    }
}