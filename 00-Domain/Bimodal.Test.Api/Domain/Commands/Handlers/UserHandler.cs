using Bimodal.Test.Api.Extensions;
using Bimodal.Test.Commands;
using Bimodal.Test.Database;
using Bimodal.Test.Events;
using Kledex.Commands;
using Kledex.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bimodal.Test.Handlers
{
    public class UserHandler : ICommandHandlerAsync<CreateUser>
    {
        private readonly AgencyContext _dbContext;

        public UserHandler(AgencyContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CommandResponse> HandleAsync(CreateUser command)
        {
            var salt = Utils.GenerateSalt();
            var passwordHash = Utils.HashPassword(command.Password, salt);
            var user = new User(command.Id, command.UserName, command.Email, salt, passwordHash);

            _dbContext.Users.Add(user);

            await _dbContext.SaveChangesAsync();

            return new CommandResponse
            {
                Events = new List<IDomainEvent>()
                {
                    new UserCreated
                    {
                        AggregateRootId = user.Id,
                        Email = user.Email,
                        UserName = user.UserName,
                        PasswordHash = passwordHash,
                        PasswordSalt = salt
                    }
                }
            };
        }
    }
}
