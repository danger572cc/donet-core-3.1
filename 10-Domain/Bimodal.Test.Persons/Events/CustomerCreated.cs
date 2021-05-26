using Kledex.Domain;

namespace Bimodal.Test.Customers.Events
{
    public class CustomerCreated : DomainEvent
    {
        public int Dni { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }
    }
}
