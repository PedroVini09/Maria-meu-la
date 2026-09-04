using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MariaEmMeuLar.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using MariaEmMeuLar.Models.ViewModels;
using MariaEmMeuLar.Models;

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

                return RedirectToAction(nameof(ControleInscricoes));
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

                return RedirectToAction(nameof(ControleInscricoes));
            }
        }

        [HttpGet]
        public async Task<IActionResult> NovaInscricao()
        {

            var missoes = await _context.Missoes
                .Where(m => m.Ativa)
                .AsNoTracking()
                .OrderBy(m => m.Nome)
                .ToListAsync();

            ViewBag.Missoes = new SelectList(
                missoes,
                "Id",
                "Nome"
            );

            return View(new NovaInscricaoViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NovaInscricao(
    NovaInscricaoViewModel model)
        {
            var missao = await _context.Missoes
                .AsNoTracking()
                .FirstOrDefaultAsync(m =>
                    m.Id == model.MissaoId &&
                    m.Ativa);

            if (missao == null)
            {
                ModelState.AddModelError(
                    nameof(model.MissaoId),
                    "Selecione uma missão válida."
                );
            }


            // ===============================
            // VALIDAR STATUS
            // ===============================

            string[] statusPermitidos =
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
                    "Selecione um status válido."
                );
            }


            // ===============================
            // VALIDAÇÕES POR MISSÃO
            // ===============================

            if (missao != null)
            {
                switch (missao.Nome)
                {
                    case "Maria em Meu Lar":

                        if (string.IsNullOrWhiteSpace(model.Endereco))
                        {
                            ModelState.AddModelError(
                                nameof(model.Endereco),
                                "Informe o endereço."
                            );
                        }

                        if (!model.DataDesejada.HasValue)
                        {
                            ModelState.AddModelError(
                                nameof(model.DataDesejada),
                                "Informe a data desejada."
                            );
                        }

                        if (!model.HorarioDesejado.HasValue)
                        {
                            ModelState.AddModelError(
                                nameof(model.HorarioDesejado),
                                "Informe o horário desejado."
                            );
                        }

                        break;


                    case "Retiro Quaresmal":

                        if (!model.Idade.HasValue)
                        {
                            ModelState.AddModelError(
                                nameof(model.Idade),
                                "Informe a idade."
                            );
                        }

                        if (string.IsNullOrWhiteSpace(model.Comunidade))
                        {
                            ModelState.AddModelError(
                                nameof(model.Comunidade),
                                "Informe a comunidade."
                            );
                        }

                        if (string.IsNullOrWhiteSpace(model.JaParticipou))
                        {
                            ModelState.AddModelError(
                                nameof(model.JaParticipou),
                                "Informe se já participou de outro retiro."
                            );
                        }

                        break;


                    case "Semana da Juventude":

                        if (!model.Idade.HasValue)
                        {
                            ModelState.AddModelError(
                                nameof(model.Idade),
                                "Informe a idade."
                            );
                        }

                        if (string.IsNullOrWhiteSpace(model.Grupo))
                        {
                            ModelState.AddModelError(
                                nameof(model.Grupo),
                                "Informe o grupo ou pastoral."
                            );
                        }

                        if (string.IsNullOrWhiteSpace(model.Turno))
                        {
                            ModelState.AddModelError(
                                nameof(model.Turno),
                                "Informe o turno."
                            );
                        }

                        break;


                    case "Terço da Juventude":

                        if (string.IsNullOrWhiteSpace(model.Participacao))
                        {
                            ModelState.AddModelError(
                                nameof(model.Participacao),
                                "Informe como a pessoa participará."
                            );
                        }

                        if (string.IsNullOrWhiteSpace(model.DiaDisponivel))
                        {
                            ModelState.AddModelError(
                                nameof(model.DiaDisponivel),
                                "Informe o dia disponível."
                            );
                        }

                        break;


                    case "Segue-me Jovem":

                        if (!model.Idade.HasValue)
                        {
                            ModelState.AddModelError(
                                nameof(model.Idade),
                                "Informe a idade."
                            );
                        }

                        if (string.IsNullOrWhiteSpace(model.Comunidade))
                        {
                            ModelState.AddModelError(
                                nameof(model.Comunidade),
                                "Informe a comunidade."
                            );
                        }

                        break;
                }
            }


            // ===============================
            // SE HOUVER ERRO
            // ===============================

            if (!ModelState.IsValid)
            {
                await CarregarMissoesNovaInscricaoAsync(
                    model.MissaoId
                );

                return View(model);
            }


            try
            {
                // ===============================
                // DADOS GERAIS
                // ===============================

                var inscricao = new Inscricao
                {
                    Nome = model.Nome.Trim(),
                    Telefone = model.Telefone.Trim(),
                    Idade = model.Idade,

                    MissaoId = model.MissaoId,

                    Status = model.Status,
                    Observacao = model.Observacao?.Trim(),

                    DataInscricao = DateTime.Now
                };


                // ===============================
                // DADOS ESPECÍFICOS
                // ===============================

                switch (missao!.Nome)
                {
                    case "Maria em Meu Lar":

                        inscricao.Endereco =
                            model.Endereco?.Trim();

                        inscricao.DataDesejada =
                            model.DataDesejada;

                        inscricao.HorarioDesejado =
                            model.HorarioDesejado;

                        break;


                    case "Retiro Quaresmal":

                        inscricao.Comunidade =
                            model.Comunidade?.Trim();

                        inscricao.JaParticipou =
                            model.JaParticipou;

                        break;


                    case "Semana da Juventude":

                        inscricao.Grupo =
                            model.Grupo?.Trim();

                        inscricao.Turno =
                            model.Turno;

                        break;


                    case "Terço da Juventude":

                        inscricao.Participacao =
                            model.Participacao;

                        inscricao.DiaDisponivel =
                            model.DiaDisponivel;

                        break;


                    case "Segue-me Jovem":

                        inscricao.Comunidade =
                            model.Comunidade?.Trim();

                        break;
                }


                // ===============================
                // SALVAR
                // ===============================

                _context.Inscricoes.Add(inscricao);

                await _context.SaveChangesAsync();


                TempData["Sucesso"] =
                    "Inscrição cadastrada com sucesso.";


                return RedirectToAction(
                    nameof(DetalhesInscricao),
                    new { id = inscricao.Id }
                );
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Erro de banco ao cadastrar inscrição manual."
                );

                ModelState.AddModelError(
                    string.Empty,
                    "Não foi possível cadastrar a inscrição."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao cadastrar inscrição manual."
                );

                ModelState.AddModelError(
                    string.Empty,
                    "Ocorreu um erro ao cadastrar a inscrição."
                );
            }


            await CarregarMissoesNovaInscricaoAsync(
                model.MissaoId
            );

            return View(model);
        }

        private async Task CarregarMissoesNovaInscricaoAsync(int? missaoId = null)
        {
            var missoes = await _context.Missoes
                .Where(m => m.Ativa)
                .AsNoTracking()
                .OrderBy(m => m.Nome)
                .ToListAsync();

            ViewBag.Missoes = new SelectList(
                missoes,
                "Id",
                "Nome",
                missaoId
            );
        }

        [HttpGet]
        public async Task<IActionResult>ControleInscricoes()
        {
            var missoes = await _context.Missoes
                .AsNoTracking()
                .OrderBy(m => m.Nome)
                .ToListAsync();

            return View(missoes);
        }

        [HttpGet]
        public IActionResult NovaMissao()
        {
            return View(new NovaMissaoViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NovaMissao(
    NovaMissaoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var nome = model.Nome.Trim();

                // Evita cadastrar duas missões com o mesmo nome
                var nomeJaExiste = await _context.Missoes
                    .AsNoTracking()
                    .AnyAsync(m => m.Nome == nome);

                if (nomeJaExiste)
                {
                    ModelState.AddModelError(
                        nameof(model.Nome),
                        "Já existe uma missão cadastrada com esse nome."
                    );

                    return View(model);
                }


                var missao = new Missao
                {
                    Nome = nome,

                    Descricao = string.IsNullOrWhiteSpace(model.Descricao)
                        ? null
                        : model.Descricao.Trim(),

                    Ativa = model.Ativa,

                    InscricoesAbertas = model.InscricoesAbertas
                };


                _context.Missoes.Add(missao);

                await _context.SaveChangesAsync();


                TempData["Sucesso"] =
                    "Missão cadastrada com sucesso.";


                return RedirectToAction(
                    nameof(ControleInscricoes)
                );
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Erro de banco ao cadastrar nova missão."
                );

                ModelState.AddModelError(
                    string.Empty,
                    "Não foi possível cadastrar a missão."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao cadastrar nova missão."
                );

                ModelState.AddModelError(
                    string.Empty,
                    "Ocorreu um erro ao cadastrar a missão."
                );
            }


            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GerenciarMissoes()
        {
            var missoes = await _context.Missoes
                .AsNoTracking()
                .OrderBy(m => m.Nome)
                .ToListAsync();

            return View(missoes);
        }

        [HttpGet]
        public async Task<IActionResult> EditarMissao(int id)
        {
            var missao = await _context.Missoes
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (missao == null)
            {
                return NotFound();
            }

            var model = new EditarMissaoViewModel
            {
                Id = missao.Id,
                Nome = missao.Nome,
                Descricao = missao.Descricao,
                Resumo = missao.Resumo,
                Slug = missao.Slug ?? string.Empty,
                ImagemLogo = missao.ImagemLogo,
                ImagemInscricao = missao.ImagemInscricao,
                ImagemFundo = missao.ImagemFundo,
                Ativa = missao.Ativa,
                ExibirIndex = missao.ExibirIndex,
                ExibirInscricao = missao.ExibirInscricao,
                OrdemExibicao = missao.OrdemExibicao
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarMissao(
    EditarMissaoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var missao = await _context.Missoes
                .FirstOrDefaultAsync(m => m.Id == model.Id);

            if (missao == null)
            {
                return NotFound();
            }

            try
            {
                var nome = model.Nome.Trim();
                var slug = model.Slug.Trim().ToLowerInvariant();


                // Evita outro registro com o mesmo nome
                var nomeExiste = await _context.Missoes
                    .AsNoTracking()
                    .AnyAsync(m =>
                        m.Id != model.Id &&
                        m.Nome == nome);

                if (nomeExiste)
                {
                    ModelState.AddModelError(
                        nameof(model.Nome),
                        "Já existe outra missão com esse nome."
                    );

                    return View(model);
                }


                // Evita outro registro com o mesmo slug
                var slugExiste = await _context.Missoes
                    .AsNoTracking()
                    .AnyAsync(m =>
                        m.Id != model.Id &&
                        m.Slug == slug);

                if (slugExiste)
                {
                    ModelState.AddModelError(
                        nameof(model.Slug),
                        "Já existe outra missão com esse identificador."
                    );

                    return View(model);
                }


                // ==============================
                // ATUALIZAÇÃO
                // ==============================

                missao.Nome = nome;

                missao.Slug = slug;

                missao.Descricao =
                    string.IsNullOrWhiteSpace(model.Descricao)
                        ? null
                        : model.Descricao.Trim();

                missao.Resumo =
                    string.IsNullOrWhiteSpace(model.Resumo)
                        ? null
                        : model.Resumo.Trim();


                missao.Ativa = model.Ativa;

                missao.ExibirIndex =
                    model.ExibirIndex;

                missao.ExibirInscricao =
                    model.ExibirInscricao;

                missao.OrdemExibicao =
                    model.OrdemExibicao;


                // As imagens serão tratadas
                // na próxima etapa.
                missao.ImagemLogo =
                    model.ImagemLogo;

                missao.ImagemInscricao =
                    model.ImagemInscricao;

                missao.ImagemFundo =
                    model.ImagemFundo;


                await _context.SaveChangesAsync();


                TempData["Sucesso"] =
                    "Missão atualizada com sucesso.";


                return RedirectToAction(
                    nameof(GerenciarMissoes)
                );
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Erro de banco ao atualizar a missão {MissaoId}.",
                    model.Id
                );

                ModelState.AddModelError(
                    string.Empty,
                    "Não foi possível atualizar a missão."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao atualizar a missão {MissaoId}.",
                    model.Id
                );

                ModelState.AddModelError(
                    string.Empty,
                    "Ocorreu um erro ao atualizar a missão."
                );
            }


            return View(model);
        }
    }
}

