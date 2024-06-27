using System.ComponentModel.DataAnnotations;

namespace ConsysApi.Data.DTO
{
    public sealed class ProdutosInputDto
    {
        [Required(ErrorMessage = "Nome do Produto é obrigatório")]
        [MaxLength(250)]
        public string Nome { get; private set; }

        [Required(ErrorMessage = "Descrição do Produto é obrigatório")]
        [MaxLength(500)]
        public string Descricao { get; private set; }

        [Required(ErrorMessage = "Valor em R$ do Produto é obrigatório")]
        public decimal Valor { get; private set; }

        [Required(ErrorMessage = "Quantidade do Produto é obrigatório")]
        [MinLength(0)]
        [MaxLength(int.MaxValue)]
        public int Quantidade { get; private set; }

        public ProdutosInputDto(string nome, string descricao, decimal valor, int quantidade)
        {
            Nome = nome;
            Descricao = descricao;
            Valor = Math.Round(valor, 6);
            Quantidade = quantidade;
        }
    }
}
