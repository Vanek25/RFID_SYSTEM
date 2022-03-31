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
    /// Логика взаимодействия для AuthDBSettings.xaml
    /// </summary>
    public partial class AuthDBSettings : Window
    {
        public AuthDBSettings()
        {
            InitializeComponent();
        }

        private void closeDbSettings(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void saveIpDb(object sender, RoutedEventArgs e)
        {

        }
    }
}
