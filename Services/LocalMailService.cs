using Microsoft.Extensions.Configuration;//Added this to access the configuration file.
using System;
using System.Collections.Generic;
using System.Diagnostics; //Added this to use Debug.
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public class LocalMailService : IMailService
    {


        private readonly IConfiguration _configuration;

        public LocalMailService(IConfiguration configuration)
        {
            _configuration = configuration ??
                throw new ArgumentException(nameof(configuration));
        }

        public void Send(string subject, string message)
        {
            //send mail - output to debug window
            Debug.WriteLine($"Mail from {_configuration["mailSettings:mailFromAddress"]} to {_configuration["mailSettings:mailToAddress"]}, with LocalMailService.");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Message: {message}");
        }

    }
}
