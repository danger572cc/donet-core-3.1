using Bimodal.Test.Common;
using Bimodal.Test.Web.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System.Threading.Tasks;

namespace Bimodal.Test.Web.Services
{
    public interface IApiRequestService
    {
        Task<Detail> GetToken(LoginRequest request);
    }

    public class ApiRequestService : IApiRequestService
    {
        private readonly IRestClient _restClient;

        private readonly IConfiguration _configuration;

        public ApiRequestService(IConfiguration configuration)
        {
            _configuration = configuration;
            _restClient = new RestClient(configuration["ApiUrl"]);
        }

        public async Task<Detail> GetToken(LoginRequest form)
        {
            var request = new RestRequest(ApiResource.LOGIN).AddJsonBody(form);
            var response = await _restClient.PostAsync<Detail>(request);
            return response;
        }
    }
}
