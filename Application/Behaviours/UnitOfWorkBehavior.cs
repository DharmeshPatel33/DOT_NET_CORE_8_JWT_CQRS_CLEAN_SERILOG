using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Behaviours
{
    public class UnitOfWorkBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ITransactionCommand<TResponse>
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<UnitOfWorkBehavior<TRequest, TResponse>> _logger;

        public UnitOfWorkBehavior(ILogger<UnitOfWorkBehavior<TRequest, TResponse>> logger, IUnitOfWork uow)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            using var connection = await _uow.BeginTransaction();
            TResponse? response = default;
            try
            {
                response = await next();
                await _uow.Commit();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured on transaction.");
                await connection.RollbackAsync();
                throw;
            }
            finally
            {
                connection.Dispose();
            }

            return response!;
        }
    }
}
