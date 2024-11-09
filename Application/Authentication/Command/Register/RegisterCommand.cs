using Application.Interfaces;
using Application.Interfaces.Repository;
using Domain.Entity;
using Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authentication.Command.Register
{
    public class RegisterCommand : ITransactionCommand<RegistrationComplete>
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }

    public class RegistrationComplete
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegistrationComplete>
    {
        private readonly IUserRepository _userRepository;

        private readonly IRepository<User> _repository;
        public RegisterCommandHandler(IUserRepository userRepository, IRepository<User> repository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }
        public async Task<RegistrationComplete> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUserNameAsync(request.Email);
            if (user != null)
            {
                return new RegistrationComplete
                {
                    IsSuccess = false,
                    ErrorMessage = "User already exist with Emmail " + request.Email,
                };
            }

            var newUser = new User
            {
                Email = request.Email,
                Password = request.Password,
                Created = DateTime.Now,
                Updated = DateTime.Now,
                Roles = "User"
            };

            await _repository.Add(newUser);

            return new RegistrationComplete { IsSuccess = true };

        }
    }


}
