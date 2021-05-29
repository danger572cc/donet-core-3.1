using Bimodal.Test.Database;
using System.Linq;

namespace Bimodal.Test.Api.Core.Extensions
{
    public static class Utils
    {
        public static bool IsExistsCustomer(AgencyContext dbContext, string documentNumber)
        {
            bool result = dbContext.Customers.Any(f => f.Dni == documentNumber);
            return !result;
        }
    }
}
