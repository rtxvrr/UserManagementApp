using UserManagementApp.Data.Entities;

namespace UserManagementApp.Business.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();

        User GetUserById(Guid id);

        void CreateUser(User user);

        void UpdateUser(User user);

        void DeleteUser(Guid id);
    }
}
