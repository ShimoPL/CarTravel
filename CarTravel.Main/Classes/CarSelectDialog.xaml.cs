using CarTravel.Main.Classes.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for CarSelectDialog.xaml
    /// </summary>
    public partial class CarSelectDialog : Window
    {
        public ObservableCollection<cars> oSelectedCars
        {
            get;
            set;
        }
        public List<cars> selectedCars
        {
            get { return oSelectedCars.ToList(); }
            set { oSelectedCars = new ObservableCollection<cars>(value); }
        }


        public ObservableCollection<cars> oAvailbleCars
        {
            get;
            set;
        }

        public List<cars> availbleCars
        {
            get { return oAvailbleCars.ToList(); }
            set { oAvailbleCars = new ObservableCollection<cars>(value.Except(selectedCars)); }
        }

        public CarSelectDialog()
        {
            InitializeComponent();
            DataContext = this;
            //if (selectedCars == null) selectedCars = new List<cars>();
            //if (selectedCars.Count > 0)
            //    availbleCars = availbleCars.Except(selectedCars).ToList();
            //oAvailbleCars = new ObservableCollection<cars>(availbleCars);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var selectedCar = CarsList.SelectedItem as cars;
            if (selectedCar != null)
            {
                if (oAvailbleCars.Contains(selectedCar)) oAvailbleCars.Remove(selectedCar);
                if (!oSelectedCars.Contains(selectedCar)) oSelectedCars.Add(selectedCar);
            }

        }

        private void CarsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var selectedCar = SelectedCars.SelectedItem as cars;
            if (selectedCar != null)
            {
                if (oSelectedCars.Contains(selectedCar)) oSelectedCars.Remove(selectedCar);
                if (!oAvailbleCars.Contains(selectedCar)) oAvailbleCars.Add(selectedCar);
            }
        }
    }
}
