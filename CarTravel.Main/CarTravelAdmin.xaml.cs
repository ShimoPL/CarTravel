using CarTravel.Main.Authorisation;
using CarTravel.Main.Classes;
using CarTravel.Main.Classes.DataAccess;
using CarTravel.Main.Classes.DataAccess.Model;
using CarTravel.Main.Classes.GeneralModel;
using CarTravel.Main.Classes.LoadingScreen;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private bool isEditing = false;
        GridViewColumnHeader _lastHeaderClicked = null;
        ReservationModel selectedReservation = new ReservationModel();
        ListSortDirection _lastDirection = ListSortDirection.Ascending;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
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
            selectedReservation = reservationsList.SelectedItem as ReservationModel;
            if (selectedReservation != null)
            {
                EditPanel.DataContext = _dataAccess.getReservation(selectedReservation.reservationId);
                reservedCarsBox.ItemsSource = _dataAccess.carsList.Where(c => selectedReservation.carsList.Contains(c.carId)).ToList();
                EditPanel.IsEnabled = true;
                UpdateBtn.IsEnabled = true;
                DeleteBtn.IsEnabled = true;
            }
        }

        public string getCarName(int carId)
        {
            return _dataAccess.carsList.Where(c => c.carId == carId).Select(c => c.displayAs).FirstOrDefault();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var carSelect = new CarSelectDialog
            {
                selectedCars = _dataAccess.carsList.Where(c => selectedReservation.carsList.Contains(c.carId)).ToList(),
                availbleCars = _dataAccess.carsList
            };
            carSelect.ShowDialog();

            if (carSelect.DialogResult == true)
            {
                selectedReservation.carsList = carSelect.selectedCars.Select(c => c.carId).ToList();
                reservedCarsBox.ItemsSource = _dataAccess.carsList.Where(c => selectedReservation.carsList.Contains(c.carId)).ToList();
            }
        }

        private void CancelEdit()
        {
            selectedReservation = new ReservationModel();
            EditPanel.DataContext = selectedReservation;
            isEditing = false;
            EditPanel.IsEnabled = false;
            UpdateBtn.Visibility = Visibility.Visible;
            UpdateBtn.IsEnabled = false;
            AddBtn.Visibility = Visibility.Visible;
            DeleteBtn.Visibility = Visibility.Visible;
            DeleteBtn.IsEnabled = false;
            SaveBtn.Visibility = Visibility.Collapsed;
            reservationsList.SelectedItem = null;
            reservedCarsBox.ItemsSource = null;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            selectedReservation = new ReservationModel();
            EditPanel.DataContext = selectedReservation;
            isEditing = true;
            EditPanel.IsEnabled = true;
            UpdateBtn.Visibility = Visibility.Collapsed;
            AddBtn.Visibility = Visibility.Collapsed;
            DeleteBtn.Visibility = Visibility.Collapsed;
            SaveBtn.Visibility = Visibility.Visible;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            CancelEdit();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (isEditing)
            {
                _dataAccess.updateReservation(selectedReservation);
            }
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedReservation != null)
            {

                if (clientListBox.SelectedItem as users != null
                    && statusListBox.SelectedValue != null
                    && fromDate.SelectedDate.Value != null
                    && toDate.SelectedDate.Value != null
                    )
                {
                    if (selectedReservation.carsList.Count > 0)
                    {
                        selectedReservation.modifiedBy = _actualUser.userId;
                        selectedReservation.modifiedOn = DateTime.Now;
                        selectedReservation.client = (clientListBox.SelectedItem as users).userId;
                        selectedReservation.statusCode = statusListBox.SelectedValue.ToString();
                        selectedReservation.startDate = fromDate.SelectedDate.Value;
                        selectedReservation.endDate = toDate.SelectedDate.Value;
                        selectedReservation.comment = CommentBox.Text;
                        if (_dataAccess.updateReservation(selectedReservation)) UpdateReservationsList();
                        CancelEdit();
                    }
                    else MessageBox.Show("Please select car for reservation", "Car not selected", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else MessageBox.Show("Please fill all required fields!", "Reservation uncoplete", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedReservation != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this reservation?", "Confirm delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    _dataAccess.removeReservation(selectedReservation.reservationId);
                    UpdateReservationsList();
                    CancelEdit();
                }
            }

        }

        private void SaveBtn_Click_1(object sender, RoutedEventArgs e)
        {
            if (clientListBox.SelectedItem as users != null
                && statusListBox.SelectedValue != null
                && fromDate.SelectedDate.Value != null
                && toDate.SelectedDate.Value != null
                )
            {
                if (selectedReservation.carsList.Count > 0)
                {
                    selectedReservation.modifiedBy = _actualUser.userId;
                    selectedReservation.modifiedOn = DateTime.Now;
                    selectedReservation.client = (clientListBox.SelectedItem as users).userId;
                    selectedReservation.statusCode = statusListBox.SelectedValue.ToString();
                    selectedReservation.startDate = fromDate.SelectedDate.Value;
                    selectedReservation.endDate = toDate.SelectedDate.Value;
                    selectedReservation.comment = CommentBox.Text;
                    if (_dataAccess.addReservation(selectedReservation)) UpdateReservationsList();
                    CancelEdit();
                }
                else MessageBox.Show("Please select car for reservation", "Car not selected", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else MessageBox.Show("Please fill all required fields!", "Reservation uncoplete", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var clientDialog = new ClientEditDialog();
            clientDialog.ShowDialog();

            UpdateReservationsList();
            clientListBox.ItemsSource = _dataAccess.getClientList();
        }
    }
}
