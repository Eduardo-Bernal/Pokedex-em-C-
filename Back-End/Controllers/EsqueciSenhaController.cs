using Microsoft.AspNetCore.Mvc;

namespace BackEndProjeto.Controllers
{
    public class EsqueciSenhaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EnviarLink(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                ViewBag.Erro = "Digite um e-mail válido.";
                return View("Index");
            }


            return RedirectToAction("Index");
        }
    }
}
