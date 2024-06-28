using ConsysApi.Data.Context;
using ConsysApi.Interfaces;
using ConsysApi.Repository;
using ConsysApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ConsysApi.ExtensionMethods
{
    public static class WebApplicationBuilderExtension
    {
        public static void CreateDependencyInjection(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<ConsysContext>();
            builder.Services.AddScoped<IProdutoRepository<ConsysContext>, ProdutosRepository<ConsysContext>>();
            builder.Services.AddTransient<ITokenService, TokenService>();
        }

        public static void ConfigurationAuthentication(this WebApplicationBuilder builder)
        {
            var key = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JwtKey"));

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, // validar chave assinatura
                    IssuerSigningKey = new SymmetricSecurityKey(key), // Gerar chave assymetrica
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });
        }
    }
}
