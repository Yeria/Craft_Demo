using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.Contracts;
using CraftBackEnd.Services.Interfaces;
using CraftBackEnd.Common.Models;
using CraftBackEnd.Common.Models.Type;

namespace CraftBackEnd.Filters
{
    public class AuthFilter : Attribute, IFilterFactory, IAllowAnonymous
    {
        private readonly bool _mustBeAuthenticated;

        public AuthFilter(bool mustBeAuthenticated = true) {
            _mustBeAuthenticated = mustBeAuthenticated;
        }

        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider) {
            return new AuthenticationFilter(serviceProvider.GetService<IIAMService>(),
                serviceProvider.GetService<IUserService>(),
                serviceProvider.GetService<IDatabaseContext>(),
                _mustBeAuthenticated);
        }

        private class AuthenticationFilter : IAsyncAuthorizationFilter
        {
            private readonly IIAMService _iamService;
            private readonly IUserService _userService;
            private readonly IDatabaseContext _dbContext;
            private readonly bool _mustBeAuthenticated;

            public AuthenticationFilter(IIAMService iamService, IUserService userService, IDatabaseContext dbContext, bool mustBeAuthenticated) {
                _iamService = iamService;
                _userService = userService;
                _dbContext = dbContext;
                _mustBeAuthenticated = mustBeAuthenticated;
            }

            public async Task OnAuthorizationAsync(AuthorizationFilterContext context) {
                
                var user = await _dbContext.User.FindAsync(GetId(context.HttpContext.User.Identity));

                if (_mustBeAuthenticated) {
                    if (user is null || user.IsDeleted) {
                        context.Result = new UnauthorizedResult();
                        return;
                    }

                    context.HttpContext.Items["User"] = user;

                    if (await IsAuthorized(user))
                        context.HttpContext.Items["Staff"] = user;

                } else if (user != null && !user.IsDeleted) {
                    context.HttpContext.Items["User"] = user;

                    if (await IsAuthorized(user))
                        context.HttpContext.Items["Staff"] = user;
                }
                
            }

            private static int GetId(IIdentity identity) {
                var claimsIdentity = identity as ClaimsIdentity;
                var value = claimsIdentity?.FindFirst(ClaimTypes.Name)?.Value;
                if (int.TryParse(value, out var id))
                    return id;

                return -1;
            }

            private async Task<bool> IsAuthorized(User user) {

                var roles = await _iamService.GetUserRolesAsync(user.Id);

                if (roles.Contains(UserRole.Premium))
                    return true;

                return false;
            }


        }
    }
}
