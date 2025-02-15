using UserManagementApp.Business.Interfaces;
using UserManagementApp.Data.Entities;
using UserManagementApp.Data.Interfaces;

namespace UserManagementApp.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IDataProvider _dataProvider;
        public UserService(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        }
        public IEnumerable<User> GetAllUsers()
        {
            return _dataProvider.GetAllUsers();
        }
        public User GetUserById(Guid id)
        {
            return _dataProvider.GetUserById(id);
        }
        public void CreateUser(User user)
        {
            _dataProvider.CreateUser(user);
        }
        public void UpdateUser(User user)
        {
            _dataProvider.UpdateUser(user);
        }
        public void DeleteUser(Guid id)
        {
            _dataProvider.DeleteUser(id);
        }
    }
}
