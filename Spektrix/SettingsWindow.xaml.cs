using Spektrix.Code;
using Spektrix.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Spektrix
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private readonly string BackgroundsResourceUrl = "pack://application:,,,/Resources/Backgrounds/";
        private readonly string IconsResourceUrl = "pack://application:,,,/Resources/AppIcons/";

        public SettingsWindow()
        {
            InitializeComponent();
            Setup();
        }

        private void Setup()
        {
            ApplicationResources resources = new ApplicationResources();
            List<string> backgrounds = resources.GetResourcesName("resources/backgrounds/");
            List<string> icons = resources.GetResourcesName("resources/appicons/");
            List<string> tabIcons = resources.GetResourcesName("resources/tabicons/");

            foreach (string icon in icons)
                this.lb.Items.Add(new ListBoxItem() { Content = icon });
        }

        private void ItemSelection(object sender, EventArgs e)
        {
            ListBoxItem lbi = ((sender as ListBox).SelectedItem as ListBoxItem);

            this.image_display.Source = new BitmapImage(new Uri(IconsResourceUrl + lbi.Content.ToString()));
        }
    }
}
