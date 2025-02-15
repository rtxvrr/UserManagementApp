using System.Xml.Serialization;
using UserManagementApp.Data.Entities;
using UserManagementApp.Data.Interfaces;

namespace UserManagementApp.Data.Providers
{
    public class XmlDataProvider : IDataProvider
    {
        private readonly string _filePath;
        private List<User> _users;

        public XmlDataProvider(string filePath = "users.xml")
        {
            _filePath = filePath;
            _users = LoadFromFile();
        }

        public IEnumerable<User> GetAllUsers() => _users.ToList();

        public User GetUserById(Guid id) =>
            _users.FirstOrDefault(u => u.Id == id);

        public void CreateUser(User user)
        {
            ValidateUser(user, isNew: true);
            user.Id = Guid.NewGuid();
            _users.Add(user);
            SaveToFile();
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
            SaveToFile();
        }

        public void DeleteUser(Guid id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                throw new ArgumentException("User not found.");

            _users.Remove(user);
            SaveToFile();
        }
        private void SaveToFile()
        {
            var serializer = new XmlSerializer(typeof(List<User>));
            using (var stream = new FileStream(_filePath, FileMode.Create))
            {
                serializer.Serialize(stream, _users);
            }
        }
        private List<User> LoadFromFile()
        {
            if (!File.Exists(_filePath))
                return new List<User>();

            var serializer = new XmlSerializer(typeof(List<User>));
            using (var stream = new FileStream(_filePath, FileMode.Open))
            {
                return (List<User>)serializer.Deserialize(stream);
            }
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
