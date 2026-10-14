using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MariaEmMeuLar.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using MariaEmMeuLar.Models.ViewModels;
using MariaEmMeuLar.Models;
using System.Diagnostics.CodeAnalysis;

namespace MariaEmMeuLar.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AdminController> _logger;

        private readonly IWebHostEnvironment _environment;

        public AdminController(AppDbContext context, ILogger<AdminController> logger, IWebHostEnvironment environment)
        {
            _context = context;
            _logger = logger;
            _environment = environment;
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
        public async Task<IActionResult> ControleInscricoes()
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
            CarregarBibliotecaImagens();
            return View(new NovaMissaoViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NovaMissao(
    NovaMissaoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                CarregarBibliotecaImagens();
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

                    CarregarBibliotecaImagens();
                    return View(model);
                }


                var missao = new Missao
                {
                    Nome = nome,

                    Descricao = string.IsNullOrWhiteSpace(model.Descricao)
                        ? null
                        : model.Descricao.Trim(),

                    Resumo = string.IsNullOrWhiteSpace(model.Resumo)
                        ? null
                        : model.Resumo.Trim(),

                    Slug = model.Slug.Trim(),

                    ImagemLogo = string.IsNullOrWhiteSpace(model.ImagemLogo)
                        ? null
                        : model.ImagemLogo.Trim(),

                    ImagemInscricao = string.IsNullOrWhiteSpace(model.ImagemInscricao)
                        ? null
                        : model.ImagemInscricao.Trim(),

                    ImagemFundo = string.IsNullOrWhiteSpace(model.ImagemFundo)
                        ? null
                        : model.ImagemFundo.Trim(),

                    Ativa = model.Ativa,

                    InscricoesAbertas = model.InscricoesAbertas,

                    ExibirIndex = model.ExibirIndex,

                    ExibirInscricao = model.ExibirInscricao,

                    OrdemExibicao = model.OrdemExibicao
                };


                _context.Missoes.Add(missao);

                await _context.SaveChangesAsync();


                TempData["Sucesso"] =
                    "Missão cadastrada com sucesso.";


                return RedirectToAction(
                    nameof(GerenciarMissoes)
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

            CarregarBibliotecaImagens();
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

            CarregarBibliotecaImagens();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarMissao(
    EditarMissaoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                CarregarBibliotecaImagens();

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

                    CarregarBibliotecaImagens();

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

                    CarregarBibliotecaImagens();

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

            CarregarBibliotecaImagens();

            return View(model);
        }

        private void CarregarBibliotecaImagens()
        {
            ViewBag.Logos = ObterImagensPasta("img", "missoes", "logos");

            ViewBag.ImagensInscricao = ObterImagensPasta("img", "missoes", "inscricao");

            ViewBag.Fundos = ObterImagensPasta("img", "missoes", "fundos");
        }

        private List<string> ObterImagensPasta(params string[] partes)
        {
            var caminho = Path.Combine(
                new[] { _environment.WebRootPath }
                    .Concat(partes)
                    .ToArray()
            );

            if (!Directory.Exists(caminho))
            {
                return new List<string>();
            }

            var extensoesPermitidas = new[]
            {
        ".png",
        ".jpg",
        ".jpeg",
        ".webp"
    };

            return Directory
                .EnumerateFiles(caminho)
                .Where(arquivo =>
                    extensoesPermitidas.Contains(
                        Path.GetExtension(arquivo).ToLowerInvariant()
                    )
                )
                .Select(Path.GetFileName)
                .Where(nome => nome != null)
                .Select(nome => nome!)
                .OrderBy(nome => nome)
                .ToList();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirOuDesativarMissao(int id)
        {
            try
            {
                var missao = await _context.Missoes.FirstOrDefaultAsync(m => m.Id == id);

                if (missao == null)
                {
                    TempData["Erro"] = "Missão Não Encontrado.";

                    return RedirectToAction(nameof(GerenciarMissoes));
                }

                var possuiInscricoes = await _context.Inscricoes.AnyAsync(i => i.MissaoId == id);

                if (possuiInscricoes)
                {
                    //Não apagamos porque existem registros ligados

                    missao.Ativa = false;
                    missao.InscricoesAbertas = false;
                    missao.ExibirIndex = false;
                    missao.ExibirInscricao = false;

                    TempData["Sucesso"] =
                        $"A missão \"{missao.Nome}\" foi desativada porque possui inscrições vinculadas.";
                }
                else
                {
                    // Sem inscrições: pode excluir fisicamente
                    _context.Missoes.Remove(missao);

                    TempData["Sucesso"] = $"A missão \"{missao.Nome}\" foi excluída com sucesso.";
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro de banco ao excluir/desativar missão {MissaoId}.", id);

                TempData["Erro"] = "Não foi possível excluir a missão porque existem dados vinculados a ela.";
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao excluir/desativar missão {MissaoId}.",
                    id
                );

                TempData["Erro"] =
                    "Ocorreu um erro ao processar a missão.";
            }

            return RedirectToAction(nameof(GerenciarMissoes));
        }

        [HttpGet]
        public async Task<IActionResult> HistoriaMissoes()
        {
            var missoes = await _context.Missoes.AsNoTracking().OrderBy(m => m.OrdemExibicao)
            .ThenBy(m => m.Nome).ToListAsync();

            var historias = await _context.HistoriasMissoes.AsNoTracking().ToDictionaryAsync(h => h.MissaoId);

            ViewBag.Historias = historias;

            return View(missoes);
        }

        [HttpGet]
        public async Task<IActionResult> EditarHistoriaMissao(int id)
        {
            var missao = await _context.Missoes.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (missao == null)
            {
                TempData["Erro"] = "Missão não encontrada.";

                return RedirectToAction(nameof(HistoriaMissoes));
            }

            var historia = await _context.HistoriasMissoes.AsNoTracking().FirstOrDefaultAsync(h => h.MissaoId == id);

            var model = new EditarHistoriaMissaoViewModel
            {
                MissaoId = missao.Id,

                MissaoNome = missao.Nome,

                Titulo = historia?.Titulo ?? $"História - {missao.Nome}",

                Conteudo = historia?.Conteudo ?? string.Empty
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarHistoriaMissao(
    EditarHistoriaMissaoViewModel model)
        {
            var missao = await _context.Missoes
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    m => m.Id == model.MissaoId
                );


            if (missao == null)
            {
                TempData["Erro"] =
                    "Missão não encontrada.";

                return RedirectToAction(
                    nameof(HistoriaMissoes)
                );
            }


            model.MissaoNome = missao.Nome;


            if (!ModelState.IsValid)
            {
                return View(model);
            }


            try
            {
                var historia = await _context.HistoriasMissoes
                    .FirstOrDefaultAsync(
                        h => h.MissaoId == model.MissaoId
                    );


                // =====================================
                // CRIAR HISTÓRIA
                // =====================================

                if (historia == null)
                {
                    historia = new MissaoHistoria
                    {
                        MissaoId = model.MissaoId,

                        Titulo = model.Titulo.Trim(),

                        Conteudo = model.Conteudo.Trim()
                    };


                    _context.HistoriasMissoes.Add(
                        historia
                    );
                }

                // =====================================
                // ATUALIZAR HISTÓRIA
                // =====================================

                else
                {
                    historia.Titulo =
                        model.Titulo.Trim();

                    historia.Conteudo =
                        model.Conteudo.Trim();
                }


                await _context.SaveChangesAsync();


                TempData["Sucesso"] =
                    $"História da missão \"{missao.Nome}\" salva com sucesso.";


                return RedirectToAction(
                    nameof(HistoriaMissoes)
                );
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Erro de banco ao salvar história da missão {MissaoId}.",
                    model.MissaoId
                );

                ModelState.AddModelError(
                    string.Empty,
                    "Não foi possível salvar a história."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao salvar história da missão {MissaoId}.",
                    model.MissaoId
                );

                ModelState.AddModelError(
                    string.Empty,
                    "Ocorreu um erro ao salvar a história."
                );
            }


            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ConteudoMissao(int id)
        {
            var missao = await _context.Missoes
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (missao == null)
            {
                TempData["Erro"] = "Missão não encontrada.";

                return RedirectToAction(
                    nameof(HistoriaMissoes)
                );
            }


            var secoes = await _context.MissoesSecoes
                .Where(s => s.MissaoId == id)

                // Carrega também os itens de cada seção
                .Include(s => s.Itens)

                .AsNoTracking()
                .OrderBy(s => s.Ordem)
                .ThenBy(s => s.Id)
                .ToListAsync();


            var model = new GerenciarConteudoMissaoViewModel
            {
                Missao = missao,
                Secoes = secoes
            };


            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> NovaSecaoMissao(int missaoId)
        {
            var missao = await _context.Missoes
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == missaoId);

            if (missao == null)
            {
                TempData["Erro"] = "Missão não encontrada.";
                return RedirectToAction(nameof(HistoriaMissoes));
            }

            var maiorOrdem = await _context.MissoesSecoes
             .Where(s => s.MissaoId == missaoId)
             .MaxAsync(s => (int?)s.Ordem) ?? 0;


            var model = new NovaSecaoMissaoViewModel
            {
                MissaoId = missao.Id,
                MissaoNome = missao.Nome,
                Ordem = maiorOrdem + 1,
                Ativa = true
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NovaSecaoMissao(
    NovaSecaoMissaoViewModel model)
        {
            var missao = await _context.Missoes
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == model.MissaoId);

            if (missao == null)
            {
                TempData["Erro"] = "Missão não encontrada.";
                return RedirectToAction(nameof(HistoriaMissoes));
            }

            model.MissaoNome = missao.Nome;

            //Normaliza a chave para evitar espaços e maiúsculas
            model.Chave = model.Chave.Trim().ToLower();

            var chaveExiste = await _context.MissoesSecoes
                .AnyAsync(s => s.MissaoId == model.MissaoId && s.Chave == model.Chave);

            if (chaveExiste)
            {
                ModelState.AddModelError(nameof(model.Chave), "Já existe uma seção com esta chave.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var secao = new MissaoSecao
                {
                    MissaoId = model.MissaoId,
                    Chave = model.Chave.Trim(),
                    Topico = string.IsNullOrWhiteSpace(model.Topico) ? null : model.Topico.Trim(),
                    Titulo = model.Titulo.Trim(),
                    Conteudo = string.IsNullOrWhiteSpace(model.Conteudo) ? null : model.Conteudo.Trim(),
                    Tipo = model.Tipo.Trim(),
                    Imagem = string.IsNullOrWhiteSpace(model.Imagem) ? null : model.Imagem.Trim(),
                    Ordem = model.Ordem,
                    Ativa = model.Ativa
                };

                _context.MissoesSecoes.Add(secao);
                await _context.SaveChangesAsync();

                TempData["Sucesso"] = $"Seção \"{secao.Titulo}\" adicionada à missão \"{missao.Nome}\" com sucesso.";

                return RedirectToAction(nameof(ConteudoMissao), new { id = missao.Id });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Erro de banco ao adicionar seção à missão {MissaoId}.", model.MissaoId);

                ModelState.AddModelError(string.Empty, "Não foi possível adicionar a seção.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar seção à missão {MissaoId}.", model.MissaoId);

                ModelState.AddModelError(string.Empty, "Ocorreu um erro ao adicionar a seção.");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditarSecaoMissao(int id)
        {
            var secao = await _context.MissoesSecoes
                .AsNoTracking()
                .Include(s => s.Missao)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (secao == null)
            {
                TempData["Erro"] = "Seção não encontrada.";

                return RedirectToAction(nameof(HistoriaMissoes));
            }


            var model = new EditarSecaoMissaoViewModel
            {
                Id = secao.Id,
                MissaoId = secao.MissaoId,
                MissaoNome = secao.Missao?.Nome ?? "Missão",

                Chave = secao.Chave,
                Topico = secao.Topico,
                Titulo = secao.Titulo,
                Conteudo = secao.Conteudo,
                Tipo = secao.Tipo,
                Imagem = secao.Imagem,
                Ordem = secao.Ordem,
                Ativa = secao.Ativa
            };


            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarSecaoMissao(
    EditarSecaoMissaoViewModel model)
        {
            var secao = await _context.MissoesSecoes
                .FirstOrDefaultAsync(s => s.Id == model.Id);

            if (secao == null)
            {
                TempData["Erro"] = "Seção não encontrada.";

                return RedirectToAction(nameof(HistoriaMissoes));
            }


            var missao = await _context.Missoes
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == secao.MissaoId);

            if (missao == null)
            {
                TempData["Erro"] = "Missão não encontrada.";

                return RedirectToAction(nameof(HistoriaMissoes));
            }


            model.MissaoId = missao.Id;
            model.MissaoNome = missao.Nome;

            model.Chave = model.Chave
                .Trim()
                .ToLowerInvariant();


            var chaveExiste = await _context.MissoesSecoes
                .AnyAsync(s =>
                    s.MissaoId == missao.Id &&
                    s.Id != model.Id &&
                    s.Chave == model.Chave);

            if (chaveExiste)
            {
                ModelState.AddModelError(
                    nameof(model.Chave),
                    "Já existe outra seção com essa chave."
                );
            }


            if (!ModelState.IsValid)
            {
                return View(model);
            }


            try
            {
                secao.Chave = model.Chave;

                secao.Topico =
                    string.IsNullOrWhiteSpace(model.Topico)
                        ? null
                        : model.Topico.Trim();

                secao.Titulo = model.Titulo.Trim();

                secao.Conteudo =
                    string.IsNullOrWhiteSpace(model.Conteudo)
                        ? null
                        : model.Conteudo.Trim();

                secao.Tipo = model.Tipo.Trim();

                secao.Imagem =
                    string.IsNullOrWhiteSpace(model.Imagem)
                        ? null
                        : model.Imagem.Trim();

                secao.Ordem = model.Ordem;

                secao.Ativa = model.Ativa;


                await _context.SaveChangesAsync();


                TempData["Sucesso"] =
                    $"Seção \"{secao.Titulo}\" atualizada com sucesso.";


                return RedirectToAction(
                    nameof(ConteudoMissao),
                    new { id = missao.Id }
                );
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao atualizar seção {SecaoId}.",
                    model.Id
                );

                ModelState.AddModelError(
                    string.Empty,
                    "Não foi possível atualizar a seção."
                );
            }


            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirOuDesativarSecao(int id)
        {
            try
            {
                var secao = await _context.MissoesSecoes
                    .Include(s => s.Itens)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (secao == null)
                {
                    TempData["Erro"] = "Seção não encontrada.";

                    return RedirectToAction(nameof(HistoriaMissoes));
                }

                var missaoId = secao.MissaoId;

                // Se possui itens, não apagamos o conteúdo
                if (secao.Itens.Any())
                {
                    secao.Ativa = false;

                    TempData["Sucesso"] =
                        $"A seção \"{secao.Titulo}\" foi desativada porque possui itens vinculados.";
                }
                else
                {
                    _context.MissoesSecoes.Remove(secao);

                    TempData["Sucesso"] =
                        $"A seção \"{secao.Titulo}\" foi excluída com sucesso.";
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(
                    nameof(ConteudoMissao),
                    new { id = missaoId }
                );
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Erro de banco ao excluir/desativar seção {SecaoId}.",
                    id
                );

                TempData["Erro"] =
                    "Não foi possível excluir ou desativar a seção.";
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro ao excluir/desativar seção {SecaoId}.",
                    id
                );

                TempData["Erro"] =
                    "Ocorreu um erro ao processar a seção.";
            }

            return RedirectToAction(nameof(HistoriaMissoes));
        }
        [HttpGet]
        public async Task<IActionResult> ItensSecao(int id)
        {
            var secao = await _context.MissoesSecoes
                .Include(s => s.Missao)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (secao == null)
            {
                TempData["Erro"] = "Seção não encontrada.";

                return RedirectToAction(nameof(HistoriaMissoes));
            }


            var itens = await _context.MissoesSecoesItens
                .Where(i => i.SecaoId == id)
                .AsNoTracking()
                .OrderBy(i => i.Ordem)
                .ThenBy(i => i.Id)
                .ToListAsync();


            var model = new GerenciarItensSecaoViewModel
            {
                Secao = secao,
                Itens = itens
            };


            return View(model);
        }
    }
}