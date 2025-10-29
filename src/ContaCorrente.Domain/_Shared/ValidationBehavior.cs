using FluentValidation;
using MediatR;

namespace ContaCorrente.Domain._Shared
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken))
                );

                var failures = validationResults
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .GroupBy(f => f.ErrorMessage)
                    .Select(g => g.First())
                    .ToList();

                if (failures.Any())
                {
                    // Lança exceção ou retorna Result<T> de erro
                    throw new FluentValidation.ValidationException(failures);
                }
            }

            return await next();
        }
    }
}
