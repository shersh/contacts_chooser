using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using KretschIT.WP_Fx.UI.Commands;
using Microsoft.Phone.Controls;

namespace ChooserSample
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            //TODO: Сontrol must be rewritten for using without waiting Loaded event
            this.Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new MainPageViewModel();
        }
    }

    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<User> InvitedUsers { get; set; }

        public ObservableCollection<User> SearchedUsers { get; set; }

        public List<User> Users;

        public ICommand SearchCommand { get; set; }

        public MainPageViewModel()
        {
            SearchedUsers = new ObservableCollection<User>();
            InvitedUsers = new ObservableCollection<User>();

            GenerateUsers();

            SearchCommand = new RelayCommand(o =>
                {
                    var searchedName = o.ToString().ToLower();
                    SearchedUsers.Clear();
                    foreach (var user in Users.Where(u => u.FirstName.ToLower().Contains(searchedName) || u.LastName.ToLower().Contains(searchedName)))
                    {
                        SearchedUsers.Add(user);    
                    }
                });
        }

        /// <summary>
        /// Just helper function
        /// </summary>
        private void GenerateUsers()
        {
            Users = new List<User>()
                {
                    new User() { FirstName = "Petr", LastName = "Pervyj"},
                    new User() { FirstName = "Petr", LastName = "Vtoroj"},
                    new User() { FirstName = "Nikolay", LastName = "Pervyj"},
                    new User() { FirstName = "Natasha", LastName = "Kuku"},
                    new User() { FirstName = "Hvatit", LastName = "Eto"},
                    new User() { FirstName = "Pashka", LastName = "Pervyj"},
                    new User() { FirstName = "Habra", LastName = "Habr"},
                };

        }
    }

    public class User
    {
        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }

        public string LastName { get; set; }

        public string FirstName { get; set; }
    }
}