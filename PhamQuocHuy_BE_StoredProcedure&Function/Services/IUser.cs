
using PhamQuocHuy_BE_StoredProcedure_Function.Model;

namespace PhamQuocHuy_BE_StoredProcedure_Function.Services
{
    public interface IUser
    {
        IEnumerable<Users> GetUsers();
        public Users AddUser(Users users);
        Users GetUserID(int id);
        Users UpdateUser(int id, Users user);
        Users DeleteUser(int id);

    }
}
