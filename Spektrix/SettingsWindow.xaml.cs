using Spektrix.Code;
using Spektrix.Components;
using Spektrix.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Spektrix
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private readonly List<string> DirectoryNames = new List<string> { "Backgrounds", "AppIcons", "TabIcons" };
        private readonly string ResourceUrl = "pack://application:,,,/Resources";
        private int CurrentGroup = 0; private int CurrentLauncher = 0;
        private string CurrentBackground = null;
        private string CurrentIcon = null;
        private string CurrentName = null;
        private string CurrentTab = null;
        private string CurrentAction = null;

        public SettingsWindow()
        {
            InitializeComponent();
            Setup();
        }

        private void Setup()
        {
            int groupCount = 4; int launcherCount = 6;

            for (int i = 0; i < groupCount; i++)
                this.cb_group.Items.Add(new ComboBoxItem() { Tag=i, Name=$"Group{i}", Content = $"Group {i}" });
            this.cb_group.SelectedIndex = CurrentGroup;

            for (int i = 0; i < launcherCount; i++)
                this.cb_launcher.Items.Add(new ComboBoxItem() { Tag=i, Name=$"Launcher{i}", Content = $"Launcher {i}" });
            this.cb_launcher.SelectedIndex = CurrentLauncher;

            ApplicationResources resources = new ApplicationResources();
            List<string> backgrounds = resources.GetResourcesName($"resources/backgrounds/");
            List<string> appicons = resources.GetResourcesName($"resources/appicons/");
            List<string> tabicons = resources.GetResourcesName($"resources/tabicons/");

            for (int i = 0; i < tabicons.Count; i++)
                this.cb_tab_icon.Items.Add(new ListBoxItem() { Tag = i, Content = tabicons[i] });

            for (int i = 0; i < backgrounds.Count; i++)
                this.cb_background.Items.Add(new ListBoxItem() { Tag = i, Content = backgrounds[i] });

            for (int i = 0; i < appicons.Count; i++)
                this.cb_icon.Items.Add(new ListBoxItem() { Tag = i, Content = appicons[i] });

            SetupLauncher();
        }

        private void GroupSelection(object sender, EventArgs e)
        {
            ComboBoxItem cbi = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            CurrentGroup = (int)cbi.Tag;
            SetupLauncher();
        }

        private void LauncherSelection(object sender, EventArgs e)
        {
            ComboBoxItem cbi = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            CurrentLauncher = (int)cbi.Tag;
            SetupLauncher();
        }

        private void TabIconSelection(object sender, EventArgs e)
        {
            ListBoxItem cbi = (ListBoxItem)((ListBox)sender).SelectedItem;
            this.tab_preview.Source = new BitmapImage(new Uri($"{ResourceUrl}/TabIcons/{cbi.Content}"));
            CurrentTab = cbi.Content.ToString();
        }

        private void BackgroundSelection(object sender, EventArgs e)
        {
            ListBoxItem cbi = (ListBoxItem)((ListBox)sender).SelectedItem;
            CurrentBackground = cbi.Content.ToString();
            UpdatePreview();
        }

        private void IconSelection(object sender, EventArgs e)
        {
            ListBoxItem cbi = (ListBoxItem)((ListBox)sender).SelectedItem;
            CurrentIcon = cbi.Content.ToString();
            UpdatePreview();
        }

        private void NameSelection(object sender, EventArgs e)
        {
            CurrentName = this.launcher_name.Text;
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            launcher_preview.SetBackground(CurrentBackground);
            launcher_preview.SetIcon(CurrentIcon);
            launcher_preview.SetName(CurrentName);
            launcher_preview.SetAction(CurrentAction);
            this.launcher_name.Text = CurrentName;
        }

        private void SetupLauncher(object sender = null, EventArgs e=null)
        {
            string key = $"Group{CurrentGroup}Launcher{CurrentLauncher}";
            CurrentBackground = ApplicationSettingsManager.Get($"{key}Background");
            CurrentIcon = ApplicationSettingsManager.Get($"{key}Icon");
            CurrentName = ApplicationSettingsManager.Get($"{key}Name");
            CurrentTab = ApplicationSettingsManager.Get($"T{CurrentGroup}");
            CurrentAction = ApplicationSettingsManager.Get($"{key}Action");


            MouseEventArgs mouse = new MouseEventArgs(Mouse.PrimaryDevice, 0);
            mouse.RoutedEvent = Mouse.MouseEnterEvent;

            launcher_preview.Setup(CurrentGroup, CurrentLauncher);
            launcher_preview.RaiseEvent(mouse);
            launcher_preview.MouseLeave -= new MouseEventHandler(launcher_preview.DefaultBackground);

            this.tab_preview.Source = new BitmapImage(new Uri(ResourceUrl + "/TabIcons/" + ApplicationSettingsManager.Get($"T{CurrentGroup}")));
            this.launcher_name.Text = CurrentName;
            this.launcher_action.Text = ApplicationSettingsManager.Get($"{key}Action");
        }

        private void SaveSettings(object sender, EventArgs e)
        {
            Console.WriteLine($"{CurrentBackground} {CurrentIcon} {CurrentName} {CurrentAction} {CurrentTab}");
            string key = $"Group{CurrentGroup}Launcher{CurrentLauncher}";
            ApplicationSettingsManager.Set($"{key}Background", CurrentBackground);
            ApplicationSettingsManager.Set($"{key}Icon", CurrentIcon);
            ApplicationSettingsManager.Set($"{key}Name", CurrentName);
            ApplicationSettingsManager.Set($"{key}Action", CurrentAction);
            ApplicationSettingsManager.Set($"T{CurrentGroup}", CurrentTab);
            SetupLauncher();
        }

        private void Exit(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ActionSelection(object sender, EventArgs e)
        {
            CurrentAction = this.launcher_action.Text;
            UpdatePreview();
        }
    }
}
