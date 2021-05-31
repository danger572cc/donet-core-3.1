using Bimodal.Test.Commands;
using Bimodal.Test.Database;
using Bimodal.Test.Events;
using Kledex.Commands;
using Kledex.Domain;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Bimodal.Test.Handlers
{
    public class UserHandler : ICommandHandlerAsync<CreateUser>
    {
        private readonly AgencyContext _dbContext;

        private const int _saltValueSize = 8;

        private const int _outputSize = 32;

        public UserHandler(AgencyContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CommandResponse> HandleAsync(CreateUser command)
        {
            var salt = GenerateSalt();
            var passwordHash = HashPassword(command.Password, salt);
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

        #region private methods
        private string GenerateSalt()
        {
            return Convert.ToBase64String(GenerateSaltValue());
        }

        private byte[] GenerateSaltValue()
        {
            var rngCsp = new RNGCryptoServiceProvider();
            var randomNumber = new byte[_saltValueSize];
            rngCsp.GetBytes(randomNumber);
            return randomNumber;
        }

        private string HashPassword(string clearData, byte[] saltValue)
        {
            if (saltValue == null)
            {
                throw new InvalidOperationException("You need a salt to hash password securely");
            }

            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(clearData, saltValue);

            var hashedPassword = pbkdf2.GetBytes(_outputSize);

            return Convert.ToBase64String(hashedPassword);
        }

        private string HashPassword(string clearData, string salt)
        {
            if (String.IsNullOrWhiteSpace(salt))
            {
                throw new InvalidOperationException("You need a salt to hash password securely");
            }

            return HashPassword(clearData, Convert.FromBase64String(salt));
        }

        private Tuple<string, string> GenerateRandomPassword()
        {
            var salt = GenerateSalt();
            var passwordHash = HashPassword(Guid.NewGuid().ToString(), salt);
            return new Tuple<string, string>(passwordHash, salt);
        }

        #endregion
    }
}
