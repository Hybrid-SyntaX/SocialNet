using SocialNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SocialNet.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUserAsync(ClaimsPrincipal principal);
        ApplicationUser GetUser(ClaimsPrincipal principal);
    }
}
