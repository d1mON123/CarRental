using System;
using System.Collections.Generic;
using System.Data;
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

namespace Program
{
    public partial class RepairCarWindow : Window
    {
        private Session s;
        private int Status;
        public RepairCarWindow(Session s, int Status)
        {
            InitializeComponent();
            this.s = s;
            this.Status = Status;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Status != 2)
            {
                reportButton.Visibility = Visibility.Collapsed;
            }
            UpdateTable();
        }

        private void UpdateTable()
        {
            if (Status == 0)
            {
                dataGrid.ItemsSource = s.ShowTable("repair").AsDataView();
                dataGrid.Columns[0].Visibility = Visibility.Collapsed;
                dataGrid.Columns[1].Header = "Марка";
                dataGrid.Columns[2].Header = "Модель";
                dataGrid.Columns[3].Header = "Причина";
                dataGrid.Columns[4].Header = "Вартість";
            }
            else if (Status == 1)
            {
                dataGrid.ItemsSource = s.ShowTable("repairarchive").AsDataView();
                dataGrid.Columns[0].Visibility = Visibility.Collapsed;
                dataGrid.Columns[1].Header = "Марка";
                dataGrid.Columns[2].Header = "Модель";
                dataGrid.Columns[3].Header = "Причина";
                dataGrid.Columns[4].Header = "Вартість";
            }
            else
            {
                dataGrid.ItemsSource = s.ShowTable("repair_report").AsDataView();
                dataGrid.Columns[0].Visibility = Visibility.Collapsed;
                dataGrid.Columns[1].Header = "Марка";
                dataGrid.Columns[2].Header = "Модель";
                dataGrid.Columns[3].Header = "К-ть";
                dataGrid.Columns[4].Header = "Затрати";
            }
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = dataGrid.SelectedItem as DataRowView;
            if (row != null)
            {
                int value = int.Parse(row.Row.ItemArray[0].ToString());
                s.UnRepair(value);
                UpdateTable();
            }
        }
    }
}
