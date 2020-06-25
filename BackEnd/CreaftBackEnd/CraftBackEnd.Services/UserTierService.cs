using CraftBackEnd.Common.Configs;
using CraftBackEnd.Common.Models.Type;
using CraftBackEnd.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CraftBackEnd.Services
{
    public class UserTierService : IUserTierService
    {
        private readonly IIAMService _iamService;
        private readonly TierCountLimit _tierCountLimit;

        public UserTierService(IIAMService iamService,
            IOptionsSnapshot<TierCountLimit> tierCountLimit) {
            _iamService = iamService;
            _tierCountLimit = tierCountLimit.Value;
        }

        public async Task<int> GetFieldCountLimitAsync() {
            var roles = await _iamService.GetUserRolesAsync(_iamService.GetReferenceId());

            if (roles.Contains(UserRole.Premium))
                return _tierCountLimit.Premium;
            else if (roles.Contains(UserRole.User))
                return _tierCountLimit.Member;

            return _tierCountLimit.Basic;
        }
    }
}
