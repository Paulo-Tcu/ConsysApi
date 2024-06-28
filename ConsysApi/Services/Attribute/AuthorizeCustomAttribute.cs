using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsysApi.Services.Attribute
{
    public class AuthorizeCustomAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var claims = context.HttpContext.User.Identity;

            if (!claims.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var valueClaim = (claims as ClaimsIdentity).Claims
                .FirstOrDefault(c => c.Type == ClaimType)
                ?.Value;

            var existPermission = valueClaim?.Split(',')?.Contains(ClaimValue) ?? false;

            if (!existPermission)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

        }
    }
}
