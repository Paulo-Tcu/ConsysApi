using ConsysApi.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ConsysApi.ExtensionMethods
{
    public static class WebApplicationBuilderExtension
    {
        public static void CreateDependencyInjection(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<ConsysContext>(options => 
            {
                options.UseNpgsql(connectionString);
            });
        }
    }
}
