using ContaCorrente.Application.UseCases.ContaCorrente.CadastrarContaCorrente;
using ContaCorrente.Domain._Shared;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ContaCorrente.Infra.IoC.Extensions
{
    internal static class MediatRExtensions
    {
        internal static IServiceCollection AdicionarMediatR(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CadastrarContaCorrenteCommand).Assembly));
            services.AddValidatorsFromAssemblyContaining<CadastrarContaCorrenteCommandValidator>();
            services.AddTransient<IValidator<CadastrarContaCorrenteCommand>, CadastrarContaCorrenteCommandValidator>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
