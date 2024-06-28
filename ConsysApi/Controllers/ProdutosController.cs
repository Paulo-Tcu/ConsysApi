using ConsysApi.Data.Context;
using ConsysApi.Data.DTO;
using ConsysApi.Helpers;
using ConsysApi.Interfaces;
using ConsysApi.Services.Attribute;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConsysApi.Controllers
{
    [Route("v1/produto")]
    [ApiController]
    [AuthorizeCustom(ClaimType = "CRUD", ClaimValue = "R")]
    public class ProdutosController : ControllerBase
    {
        private readonly ConsysContext _consysContext;

        private readonly IProdutoRepository<ConsysContext> _repository;

        public ProdutosController(IProdutoRepository<ConsysContext> repositoryContext)
        {
            _repository = repositoryContext;
            _consysContext = _repository.Context;
        }

        [HttpGet("produtos-convertidos")]
        public async Task<IActionResult> AllProdutosConvertidos()
        {
            var produtos = await _repository.GetAllProdutos();
            return Ok(new ApiResult(produtos));
        }

        [HttpGet("produtos")]
        public IActionResult AllProdutos()
        {
            var produtos = _consysContext.Produtos.AsNoTracking();
            return Ok(new ApiResult(produtos));
        }

        [HttpGet("produto-convertido/{id:int}")]
        public async Task<IActionResult> GetProduto(int id)
        {
            var produto = await _repository.GetProduto(id);

            return produto == null 
                ? NotFound(new ApiResult(false, "Produto não encontrado")) 
                : Ok(new ApiResult(produto));
        }

        [HttpPost("create")]
        [AuthorizeCustom(ClaimType = "CRUD", ClaimValue = "C")]
        public async Task<IActionResult> Post([FromBody] ProdutosInputDto produto)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);

                var produtoEntity = _repository.CreateProduto(produto);
                await _repository.CommitAsync();

                return StatusCode(201, new ApiResult(produtoEntity));
            }
            catch (Exception ex) 
            {
                return StatusCode(500, new ApiResult(false, ex.Message));
            }
        }

        [HttpPut("update/id/{id:int}")]
        [AuthorizeCustom(ClaimType = "CRUD", ClaimValue = "U")]
        public async Task<IActionResult> Put(int id, [FromBody] ProdutosInputDto produtoDto)
        {
            try
            {
                _repository.UpdateProduto(id, produtoDto);
                await _repository.CommitAsync();

                return Ok(new ApiResult(true, "Atualizado com sucesso"));
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(new ApiResult(false, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResult(false, ex.Message));
            }
        }

        [HttpDelete("delete/id/{id:int}")]
        [AuthorizeCustom(ClaimType = "CRUD", ClaimValue = "D")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            try
            {
                _repository.RemoveProduto(id);
                await _repository.CommitAsync();

                return Ok(new ApiResult(true, "Removido com sucesso."));
            }
            catch(ArgumentNullException ex)
            {
                return NotFound(new ApiResult(false, ex.Message));
            }
            catch(Exception ex)
            {
                return StatusCode(500, new ApiResult(false, ex.Message));
            }
        }
    }
}
