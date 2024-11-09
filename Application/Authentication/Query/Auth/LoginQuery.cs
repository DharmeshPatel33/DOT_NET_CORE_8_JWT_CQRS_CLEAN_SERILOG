using Application.Interfaces;
using Domain.Model;
using Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authentication.Query.Auth
{
    public class LoginQuery : IRequest<LoginResponse>
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }

    public class LoginQueryHandler : IRequestHandler<LoginQuery, LoginResponse>
    {
        private readonly IAuthenticationService _authenticationService;
        
        public LoginQueryHandler(IAuthenticationService authenticationService) 
        {
            _authenticationService = authenticationService;
        }

        public async Task<LoginResponse> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            return await _authenticationService.Login(request.UserName, request.Password);
        }
    }

}
