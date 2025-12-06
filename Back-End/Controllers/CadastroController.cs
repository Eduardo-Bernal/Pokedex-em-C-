using Microsoft.AspNetCore.Mvc;
using BackEndProjeto.Data;
using BackEndProjeto.Models;
using System.Security.Cryptography;
using System.Text;

namespace BackEndProjeto.Controllers
{
    public class CadastroController : Controller
    {
        private readonly AppDbContext _context;

        public CadastroController(AppDbContext context)
        {
            _context = context;
        }

        // GET /Cadastro/Index
        public IActionResult Index()
        {
            return View();
        }

        // POST /Cadastro/Criar
        [HttpPost]
        public IActionResult Criar(string email, string nomeUsuario, string senha)
        {
            // Verifica se email já existe
            var usuarioExiste = _context.Usuarios.FirstOrDefault(u => u.Email == email);

            if (usuarioExiste != null)
            {
                ViewBag.Erro = "Este email já está sendo utilizado!";
                return View("Index");
            }

            // Cria o hash da senha
            string senhaHash = GerarHash(senha);

            var novoUsuario = new Usuario
            {
                Email = email,
                NomeUsuario = nomeUsuario,
                SenhaHash = senhaHash
            };

            _context.Usuarios.Add(novoUsuario);
            _context.SaveChanges();

            // Após cadastrar, redireciona pro Login
            return RedirectToAction("Index", "Login");
        }

        // Função para gerar hash
        private string GerarHash(string senha)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
                return Convert.ToBase64String(bytes); // deixa a senha como string hexadecimal
            }
        }
    }
}
