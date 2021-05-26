using Bimodal.Test.Infraestructure.Database;
using Kledex.Domain;

namespace Bimodal.Test.Customers.Commands
{
    public class UpdateCustomer : DomainCommand<Customer>
    {
        public int DocumentNumber { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }
    }
}
