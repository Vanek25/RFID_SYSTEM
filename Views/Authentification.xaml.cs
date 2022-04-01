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

namespace RFID_SYSTEM.Views
{
    /// <summary>
    /// Логика взаимодействия для Authenti.xaml
    /// </summary>
    public partial class Authentification : Window
    {
        public Authentification()
        {
            InitializeComponent();
        }

        private void closeApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void openConSettings(object sender, RoutedEventArgs e)
        {
            AuthDBSettings authDBSettings = new AuthDBSettings();
            authDBSettings.Show();
        }

        private void connectToDb(object sender, RoutedEventArgs e)
        {
            var connection = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectToRfid_system"].ConnectionString;


        }
    }
}
