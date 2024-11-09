using Application.Interfaces;
using Application.Interfaces.Repository;
using Domain.Entity;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddInfratructure(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepository<>),typeof(EntityFrameworkRepository<>));
            //services.AddTransient<IRepository<SampleEntity>, EntityFrameworkRepository<SampleEntity>>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IIdentityService, IdentityService>();
            return services;
        }
    }
}
