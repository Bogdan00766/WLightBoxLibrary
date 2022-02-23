// See https://aka.ms/new-console-template for more information

using WLightBox.Library;

Console.WriteLine("Hello, World!");
//Connection conn = await Connection.CreateAsync();
//foreach (Device dev in conn.devices)
//{
//    Console.WriteLine(dev.Ip);
//    Console.WriteLine(await dev.GetCurrentColorAsync());
//    await dev.SetColorAsync("00ff00ff00");
//    Console.WriteLine(await dev.GetCurrentColorAsync());
//    Console.WriteLine(await dev.GetCurrentEffectAsync());
//    await dev.SetEffectAsync(1);

//    Console.WriteLine(await dev.GetCurrentEffectAsync());

//}

WLightBox.Library.LightBox wlb = new WLightBox.Library.LightBox();
foreach (var device in wlb.devices)
{
    Console.WriteLine(device.Ip);
    await device.SetColorAsync("1234567811");
    Console.WriteLine(await device.GetCurrentColorAsync());
}
while (true) ;
