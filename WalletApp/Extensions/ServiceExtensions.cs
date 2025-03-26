using WalletApp.Core.Interfaces.Repositories;
using WalletApp.DataLayerMock.Repositories;
using WalletApp.Interfaces.Services;
using WalletApp.Services;

namespace WalletApp.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            // services
            services.AddTransient<IWalletService, WalletService>();

            // repositories
            services.AddScoped<ITransactionRepository, TransactionRepositoryMock>();
            services.AddScoped<IWalletRepository, WalletRepository>();

            return services;
        }
    }
}
