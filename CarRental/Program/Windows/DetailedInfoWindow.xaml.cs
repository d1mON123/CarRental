using System.Windows;

namespace Program
{
    public partial class DetailedInfoWindow : Window
    {
        Session s;
        DetCarInfo cinfo;
        public DetailedInfoWindow(Session s, DetCarInfo cinfo)
        {
            InitializeComponent();
            this.s = s;
            this.cinfo = cinfo;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
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
}
