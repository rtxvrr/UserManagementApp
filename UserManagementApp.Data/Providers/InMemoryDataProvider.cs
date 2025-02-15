using UserManagementApp.Data.Entities;
using UserManagementApp.Data.Interfaces;

namespace UserManagementApp.Data.Providers
{
    public class InMemoryDataProvider : IDataProvider
    {
        private readonly List<User> _users = new List<User>();

        public IEnumerable<User> GetAllUsers() => _users.ToList();

        public User GetUserById(Guid id) =>
            _users.FirstOrDefault(u => u.Id == id);

        public void CreateUser(User user)
        {
            ValidateUser(user, isNew: true);
            user.Id = Guid.NewGuid();
            _users.Add(user);
        }

        public void UpdateUser(User user)
        {
            ValidateUser(user, isNew: false);
            var existing = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existing == null)
                throw new ArgumentException("User not found.");

            existing.Login = user.Login;
            existing.FirstName = user.FirstName;
            existing.LastName = user.LastName;
        }

        public void DeleteUser(Guid id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                throw new ArgumentException("User not found.");

            _users.Remove(user);
        }
        private void ValidateUser(User user, bool isNew)
        {
            if (string.IsNullOrWhiteSpace(user.Login))
                throw new ArgumentException("Login cannot be empty.");

            bool duplicateExists = _users.Any(u =>
                u.Login.Equals(user.Login, StringComparison.OrdinalIgnoreCase) &&
                (isNew || u.Id != user.Id));

            if (duplicateExists)
                throw new ArgumentException("User with this login already exists.");
        }
    }
}
