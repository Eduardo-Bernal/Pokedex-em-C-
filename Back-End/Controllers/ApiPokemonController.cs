using BackEndProjeto.Services;
using Microsoft.AspNetCore.Mvc;
using BackEndProjeto.Models;
using System.Threading.Tasks;
using BackEndProjeto.Data;

namespace Back_End.Controllers
{
    public class ApiPokemonController : Controller
    {
        private readonly ApiPokemonService _apiPokemonService;
        private readonly AppDbContext _context;

        public ApiPokemonController(ApiPokemonService apiPokemonService, AppDbContext context)
        {
            _apiPokemonService = apiPokemonService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CriarImagem(Pokemon model)
        {
            // Valida se o Pokémon existe
            bool nomeValido = await _apiPokemonService.NomeValidoAsync(model.Nome);
            if (!nomeValido)
            {
                ModelState.AddModelError("", "Digite um Pokémon que exista.");
                return View(model);
            }

            // Busca a imagem do Pokémon
            model.Imagem = await _apiPokemonService.BuscarImagemAsync(model.Nome);

            // Salva ou atualiza no banco
            if (model.PokemonId == 0)
                _context.Pokemons.Add(model);
            else
                _context.Pokemons.Update(model);

            await _context.SaveChangesAsync();

            
            return RedirectToAction("Index");
        }
    }
}
