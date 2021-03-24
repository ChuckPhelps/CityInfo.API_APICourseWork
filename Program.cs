using CityInfo.API.Context;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API
{
    public class Program
    {
        public static void Main(string[] args) //Configure and Run the Application
        {
            var logger = NLogBuilder
                   .ConfigureNLog("nlog.config")
                   .GetCurrentClassLogger();
            try
            {
                //Built in statement in Nlogger.
               
                logger.Info("Initalizing application....");
                //Build and the Run
                var host = CreateWebHostBuilder(args).Build();

                //Create in the current scope method.
                using(var scope = host.Services.CreateScope())
                {
                    try
                    {
                        //Get an instance of the Context.
                        var context = scope.ServiceProvider.GetService<CityInfoContext>();
                        //for demo purposes, delete the database and migrate on startup so
                        //we can start with a clean slate.
                        context.Database.EnsureDeleted();
                        context.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "An error occured while migrating the database.");
                    }
                }

                //If all checks pass run the host.
                host.Run();


            } catch (Exception ex)
            {
                //If there is an execption that caused the application to stop working we log it here.
                logger.Error(ex, "Application Stopped because of an execption");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown(); //Clean up the Logger.
            }
           
        }

        //Need to host the Web Application
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseNLog(); //Setup Logging for Logger. Integrates it easier.
    }
}
