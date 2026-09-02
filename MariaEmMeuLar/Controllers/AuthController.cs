using MariaEmMeuLar.Data;
using MariaEmMeuLar.Models;
using MariaEmMeuLar.Models.ViewModels;
using MariaEmMeuLar.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MariaEmMeuLar.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasher<UsuarioAdmin> _passwordHasher;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AppDbContext context, IJwtService jwtService, IPasswordHasher<UsuarioAdmin> passwordHasher, ILogger<AuthController> logger)
        {
            _context = context;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        //Get: Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        //POST: /Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var email = model.Email.Trim();

                var usuario = await _context.UsuariosAdmin.FirstOrDefaultAsync(u => u.Email == email);

                if(usuario == null || !usuario.Ativo)
                {
                    ModelState.AddModelError(string.Empty, "E-mail ou senha inválido.");

                    return View(model);
                }

                var resultadoSenha = _passwordHasher.VerifyHashedPassword(usuario,usuario.Password,model.Password);

                if(resultadoSenha == PasswordVerificationResult.Failed)
                {
                    ModelState.AddModelError(string.Empty,"E-mail ou senha inválidos.");

                    return View(model);
                }

                //caso ocorra recomendação de atualizar o hash
                if(resultadoSenha == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    usuario.Password = _passwordHasher.HashPassword(usuario, model.Password);
                }

                usuario.UltimoAcesso = DateTime.Now;

                await _context.SaveChangesAsync();

                var token = _jwtService.GerarToken(usuario);

                Response.Cookies.Append("AdminToken", token, new CookieOptions
                {
                    HttpOnly= true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    IsEssential = true,
                    Path = "/",

                    Expires = model.LembrarMe
                       ? DateTimeOffset.UtcNow.AddMinutes(60) : null
                });

                return RedirectToAction("Index", "Home");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Erro durante o login administrador");

                ModelState.AddModelError(string.Empty,"Não foi possivel realizar o login. Tente novamente.");
                return View(model);
            }
        }

        //POST: /Auth/logout no login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AdminToken", new CookieOptions
            {
                Path = "/"
            });

            return RedirectToAction("Index", "Home");
        }
    }
}