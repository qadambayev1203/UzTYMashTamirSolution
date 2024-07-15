using Entities.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IUserRepository
    {
        Task<User> LoginAsync(string login, string password, bool tracking, CancellationToken cancellationToken = default);
    }
}
