namespace BackEndProjeto.Models
{
    public partial class Pokemon
    {
    
        public int PokemonId { get; set; }

    
        public string Nome { get; set; } = null!;

        
        public string Tipo { get; set; } = null!;

        
        public string? Tipo2 { get; set; }

        
        public int IdUsuario { get; set; }

        // URL da imagem do pikomon adicionado ou editado
        public string? Imagem { get; set; }
    }
}
