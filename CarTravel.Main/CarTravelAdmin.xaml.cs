using CarTravel.Main.Authorisation;
using CarTravel.Main.Classes.DataAccess;
using CarTravel.Main.Classes.GeneralModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CarTravel.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private users _actualUser;
        private DataAccess _dataAccess = new DataAccess();
        public MainWindow()
        {
            InitializeComponent();
            _actualUser = new UserLogOn().ActualUser;
            if (_actualUser == null || _actualUser.role != "E") Application.Current.Shutdown();
            else MainLogic();
        }
        private void MainLogic()
        {
            //UpdateReservationsList();
            FilterSettingsModel filters = new FilterSettingsModel();
            filterSettings.DataContext = filters; 
            reservationsList.ItemsSource = _dataAccess.getReservationsList(DateTime.Today.AddDays(-7), DateTime.Today);
        }

        private void UpdateReservationsList()
        {
            var query = _dataAccess.getReservationsList(DateTime.Today, DateTime.Today);
        }
    }
}
