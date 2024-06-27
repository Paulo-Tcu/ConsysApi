using ConsysApi.Data.Context;
using ConsysApi.Data.DTO;
using ConsysApi.Data.Interfaces;
using ConsysApi.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ConsysApi.Controllers
{
    [Route("v1/produto")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly ConsysContext _consysContext;

        private readonly IRepositoryEF<ConsysContext> _repository;

        public ProdutosController(IRepositoryEF<ConsysContext> repositoryContext)
        {
            _repository = repositoryContext;
            _consysContext = repositoryContext.Context;
        }

        [HttpGet("/produtos")]
        public async Task<IEnumerable<Produtos>> Get()
        {
            var produtos = await _consysContext.Produtos
                .AsNoTracking()
                .ToListAsync();

            return produtos;
        }

        [HttpGet("/{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var produto = await _consysContext.Produtos.FirstOrDefaultAsync(f => f.Id == id);

            return produto == null
                ? NotFound()
                : Ok(); 
        }

        [HttpPost("/create")]
        public async Task<IActionResult> Post([FromBody] ProdutosInputDto inputDto)
        {
        }

        [HttpPut("/update")]
        public async Task<IActionResult> Put([FromBody] string value)
        {
        }

        [HttpDelete("/delete/id/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
        }
    }
}
