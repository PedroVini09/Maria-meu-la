using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MariaEmMeuLar.Models;
using MariaEmMeuLar.Models.ViewModels;
using MariaEmMeuLar.Services;

namespace MariaEmMeuLar.Controllers;

public class HomeController : Controller
{
    private readonly IEmailService _emailService;
    public HomeController(IEmailService emailService)
    {
        _emailService = emailService;
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
    
    public IActionResult Inscricao()
    { 
        return View();
    }
    
    public IActionResult Contatos()
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