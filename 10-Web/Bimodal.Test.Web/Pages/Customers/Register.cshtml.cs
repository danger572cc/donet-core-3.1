using Bimodal.Test.Common;
using Bimodal.Test.Web.Services;
using Bimodal.Test.Web.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace Bimodal.Test.Web.Pages.Customers
{
    public class Register : PageModel
    {
        private readonly ILogger<Register> _logger;

        private readonly IApiRequestService _apiRequestService;

        [BindProperty]
        public CustomerFormModel Customer { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public Register(ILogger<Register> logger, IApiRequestService apiRequestService) 
        {
            _logger = logger;
            _apiRequestService = apiRequestService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) 
            {
                return Page();
            }
            var token = User.Claims.FirstOrDefault(f => f.Type == "BearerToken").Value;
            var backendResult = await _apiRequestService.CreateCustomer(Customer, token);
            if (backendResult.StatusCode == StatusCodes.Status201Created)
            {
                return RedirectToPage("/Customers/Index");
            }
            else 
            {
                ErrorMessage = JsonConvert.SerializeObject(backendResult.Response);
                return Page();
            }
        }
    }
}
