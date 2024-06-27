namespace ConsysApi.Data.DTO
{
    public sealed class ProdutoOutputDto
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public decimal Valor { get; private set; }
        public int Quantidade { get; private set; }
        public IEnumerable<object> Conversoes { get; private set; }

        public ProdutoOutputDto(int id, string nome, string descricao, decimal valor, int quantidade, IEnumerable<object> conversoes)
        {
            Id = id;
            Nome = nome;
            Descricao = descricao;
            Valor = Math.Round(valor, 2);
            Quantidade = quantidade;
            Conversoes = conversoes;
        }
    }
}
