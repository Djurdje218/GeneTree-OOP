using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GeneTree.Presentation.Infrastructure;
using GeneTree.Infrastructure;
namespace GeneTree.Presentation
{
        class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var serviceProvider = new ServiceCollection()
                .RegisterServices(configuration)
                .AddSingleton<GeneApp>()
                .BuildServiceProvider();


            var app = serviceProvider.GetRequiredService<GeneApp>();
            await app.RunAsync();
        }
    }
}
