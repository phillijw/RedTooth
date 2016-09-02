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
            var nearbyDevices = new List<Tool>();
            var nearbyDevicesController = new ToolController();
            InitializeComponent();
        }

        private void ScanDevices_Click(object sender, RoutedEventArgs e)
        {
            var tools = ToolController.MockTools().Select(x => new ToolViewModel
            {
                 ID=x.Key,
                 BluetoothAddress=ToolController.ByteArrayToString(x.Value.BluetoothAddress),
                 MPBID=x.Value.MPBID,
                 Name=x.Value.Name,
                 RSSI=x.Value.RSSI
            });
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
    }
}
