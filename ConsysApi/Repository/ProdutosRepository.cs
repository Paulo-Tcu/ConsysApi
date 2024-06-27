using ConsysApi.Data.Context;
using ConsysApi.Data.DTO;
using ConsysApi.Data.Model;
using ConsysApi.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ConsysApi.Repository
{
    public class ProdutosRepository : RepositoryEF<ConsysContext>
    {
        private FixerApiHelper _fixerApi;
        private Dictionary<string, decimal> Rates
        {
            get
            {
                if(_fixerApi == null)
                {
                    _fixerApi = new FixerApiHelper(Configuration);
                }

                return _fixerApi.GetRates().Result;
            }
        }

        public ProdutosRepository(ConsysContext context, IConfiguration config) : base(context, config)
        {
        }

        public IEnumerable<ProdutoOutputDto> GetAllProdutos() 
        {
            var keys = Rates.Keys;
            var valorRealBaseEuro = Rates["BRL"];

            return Context.Produtos
                .AsNoTracking()
                .Select(prod => 
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
        }

        public Produtos GetProduto(int id) 
        {
            return Context.Produtos.FirstOrDefault(p => p.Id == id);
        }

        public void UpdateProduto(Produtos produto)
        {
            var produtoBase = Context.Produtos.FirstOrDefault(prod => prod.Id == produto.Id);

            if (produtoBase == null)
                new ArgumentException("Produto não localizado", nameof(produto));

            UpdateEntity(produtoBase, (entityBase) => 
            {
                entityBase.Nome = produto.Nome;
                entityBase.Descricao = produto.Descricao;
                entityBase.Valor = produto.Valor;
                entityBase.Quantidade = produto.Quantidade;
            });
        }

        public void RemoveProduto(int id)
        {
            var produto = Context.Produtos.FirstOrDefault(prod => prod.Id == id);

            if (produto == null)
                new ArgumentException("Produto não localizado", nameof(id));

            base.DeleteEntity(produto);
        }

        public void CreateProduto(ProdutosInputDto produtoDto)
        {
            base.AddEntity(new Produtos 
            {
                Nome = produtoDto.Nome,
                Descricao = produtoDto.Descricao,
                Valor = produtoDto.Valor,
                Quantidade = produtoDto.Quantidade
            });
        }
    }
}
