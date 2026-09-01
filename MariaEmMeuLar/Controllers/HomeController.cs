using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MariaEmMeuLar.Models;
using MariaEmMeuLar.Models.ViewModels;
using MariaEmMeuLar.Services;
using MariaEmMeuLar.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MariaEmMeuLar.Controllers;

public class HomeController : Controller
{
    private readonly IEmailService _emailService;
    private readonly AppDbContext _context;
    public HomeController(IEmailService emailService, AppDbContext context)
    {
        _emailService = emailService;
        _context = context;
    }
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Programacao()
    {
        return View();
    }
    public IActionResult Galeria()
    {
        return View();
    }

    // public IActionResult Inscricao()
    // { 
    //     return View();
    // }
    [HttpGet]
    public async Task<IActionResult> Inscricao()
    {
        var missoes = await _context.Missoes
            .Where(m => m.Ativa)
            .ToListAsync();

        ViewBag.MissaoIds = missoes.ToDictionary(
            m => m.Nome,
            m => m.Id
        );

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Inscricao(Inscricao inscricao)
    {
        var missaoExiste = await _context.Missoes
            .AnyAsync(m => m.Id == inscricao.MissaoId && m.Ativa);

        if (!missaoExiste)
        {
            ModelState.AddModelError(
                nameof(inscricao.MissaoId),
                "Selecione uma missão."
            );
        }

        if (!ModelState.IsValid)
        {
            var missoes = await _context.Missoes
                .Where(m => m.Ativa)
                .ToListAsync();

            ViewBag.MissaoIds = missoes.ToDictionary(
                m => m.Nome,
                m => m.Id
            );

            return View(inscricao);
        }

        inscricao.Status = "Pendente";
        inscricao.DataInscricao = DateTime.Now;

        _context.Inscricoes.Add(inscricao);
        await _context.SaveChangesAsync();

        TempData["Sucesso"] = "Inscrição realizada com sucesso!";

        return RedirectToAction(nameof(Inscricao));
    }
    public IActionResult Contatos()
    {
        return View();
    }

    public IActionResult MissaoMaria()
    {
        return View();
    }
    
    public IActionResult MissaoSemana()
    {
        return View();
    }

     public IActionResult MissaoTerco()
    {
        return View();
    }

    public IActionResult MissaoRetiro()
    {
        return View();
    }

    public IActionResult MissaoSegue()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EnviarContato(ContatoMensagemViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["ContatoErro"] = "Preencha todos os campos corretamente.";
            return RedirectToAction("Contatos");
        }

        try
        {
            await _emailService.EnviarMensagemContatoAsync(model);

            TempData["ContatoSucesso"] = "Mensagem enviada com sucesso!";
            return RedirectToAction("Contatos");
        }
        catch (Exception ex)
        {
            TempData["ContatoErro"] = "Erro ao enviar e-mail: " + ex.Message;
            return RedirectToAction("Contatos");
        }
    }
    
    public IActionResult Privacy()
    {
        return View();
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    
}