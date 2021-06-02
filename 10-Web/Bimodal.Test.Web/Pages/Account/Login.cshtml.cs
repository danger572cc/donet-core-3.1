using Bimodal.Test.Common;
using Bimodal.Test.Web.Services;
using Bimodal.Test.Web.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bimodal.Test.Web.Pages.Account
{
    public class Login : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly IApiRequestService _apiRequestService;

        [BindProperty]
        public LoginRequest LoginModel { get; set; }

        public string ReturnUrl { get; private set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public Login(ILogger<IndexModel> logger, IApiRequestService apiRequestService)
        {
            _logger = logger;
            _apiRequestService = apiRequestService;
        }

        public async Task OnGet(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
            // Clear the existing external cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            try
            {
                ReturnUrl = returnUrl;
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                var response = await _apiRequestService.GetToken(LoginModel);
                if (response.StatusCode == StatusCodes.Status200OK)
                {
                    var tokenData = response.Response.ToObject<LoginResultDTO>();
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, tokenData.UserName),
                        new Claim(ClaimTypes.Role, tokenData.Role),
                        new Claim("BearerToken", tokenData.AccessToken)
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties { 
                            ExpiresUtc = DateTime.UtcNow.AddMinutes(tokenData.Expires),
                            IsPersistent = false,
                            AllowRefresh = false,
                            IssuedUtc = DateTime.UtcNow
                        });
                    return LocalRedirect(Url.GetLocalUrl(returnUrl));
                }
                else
                {
                    var customMessage = response.Response.ToDictionary<string>();
                    ErrorMessage = customMessage["message"];
                    return Page();
                }
            }
            catch (Exception e) 
            {
                return RedirectToPage("./Error");
            }
        }
    }
}
