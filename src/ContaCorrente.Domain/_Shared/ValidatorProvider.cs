using Microsoft.Extensions.DependencyInjection;

namespace ContaCorrente.Domain._Shared
{
    public class ValidatorProvider
    {
        private static ValidatorProvider _instance;
        private readonly IServiceProvider _serviceProvider;

        private ValidatorProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static ValidatorProvider Instance
        {
            get
            {
                if (_instance == null)
                    throw new InvalidOperationException("ValidationProvider não foi iniciado.");

                return _instance;
            }
        }

        public static void Initialize(IServiceProvider serviceProvider)
        {
            if (_instance == null)
                _instance = new ValidatorProvider(serviceProvider);
        }

        public T GetValidators<T>()
            where T : DomainValidator
        {
            return _serviceProvider
                .CreateScope().ServiceProvider
                .GetRequiredService<T>();
        }
    }
}
