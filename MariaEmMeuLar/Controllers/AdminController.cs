using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MariaEmMeuLar.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using MariaEmMeuLar.Models.ViewModels;

namespace MariaEmMeuLar.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(AppDbContext context, ILogger<AdminController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalInscricoes = await _context.Inscricoes.CountAsync();
            ViewBag.TotalProgramacoes = await _context.Programacoes.CountAsync();
            ViewBag.TotalNoticias = await _context.Noticias.CountAsync();
            ViewBag.TotalGaleria = await _context.Galerias.CountAsync();
            return View();
        }

        public async Task<IActionResult> Inscricoes(int? missaoId)
        {
           var query = _context.Inscricoes.Include(i => i.Missao).AsQueryable().AsNoTracking();

            if (missaoId.HasValue)
            {
                query = query.Where(i => i.MissaoId == missaoId.Value);
            }

            var inscricoes = await query.OrderByDescending(i => i.DataInscricao).ToListAsync();
            var missoes = await _context.Missoes.AsNoTracking().OrderBy(m => m.Nome).ToListAsync();
            var controleMissoes = await _context.Missoes.AsNoTracking().OrderBy(m => m.Nome).ToListAsync();
            ViewBag.Missoes = new SelectList(missoes, "Id", "Nome", missaoId);
            ViewBag.ControleMissoes = controleMissoes;
            return View(inscricoes);
        }

        public async Task<IActionResult> DetalhesInscricao(int id)
        {
            var inscricao = await _context.Inscricoes
                .AsNoTracking()
                .Include(i => i.Missao)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inscricao == null)
            {
                return NotFound();
            }

            return View(inscricao);
        }

        [HttpGet]
        public async Task<IActionResult> EditarInscricao(int id)
        {
            var inscricao = await _context.Inscricoes
                .AsNoTracking()
                .Include(i => i.Missao)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inscricao == null)
            {
                return NotFound();
            }

            var model = new EditarInscricaoViewModel
            {
                Id = inscricao.Id,

                MissaoId = inscricao.MissaoId,

                MissaoNome =
            inscricao.Missao?.Nome ?? "Missão não especificada",

                Nome = inscricao.Nome,
                Idade = inscricao.Idade,
                Telefone = inscricao.Telefone,
                Observacao = inscricao.Observacao,
                Status = inscricao.Status,

                Endereco = inscricao.Endereco,
                DataDesejada = inscricao.DataDesejada,
                HorarioDesejado = inscricao.HorarioDesejado,

                Comunidade = inscricao.Comunidade,
                JaParticipou = inscricao.JaParticipou,

                Grupo = inscricao.Grupo,
                Turno = inscricao.Turno,

                Participacao = inscricao.Participacao,
                DiaDisponivel = inscricao.DiaDisponivel
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarInscricao(int id, EditarInscricaoViewModel model)
        {

            var inscricao = await _context.Inscricoes
                .Include(i => i.Missao)
                .FirstOrDefaultAsync(i => i.Id == model.Id);

            if (inscricao == null)
            {
                return NotFound();
            }

            // Não confiamos nesses dados vindos do navegador.
            model.MissaoId = inscricao.MissaoId;
            model.MissaoNome =
                inscricao.Missao?.Nome ?? "Missão não especificada";


            var statusPermitidos = new[]
            {
        "Pendente",
        "Em análise",
        "Confirmada",
        "Cancelada"
    };

            if (!statusPermitidos.Contains(model.Status))
            {
                ModelState.AddModelError(
                    nameof(model.Status),
                    "Status inválido."
                );
            }


            if (!ModelState.IsValid)
            {
                return View(model);
            }


            try
            {
                // Campos gerais
                inscricao.Nome = model.Nome;
                inscricao.Idade = model.Idade;
                inscricao.Telefone = model.Telefone;
                inscricao.Observacao = model.Observacao;
                inscricao.Status = model.Status;


                // Campos específicos da missão
                switch (inscricao.Missao?.Nome)
                {
                    case "Maria em Meu Lar":

                        inscricao.Endereco = model.Endereco;
                        inscricao.DataDesejada = model.DataDesejada;
                        inscricao.HorarioDesejado = model.HorarioDesejado;

                        break;


                    case "Retiro Quaresmal":

                        inscricao.Comunidade = model.Comunidade;
                        inscricao.JaParticipou = model.JaParticipou;

                        break;


                    case "Semana da Juventude":

                        inscricao.Grupo = model.Grupo;
                        inscricao.Turno = model.Turno;

                        break;


                    case "Terço da Juventude":

                        inscricao.Participacao = model.Participacao;
                        inscricao.DiaDisponivel = model.DiaDisponivel;

                        break;


                    case "Segue-me Jovem":

                        inscricao.Comunidade = model.Comunidade;

                        break;
                }


                await _context.SaveChangesAsync();

                TempData["Sucesso"] =
                    "Inscrição atualizada com sucesso!";

                return RedirectToAction(
                    nameof(DetalhesInscricao),
                    new { id = inscricao.Id }
                );
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao atualizar inscrição {InscricaoId}.",
                    model.Id
                );

                ModelState.AddModelError(
                    string.Empty,
                    "Não foi possível atualizar a inscrição."
                );

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro inesperado ao atualizar inscrição {InscricaoId}.",
                    model.Id
                );

                ModelState.AddModelError(
                    string.Empty,
                    "Ocorreu um erro inesperado."
                );

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirInscricao(int id)
        {
            var inscricao = await _context.Inscricoes
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inscricao == null)
            {
                return NotFound();
            }

            try
            {
                _context.Inscricoes.Remove(inscricao);

                await _context.SaveChangesAsync();

                TempData["Sucesso"] =
                    "Inscrição excluída com sucesso!";

                return RedirectToAction(nameof(Inscricoes));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao excluir inscrição {InscricaoId}.",
                    id
                );

                TempData["Erro"] =
                    "Não foi possível excluir a inscrição.";

                return RedirectToAction(nameof(Inscricoes));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro inesperado ao excluir inscrição {InscricaoId}.",
                    id
                );

                TempData["Erro"] =
                    "Ocorreu um erro inesperado.";

                return RedirectToAction(nameof(Inscricoes));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AlternarInscricoesMissao(int id)
        {
            var missao = await _context.Missoes
                .FirstOrDefaultAsync(m => m.Id == id);

            if (missao == null)
            {
                return NotFound();
            }

            try
            {
                missao.InscricoesAbertas =
                    !missao.InscricoesAbertas;

                await _context.SaveChangesAsync();

                TempData["Sucesso"] =
                    missao.InscricoesAbertas
                        ? $"Inscrições de {missao.Nome} foram abertas."
                        : $"Inscrições de {missao.Nome} foram fechadas.";

                return RedirectToAction(nameof(Inscricoes));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao alterar inscrições da missão {MissaoId}.",
                    id
                );

                TempData["Erro"] =
                    "Não foi possível alterar o status das inscrições.";

                return RedirectToAction(nameof(Inscricoes));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro inesperado ao alterar missão {MissaoId}.",
                    id
                );

                TempData["Erro"] =
                    "Ocorreu um erro inesperado.";

                return RedirectToAction(nameof(Inscricoes));
            }
        }
    }
}