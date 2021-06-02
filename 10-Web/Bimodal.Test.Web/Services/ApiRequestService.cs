using Bimodal.Test.Common;
using Bimodal.Test.Web.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bimodal.Test.Web.Services
{
    public interface IApiRequestService
    {
        Task<Detail> CreateCustomer(CustomerFormModel form, string token);

        Task<Detail> GetToken(LoginRequest request);

        Task<Detail> GetAllCustomers(string token);

        Task<Detail> GetCustomerById(Guid id, string token);

        Task<Detail> DeleteCustomer(Guid id, string token);

        Task<Detail> UpdateCustomer(CustomerUpdateFormModel form, string token);
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

        public async Task<Detail> CreateCustomer(CustomerFormModel form, string token)
        {
            Detail responseBackend = null;
            var request = new RestRequest(ApiResource.CUSTOMERS).AddJsonBody(form);
            var response = await _restClient.ExecuteAsync(request, Method.POST);

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var newCustomer = response.Content.ToObject<CustomerDTO>();
                    responseBackend = new Detail(StatusCodes.Status201Created, newCustomer);
                    break;
                case System.Net.HttpStatusCode.Unauthorized:
                    responseBackend = new Detail(StatusCodes.Status401Unauthorized);
                    break;
                case System.Net.HttpStatusCode.UnprocessableEntity:
                    var problemDetail = response.Content.ToObject<ValidationProblemDetails>();
                    responseBackend = new Detail(StatusCodes.Status422UnprocessableEntity, problemDetail);
                    break;
            }
            return responseBackend;
        }


        public async Task<Detail> DeleteCustomer(Guid id, string token)
        {
            Detail response = null;
            var request = new RestRequest(ApiResource.CUSTOMERS + "/" + id)
            {
                RequestFormat = DataFormat.Json
            };
            _restClient.Authenticator = new JwtAuthenticator(token);
            var responseBackend = await _restClient.ExecuteAsync<Detail>(request, Method.DELETE);
            if (responseBackend.StatusCode == System.Net.HttpStatusCode.OK)
            {
                response = responseBackend.Data;
            }
            else if (responseBackend.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                response = new Detail(StatusCodes.Status401Unauthorized);
            }
            return response;
        }

        public async Task<Detail> GetAllCustomers(string token)
        {
            Detail response = null;
            var request = new RestRequest(ApiResource.CUSTOMERS)
            {
                RequestFormat = DataFormat.Json
            };
            _restClient.Authenticator = new JwtAuthenticator(token);
            var responseBackend = await _restClient.ExecuteAsync<Detail>(request, Method.GET);
            if (responseBackend.StatusCode == System.Net.HttpStatusCode.OK)
            {
                response = responseBackend.Data;
            }
            else if (responseBackend.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                response = new Detail(StatusCodes.Status401Unauthorized);
            }
            return response;
        }

        public async Task<Detail> GetCustomerById(Guid id, string token)
        {
            Detail response = null;
            var request = new RestRequest(ApiResource.CUSTOMERS + "/" + id)
            {
                RequestFormat = DataFormat.Json
            };
            _restClient.Authenticator = new JwtAuthenticator(token);
            var responseBackend = await _restClient.ExecuteAsync<Detail>(request, Method.GET);
            if (responseBackend.StatusCode == System.Net.HttpStatusCode.OK)
            {
                response = responseBackend.Data;
            }
            else if (responseBackend.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                response = new Detail(StatusCodes.Status401Unauthorized);
            }
            return response;
        }

        public async Task<Detail> GetToken(LoginRequest form)
        {
            var request = new RestRequest(ApiResource.LOGIN).AddJsonBody(form);
            var response = await _restClient.PostAsync<Detail>(request);
            return response;
        }

        public async Task<Detail> UpdateCustomer(CustomerUpdateFormModel form, string token)
        {
            Detail responseBackend = null;
            var request = new RestRequest(ApiResource.CUSTOMERS).AddJsonBody(form);
            var response = await _restClient.ExecuteAsync<Detail>(request, Method.PUT);

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    responseBackend = response.Data;
                    break;
                case System.Net.HttpStatusCode.Unauthorized:
                    responseBackend = new Detail(StatusCodes.Status401Unauthorized);
                    break;
                case System.Net.HttpStatusCode.UnprocessableEntity:
                    responseBackend = new Detail(StatusCodes.Status422UnprocessableEntity, responseBackend.Response);
                    break;
            }
            return responseBackend;
        }
    }
}
