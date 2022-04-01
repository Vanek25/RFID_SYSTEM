using System;
using System.Collections.Generic;
using System.IO.Ports;
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
    /// Логика взаимодействия для IdentificationOfPersonal.xaml
    /// </summary>
    public partial class IdentificationOfPersonal : Window
    {
        private delegate void updateDelegate(string txt);
        SerialPort serialPort;
        string[] serialPorts;

        public IdentificationOfPersonal()
        {
            InitializeComponent();

            using (rfid_systemContext db = new rfid_systemContext())
            {
                var employees = db.Employees.ToList();

                foreach (Employees u in employees)
                {
                    listOfEmployees.Items.Add(u.Fio);
                }
            }

            InitializedSerialPorts();
        }

        private void InitializedSerialPorts()
        {
            serialPorts = SerialPort.GetPortNames();

            if(serialPorts.Count() != 0)
            {
                foreach(var serial in serialPorts)
                {
                    var serialItems = SerialPortsCmbBox.Items;
                    if (!serialItems.Contains(serial))
                    {
                        SerialPortsCmbBox.Items.Add(serial);
                    }
                }
                SerialPortsCmbBox.SelectedItem = serialPorts[0];
            }
        }


        private void updateInfo(string txt)
        {
            listOfRfidNumbers.Items.Add(txt);
        }


        #region Подключение к ардуино
        bool isConnectToArduino = false;

        private void ConnectToArduino()
        {
            try
            {
                string selectedSerialPort = SerialPortsCmbBox.SelectedItem.ToString();
                serialPort = new SerialPort(selectedSerialPort, 9600);
                serialPort.Open();
                SerialPortConnectBtn.Content = "Отключить";
                SerialPortsCmbBox.IsEnabled = false;
                isConnectToArduino = true;
                /*string rfidNumber = serialPort.ReadLine();
                listOfRfidNumbers.Dispatcher.BeginInvoke(new updateDelegate(updateInfo), rfidNumber);*/
            }

            catch(UnauthorizedAccessException)
            {
                MessageBox.Show("Выбранный COM порт занят! Выберите другой.");
            }

            catch (NullReferenceException)
            {
                MessageBox.Show("Это не тот COM порт, он пуст! Выберите другой.");
            }
            
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        private void DisconnectFromArduino()
        {
            SerialPortsCmbBox.IsEnabled = true;
            SerialPortConnectBtn.Content = "Подключить";
            isConnectToArduino = false;
            serialPort.Close();
        }

        private void closeApp(object sender, RoutedEventArgs e)
        {
            Application application = Application.Current;
            application.Shutdown();
        }

        private void openHomePage(object sender, RoutedEventArgs e)
        {
            
        }

        private void openSettings(object sender, RoutedEventArgs e)
        {
            
        }

        private void connectArduino(object sender, RoutedEventArgs e)
        {
            if (!isConnectToArduino)
            {
                ConnectToArduino();
            }
            else
            {
                DisconnectFromArduino();
            }
        }

        private void refreshComPort(object sender, RoutedEventArgs e)
        {
            InitializedSerialPorts();
        }
    }
}
