using ConsysApi.Data.Context;
using ConsysApi.Data.DTO;
using ConsysApi.Data.Model;
using ConsysApi.Helpers;
using ConsysApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ConsysApi.Repository
{
    public class ProdutosRepository<TContext> : RepositoryEF<TContext>, IProdutoRepository<TContext>
        where TContext : ConsysContext
    {
        private readonly FixerApiHelper _fixerApi;
        private Dictionary<string, decimal> Rates => _fixerApi.GetRates().Result;

        public ProdutosRepository(TContext context, IConfiguration config) : base(context, config)
        {
            _fixerApi = new FixerApiHelper(Configuration);
        }

        public async Task<IEnumerable<ProdutoOutputDto>> GetAllProdutos() 
        {
            var keys = Rates.Keys;
            var valorRealBaseEuro = Rates["BRL"];

            var produtos = await Context.Produtos
                .AsNoTracking()
                .Distinct()
                .ToListAsync();

            var produtosOutput = produtos.Select(prod =>
                    new ProdutoOutputDto
                    (
                        prod.Id,
                        prod.Nome,
                        prod.Descricao,
                        prod.Valor,
                        prod.Quantidade,
                        keys.Select(key => new
                        {
                            Moeda = key != "BRL" ? key : "EUR",
                            Valor = key != "BRL"
                                ? Math.Round((prod.Valor * Rates[key]) / valorRealBaseEuro, 6)
                                : Math.Round(prod.Valor / valorRealBaseEuro, 6)
                        }).Distinct()
                    ));

            return produtosOutput;
        }

        public async Task<ProdutoOutputDto?> GetProduto(int id) 
        {
            var keys = Rates.Keys;
            var valorRealBaseEuro = Rates["BRL"];

            return Context.Produtos
                .AsNoTracking()
                .Where(w => w.Id == id)
                .Select(s => new ProdutoOutputDto
                (
                    s.Id,
                    s.Nome,
                    s.Descricao,
                    s.Valor,
                    s.Quantidade,
                    keys.Select(key => new
                    {
                        Moeda = key != "BRL" ? key : "EUR",
                        Valor = key != "BRL"
                                ? Math.Round((s.Valor * Rates[key]) / valorRealBaseEuro, 6)
                                : Math.Round(s.Valor / valorRealBaseEuro, 6)
                    }).Distinct()
                )).FirstOrDefault();
        }

        public void UpdateProduto(int id, ProdutosInputDto produtoDto)
        {
            var produtoBase = Context.Produtos.FirstOrDefault(prod => prod.Id == id);

            if (produtoBase == null)
                throw new ArgumentNullException(nameof(produtoBase), "Produto não localizado");

            UpdateEntity(produtoBase, (entityBase) => 
            {
                entityBase.Nome = produtoDto.Nome;
                entityBase.Descricao = produtoDto.Descricao;
                entityBase.Valor = produtoDto.Valor;
                entityBase.Quantidade = produtoDto.Quantidade;
            });
        }

        public void RemoveProduto(int id)
        {
            var produto = Context.Produtos.FirstOrDefault(prod => prod.Id == id);

            if (produto == null)
                throw new ArgumentNullException(nameof(produto), "Produto não localizado");

            base.DeleteEntity(produto);
        }

        public Produtos CreateProduto(ProdutosInputDto produtoDto)
        {
            var produto = new Produtos
            {
                Nome = produtoDto.Nome,
                Descricao = produtoDto.Descricao,
                Valor = produtoDto.Valor,
                Quantidade = produtoDto.Quantidade
            };

            base.AddEntity(produto);

            return produto;
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}
