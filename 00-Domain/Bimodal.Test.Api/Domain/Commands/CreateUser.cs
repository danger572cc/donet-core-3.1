using Bimodal.Test.Database;
using Kledex.Domain;

namespace Bimodal.Test.Commands
{
    public class CreateUser : DomainCommand<User>
    {
        public CreateUser() 
        {
            ValidateCommand = true;
        }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
