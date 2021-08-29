// See https://aka.ms/new-console-template for more information

using CdImage;

string res = PowerShellMount.Mount(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "Win10_21H1_English_x64.iso"));
Console.WriteLine("{0}:\\", res);