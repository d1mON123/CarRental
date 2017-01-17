using System;
using System.Windows;

namespace Program
{
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

        private void driverBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void childBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void gpsBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }
    }
}
