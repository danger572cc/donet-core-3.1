using Bimodal.Test.Database;
using Kledex.Queries;
using System;
using System.Collections.Generic;

namespace Bimodal.Test.Handlers
{
    public class UserListModel : Query<IList<User>>
    {
    }

    public class UserQueryModel : Query<User>
    {
        public Guid Id { get; set; }
    }

    public class UserLoginModel : Query<User> 
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
