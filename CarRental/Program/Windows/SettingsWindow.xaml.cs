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
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            driverBox.Text = Properties.Settings.Default.Driver.ToString();
            childBox.Text = Properties.Settings.Default.Child.ToString();
            gpsBox.Text = Properties.Settings.Default.GPS.ToString();
            moneyBox.Text = Properties.Settings.Default.Сurrency.ToString();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (driverBox.Text == "" || childBox.Text == "" || gpsBox.Text == "" || moneyBox.Text == "")
            {
                MessageBox.Show("Одне або декілька полів пусті");
            }
            else
            {
                Properties.Settings.Default.Driver = int.Parse(driverBox.Text);
                Properties.Settings.Default.Child = int.Parse(childBox.Text);
                Properties.Settings.Default.GPS = int.Parse(gpsBox.Text);
                Properties.Settings.Default.Сurrency = Convert.ToChar(moneyBox.Text);
                Properties.Settings.Default.Save();
                DialogResult = true;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
