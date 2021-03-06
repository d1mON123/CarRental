﻿using System.Data;
using System.Windows;

namespace Program
{
    public partial class RentedCarWindow : Window
    {
        private Session s;
        private int Status;
        private ReportWindow rw;
        public RentedCarWindow(Session s, int Status)
        {
            InitializeComponent();
            this.s = s;
            this.Status = Status;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Status == 0)
            {
                reportButton.Visibility = Visibility.Collapsed;
            }
            else if(Status ==2)
            {
                deleteButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                deleteButton.Visibility = Visibility.Collapsed;
                reportButton.Visibility = Visibility.Collapsed;
            }
            UpdateTable();
        }

        private void UpdateTable()
        {
            if (Status <= 2)
            {
                dataGrid.ItemsSource = s.ShowTable("rented_car", Status).AsDataView();
                dataGrid.Columns[0].Visibility = Visibility.Collapsed;
                dataGrid.Columns[1].Header = "Марка";
                dataGrid.Columns[2].Header = "Модель";
                dataGrid.Columns[3].Header = "Рік";
                dataGrid.Columns[4].Header = "Прізвище";
                dataGrid.Columns[5].Header = "Ім'я";
                dataGrid.Columns[6].Header = "По-батькові";
                dataGrid.Columns[7].Header = "Паспорт";
                dataGrid.Columns[8].Header = "Ідент. номер";
                dataGrid.Columns[9].Header = "Водійське посвідчення";
                dataGrid.Columns[10].Header = "Початок прокату";
                dataGrid.Columns[11].Header = "Кінець прокату";
                dataGrid.Columns[12].Header = "К-ть годин";
                dataGrid.Columns[13].Header = "Ціна";
            }
            else if (Status ==3)
            {
                dataGrid.ItemsSource = s.ShowTable("archive").AsDataView();
                dataGrid.Columns[0].Visibility = Visibility.Collapsed;
                dataGrid.Columns[1].Header = "Марка";
                dataGrid.Columns[2].Header = "Модель";
                dataGrid.Columns[3].Header = "Прізвище";
                dataGrid.Columns[4].Header = "Ім'я";
                dataGrid.Columns[5].Header = "По-батькові";
                dataGrid.Columns[6].Header = "Початок прокату";
                dataGrid.Columns[7].Header = "Кінець прокату";
                dataGrid.Columns[8].Header = "К-ть годин";
                dataGrid.Columns[9].Header = "Ціна";
            }
            else
            {
                dataGrid.ItemsSource = s.ShowTable("report").AsDataView();
                dataGrid.Columns[0].Visibility = Visibility.Collapsed;
                dataGrid.Columns[1].Header = "Марка";
                dataGrid.Columns[2].Header = "Модель";
                dataGrid.Columns[3].Header = "Пробіг";
                dataGrid.Columns[4].Header = "К-ть годин";
                dataGrid.Columns[5].Header = "Прибуток";
            }
        }

        private void Delete_click(object sender, RoutedEventArgs e)
        {
            DataRowView row = dataGrid.SelectedItem as DataRowView;
            if (row != null)
            {
                int value = int.Parse(row.Row.ItemArray[0].ToString());
                if (MessageBox.Show("Ви дійсно бажаєте відмінити оренду?", "Відміна", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    s.Delete("rented_car", value);
                    UpdateTable();
                }
            }
        }

        private void Report_click(object sender, RoutedEventArgs e)
        {
            DataRowView row = dataGrid.SelectedItem as DataRowView;
            if (row != null)
            {
                int value = int.Parse(row.Row.ItemArray[0].ToString());
                RentedCar rc = s.GetRentedCarInfo(value);
                if (rc != null)
                {
                    rw = new ReportWindow(s, rc);
                    if (rw.ShowDialog() == true)
                    {
                        UpdateTable();
                    }
                }
            }
        }
    }
}
