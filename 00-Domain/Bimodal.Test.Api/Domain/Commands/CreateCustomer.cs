using Bimodal.Test.Database;
using Kledex.Domain;

namespace Bimodal.Test.Commands
{
    public class CreateCustomer : DomainCommand<Customer>
    {
        public CreateCustomer() 
        {
            ValidateCommand = true;
        }

        public string DocumentNumber { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }
    }
}
