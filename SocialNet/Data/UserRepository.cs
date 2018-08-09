using Microsoft.AspNetCore.Identity;
using SocialNet.Data.Interfaces;
using SocialNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SocialNet.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }

        public ApplicationUser GetUser(ClaimsPrincipal principal)
        {
            throw new NotImplementedException();
        }

        //public ApplicationUser GetUser(ClaimsPrincipal principal)
        //{
        //    var user = await _userManager.GetUserAsync(principal)
        //    return user;
        //}

        public async Task<ApplicationUser> GetUserAsync(ClaimsPrincipal principal)
        {
            return await _userManager.GetUserAsync(principal);
        }
    }
}
