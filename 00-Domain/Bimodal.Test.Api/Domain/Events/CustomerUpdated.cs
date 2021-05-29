using Kledex.Domain;

namespace Bimodal.Test.Events
{
    public class CustomerUpdated : DomainEvent
    {
        public string FullName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }
    }
}
