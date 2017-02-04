using System.Windows;

namespace Program
{
    public partial class EditWindow : Window
    {
        Session s;
        DetCarInfo cinfo;
        public EditWindow(Session s)
        {
            InitializeComponent();
            this.s = s;
        }

        public EditWindow(Session s, DetCarInfo cinfo)
        {
            InitializeComponent();
            this.s = s;
            this.cinfo = cinfo;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            companyBox.ItemsSource = s.FillBox("company");
            categoryBox.ItemsSource = s.FillBox("category");
            typeBox.ItemsSource = s.FillBox("type");
            fuelBox.ItemsSource = s.FillBox("fuel");
            transmissionBox.ItemsSource = s.FillBox("transmission");
            driveBox.ItemsSource = s.FillBox("drive");
            priceLabel.Content = priceLabel.Content + " " + Properties.Settings.Default.Сurrency;
            pledgeLabel.Content = pledgeLabel.Content + " " + Properties.Settings.Default.Сurrency;
            if (cinfo != null)
            {
                companyBox.Text = cinfo.Company;
                modelBox.Text = cinfo.Model;
                yearBox.Text = cinfo.Year.ToString();
                categoryBox.Text = cinfo.Category;
                typeBox.Text = cinfo.Type;
                fuelBox.Text = cinfo.Fuel;
                transmissionBox.Text = cinfo.Transmission;
                driveBox.Text = cinfo.Drive;
                engineBox.Text = cinfo.Engine.ToString();
                dischargesBox.Text = cinfo.DischargeS.ToString();
                dischargeoBox.Text = cinfo.DischargeO.ToString();
                dischargemBox.Text = cinfo.DischargeM.ToString();
                powerBox.Text = cinfo.Power.ToString();
                speedBox.Text = cinfo.Maxspeed.ToString();
                seatsBox.Text = cinfo.Seats.ToString();
                doorsBox.Text = cinfo.Doors.ToString();
                accelerationBox.Text = cinfo.Acceleration.ToString();
                priceBox.Text = cinfo.Price.ToString();
                pledgeBox.Text = cinfo.Pledge.ToString();
            }
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (companyBox.SelectedIndex != -1 && modelBox.Text != "" && categoryBox.SelectedIndex != -1 &&
                yearBox.Text != "" && typeBox.SelectedIndex != -1 && fuelBox.SelectedIndex != -1 &&
                transmissionBox.SelectedIndex != -1 && driveBox.SelectedIndex != -1 && engineBox.Text != "" &&
                dischargesBox.Text != "" && dischargeoBox.Text != "" && dischargemBox.Text != "" &&
                powerBox.Text != "" && speedBox.Text != "" && seatsBox.Text != "" && doorsBox.Text != "" &&
                accelerationBox.Text != "" && priceBox.Text != "" && pledgeBox.Text != "")
            {
                Car newCar = new Car();
                Catalog c = new Catalog();
                c = companyBox.SelectedItem as Catalog;
                newCar.Company_id = c.ID;
                newCar.Model = modelBox.Text;
                c = categoryBox.SelectedItem as Catalog;
                newCar.Year = int.Parse(yearBox.Text);
                newCar.Category_id = c.ID;
                c = typeBox.SelectedItem as Catalog;
                newCar.Type_id = c.ID;
                c = fuelBox.SelectedItem as Catalog;
                newCar.Fuel_id = c.ID;
                c = transmissionBox.SelectedItem as Catalog;
                newCar.Transmission_id = c.ID;
                c = driveBox.SelectedItem as Catalog;
                newCar.Drive_id = c.ID;
                newCar.Engine = int.Parse(engineBox.Text);
                newCar.DischargeS = double.Parse(dischargesBox.Text);
                newCar.DischargeO = double.Parse(dischargeoBox.Text);
                newCar.DischargeM = double.Parse(dischargemBox.Text);
                newCar.Power = int.Parse(powerBox.Text);
                newCar.Maxspeed = int.Parse(speedBox.Text);
                newCar.Seats = int.Parse(seatsBox.Text);
                newCar.Doors = int.Parse(doorsBox.Text);
                newCar.Acceleration = double.Parse(accelerationBox.Text);
                newCar.Price = int.Parse(priceBox.Text);
                newCar.Pledge = int.Parse(pledgeBox.Text);
                if (cinfo == null)
                {
                    s.Insert(newCar, "car");
                }
                else
                {
                    s.Update(newCar, cinfo.Id);
                }
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Одне або декілька полів вводу пусті");
            }
            
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void yearBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void engineBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void powerBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void seatsBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void doorsBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void speedBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void priceBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void pledgeBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void dischargesBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void dischargeoBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void dischargemBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void accelerationBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }
    }
}
