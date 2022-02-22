using System.Windows;
using WLightBox.Library;

namespace WLightBox.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int counter = 0;
        public MainWindow()
        {
            
            InitializeComponent();
            //Connection conn = new Connection();
            //foreach (Device dev in conn.devices)
            //{
            //    currentColorText.Text = dev.Ip;
            //}
                //Task.Delay(1000).Wait();
                // currentColorText.Text = connection.devices[0].GetCurrentColorAsync().Result;
                //currentEffectText.Text = connection.devices[0].GetCurrentEffectAsync().Result;
            }

        private void testButton_Click(object sender, RoutedEventArgs e)
        {
            
            currentEffectText.Text = "XDDD";
            foreach (Device dev in Connection.devices)
            {
                currentEffectText.Text = dev.Ip;
            }

            currentColorText.Text = counter++.ToString();
        }
    }
}
