using System.ComponentModel.DataAnnotations;
using MariaEmMeuLar.Models;

namespace MariaEmMeuLar.Tests.Models;

public class GaleriaTests
{
    [Fact]
    public void Galeria_Valida_DevePassarValidacao()
    {
        var galeria = new Galeria
        {
            Titulo = "Retiro Quaresmal 2026",
            Descricao = "Fotos do Retiro Quaresmal.",
            CaminhoImagem = "/uploads/galeria/retiro.webp",
            MissaoId = 1,
            Ativa = true
        };

        var contexto = new ValidationContext(galeria);
        var resultados = new List<ValidationResult>();

        var valido = Validator.TryValidateObject(
            galeria,
            contexto,
            resultados,
            true
        );

        Assert.True(valido);
        Assert.Empty(resultados);
    }

    [Fact]
    public void Galeria_SemTitulo_DeveSerInvalida()
    {
        var galeria = new Galeria
        {
            Titulo = "",
            Descricao = "Foto de teste",
            CaminhoImagem = "/uploads/galeria/teste.webp",
            MissaoId = 1
        };

        var contexto = new ValidationContext(galeria);
        var resultados = new List<ValidationResult>();

        var valido = Validator.TryValidateObject(
            galeria,
            contexto,
            resultados,
            true
        );

        Assert.False(valido);

        Assert.Contains(
            resultados,
            r => r.MemberNames.Contains(nameof(Galeria.Titulo))
        );
    }

    [Fact]
    public void Galeria_SemImagem_DeveSerInvalida()
    {
        var galeria = new Galeria
        {
            Titulo = "Galeria Teste",
            Descricao = "Foto de teste",
            CaminhoImagem = "",
            MissaoId = 1
        };

        var contexto = new ValidationContext(galeria);
        var resultados = new List<ValidationResult>();

        var valido = Validator.TryValidateObject(
            galeria,
            contexto,
            resultados,
            true
        );

        Assert.False(valido);

        Assert.Contains(
            resultados,
            r => r.MemberNames.Contains(nameof(Galeria.CaminhoImagem))
        );
    }

    [Fact]
    public void Galeria_TituloMaiorQueLimite_DeveSerInvalida()
    {
        var galeria = new Galeria
        {
            Titulo = new string('A', 151),
            Descricao = "Foto de teste",
            CaminhoImagem = "/uploads/galeria/teste.webp",
            MissaoId = 1
        };

        var contexto = new ValidationContext(galeria);
        var resultados = new List<ValidationResult>();

        var valido = Validator.TryValidateObject(
            galeria,
            contexto,
            resultados,
            true
        );

        Assert.False(valido);

        Assert.Contains(
            resultados,
            r => r.MemberNames.Contains(nameof(Galeria.Titulo))
        );
    }

    [Fact]
    public void Galeria_DescricaoMaiorQueLimite_DeveSerInvalida()
    {
        var galeria = new Galeria
        {
            Titulo = "Galeria Teste",
            Descricao = new string('A', 301),
            CaminhoImagem = "/uploads/galeria/teste.webp",
            MissaoId = 1
        };

        var contexto = new ValidationContext(galeria);
        var resultados = new List<ValidationResult>();

        var valido = Validator.TryValidateObject(
            galeria,
            contexto,
            resultados,
            true
        );

        Assert.False(valido);

        Assert.Contains(
            resultados,
            r => r.MemberNames.Contains(nameof(Galeria.Descricao))
        );
    }

    [Fact]
    public void Galeria_CategoriaOutro_DeveSerValida()
    {
        var galeria = new Galeria
        {
            Titulo = "Encontro da Juventude",
            Descricao = "Fotos de outra atividade.",
            CaminhoImagem = "/uploads/galeria/encontro.webp",
            MissaoId = null,
            Ativa = true
        };

        var contexto = new ValidationContext(galeria);
        var resultados = new List<ValidationResult>();

        var valido = Validator.TryValidateObject(
            galeria,
            contexto,
            resultados,
            true
        );

        Assert.True(valido);
        Assert.Empty(resultados);
    }
}