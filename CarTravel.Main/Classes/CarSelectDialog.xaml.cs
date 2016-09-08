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
        ObservableCollection<cars> oSelectedCars;
        ObservableCollection<cars> oAvailbleCars;
        public List<cars> selectedCars
        {
            set {oSelectedCars = new ObservableCollection<cars>(value); }
        }


        public List<cars> availbleCars
        {
            get; set;
        }


        public CarSelectDialog()
        {
            InitializeComponent();
            //if (selectedCars == null) selectedCars = new List<cars>();
            //if (selectedCars.Count > 0)
            //    availbleCars = availbleCars.Except(selectedCars).ToList();
            //oAvailbleCars = new ObservableCollection<cars>(availbleCars);
            DataContext = this;
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
            //CarsList.ItemsSource = availbleCars.RemoveAll(x => x.carId == 1);
            
        }
    }
}
