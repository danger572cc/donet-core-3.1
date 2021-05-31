using Kledex.Domain;

namespace Bimodal.Test.Events
{
    public class UserCreated : DomainEvent
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string PasswordSalt { get; set; }

        public string PasswordHash { get; set; }
    }
}
