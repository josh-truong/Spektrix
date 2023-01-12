using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Spektrix.Code
{
    /**
     * Credit to vadimffe.
     * https://github.com/vadimffe/InstalledAppsViewer
     */
    class InstalledApp
    {
        public string DisplayName { get; set; }
        public string InstallationLocation { get; set; }
    }

    class InstalledApplication
    {
        private static List<InstalledApp> GetInstalledApplication(RegistryKey regKey, string registryKey)
        {
            List<InstalledApp> list = new List<InstalledApp>();
            using (Microsoft.Win32.RegistryKey key = regKey.OpenSubKey(registryKey))
            {
                if (key == null) return list;
                foreach (string name in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(name))
                    {
                        string installLocation = (string)subkey.GetValue("InstallLocation") ?? (string)subkey.GetValue("");

                        if (string.IsNullOrEmpty(installLocation)) continue;
                        installLocation = installLocation.Trim().Replace(@"\\", @"/").Replace(@"\", @"/").Replace("\"", "");
                        if (Uri.IsWellFormedUriString(installLocation, UriKind.Absolute)) continue;

                        string displayName = (string)subkey.GetValue("DisplayName");
                        displayName = !string.IsNullOrEmpty(displayName) ? displayName : Path.GetFileNameWithoutExtension(installLocation).Trim();
                        installLocation = Regex.Replace(installLocation, $"/{displayName}.exe", "", RegexOptions.IgnoreCase);
                        installLocation = (!installLocation.EndsWith("/")) ? installLocation + "/" : installLocation;

                        list.Add(new InstalledApp()
                        {
                            DisplayName = displayName.Trim(),
                            InstallationLocation = installLocation
                        });
                    }
                }

            }
            return list;
        }

        public List<InstalledApp> GetFullListInstalledApplication()
        {
            IEnumerable<InstalledApp> finalList = new List<InstalledApp>();

            string registry = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths";
            string registry_key_32 = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            string registry_key_64 = @"SOFTWARE\WoW6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

            List<InstalledApp> win32AppsCU = GetInstalledApplication(Registry.CurrentUser, registry_key_32);
            List<InstalledApp> win32AppsLM = GetInstalledApplication(Registry.LocalMachine, registry_key_32);
            List<InstalledApp> win64AppsCU = GetInstalledApplication(Registry.CurrentUser, registry_key_64);
            List<InstalledApp> win64AppsLM = GetInstalledApplication(Registry.LocalMachine, registry_key_64);
            List<InstalledApp> Apps = GetInstalledApplication(Registry.LocalMachine, registry);

            finalList = win32AppsCU.Concat(win32AppsLM).Concat(win64AppsCU).Concat(win64AppsLM).Concat(Apps);

            finalList = finalList.GroupBy(d => d.DisplayName).Select(d => d.First());

            return finalList.ToList();
        }

    }
}



//string FindExePath(string path, string exeName)
//{
//    string[] files = System.IO.Directory.GetFiles(path, exeName, System.IO.SearchOption.AllDirectories);
//    Array.Sort(files, StringComparer.CurrentCultureIgnoreCase);
//    Array.Reverse(files);
//    return files[0].Replace(@"\", @"/");
//}

//panelInfo.Action = FindExePath(panelInfo.Action, $"{panelInfo.ActionName}.exe");
//string path = "";
//List<string> strings = panelInfo.Action.Split('\\').ToList();
//for (int i = 0; i < strings.Count; i++)
//{
//    if (String.Equals(strings[i], strings[strings.Count - 1], StringComparison.OrdinalIgnoreCase)) break;
//    path += $"{strings[i]}/";
//}
//path += $"{strings[strings.Count - 1]}";


//public void SearchExecutablePaths()
//{
//    InstalledApplication installedApplication = new InstalledApplication();
//    List<InstalledApp> paths = installedApplication.GetFullListInstalledApplication();

//    ApplicationResources resources = new ApplicationResources();
//    List<string> resourceLocation = resources.GetResourcesLocation();
//    List<string> iconNames = resources.GetIcons(resourceLocation);
//    List<string> backgroundNames = resources.GetBackgrounds(resourceLocation);

//    int panelCount = 0;
//    int panelsPerTab = 6;
//    int maxPanels = 24; // 6 launchers per tab
//    for (int i = 0; i < paths.Count; i++)
//    {
//        for (int j = 0; j < iconNames.Count; j++)
//        {
//            // Check if iconNames contains DisplayName cont
//            if (panelCount == maxPanels) { break; }
//            if (iconNames[j].IndexOf(paths[i].DisplayName, StringComparison.OrdinalIgnoreCase) < 0) { break; }

//            string background = backgroundNames[new Random().Next(0, backgroundNames.Count() - 1)];
//            string icon = iconNames[j];
//            string name = paths[i].DisplayName;
//            string action = paths[i].InstallationLocation;

//            int tabIdx = (panelCount % panelsPerTab) + 1;
//            int panelIdx = (panelCount / panelsPerTab) + 1;

//            StringCollection collection = new StringCollection();
//            collection.AddRange(new String[4] { background, icon, name, action });
//            Settings.Default[$"T{tabIdx}P{panelIdx}"] = collection;
//            Settings.Default.Save();
//        }
//    }
//}