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

namespace RedTooth.UIApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IList<Tool> nearbyDevices;
        private ToolController nearbyDevicesController;

        public MainWindow()
        {
            var nearbyDevices = new List<Tool>();
            //var nearbyDevicesController = new ToolController();
            InitializeComponent();
        }

        private void ScanDevices_Click(object sender, RoutedEventArgs e)
        {
            var tools = ToolController.MockTools();
            nearbyDevicesListBox.ItemsSource = tools;            
        }
    }
}
