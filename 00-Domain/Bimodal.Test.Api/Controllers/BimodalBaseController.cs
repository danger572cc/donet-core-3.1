using Bimodal.Test.Common;
using Microsoft.AspNetCore.Mvc;

namespace Bimodal.Test.Api.Controllers
{
    public class BimodalBaseController : ControllerBase
    {
        protected new OkObjectResult Ok(object response)
        {
            var result = new Detail(200, response);
            return new OkObjectResult(result);
        }

        protected OkObjectResult Ok(int status) 
        {
            var result = new Detail(status);
            return new OkObjectResult(result);
        }
    }
}
