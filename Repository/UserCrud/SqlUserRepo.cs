using Contracts.UserCrud;
using Entities.Model.User;
using Entities.UserContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.UserCrud
{
    public class SqlUserRepo : IRepository
    {
        public readonly UserDbContext _context;

        public SqlUserRepo(UserDbContext userDbContext)
        {
            _context = userDbContext;
        }

        public void CreateUser(User user, int loginiduser)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _context.CreateUser(user,loginiduser);
        }

        public void DeleteUser(int id, int loginiduser)
        {
            _context.DeleteUser(id,loginiduser);
        }

        public IEnumerable<User> GetAllUser()
        {
            return _context.GetAllUser();
        }

        public User GetUserByID(int id)
        {
            return _context.GetByIdUser(id);
        }

        public void UpdateUser(int id, User user, int loginiduser)
        {
            if (user != null)
            {
                _context.UpdateUser(id,user,loginiduser);
            }
            
        }
    }
}
