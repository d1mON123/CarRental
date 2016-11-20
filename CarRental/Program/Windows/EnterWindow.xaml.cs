using System.Windows;

namespace Program
{
    public partial class EnterWindow : Window
    {
        private Session s;
        public EnterWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ServerTextBox.Text = Properties.Settings.Default.Server;
            if (ServerTextBox.Text != "") UserTextBox.Focus();
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            if (ServerTextBox.Text != "" && UserTextBox.Text != "" && passwordBox.Password != "")
            {
                s = new Session(ServerTextBox.Text, UserTextBox.Text, passwordBox.Password);
                if (s.ConnectToServer())
                {
                    Properties.Settings.Default.Server = ServerTextBox.Text;
                    Properties.Settings.Default.Save();
                    s.InitializeDB();
                    this.Hide();
                    new MainWindow(s).ShowDialog();
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Одне або декілька полів пусті");
            }
        }
    }
}
