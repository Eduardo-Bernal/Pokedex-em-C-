using BackEndProjeto.Data;
using Microsoft.AspNetCore.Mvc;
using BackEndProjeto.Models;
using BackEndProjeto.Services;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndProjeto.Controllers
{
    public class PokemonController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ApiPokemonService _apiPokemonService;

        public PokemonController(AppDbContext context, ApiPokemonService apiPokemonService)
        {
            _context = context;
            _apiPokemonService = apiPokemonService;
        }

        // Criar ou editar 
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Pokemon pokemon)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null)
            {
                TempData["Error"] = "Usuário não logado.";
                return RedirectToAction("Index", "Home");
            }

            if (string.IsNullOrEmpty(pokemon.Nome) || string.IsNullOrEmpty(pokemon.Tipo))
            {
                TempData["Error"] = "Nome e Tipo são obrigatórios.";
                return RedirectToAction("Index", "Home");
            }

            // Esse Valida nome na API - caiquao amassou nesse 
            bool nomeValido = await _apiPokemonService.NomeValidoAsync(pokemon.Nome);
            if (!nomeValido)
            {
                TempData["Error"] = "Esse Pokémon não existe.";
                return RedirectToAction("Index", "Home");
            }

            // Dono dos pikomon ai pra quem ta lendo os coment
            pokemon.IdUsuario = usuarioId.Value;

            
            //  SE EXISTE EDITA
            
            if (pokemon.PokemonId > 0)
            {
                var existingPokemon = _context.Pokemons
                    .FirstOrDefault(p => p.PokemonId == pokemon.PokemonId && p.IdUsuario == usuarioId);

                if (existingPokemon == null)
                {
                    TempData["Error"] = "Pokémon não encontrado.";
                    return RedirectToAction("Index", "Home");
                }

                // Atualiza campos
                existingPokemon.Nome = pokemon.Nome;
                existingPokemon.Tipo = pokemon.Tipo;
                existingPokemon.Tipo2 = pokemon.Tipo2;

                //  SE O NOME FOR ALTERADO BUSCA OTRA IMAGE
                existingPokemon.Imagem = await _apiPokemonService.BuscarImagemAsync(pokemon.Nome);

                _context.SaveChanges();

                TempData["Success"] = "Pokémon editado com sucesso!";
            }
            else
            {

                // BUSCA A IMAGEM NA API E SALVA NO BANCO - esse aqui eu amassei mesmo (dudu)
                pokemon.Imagem = await _apiPokemonService.BuscarImagemAsync(pokemon.Nome);

                _context.Pokemons.Add(pokemon);
                _context.SaveChanges();

                
            }

            return RedirectToAction("Index", "Home");
        }

        // Excluir
        [HttpPost]
        public IActionResult Delete(int pokemonId)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null)
            {
                TempData["Error"] = "Usuário não logado.";
                return RedirectToAction("Index", "Home");
            }

            var pokemon = _context.Pokemons
                .FirstOrDefault(p => p.PokemonId == pokemonId && p.IdUsuario == usuarioId);

            if (pokemon == null)
            {
                TempData["Error"] = "Pokémon não encontrado.";
                return RedirectToAction("Index", "Home");
            }

            _context.Pokemons.Remove(pokemon);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
