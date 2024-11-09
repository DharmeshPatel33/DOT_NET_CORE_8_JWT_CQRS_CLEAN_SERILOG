﻿using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByUserNameAsync(string username);
        Task<bool> CheckPasswordAsync(User user, string password);
    }
}
