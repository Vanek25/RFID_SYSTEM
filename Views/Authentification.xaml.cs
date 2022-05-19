using System;
using System.Collections.Generic;
using System.Configuration;
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
            
            if(Properties.Settings.Default.Login != null)
            {
                txtbxLogin.Text = Properties.Settings.Default.Login;
                txtbxPass.Password = Properties.Settings.Default.Password;
                txtbxDbIp.Text = Properties.Settings.Default.IP;
                txtbxDbPort.Text = Properties.Settings.Default.Port;
                txtbxDbName.Text = Properties.Settings.Default.DbName;
            }
        }

        private void closeApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void openConSettings(object sender, RoutedEventArgs e)
        {
            
        }

        private void connectToDb(object sender, RoutedEventArgs e)
        {
            var connection = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectToRfid_system"].ConnectionString;

            string login = txtbxLogin.Text;
            string password = txtbxPass.Password;
            string ip = txtbxDbIp.Text;
            string port = txtbxDbPort.Text;
            string name = txtbxDbName.Text;


            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            connectionStringsSection.ConnectionStrings["ConnectToRfid_system"].ConnectionString = $"server={ip};port={port};database={name};uid={login};password={password}";
            config.Save();
            ConfigurationManager.RefreshSection("connectionStrings");
            
            using (rfid_systemContext db = new rfid_systemContext())
            {
                try
                {
                    var employee = (from z in db.Employees
                                    select e).ToList();
                }
                catch 
                {
                    MessageBox.Show("Авторизация не удалась.\nПроверьте правильность вводимых данных!");
                    return;
                }
            }

            if(login == "Administrator")
            {
                AdminPage adminPage = new AdminPage();
                adminPage.Show();
            }
            else
            {
                IdentificationOfPersonal identificationOfPersonal = new IdentificationOfPersonal();
                identificationOfPersonal.Show();
            }
        }

        private void chckBxSaveChecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Login = txtbxLogin.Text;
            Properties.Settings.Default.DbName = txtbxDbName.Text;
            Properties.Settings.Default.Port = txtbxDbPort.Text;
            Properties.Settings.Default.IP = txtbxDbIp.Text;
            Properties.Settings.Default.Password = txtbxPass.Password;
            Properties.Settings.Default.Save();
        }
    }
}
