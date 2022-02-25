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
await wlb.SetupConnection();
var x = await wlb.Devices[0].GetEffectsListAsync();
foreach (var effect in x)
{
    Console.WriteLine(effect);
}
//foreach (var device in wlb.Devices)
//{
//    Console.WriteLine(device.Ip);
//    await device.SetColor(111,112,113,114,115);
//    var color = await device.GetCurrentColorAsync();
//    Console.WriteLine($"{color.Red} {color.Green} {color.Blue} {color.WarmWhite} {color.ColdWhite}");
//}
while (true) ;
