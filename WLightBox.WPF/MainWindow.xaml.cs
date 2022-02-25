using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WLightBox.Library;

namespace WLightBox.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window

    {
        private LightBox lb;
        private Device? currentDevice;
        public MainWindow()
        {          
            InitializeComponent();
            devicesListBox.Items.Add("Searching...");
            devicesListBox.SelectionChanged += changedSelection;

        }
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            lb = new LightBox();
            lb.SetupConnection();
            SetList();
        }

        private void changedSelection(object sender, SelectionChangedEventArgs e)
        {
            Device? dev = lb.Devices.Where(x => x.Ip == (string)devicesListBox.SelectedItem).FirstOrDefault();
            currentDevice = dev;
            updateInfo();
        }

        private async void updateInfo()
        {
            if (currentDevice != null)
            {
                Rgbww rgbww = await currentDevice.GetCurrentColorAsync();
                currentColorText.Text = $"RGB WW CW: {rgbww.Red}, {rgbww.Green}, {rgbww.Blue}, {rgbww.WarmWhite}, {rgbww.ColdWhite}";
                //currentEffectText.Text = await currentDevice.GetCurrentEffectAsync();
            }
        }

        private void testButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            devicesListBox.Items.Clear();
            devicesListBox.Items.Add("Searching...");
            lb.ResearchDevices();
            SetList();
        }

        private void SetList()
        {
            devicesListBox.Items.Clear();
            foreach (Device device in lb.Devices)
            {
                devicesListBox.Items.Add(device.Ip);
            }
            if (lb.Devices.Count < 0)
            
            {
                devicesListBox.ItemsSource = new string[] { "Not Found!" };
                devicesListBox.IsEnabled = false;
            }
        }
         
        private void setEffectButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void setColorButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentDevice != null)
            {
                currentEffectText.Text = colorPicker.SelectedColor.Value.R.ToString() + colorPicker.SelectedColor.Value.G.ToString() + colorPicker.SelectedColor.Value.B.ToString();
                currentDevice.SetColor(colorPicker.SelectedColor.Value.R, colorPicker.SelectedColor.Value.G, colorPicker.SelectedColor.Value.B, 1, 1);
                updateInfo();
            }
        }
    }
}
