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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArduinoHomeAutomation
{
    public partial class MainWindow : Window
    {
        SerialPort mySerialPort = new SerialPort("COM3");
        //string temp = "TEMP: 00.00";
        //string motion = "MOTION: 0";
        //string lightSensor = "LDR'S VALUE: 0";
        //string alarm = "ALARM: ON";
        string temp = "NULL";
        string motion = "NULL";
        string lightSensor = "NULL";
        string alarm = "NULL";

        public MainWindow()
        {
            InitializeComponent();
            mySerialPort.BaudRate = 9600;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None;
            mySerialPort.RtsEnable = true;
            mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            mySerialPort.Open();
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadLine();
            if (indata.Contains("TEMP"))
            {
                temp = indata.Substring(indata.IndexOf(": ")+1).Trim();
            }
            if (indata.Contains("MOTION"))
            {
                motion = indata.Substring(indata.IndexOf(": ")+1).Trim();
            }
            if (indata.Contains("LDR"))
            {
                lightSensor = indata.Substring(indata.IndexOf(": ")+1).Trim();
            }
            if (indata.Contains("ALARM"))
            {
                alarm = indata.Substring(indata.IndexOf(": ")+1).Trim();
            }
            Console.WriteLine("Data Received:");
            Console.Write(indata);
            this.Dispatcher.Invoke(() => {
                tempIs.Content = temp;
                motionIs.Content = motion;
                lightIs.Content = lightSensor;
                alarmIs.Content = alarm;
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
    }
}
