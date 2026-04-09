using JobTracker.Application.Interfaces;
using JobTracker.Application.UseCases.CreateJobApplication;
using JobTracker.Application.UseCases.DeleteJobApplication;
using JobTracker.Application.UseCases.GetAllJobApplications;
using JobTracker.Application.UseCases.GetJobApplication;
using JobTracker.Application.UseCases.LoginUser;
using JobTracker.Application.UseCases.RegisterUser;
using JobTracker.Application.UseCases.UpdateJobApplication;
using JobTracker.Domain.Interfaces;
using JobTracker.Infrastructure.Persistence;
using JobTracker.Infrastructure.Persistence.Repositories;
using JobTracker.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobTracker.Infrastructure
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<CreateJobApplicationHandler>();
            services.AddScoped<GetJobApplicationHandler>();
            services.AddScoped<GetAllJobApplicationsHandler>();
            services.AddScoped<UpdateJobApplicationHandler>();
            services.AddScoped<DeleteJobApplicationHandler>();
            services.AddScoped<RegisterUserHandler>();
            services.AddScoped<LoginUserHandler>();

            return services;
        }
    }
}
