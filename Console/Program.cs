
// See https://aka.ms/new-console-template for more information
using WLightBox.Library;

Console.WriteLine("Hello, World!");
Connection conn = new Connection();
foreach(Device dev in conn.devices)
{
    Console.WriteLine(dev.Ip);
    Console.WriteLine(await dev.GetCurrentColorAsync());
    dev.SetColorAsync("00ff00ff00");
    Console.WriteLine(await dev.GetCurrentEffectAsync());
    dev.SetEffectAsync(1);
    Task.Delay(1000).Wait();
    Console.WriteLine(await dev.GetCurrentEffectAsync());

}
while (true) ;
