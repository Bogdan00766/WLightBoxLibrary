using System;
using System.Collections.Generic;
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
        private string? currentEffect;
        private Device? currentDevice;
        private List<string>? currentEffectsList;
        public MainWindow()
        {
            InitializeComponent();
            devicesListBox.Items.Add("Searching...");
            effectsListBox.Items.Add("Searching...");
            devicesListBox.SelectionChanged += changedSelection;
            effectsListBox.SelectionChanged += elbChangedSelection;
            cwSlider.ValueChanged += cwSliderValueChanged;
            wwSlider.ValueChanged += wwSliderValueChanged;

        }



        private void cwSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            cwSliderValueText.Text = cwSlider.Value.ToString();
        }

        private void wwSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            wwSliderValueText.Text = wwSlider.Value.ToString();
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            lb = new LightBox();
            lb.SetupConnection();
            SetList();
        }

        private void elbChangedSelection(object sender, SelectionChangedEventArgs e)
        {
            if (effectsListBox.SelectedItem != null)
            {
                currentEffect = effectsListBox.SelectedItem.ToString();
            }
        }

        private async void changedSelection(object sender, SelectionChangedEventArgs e)
        {
            Device? dev = lb.Devices.Where(x => x.Ip == (string)devicesListBox.SelectedItem).FirstOrDefault();
            currentDevice = dev;
            if (currentDevice != null)
            {
                effectsListBox.Items.Clear();
                currentEffectsList = await currentDevice.GetEffectsListAsync();
                foreach (var effect in currentEffectsList)
                {
                    effectsListBox.Items.Add(effect);
                }
            }
            updateInfo();
        }

        private async void updateInfo()
        {
            if (currentDevice != null)
            {
                Rgbww rgbww = await currentDevice.GetCurrentColorAsync();
                currentColorText.Text = $"RGB WW CW: {rgbww.Red}, {rgbww.Green}, {rgbww.Blue}, {rgbww.WarmWhite}, {rgbww.ColdWhite}";
                wwSlider.Value = rgbww.WarmWhite;
                cwSlider.Value = rgbww.ColdWhite;
                currentEffectText.Text = await currentDevice.GetCurrentEffectAsync();


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
            if (effectsListBox.SelectedItem != null && currentEffectsList != null && currentDevice != null)
            {
                int? index = currentEffectsList.IndexOf(effectsListBox.SelectedItem.ToString());
                if (index != null)
                {
                    currentDevice.SetEffect((int)index);
                    updateInfo();
                }
            }
        }

        private void setColorButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentDevice != null && colorPicker.SelectedColor != null)
            {
                currentDevice.SetColor(colorPicker.SelectedColor.Value.R, colorPicker.SelectedColor.Value.G, colorPicker.SelectedColor.Value.B, (int)wwSlider.Value, (int)cwSlider.Value);
                Task.Delay(100).Wait();
                updateInfo();
            }
        }
    }
}