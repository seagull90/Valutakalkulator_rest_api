using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using System;
using System.IO;
using System.Reflection;
using Valutakalkulator_rest_api.Models;
using Valutakalkulator_rest_api.Repositories;
using Valutakalkulator_rest_api.Services;

namespace Valutakalkulator_rest_api
{
    public class Startup
    {
        private IConfiguration Configuration { get; set; }

        public Startup()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen();

            var clientConfig = Configuration.GetSection(nameof(ClientConfig)).Get<ClientConfig>();

            services.AddHttpClient<IConversionService, ConversionService>(c =>
            {
                c.Timeout = TimeSpan.FromSeconds(clientConfig.Timeout);
                c.BaseAddress = new Uri(clientConfig.BaseAddress);
                c.DefaultRequestHeaders.Add(clientConfig.HeaderName, clientConfig.HeaderValue);
            });
            services.AddMvc()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            var jobConfig = Configuration.GetSection(nameof(JobConfig)).Get<JobConfig>();
            services.Configure<JobConfig>(Configuration.GetSection(nameof(JobConfig)));

            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();
                var jobKey = new JobKey(nameof(CollectRatesJob));
                q.AddJob<CollectRatesJob>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("CollectRatesJob-trigger")
                    .WithCronSchedule(jobConfig.Schedule));
            });

            services.AddQuartzHostedService(
                q => q.WaitForJobsToComplete = true);

            services.AddDbContext<CurrencyContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString(nameof(CurrencyContext)),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(CurrencyContext).GetTypeInfo().Assembly.GetName().Name);
                    });
            });

            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}