
// See https://aka.ms/new-console-template for more information
using WLightBox.Library;

Console.WriteLine("Hello, World!");
Connection conn = new Connection();
foreach(Device dev in conn.devices)
{
    Console.WriteLine(dev.Ip);
    Console.WriteLine(await dev.GetCurrentColorAsync());
}
while (true) ;
