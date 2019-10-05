using Api.Utils;
using Logic.Decorators;
using Logic.Dtos;
using Logic.Students;
using Logic.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var config = new Config(3);
            services.AddSingleton(config);

            var connectionString = new ConnectionString(Configuration["ConnectionString"]);
            services.AddSingleton(connectionString);

            services.AddSingleton<SessionFactory>();
            services.AddSingleton<Messages>();

            //services.AddTransient<ICommandHandler<EditPersonalInfoCommand>>(provider =>
            //    new DatabaseRetryDecorator<EditPersonalInfoCommand>(
            //        new EditPersonalInfoCommandHandler(provider.GetService<SessionFactory>()), 
            //        provider.GetService<Config>()));
            //services.AddTransient<IQueryHandler<GetListQuery, List<StudentDto>>, GetListQueryHandler>();
            //services.AddTransient<ICommandHandler<RegisterCommand>, RegisterCommandHandler>();
            //services.AddTransient<ICommandHandler<EnrollCommand>, EnrollCommandHandler>();
            //services.AddTransient<ICommandHandler<TransferCommand>, TransferCommandHandler>();
            //services.AddTransient<ICommandHandler<DisenrollCommand>, DisenrollCommandHandler>();

            //This should be last because it might need other services registered before.
            services.AddHandlers();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandler>();
            app.UseMvc();
        }
    }
}
