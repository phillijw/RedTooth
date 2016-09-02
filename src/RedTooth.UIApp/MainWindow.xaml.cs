using RedTooth.Core.Controller;
using RedTooth.Core.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;

namespace RedTooth.UIApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        private ToolController nearbyDevicesController;

        public MainWindow()
        {
            InitializeComponent();
            nearbyDevicesController = new ToolController();
        }

        private void ScanDevices_Click(object sender, RoutedEventArgs e)
        {
            nearbyDevicesListBox.Items.Clear();                      

            var tools = nearbyDevicesController.AllTools().Select(x => new ToolViewModel
            {
                 ID=x.Key,
                 BluetoothAddress=ToolController.ByteArrayToString(x.Value.BluetoothAddress),
                 MPBID=x.Value.MPBID,
                 Name=x.Value.Name,
                 RSSI=x.Value.RSSI
            }).OrderBy(x=>x.RSSI);

            nearbyDevicesListBox.ItemsSource = tools;                        
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            var tool = nearbyDevicesListBox.SelectedItem as ToolViewModel;
            if (tool != null)
            {                
                var connected = nearbyDevicesController.SendCommand(tool.ID);
                if (connected)
                {
                    MessageBox.Show("Connected!");
                }
                else {
                    MessageBox.Show("Failed!");
                }
            }
            else
            {
                MessageBox.Show("SELECT A TOOL!");
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
