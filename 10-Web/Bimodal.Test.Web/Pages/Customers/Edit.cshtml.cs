using Bimodal.Test.Common;
using Bimodal.Test.Web.Services;
using Bimodal.Test.Web.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bimodal.Test.Web.Pages.Customers
{
    public class Edit : PageModel
    {
        private readonly ILogger<Edit> _logger;

        private readonly IApiRequestService _apiRequestService;

        [BindProperty]
        public CustomerUpdateFormModel Customer { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public Edit(ILogger<Edit> logger, IApiRequestService apiRequestService) 
        {
            _logger = logger;
            _apiRequestService = apiRequestService;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var token = User.Claims.FirstOrDefault(f => f.Type == "BearerToken").Value;
            var backendResult = await _apiRequestService.GetCustomerById(id, token);
            if (backendResult.StatusCode == StatusCodes.Status200OK)
            {
                var customerData = backendResult.Response.ToObject<List<CustomerDTO>>().FirstOrDefault();
                Customer = new CustomerUpdateFormModel
                {
                    Address = customerData.Address,
                    FullName = customerData.FullName,
                    PhoneNumber = customerData.PhoneNumber,
                    Id = customerData.Id
                };
            }
            else if (backendResult.StatusCode == StatusCodes.Status401Unauthorized)
            {
                return RedirectToPage("./Account/Login");
            }
            return Page();
        }
    }
}
