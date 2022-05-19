using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Net;
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
        private delegate void updateDelegatePhoto(byte[] txt);
        private delegate void updateDelegatePhotoWarn();
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
            lblFio.Text = txt;
        }

        private void updateCompany(string txt)
        {
            lblCompany.Text = txt;
        }

        private void updatePost(string txt)
        {
            lblPost.Text = txt;
        }

        public BitmapImage ToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        private void updatePhoto(byte[] txt)
        {
            imgEmp.Source = ToImage(txt);
        }

        private void updateWarnPhoto()
        {
            imgEmp.Source = new BitmapImage(new Uri("/Resources/Icons/Warning.png", UriKind.Relative));
        }

        private void getPresonFio(string rfidNum) // разобраться с типами возвращаемых значений (войд не катит)
        {
            using (rfid_systemContext db = new rfid_systemContext())
            {
                var employee = (from e in db.Employees
                                join rf in db.RfidNumbers on e.Rfid equals rf.IdRfid
                                where rf.Number == rfidNum
                                select e).ToList();
                
                if(employee.Count() == 0)
                {
                    lblFio.Dispatcher.BeginInvoke(new updateDelegate(updateEmp), "ДАННОГО ЧЕЛОВЕКА НЕТ В СИСТЕМЕ!!!");
                    serialPort.Write("404");
                }
                else
                {
                    lblFio.Dispatcher.BeginInvoke(new updateDelegate(updateEmp), employee[employee.Count - 1].Fio);
                    serialPort.Write("101");
                }
            }
        }

        private void getPresonCompany(string rfidNum) // разобраться с типами возвращаемых значений (войд не катит)
        {
            using (rfid_systemContext db = new rfid_systemContext())
            {
                var company = (from e in db.Companies
                               join emp in db.Employees on e.IdComp equals emp.Company
                               join rf in db.RfidNumbers on emp.Rfid equals rf.IdRfid
                               where rf.Number == rfidNum
                               select e).ToList();

                if (company.Count() == 0)
                {
                    lblCompany.Dispatcher.BeginInvoke(new updateDelegate(updateCompany), "------");
                }
                else
                {
                    lblCompany.Dispatcher.BeginInvoke(new updateDelegate(updateCompany), company[company.Count - 1].Name);
                    
                }
            }
        }

        private void getPresonPost(string rfidNum) // разобраться с типами возвращаемых значений (войд не катит)
        {
            using (rfid_systemContext db = new rfid_systemContext())
            {
                var post = (from e in db.Posts
                               join emp in db.Employees on e.IdPost equals emp.Post
                               join rf in db.RfidNumbers on emp.Rfid equals rf.IdRfid
                               where rf.Number == rfidNum
                               select e).ToList();

                if (post.Count() == 0)
                {
                    lblPost.Dispatcher.BeginInvoke(new updateDelegate(updatePost), "------");
                }
                else
                {
                    lblPost.Dispatcher.BeginInvoke(new updateDelegate(updatePost), post[post.Count - 1].Name);

                }
            }
        }

        private void getPresonPhoto(string rfidNum) // разобраться с типами возвращаемых значений (войд не катит)
        {
            using (rfid_systemContext db = new rfid_systemContext())
            {
                var employee = (from e in db.Employees
                                join rf in db.RfidNumbers on e.Rfid equals rf.IdRfid
                                where rf.Number == rfidNum
                                select e).ToList();

                if (employee.Count() != 0)
                {
                    imgEmp.Dispatcher.BeginInvoke(new updateDelegatePhoto(updatePhoto), employee[employee.Count - 1].Photo);
                }
                else
                {
                    imgEmp.Dispatcher.BeginInvoke(new updateDelegatePhotoWarn(updateWarnPhoto));
                }
                
            }
        }

        private void readingInfoFromArduino()
        {
            string rfidNumber = "";

            while (isConnectToArduino) //разобраться с коннектом
            {
                rfidNumber = serialPort.ReadLine();
                
                getPresonFio(rfidNumber.Substring(0, rfidNumber.Length - 1));
                getPresonCompany(rfidNumber.Substring(0, rfidNumber.Length - 1));
                getPresonPost(rfidNumber.Substring(0, rfidNumber.Length - 1));
                getPresonPhoto(rfidNumber.Substring(0, rfidNumber.Length - 1));
            }
            
        }


        #region Подключение к ардуино
        bool isConnectToArduino = false;

        public string ReadSettingsConnectToArduinoAsync()
        {
            string path = "/Resources/Settings/SettingCOMports.txt";
            string com = "";

            // асинхронное чтение
            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    com = line;
                }
            }

            return com;
        }

        public void ConnectToArduino()
        {
            try
            {
                //ReadSettingsConnectToArduinoAsync(); // чтение COM порта из файла настроек
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
            System.Environment.Exit(1);
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
