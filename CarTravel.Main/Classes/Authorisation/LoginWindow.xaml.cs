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

namespace CarTravel.Main
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public string login
        {
            get { return loginBox.Text; }
            set { loginBox.Text = value; }
        }

        public string pass
        {
            get { return passBox.Password; }
        }
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, EventArgs e)
        {
            if (loginBox.Text == "")
                wrongLabel.Visibility = Visibility.Collapsed;
            else
                wrongLabel.Visibility = Visibility.Visible;
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (loginBox.Text != "" && passBox.Password != "")
                DialogResult = true;
            else return;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
