using System.Windows;
using UserManagementApp.Business.Interfaces;
using UserManagementApp.Business.Services;
using UserManagementApp.Data.Interfaces;
using UserManagementApp.Data.Providers;
using UserManagementApp.UI.ViewModels;

namespace UserManagementApp.UI.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            IDataProvider dataProvider = DataProviderFactory.CreateDataProvider();

            IUserService userService = new UserService(dataProvider);

            DataContext = new MainViewModel(userService);
        }
    }
}