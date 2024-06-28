using ConsysApi.Data.Context;
using ConsysApi.Helpers;
using ConsysApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;
using System.ComponentModel.DataAnnotations;

namespace ConsysApi.Controllers
{
    [Route("v1/login")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        public record UserLogin
        {
            [Required]
            public string Usuario { get; set; }

            [Required]
            public string Senha { get; set; }
        }

        private ConsysContext _consysContext;
        private ITokenService _tokenService;

        public LoginController(ConsysContext context, ITokenService tokenService)
        {
            _consysContext = context;
            _tokenService = tokenService;
        }

        [HttpPost("")]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var userBase = await _consysContext.Usuarios
                .FirstOrDefaultAsync(f => f.NomeId.Trim() == userLogin.Usuario.Trim());

            if (userBase == null)
                return StatusCode(401, new ApiResult(false, "Usuário ou senha inválido(s)"));

            else if (!PasswordHasher.Verify(userBase.Senha, userLogin.Senha))
                return StatusCode(401, new ApiResult(false, "Usuário ou senha inválido(s)"));

            var token = _tokenService.GenereteToken(userBase);

            return Ok(new ApiResult(token));
        }


        [HttpPost("senha-hash")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult GetSenhaHash(string senha)
        {
            if (string.IsNullOrEmpty(senha))
                return BadRequest(new ApiResult(false, "Valor deve ser uma string não vazia ou nula"));

            var senhaHash = PasswordHasher.Hash(senha);

            return Ok(new ApiResult(true, senhaHash, "resultado da palavra para inserção no BD"));
        }
    }
}
