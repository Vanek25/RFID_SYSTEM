using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
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
        SerialPort serialPort = new SerialPort();
        Thread thrdGetingInfoFromArduion;
        string[] serialPorts;
 
        public IdentificationOfPersonal()
        {
            InitializeComponent();

            Menu.SelectedIndex = 0;

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


        private void updateEmp(string txt)
        {
            listOfEmployees.Items.Add(txt);
        }

        private void updateInfo(string txt)
        {
            listOfRfidNumbers.Items.Add(txt);
        }

        private void getPresonFio(string rfidNum) // разобраться с типами возвращаемых значений (войд не катит)
        {
            using (rfid_systemContext db = new rfid_systemContext())
            {
                var employee = (from e in db.Employees
                                join rf in db.RfidNumbers on e.Rfid equals rf.IdRfid
                                where rf.Number == rfidNum
                                select e).ToList();

                listOfEmployees.Dispatcher.BeginInvoke(new updateDelegate(updateEmp), employee[employee.Count - 1].Fio);
                
            }
        }


        private void readingInfoFromArduino()
        {
            string rfidNumber = "";
            while (isConnectToArduino) //разобраться с коннектом
            {
                rfidNumber = serialPort.ReadLine();
                listOfRfidNumbers.Dispatcher.BeginInvoke(new updateDelegate(updateInfo), rfidNumber);
                getPresonFio(rfidNumber.Substring(0, rfidNumber.Length - 1));
            }
            
        }


        #region Подключение к ардуино
        bool isConnectToArduino = false;

        public void ConnectToArduino()
        {
            try
            {
                string selectedSerialPort = SerialPortsCmbBox.SelectedItem.ToString();
                serialPort = new SerialPort(selectedSerialPort, 9600);
                serialPort.Open();
                thrdGetingInfoFromArduion = new Thread(readingInfoFromArduino);
                thrdGetingInfoFromArduion.Start();

                ElipseAboutConnect.Fill = Brushes.Green;
                SerialPortConnectBtn.Content = "Отключить";
                LblConnect.Content = "Устройство подключено";
                SerialPortsCmbBox.IsEnabled = false;
                isConnectToArduino = true;
               
            }

            catch(UnauthorizedAccessException)
            {
                MessageBox.Show("Выбранный COM порт занят! Выберите другой.");
            }

            catch (NullReferenceException)
            {
                MessageBox.Show("COM порт не может быть пустым! Выберите другой.");
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
            ElipseAboutConnect.Fill = Brushes.Red;
            LblConnect.Content = "Устройство не подключено";
            isConnectToArduino = false;
            thrdGetingInfoFromArduion.Suspend();
            serialPort.Close();
            
        }

        private void closeApp(object sender, RoutedEventArgs e)
        {
            Application application = Application.Current;
            application.Shutdown();
        }

        private void openHomePage(object sender, RoutedEventArgs e)
        {
            Menu.SelectedIndex = 0;
        }

        private void openSettings(object sender, RoutedEventArgs e)
        {
            Menu.SelectedIndex = 1;
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
