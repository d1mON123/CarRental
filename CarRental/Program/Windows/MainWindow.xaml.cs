using System;
using System.Data;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Program
{
    public partial class MainWindow : Window
    {
        Session s;
        EditWindow edw;
        CatalogWindow ctw;
        RentWindow rcw;
        RentedCarWindow rdcw;
        RepairCarWindow rpc;
        public MainWindow(Session s)
        {
            InitializeComponent();
            this.s = s;
        }

        private void UpdateTable()
        {
            dataGrid.ItemsSource = s.ShowTable("car").AsDataView();
            dataGrid.Columns[0].Visibility = Visibility.Collapsed;
            dataGrid.Columns[1].Header = "Марка";
            dataGrid.Columns[2].Header = "Модель";
            dataGrid.Columns[3].Header = "Рік";
            dataGrid.Columns[4].Header = "Клас";
            dataGrid.Columns[5].Header = "Тип кузова";
            dataGrid.Columns[6].Header = "К-ть місць";
            dataGrid.Columns[7].Header = "Ціна (" + Properties.Settings.Default.Сurrency + "/год)";
            dataGrid.Columns[8].Header = "Застава";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateTable();
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            DateTime dt = DateTime.Now;
            s.CheckStatus(dt);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            lblTime.Text = dt.ToString();
            if (dt.Minute == 0 && dt.Second == 0)
            {
                s.CarRented(dt);
            }
            CommandManager.InvalidateRequerySuggested();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            edw = new EditWindow(s);
            if (edw.ShowDialog() == true) UpdateTable();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = dataGrid.SelectedItem as DataRowView;
            if (row != null)
            {
                int value = int.Parse(row.Row.ItemArray[0].ToString());
                DetCarInfo sc = s.GetSavedCar(value);
                if (sc != null)
                {
                    edw = new EditWindow(s, sc);
                    if (edw.ShowDialog() == true)
                    {
                        UpdateTable();
                    }
                }
            }
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = dataGrid.SelectedItem as DataRowView;
            if (row != null)
            {
                int value = int.Parse(row.Row.ItemArray[0].ToString());
                if (MessageBox.Show("Ви дійсно бажаєте видалити автомобіль?", "Видалення", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                        s.DeleteCar(value);
                        UpdateTable();
                }
            }
        }

        private void Rent_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = dataGrid.SelectedItem as DataRowView;
            if (row != null)
            {
                int value = int.Parse(row.Row.ItemArray[0].ToString());
                Car c = s.GetCarInfo(value);
                if (c != null)
                {
                    rcw = new RentWindow(s, c);
                    rcw.ShowDialog();
                    UpdateTable();
                }
            }
        }

        private void CarInfo_Click(object sender, MouseButtonEventArgs e)
        {
            DataRowView row = dataGrid.SelectedItem as DataRowView;
            if (row != null)
            {
                int value = int.Parse(row.Row.ItemArray[0].ToString());
                DetCarInfo sc = s.GetSavedCar(value);
                if (sc != null)
                {
                    new DetailedInfoWindow(s, sc).ShowDialog();
                }
            }
        }

        private void Company_Click(object sender, RoutedEventArgs e)
        {
            ctw = new CatalogWindow(s, "Марка", "company");
            ctw.ShowDialog();
        }

        private void Type_Click(object sender, RoutedEventArgs e)
        {
            ctw = new CatalogWindow(s, "Тип кузова", "type");
            ctw.ShowDialog();
        }

        private void Transmission_Click(object sender, RoutedEventArgs e)
        {
            ctw = new CatalogWindow(s, "Коробка передач", "transmission");
            ctw.ShowDialog();
        }

        private void Drive_Click(object sender, RoutedEventArgs e)
        {
            ctw = new CatalogWindow(s, "Привід", "drive");
            ctw.ShowDialog();
        }

        private void Category_Click(object sender, RoutedEventArgs e)
        {
            ctw = new CatalogWindow(s, "Клас", "category");
            ctw.ShowDialog();
        }

        private void Fuel_Click(object sender, RoutedEventArgs e)
        {
            ctw = new CatalogWindow(s, "Тип палива", "fuel");
            ctw.ShowDialog();
        }

        private void RentedCar_Click(object sender, RoutedEventArgs e)
        {
            rdcw = new RentedCarWindow(s, 0);
            rdcw.ShowDialog();
        }

        private void ActiveRented_Click(object sender, RoutedEventArgs e)
        {
            rdcw = new RentedCarWindow(s, 1);
            rdcw.ShowDialog();
        }

        private void FinishedRented_Click(object sender, RoutedEventArgs e)
        {
            rdcw = new RentedCarWindow(s, 2);
            rdcw.ShowDialog();
        }

        private void Archive_Click(object sender, RoutedEventArgs e)
        {
            rdcw = new RentedCarWindow(s, 3);
            rdcw.ShowDialog();
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            rdcw = new RentedCarWindow(s, 4);
            rdcw.ShowDialog();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            new SettingsWindow().ShowDialog();
            UpdateTable();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Ви дійсно  бажаєте вийти?", "Вихід", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        private void Repair_Click(object sender, RoutedEventArgs e)
        {
            rpc = new RepairCarWindow(s, 0);
            rpc.ShowDialog();

        }

        private void RepairArchive_Click(object sender, RoutedEventArgs e)
        {
            rpc = new RepairCarWindow(s, 1);
            rpc.ShowDialog();

        }

        private void RepairReport_Click(object sender, RoutedEventArgs e)
        {
            rpc = new RepairCarWindow(s, 2);
            rpc.ShowDialog();

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
