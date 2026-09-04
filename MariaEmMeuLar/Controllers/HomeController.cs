using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MariaEmMeuLar.Models;
using MariaEmMeuLar.Models.ViewModels;
using MariaEmMeuLar.Services;
using MariaEmMeuLar.Data;
using Microsoft.EntityFrameworkCore;

namespace MariaEmMeuLar.Controllers;

public class HomeController : Controller
{
    private readonly IEmailService _emailService;
    private readonly AppDbContext _context;

    private readonly ILogger<HomeController> _logger;
    public HomeController(IEmailService emailService, AppDbContext context, ILogger<HomeController> logger)
    {
        _emailService = emailService;
        _context = context;
        _logger = logger;
    }
    public async Task<IActionResult> Index()
    {
        var missoes = await _context.Missoes
            .Where(m =>
                m.Ativa &&
                m.ExibirIndex)
            .AsNoTracking()
            .OrderBy(m => m.OrdemExibicao)
            .ThenBy(m => m.Nome)
            .ToListAsync();

        return View(missoes);
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
        await CarregarMissoesAsync();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Inscricao(Inscricao inscricao)
    {
        var missao = await _context.Missoes.AsNoTracking().FirstOrDefaultAsync( m => m.Id == inscricao.MissaoId);

        if (missao == null || !missao.Ativa)
        {
            ModelState.AddModelError(
                nameof(inscricao.MissaoId),
                "Selecione uma missão válida."
            );
        }
        else if (!missao.InscricoesAbertas)
        {
            ModelState.AddModelError(
                nameof(inscricao.MissaoId),
                "As inscrições para esta missão estão fechadas."
            );
        }

        if (!ModelState.IsValid)
        {

            await CarregarMissoesAsync();

            TempData["Erro"] = "Verifique os dados informados.";

            return View(inscricao);
            // var missoes = await _context.Missoes
            //     .Where(m => m.Ativa)
            //     .ToListAsync();

            // ViewBag.MissaoIds = missoes.ToDictionary(
            //     m => m.Nome,
            //     m => m.Id
            // );

            // return View(inscricao);
        }

        try
        {
            inscricao.Status = "Pendente";
            inscricao.DataInscricao = DateTime.Now;

            _context.Inscricoes.Add(inscricao);
            await _context.SaveChangesAsync();

            TempData["Sucesso"] = "Inscrição realizada com sucesso!";

            return RedirectToAction(nameof(Inscricao));
        }
        catch(DbUpdateException ex)
        {
            _logger.LogError(ex, "Error ao salvar inscrição no banco.");

            await CarregarMissoesAsync();

            TempData["Sucesso"] = "Sua Inscrição foi enviada com Sucesso";
            
            return RedirectToAction(nameof(Inscricao));
        }
        catch(Exception ex)
        {
            _logger.LogError( ex ," Erro inesperado ao processar inscrição");

            await CarregarMissoesAsync();

            TempData["Erro"] = "Ocorreu um erro inesperado. Tente novamente.";

            return View(inscricao);
        }
    }

    private async Task CarregarMissoesAsync()
    {
        var missoes = await _context.Missoes.Where(m => m.Ativa).AsNoTracking() .ToListAsync();

        ViewBag.MissaoIds = missoes.ToDictionary(  m => m.Nome,  m => m.Id);

        ViewBag.MissaoStatus = missoes.ToDictionary( m => m.Nome, m => m.InscricoesAbertas );

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
            TempData["Erro"] = "Preencha todos os campos corretamente.";
            return RedirectToAction("Contatos");
        }

        try
        {
            await _emailService.EnviarMensagemContatoAsync(model);

            TempData["Sucesso"] = "Mensagem enviada com sucesso!";
            return RedirectToAction("Contatos");
        }
        catch (Exception ex)
        {
          _logger.LogError(ex, "Erro ao enviar mensagem pelo formulario de contato.");

          TempData["Erro"]= "Não foi possivel enviar sua mensagem. Tente novamente";

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