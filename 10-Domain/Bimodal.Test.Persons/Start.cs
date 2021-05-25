using AutoMapper.Configuration;
using Bimodal.Test.Infraestructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bimodal.Test.Customers
{
    [OnStartUp(Module = "Customers", Order = 1)]
    public class Start
    {
        public Start(IServiceCollection services, IConfiguration configuration)
        {

        }
    }
}
