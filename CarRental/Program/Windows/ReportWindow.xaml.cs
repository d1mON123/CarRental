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

namespace Program.Windows
{
    public partial class ReportWindow : Window
    {
        Session s;
        RentedCar rc;
        public ReportWindow(Session s, RentedCar rc)
        {
            InitializeComponent();
            this.s = s;
            this.rc = rc;
        }

        private void acceptButton_Click(object sender, RoutedEventArgs e)
        {
            s.Report(rc, double.Parse(mileageBox.Text));
            s.Delete("rented_car", rc.ID);
        }
    }
}
