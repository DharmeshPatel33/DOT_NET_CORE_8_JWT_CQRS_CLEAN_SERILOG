using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public IdentityService(IHttpContextAccessor httpContextAccessor) 
        {
            _contextAccessor = httpContextAccessor;
        }
        public int GetUserId()
        {
            string userId =  _contextAccessor.HttpContext.User.Claims?.FirstOrDefault(x => x.Type.Equals("UserId"))?.Value ?? "0";
            return int.Parse(userId);
        }

        public string GetUserName()
        {
            return _contextAccessor.HttpContext.User.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ?? "";
        }
    }
}
