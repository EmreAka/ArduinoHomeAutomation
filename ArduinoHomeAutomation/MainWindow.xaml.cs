using System;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Windows;

namespace ArduinoHomeAutomation
{
    public partial class MainWindow : Window
    {
        SerialPort mySerialPort = new SerialPort("COM3");
        string temp = "NULL";
        string motion = "NULL";
        string alarm = "NULL";
        string desiredTemperature= "NULL";
        string relay = "NULL";

        public MainWindow()
        {
            InitializeComponent();
            mySerialPort.BaudRate = 9600;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None;
            mySerialPort.RtsEnable = true;
            mySerialPort.DataReceived += new SerialDataReceivedEventHandler
                (DataReceivedHandler);
            mySerialPort.Open();
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadLine();
            if (indata.StartsWith("TEMP"))
            {
                temp = indata.Substring(indata.IndexOf(": ")+1).Trim();
            }
            if (indata.Contains("MOTION"))
            {
                motion = indata.Substring(indata.IndexOf(": ")+1).Trim();
            }
            if (indata.Contains("ALARM"))
            {
                alarm = indata.Substring(indata.IndexOf(": ")+1).Trim();
            }
            if (indata.StartsWith("DESIRED"))
            {
                desiredTemperature = indata.Substring(indata.IndexOf(": ")+ 1).Trim();
            }
            if (indata.StartsWith("RELAY"))
            {
                relay = indata.Substring(indata.IndexOf(": ") + 1).Trim();
            }
            Console.WriteLine("Data Received:");
            Console.Write(indata);
            this.Dispatcher.Invoke(() => {
                tempIs.Content = temp;
                motionIs.Content = motion;
                alarmIs.Content = alarm;
                desiredTempIs.Content = desiredTemperature;
                relayIs.Content = relay;
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mySerialPort.WriteLine("SETALARMON");
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            mySerialPort.WriteLine("SETALARMOFF");
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            mySerialPort.WriteLine("SETTEMP:10");
        }

        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            mySerialPort.WriteLine("SETTEMP:100");
        }

        private void SendTemp(object sender, RoutedEventArgs e)
        {
            mySerialPort.WriteLine("SETTEMP:" + desiredTemp.Text);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            mySerialPort.WriteLine("SETRELAY01ON");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            mySerialPort.WriteLine("SETRELAY01OFF");
        }
    }
}
