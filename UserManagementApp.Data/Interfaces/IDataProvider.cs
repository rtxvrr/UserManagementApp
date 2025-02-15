using UserManagementApp.Data.Entities;

namespace UserManagementApp.Data.Interfaces
{
    public interface IDataProvider
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(Guid id);
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(Guid id);
    }
}
