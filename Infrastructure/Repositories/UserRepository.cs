using Domain.Context;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
            
        }
        public async Task<User> GetByUserNameAsync(string username) => await _context.Users.FirstOrDefaultAsync(x => x.Email == username);

        public async Task<bool> CheckPasswordAsync(User user, string password) => await _context.Users.CountAsync(x => x.Email.Equals(user.Email) && x.Password.Equals(password)) > 0;
    }
}
