using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BDWebAPI.ApiContext;
using BDWebAPI.ApiContext.Repository;
using BDWebAPI.Models.Entities;
using BDWebAPI.Services;
using BDWebAPI.Worker;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BDWebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //remove default json formatting
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.DictionaryKeyPolicy = null;
            });

            //add cors package
            services.AddCors();


            services.AddDbContext<RepositoryContext>(context => { context.UseInMemoryDatabase("BDAssessment"); }, ServiceLifetime.Transient);
            services.AddTransient<IBatchRepository, BatchRepository>();
            services.AddTransient<IProcessorService, ProcessorService>();
            services.AddTransient<IGeneratorManager, GeneratorManager>();
            services.AddTransient<IMultiplierManager, MultiplierManager>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //configurations to cosume the Web API from port : 4200 (Angualr App)
            app.UseCors(options =>
            options.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader());

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<RepositoryContext>();

                //AddTestData(context);
            }

            //var context = app.ApplicationServices.GetService<ApiContext>();
            //AddTestData(context);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void AddTestData(RepositoryContext context)
        {
            var batch1 = new Batch() { BatchId = 1, Total = 5, TotalProcessedItem = 2, TotalRemainingItem = 3 };
            var batch2 = new Batch() { BatchId = 2, Total = 5, TotalProcessedItem = 2, TotalRemainingItem = 3 };
            var batch3 = new Batch() { BatchId = 3, Total = 5, TotalProcessedItem = 2, TotalRemainingItem = 3 };
            var batch4 = new Batch() { BatchId = 4, Total = 5, TotalProcessedItem = 2, TotalRemainingItem = 3 };



            context.Batches.Add(batch1);

            context.Batches.Add(batch2);

            context.Batches.Add(batch3);

            context.Batches.Add(batch4);
            context.SaveChanges();

        }
    }
}
