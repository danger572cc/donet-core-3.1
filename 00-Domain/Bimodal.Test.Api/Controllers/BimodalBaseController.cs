using Bimodal.Test.Common;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Bimodal.Test.Api.Controllers
{
    public class BimodalBaseController : ControllerBase
    {
        public OkObjectResult Ok<T>(List<T> response, int statusCode = 200) where T : class
        {
            var result = new Detail<T>(statusCode, response);
            return new OkObjectResult(result);
        }
    }
}
