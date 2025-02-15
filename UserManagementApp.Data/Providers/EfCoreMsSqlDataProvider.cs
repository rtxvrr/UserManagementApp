using Microsoft.EntityFrameworkCore;
using UserManagementApp.Data.Contexts;
using UserManagementApp.Data.Entities;
using UserManagementApp.Data.Interfaces;

namespace UserManagementApp.Data.Providers
{
    public class EfCoreMsSqlDataProvider : IDataProvider
    {
        private readonly UserDbContext _context;

        public EfCoreMsSqlDataProvider(UserDbContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetAllUsers() =>
            _context.Users.AsNoTracking().ToList();

        public User GetUserById(Guid id) =>
            _context.Users.AsNoTracking().FirstOrDefault(u => u.Id == id);

        public void CreateUser(User user)
        {
            ValidateUser(user, isNew: true);
            user.Id = Guid.NewGuid();
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            ValidateUser(user, isNew: false);
            var existing = _context.Users.FirstOrDefault(u => u.Id == user.Id);
            if (existing == null)
                throw new ArgumentException("User not found.");

            existing.Login = user.Login;
            existing.FirstName = user.FirstName;
            existing.LastName = user.LastName;
            _context.SaveChanges();
        }

        public void DeleteUser(Guid id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                throw new ArgumentException("User not found.");

            _context.Users.Remove(user);
            _context.SaveChanges();
        }
        private void ValidateUser(User user, bool isNew)
        {
            if (string.IsNullOrWhiteSpace(user.Login))
                throw new ArgumentException("Login cannot be empty.");

            bool duplicateExists;
            if (isNew)
            {
                duplicateExists = _context.Users
                    .Any(u => u.Login.ToLower() == user.Login.ToLower());
            }
            else
            {
                duplicateExists = _context.Users
                    .Any(u => u.Login.ToLower() == user.Login.ToLower() && u.Id != user.Id);
            }

            if (duplicateExists)
                throw new ArgumentException("User with this login already exists.");
        }
    }
}
