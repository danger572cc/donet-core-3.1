using Bimodal.Test.Api.Extensions;
using Bimodal.Test.Database;
using Kledex.Exceptions;
using Kledex.Queries;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bimodal.Test.Handlers
{
    public class UserQueryHandler : 
        IQueryHandlerAsync<UserListModel, IList<User>>, 
        IQueryHandlerAsync<UserQueryModel, User>,
        IQueryHandlerAsync<UserLoginModel, User>
    {
        private readonly AgencyContext _dbContext;

        public UserQueryHandler(AgencyContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<User>> HandleAsync(UserListModel query)
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User> HandleAsync(UserQueryModel query)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == query.Id);

            if (user == null)
            {
                throw new ValidationException("id-User not found.");
            }

            return user;
        }

        public async Task<User> HandleAsync(UserLoginModel query)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == query.UserName);

            if (user == null)
            {
                throw new ValidationException("userName-UserName not found.");
            }

            string passwordHash = Utils.HashPassword(query.Password, user.PasswordSalt);
            if (passwordHash != user.PasswordHash) 
            {
                return null;
            }
            return user;
        }
    }
}
