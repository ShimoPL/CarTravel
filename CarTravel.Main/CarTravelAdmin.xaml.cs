using CarTravel.Main.Authorisation;
using CarTravel.Main.Classes.DataAccess;
using CarTravel.Main.Classes.DataAccess.Model;
using CarTravel.Main.Classes.GeneralModel;
using CarTravel.Main.Classes.LoadingScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
    /// 
    public partial class MainWindow : Window
    {
        private users _actualUser;
        private DataAccess _dataAccess;
        private FilterSettingsModel filters;
        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;
        public MainWindow()
        {
            InitializeComponent();
            _actualUser = new UserLogOn().ActualUser;
            _dataAccess = new DataAccess();
            filters = new FilterSettingsModel();

            if (_actualUser == null || _actualUser.role != "E") Application.Current.Shutdown();
            else MainLogic();
        }
        private void MainLogic()
        {
            filterSettings.DataContext = filters;
            UpdateReservationsList();
            clientListBox.ItemsSource = _dataAccess.getClientList();
            statusListBox.ItemsSource = _dataAccess.getStatusList();
            _dataAccess.getCarsList();
        }

        private void UpdateReservationsList()
        {

            var list = _dataAccess.getReservationsList(filters.DateFrom, filters.DateTo);
            reservationsList.ItemsSource = list;
            if (list.Count() > 0)
            {
                contentLabel.Visibility = Visibility.Collapsed;
            }
            else contentLabel.Visibility = Visibility.Visible;
        }

        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            UpdateReservationsList();
        }

        private void date_UpdateReservationsList(object sender, SelectionChangedEventArgs e)
        {
            UpdateReservationsList();
        }

        private void reservationsList_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    string header = headerClicked.Column.Header as string;
                    Sort(header, direction);

                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["ArrowUp"] as DataTemplate;
                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["ArrowDown"] as DataTemplate;
                    }

                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }

                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }

        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView =
              CollectionViewSource.GetDefaultView(reservationsList.ItemsSource);

            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }

        private void reservationsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ReservationModel selectedRow = reservationsList.SelectedItem as ReservationModel;
            if (selectedRow != null)
            {
                EditPanel.DataContext = _dataAccess.getReservation(selectedRow.reservationId);
                reservedCarsBox.ItemsSource = _dataAccess.carsList.Where(c => selectedRow.carsList.Contains(c.carId));
            }
        }

        public string getCarName(int carId)
        {
            return _dataAccess.carsList.Where(c => c.carId == carId).Select(c => c.displayAs).FirstOrDefault();
        }
    }
}
