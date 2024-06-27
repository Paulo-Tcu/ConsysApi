using ConsysApi.Data.Context;
using ConsysApi.Data.DTO;
using ConsysApi.Data.Model;
using ConsysApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConsysApi.Controllers
{
    [Route("v1/produto")]
    [ApiController]
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
        public async Task<IEnumerable<ProdutoOutputDto>> AllProdutosConvertidos()
        {
            return await _repository.GetAllProdutos();
        }

        [HttpGet("produtos")]
        public IEnumerable<Produtos> AllProdutos()
        {
            return _consysContext.Produtos.AsNoTracking();
        }

        [HttpGet("produto-convertido/{id:int}")]
        public async Task<IActionResult> GetProduto(int id)
        {
            var produto = await _repository.GetProduto(id);

            return produto == null ? NotFound() : Ok(produto);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Post([FromBody] ProdutosInputDto produto)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);

                var produtoEntity = _repository.CreateProduto(produto);
                await _repository.CommitAsync();

                return new CreatedAtRouteResult("GetProduto", produtoEntity);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpPut("update/id/{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProdutosInputDto produtoDto)
        {
            try
            {
                _repository.UpdateProduto(id, produtoDto);
                await _repository.CommitAsync();

                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }

        [HttpDelete("delete/id/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _repository.RemoveProduto(id);
                await _repository.CommitAsync();

                return Ok();
            }
            catch(ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }
    }
}
