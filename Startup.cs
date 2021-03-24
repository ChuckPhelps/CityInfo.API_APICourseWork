using AutoMapper;
using CityInfo.API.Context;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)//Injesct the configuration.
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Add the MVC service to help us bulid our API.
            services.AddMvc()
                .AddMvcOptions(o =>
                {
                    //Output Data Contract Serializer so we can have some XML format.
                    o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                    //We still can accept JSON from this MVC.
                });
            //Add a lambda expression with o.
            //       .AddJsonOptions(o => {
            //           if (o.SerializerSettings.ContractResolver != null) //Looking for Contract Resolver.
            //           {
            //              var castedResolver = o.SerializerSettings.ContractResolver
            //                                   as DefaultContractResolver;
            //             castedResolver.NamingStrategy = null;
            //         }
            //     
            //     });

            //services.AddTransient - This class works best for light wieght services.
            //services.AddScoped - Created Once for each Request.
            //services.AddSingleton - Created the first time they are requested. 

            //Our Mail Service is Stateless so we are using Transient here.
            //Use the compiler
#if DEBUG
            services.AddTransient<IMailService, LocalMailService>();
            //If we are in DEBUG, use the LocalMailService, if we are not use the Cloud Mail Service.
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif
          
            //Setup a connection string to be passed into the UseSqlServer down below.
            //Using the default database install.
            var connectionString = _configuration["connectionStrings:cityInfoDBConnectionString"];
            services.AddDbContext<CityInfoContext>(o =>
            {
                o.UseSqlServer(connectionString);
            }); //Add a scoped lifetime and allow it be accessibe for dependency injection.

            //Register the serivces that were just added. Scoped once per request.
            services.AddScoped<ICityInfoRepository, CityInfoRepository>();

            //Adding Automapper and can add a set of assemblies.
            //Profile is a way to map configurations.
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) //Provided by the container.
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            //Contains Status codes pages. 
            app.UseStatusCodePages();

            app.UseMvc();//MVC middleware will handle MVC requests.

           
        }
    }
}
