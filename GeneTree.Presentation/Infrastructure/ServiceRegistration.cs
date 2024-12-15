using AutoMapper;
using GeneTree.BLL.AutoMapper;
using GeneTree.BLL.Interface;
using GeneTree.BLL.Service;
//using GeneTree.DAL.Data;
using GeneTree.DAL.Repositories;
using GeneTree.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GeneTree.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add AutoMapper
            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

            // Determine repository type from configuration
            string repositoryType = configuration["RepositoryType"];
            if (string.IsNullOrWhiteSpace(repositoryType))
            {
                throw new Exception("RepositoryType is not configured in appsettings.json.");
            }

            if (repositoryType == "FileSystem")
            {
                // File-based repositories
                services.AddSingleton<IPersonRepository>(
                    new FilePersonRepository(configuration["FileSystem:PersonFilePath"]));
            }
            else if (repositoryType == "Database")
            {
                // Add DbContext for database repository
               // services.AddDbContext<TreeContext>(options =>
                //    options.UseSqlite(configuration.GetConnectionString("DatabaseConnection")));

              //  services.AddScoped<IPersonRepository, DatabasePersonRepository>();
            }
            else
            {
                throw new Exception($"Invalid RepositoryType specified: {repositoryType}");
            }


            // Add BLL Services
            services.AddScoped<IGeneService, GeneService>();

            return services;
        }
    }
}
