using Microsoft.AspNetCore.Mvc;
using BackEndProjeto.Data;
using BackEndProjeto.Services;

namespace BackEndProjeto.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Entrar(string email, string senha)
        {
            if(string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            {
                ViewBag.Erro = "Preencha todos os campos.";
                return View("Index");
            }

            // Converte hash para string Base64 (caso seu HashService retorne bytes)
            byte[] senhaDigitadaBytes = HashService.GerarHashBytes(senha);
            string senhaDigitadaHash = Convert.ToBase64String(senhaDigitadaBytes);

            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

            if(usuario == null)
            {
                ViewBag.Erro = "E-mail ou senha incorretos.";
                return View("Index");
            }

            if(usuario.SenhaHash != senhaDigitadaHash)
            {
                ViewBag.Erro = "E-mail ou senha incorretos.";
                return View("Index");
            }

            
            HttpContext.Session.SetString("UsuarioNome", usuario.NomeUsuario);
            HttpContext.Session.SetInt32("UsuarioId", usuario.UsuarioId);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Sair()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
