using Application.Interfaces;
using Domain.Context;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UnitOfWork(ApplicationDbContext context) : IDisposable, IUnitOfWork
    {

        public async Task<IDbContextTransaction> BeginTransaction()
        {
            return await context.Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
            await context.SaveChangesAsync();
            await context.Database.CommitTransactionAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }

    }
}
