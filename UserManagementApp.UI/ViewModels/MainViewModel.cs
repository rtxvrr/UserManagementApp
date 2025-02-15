using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using UserManagementApp.Business.Interfaces;
using UserManagementApp.Data.Entities;

namespace UserManagementApp.UI.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IUserService _userService;
        private User _selectedUser;

        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                if (_selectedUser != value)
                {
                    _selectedUser = value;
                    OnPropertyChanged(nameof(SelectedUser));
                    ((RelayCommand)UpdateUserCommand).RaiseCanExecuteChanged();
                    ((RelayCommand)DeleteUserCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public ObservableCollection<User> Users { get; set; }

        public ICommand LoadUsersCommand { get; }
        public ICommand AddUserCommand { get; }
        public ICommand UpdateUserCommand { get; }
        public ICommand DeleteUserCommand { get; }

        public MainViewModel(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            Users = new ObservableCollection<User>();

            LoadUsersCommand = new RelayCommand(_ => LoadUsers());
            AddUserCommand = new RelayCommand(_ => AddUser());
            UpdateUserCommand = new RelayCommand(_ => UpdateUser(), _ => SelectedUser != null);
            DeleteUserCommand = new RelayCommand(_ => DeleteUser(), _ => SelectedUser != null);

            LoadUsers();
        }

        private void LoadUsers()
        {
            Users.Clear();
            var users = _userService.GetAllUsers();
            foreach (var user in users)
                Users.Add(user);
        }

        private void AddUser()
        {
            var newUser = new User
            {
                Login = "User" + DateTime.Now.Ticks,
                FirstName = "FirstName",
                LastName = "LastName"
            };

            try
            {
                _userService.CreateUser(newUser);
                Users.Add(newUser);
            }
            catch (Exception ex)
            {
                
            }
        }

        private void UpdateUser()
        {
            if (SelectedUser == null)
                return;

            SelectedUser.FirstName += " Updated";
            try
            {
                _userService.UpdateUser(SelectedUser);
                LoadUsers();
            }
            catch (Exception ex)
            {
                
            }
        }

        private void DeleteUser()
        {
            if (SelectedUser == null)
                return;

            try
            {
                _userService.DeleteUser(SelectedUser.Id);
                Users.Remove(SelectedUser);
            }
            catch (Exception ex)
            {
                
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
