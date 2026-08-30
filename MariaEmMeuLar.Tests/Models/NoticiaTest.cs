using System.ComponentModel.DataAnnotations;
using MariaEmMeuLar.Models;

namespace MariaEmMeuLar.Tests.Models;

public class NoticiaTest
{
    [Fact]
    public void Noticia_Valida_DevePassarValidacao()
    {
        var noticia = new Noticia
        {
            Titulo = "Notícia de Teste",
            Resumo = "Resumo da notícia",
            ImagemCapa = "/img/noticia.webp",
            LinkInstagram = "https://www.instagram.com/p/teste/",
            UsuarioAdminId = 1
        };

        var contexto = new ValidationContext(noticia);
        var resultados = new List<ValidationResult>();

        var valido = Validator.TryValidateObject(noticia,contexto, resultados, true );

        Assert.True(valido);
        Assert.Empty(resultados);
    }

    [Fact]
    public void Noticia_SemTitulo_DeveSerInvalida()
    {
        var noticia = new Noticia
        {
            Titulo = "",
            Resumo = "Resumo da notícia",
            ImagemCapa = "/img/noticia.webp",
            LinkInstagram = "https://www.instagram.com/p/teste/",
            UsuarioAdminId = 1
        };

        var contexto = new ValidationContext(noticia);
        var resultados = new List<ValidationResult>();

        var valido = Validator.TryValidateObject(noticia, contexto, resultados, true);

        Assert.False(valido);
        Assert.Contains(resultados, r => r.MemberNames.Contains(nameof(Noticia.Titulo)));
    }
    [Fact]
    public void Noticia_SemResumo_DeveSerInvalida()
    {
        var noticia = new Noticia
        {
            Titulo = "Noticia de Teste",
            Resumo = "",
            ImagemCapa = "/img/noticia.webp",
            LinkInstagram = "https://www.instagram.com/p/teste/",
            UsuarioAdminId = 1
        };

        var contexto = new ValidationContext(noticia);
        var resultados = new List<ValidationResult>();

        var valido = Validator.TryValidateObject(noticia, contexto, resultados, true);

        Assert.False(valido);
        Assert.Contains(resultados, r => r.MemberNames.Contains(nameof(Noticia.Resumo)));
    }

    [Fact]
    public void Noticia_SemImagem_DeveSerInvalida()
    {
        var noticia = new Noticia
        {
            Titulo = "Noticia de Teste",
            Resumo = "Resumo de teste",
            ImagemCapa = "",
            LinkInstagram = "https://www.instagram.com/p/teste/",
            UsuarioAdminId = 1
        };

        var contexto = new ValidationContext(noticia);
        var resultados = new List<ValidationResult>();

        var valido = Validator.TryValidateObject(noticia, contexto, resultados, true);

        Assert.False(valido);
        Assert.Contains(resultados, r => r.MemberNames.Contains(nameof(Noticia.ImagemCapa)));
    }
    [Fact]
    public void Noticia_SemLink_DeveSerInvalida()
    {
        var noticia = new Noticia
        {
            Titulo = "Noticia de Teste",
            Resumo = "Resumo de teste",
            ImagemCapa = "/img/noticia.webp",
            LinkInstagram = "",
            UsuarioAdminId = 1
        };

        var contexto = new ValidationContext(noticia);
        var resultados = new List<ValidationResult>();

        var valido = Validator.TryValidateObject(noticia, contexto, resultados, true);

        Assert.False(valido);
        Assert.Contains(resultados, r => r.MemberNames.Contains(nameof(Noticia.LinkInstagram)));
    }

    [Fact]
    public void Noticia_SemUsuarioAdmin_DeveSerInvalida()
    {
        var noticia = new Noticia
        {
            Titulo = "Noticia de Teste",
            Resumo = "Resumo de teste",
            ImagemCapa = "/img/noticia.webp",
            LinkInstagram = "https://www.instagram.com/p/teste/",
            UsuarioAdminId = 0
        };

        var contexto = new ValidationContext(noticia);
        var resultados = new List<ValidationResult>();

        var valido = Validator.TryValidateObject(noticia, contexto, resultados, true);

        Assert.False(valido);
        Assert.Contains(resultados, r => r.MemberNames.Contains(nameof(Noticia.UsuarioAdminId)));
    }
}