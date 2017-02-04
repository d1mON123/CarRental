using System;
using System.Windows;

namespace Program
{
    public partial class RentWindow : Window
    {
        Session s;
        Car c;
        public RentWindow(Session s, Car c)
        {
            InitializeComponent();
            this.s = s;
            this.c = c;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CompanyBox.Text = c.Company;
            ModelBox.Text = c.Model;
            if (Properties.Settings.Default.Driver == 0)
            {
                driverBox.IsEnabled = false;
            }
            else
            {
                driverLabel.Content = Properties.Settings.Default.Driver + " " + Properties.Settings.Default.Сurrency + "/год";
            }
            if (Properties.Settings.Default.Child == 0)
            {
                childBox.IsEnabled = false;
            }
            else
            {
                childLabel.Content = Properties.Settings.Default.Child + " " + Properties.Settings.Default.Сurrency + "/год"; ;
            }
            if (Properties.Settings.Default.GPS == 0)
            {
                gpsBox.IsEnabled = false;
            }
            else
            {
                gpsLabel.Content = Properties.Settings.Default.GPS + " " + Properties.Settings.Default.Сurrency + "/год"; ;
            }
            for (int i = 8; i <= 20; i++)
            {
                TimeLBox.Items.Add(i + ":00");
                TimeFBox.Items.Add(i + ":00");
            }
        }

        private void RentButton_Click(object sender, RoutedEventArgs e)
        {
            if (SNameBox.Text != "" && FNameBox.Text != "" && TNameBox.Text != "" && PassportBox.Text != "" && IDBox.Text != "" &&
                LicenseBox.Text != "" && PriceBox.Text != "" && TimeFBox.SelectedIndex != -1 && TimeLBox.SelectedIndex != -1 &&
                TimeF.SelectedDate != null && TimeL.SelectedDate != null)
            {
                TimeSpan span = TimeSpan.Zero;
                DateTime dt = new DateTime(TimeF.SelectedDate.Value.Year, TimeF.SelectedDate.Value.Month, TimeF.SelectedDate.Value.Day, (TimeFBox.SelectedIndex + 8), 0, 0);
                DateTime dt1 = new DateTime(TimeL.SelectedDate.Value.Year, TimeL.SelectedDate.Value.Month, TimeL.SelectedDate.Value.Day, (TimeLBox.SelectedIndex + 8), 0, 0);
                if (dt >= dt1) MessageBox.Show("Неправильно введений час");
                else
                {
                    span = dt1.Subtract(dt);
                }
                RentedCar rc = new RentedCar();
                rc.Car_ID = c.Id;
                rc.Sname = SNameBox.Text;
                rc.Fname = FNameBox.Text;
                rc.Tname = TNameBox.Text;
                rc.Passport = PassportBox.Text;
                rc.Id_number = IDBox.Text;
                rc.License = LicenseBox.Text;
                rc.FDate = dt;
                rc.LDate = dt1;
                rc.Hours = int.Parse(span.TotalHours.ToString());
                rc.Price = int.Parse(PriceBox.Text);
                if (!s.CheckTime(rc))
                {
                    s.RentCar(rc);
                    DialogResult = true;
                }
            }
            else
            {
                MessageBox.Show("Одне або декілька полів вводу пусті");
            }
            
        }

        private void PriceButton_Click(object sender, RoutedEventArgs e)
        {
            PriceChange();
        }

        private void TimeF_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            PriceChange();
        }

        private void PriceChange()
        {
            if (TimeF.Text!="" && TimeL.Text != "" && TimeLBox.Text != "" && TimeFBox.Text != "")
            {
                TimeSpan span = TimeSpan.Zero;
                DateTime dt = new DateTime(TimeF.SelectedDate.Value.Year, TimeF.SelectedDate.Value.Month, TimeF.SelectedDate.Value.Day, (TimeFBox.SelectedIndex + 8), 0, 0);
                DateTime dt1 = new DateTime(TimeL.SelectedDate.Value.Year, TimeL.SelectedDate.Value.Month, TimeL.SelectedDate.Value.Day, (TimeLBox.SelectedIndex + 8), 0, 0);
                if (dt >= dt1) MessageBox.Show("Неправильно введений час");
                else
                {
                    span = dt1.Subtract(dt);
                    int totalPrize = int.Parse(span.TotalHours.ToString()) * c.Price;
                    if (driverBox.IsChecked == true) totalPrize += int.Parse(span.TotalHours.ToString()) * Properties.Settings.Default.Driver;
                    if (childBox.IsChecked == true) totalPrize += int.Parse(span.TotalHours.ToString()) * Properties.Settings.Default.Child;
                    if (gpsBox.IsChecked == true) totalPrize += int.Parse(span.TotalHours.ToString()) * Properties.Settings.Default.GPS;
                    PriceBox.Text = totalPrize.ToString();
                    RentButton.IsEnabled = true;
                }
            }
        }

        private void TimeL_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            PriceChange();
        }

        private void TimeFBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            PriceChange();
        }

        private void TimeLBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            PriceChange();
        }

        private void driverBox_Click(object sender, RoutedEventArgs e)
        {
            PriceButton.IsEnabled = true;
            RentButton.IsEnabled = false;
        }
    }
}
