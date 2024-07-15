using Entities.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.UserCrud
{
    public interface IRepository
    {
        public IEnumerable<User> GetAllUser();

        public User GetUserByID(int id);

        public void CreateUser(User user, int loginiduser);

        public void UpdateUser(int id,User user, int loginiduser);

        public void DeleteUser(int id, int loginiduser);

    }
}
