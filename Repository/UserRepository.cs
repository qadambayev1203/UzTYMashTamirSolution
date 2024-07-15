using Contracts;
using Entities;
using Entities.Model.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        { }
        public async Task<User> LoginAsync(string login, string password, bool tracking, CancellationToken cancellationToken = default)
        => await FindByCondition(x => x.login.Equals(login) && x.password.Equals(password), tracking)
            .SingleOrDefaultAsync(cancellationToken);
    }
}
