using CarTravel.Main.Classes.DataAccess;
using CarTravel.Main.Classes.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CarTravel.Main.Classes
{
    /// <summary>
    /// Interaction logic for ClientEditDialog.xaml
    /// </summary>
    public partial class ClientEditDialog : Window
    {
        private DataAccess.DataAccess _dataAccess = new DataAccess.DataAccess();
        private users selectedUser;
        public ClientEditDialog()
        {
            InitializeComponent();
            ClientListBox.ItemsSource = _dataAccess.getClientList();
        }

        private void ClientListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedUser = ClientListBox.SelectedItem as users;
            MainGrid.DataContext = selectedUser;
            SaveBtn.Visibility = Visibility.Collapsed;
            UpdateBtn.Visibility = Visibility.Visible;
            UserNameBox.IsEnabled = true;
            UserLastNameBox.IsEnabled = true;
            UserEmailBox.IsEnabled = true;
            UserAdressBox.IsEnabled = true;
        }

        private void CancelEdit()
        {
            selectedUser = null;
            MainGrid.DataContext = null;
            ClientListBox.SelectedItem = null;
            SaveBtn.Visibility = Visibility.Visible;
            SaveBtn.IsEnabled = false;
            UserNameBox.IsEnabled = false;
            UserLastNameBox.IsEnabled = false;
            UserEmailBox.IsEnabled = false;
            UserAdressBox.IsEnabled = false;
            UpdateBtn.Visibility = Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser != null)
            {
                MessageBox.Show("This operation will remove client and all his reservations!", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this user?", "Confirm delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    _dataAccess.removeUser(selectedUser.userId);
                    ClientListBox.ItemsSource = _dataAccess.getClientList();
                    CancelEdit();
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CancelEdit();
            selectedUser = new users();
            UserNameBox.IsEnabled = true;
            UserLastNameBox.IsEnabled = true;
            UserEmailBox.IsEnabled = true;
            UserAdressBox.IsEnabled = true;
            SaveBtn.IsEnabled = true;
            MainGrid.DataContext = selectedUser;
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser != null
                && selectedUser.adress != null
                && selectedUser.email != null
                && selectedUser.firstName != null
                && selectedUser.lastName != null
                )
            {
                _dataAccess.updateUser(selectedUser);
            }
            else MessageBox.Show("Please fill all required fields!", "Client data uncoplete", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser != null
                && selectedUser.adress != null
                && selectedUser.email != null
                && selectedUser.firstName != null
                && selectedUser.lastName != null
                )
            {
                selectedUser.role = "C";
                _dataAccess.addUser(selectedUser);
                ClientListBox.ItemsSource = _dataAccess.getClientList();
                CancelEdit();
            }
            else MessageBox.Show("Please fill all required fields!", "Client data uncoplete", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
