using ConsysApi.Data.Context;
using ConsysApi.Data.DTO;
using ConsysApi.Data.Model;

namespace ConsysApi.Interfaces
{
    public interface IProdutoRepository<TContext>
        where TContext : ConsysContext
    {
        public TContext Context { get; }

        Task<IEnumerable<ProdutoOutputDto>> GetAllProdutos();

        Task<ProdutoOutputDto?> GetProduto(int id);

        void UpdateProduto(int id, ProdutosInputDto produtoDto);

        void RemoveProduto(int id);

        Produtos CreateProduto(ProdutosInputDto produtoDto);

        void Commit();
        Task CommitAsync();
    }
}
