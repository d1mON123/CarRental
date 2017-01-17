using System.Data;
using System.Windows;

namespace Program
{
    public partial class CatalogWindow : Window
    {
        private Session s;
        private string Table;
        public CatalogWindow(Session s, string Name, string Table)
        {
            InitializeComponent();
            this.s = s;
            this.Title = Name;
            this.Table = Table;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateTable();
        }

        private void UpdateTable()
        {
            dataGrid.ItemsSource = (s.ShowTable(Table)).AsDataView();
            dataGrid.Columns[0].Visibility = Visibility.Collapsed;
            dataGrid.Columns[1].Header = this.Title;
            dataGrid.Columns[1].Width = 170;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text != "")
            {
                s.Insert(Table, Table, textBox.Text);
                UpdateTable();
                textBox.Clear();
            }
            else
            {
                MessageBox.Show("Нічого не введено");
            }
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = dataGrid.SelectedItem as DataRowView;
            if (row != null)
            {
                int value = int.Parse(row.Row.ItemArray[0].ToString());
                s.Delete(Table, value);
                UpdateTable();
            }
            else
            {
                MessageBox.Show("Нічого не обрано");
            }
        }
    }
}
