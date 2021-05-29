using Kledex.Domain;

namespace Bimodal.Test.Api.Domain.Events
{
    public class CustomerUpdated : DomainEvent
    {
        public string Dni { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }
    }
}
