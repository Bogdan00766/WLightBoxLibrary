using System;
using System.Threading.Tasks;
using System.Windows;
using WLightBox.Library;

namespace WLightBox.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LightBox lb;
        public MainWindow()
        {          
            InitializeComponent();
            lb = new LightBox();
        }
        //protected override async void OnInitialized(EventArgs e)
        //{
            //Connection connection = await Connection.CreateAsync();
            ////Task.Delay(2000).Wait();
            //currentColorText.Text = await connection.devices[0].GetCurrentColorAsync();
            //currentEffectText.Text = await connection.devices[0].GetCurrentEffectAsync();
        //}

        private void testButton_Click(object sender, RoutedEventArgs e)
        {
            lb.ResearchDevices();
        }
    }
}
