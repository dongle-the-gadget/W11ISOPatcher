// See https://aka.ms/new-console-template for more information

using WindowsUpdateLib;
using DownloadLib;

CTAC ctac = new CTAC(OSSkuId.HomeBasic, "10.0.19041.200", MachineType.amd64, "Retail", "", "CB", "vb_release", "Production", false);

IEnumerable<UpdateData> updates = await FE3Handler.GetUpdates(null, ctac, "", FileExchangeV3UpdateFilter.ProductRelease);

foreach (var update in updates)
{
    Console.WriteLine(update.Xml.LocalizedProperties.Title);
    await UpdateUtils.ProcessUpdateAsync(update, @"C:\Users\minh3\Donwloads", MachineType.amd64, new Test(), "core_en-us.esd", "en-us");
}

public class Test : IProgress<GeneralDownloadProgress>
{
    public void Report(GeneralDownloadProgress value)
    {
        
    }
}