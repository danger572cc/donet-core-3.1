using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bimodal.Test.Common;
using Bimodal.Test.Web.Services;
using Bimodal.Test.Web.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

namespace Bimodal.Test.Web.Pages.Customers
{
    [IgnoreAntiforgeryToken]
    public class Customers : PageModel
    {
        private readonly ILogger<Customers> _logger;

        private readonly IApiRequestService _apiRequestService;

        public Customers(ILogger<Customers> logger, IApiRequestService apiRequestService)
        {
            _logger = logger;
            _apiRequestService = apiRequestService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostGetCustomers() 
        {
            OkObjectResult result = null;
            var token = User.Claims.FirstOrDefault(f => f.Type == "BearerToken").Value;
            var backendResult = await _apiRequestService.GetAllCustomers(token);
            if (backendResult.StatusCode == StatusCodes.Status200OK)
            {
                IEnumerable<CustomerDTO> customerData = backendResult.Response.ToObject<List<CustomerDTO>>();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][data]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    customerData = customerData.AsQueryable().OrderBy(sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => m.DocumentNumber.Contains(searchValue)
                                                || m.FullName.Contains(searchValue)
                                                || m.PhoneNumber.Contains(searchValue));
                }
                recordsTotal = customerData.Count();
                var data = customerData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                result = new OkObjectResult(jsonData);
            }
            else if (backendResult.StatusCode == StatusCodes.Status401Unauthorized)
            {
                return RedirectToPage("./Account/Login");
            }
            return result;
        }

        public async Task<IActionResult> OnPostDeleteCustomer(string id) 
        {
            OkObjectResult result = null;
            Guid customerId = Guid.Parse(id);
            var token = User.Claims.FirstOrDefault(f => f.Type == "BearerToken").Value;
            var backendResult = await _apiRequestService.DeleteCustomer(customerId, token);
            if (backendResult.StatusCode == StatusCodes.Status204NoContent)
            {
                result = new OkObjectResult(backendResult);
            }
            else if (backendResult.StatusCode == StatusCodes.Status401Unauthorized)
            {
                return RedirectToPage("./Account/Login");
            }
            return result;
        }
    }
}
