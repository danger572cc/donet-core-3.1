using Bimodal.Test.Infraestructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bimodal.Test.Travel
{
    [OnStartUp(Module = "Travels", Order = 1)]
    public class Start
    {
        public Start(IServiceCollection services, IConfiguration configuration)
        {

        }
    }
}
