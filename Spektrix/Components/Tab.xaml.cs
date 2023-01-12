using Spektrix.Code;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Spektrix.Components
{
    /// <summary>
    /// Interaction logic for Tab.xaml
    /// </summary>
    public partial class Tab : UserControl
    {
        private readonly string TabIconsResourceUrl = "pack://application:,,,/Resources/TabIcons/";
        private int group = -1;
        public Tab()
        {
            InitializeComponent();
            Uninitialize();
        }

        // Setup sets the information to be displayed
        public void Setup(int group)
        {
            this.group = group;
            //string icon = Properties.Settings.Default.Get($"T{group}", "");
            //string icon = ConfigurationManager.AppSettings.Get($"T{group}");
            string icon = ApplicationSettingsManager.Get($"T{group}");
            string name = $"0{group + 1}";

            try { this.IconSrc.Source = new BitmapImage(new Uri(TabIconsResourceUrl + icon)); }
            catch { this.IconSrc.Source = null; }
            this.Id.Text = name;
        }

        // Initialize initializes the UI of the component
        public void Initialize(object sender = null, RoutedEventArgs e = null)
        {
            // Set Background to be white and semi-transparent
            this.Background = new SolidColorBrush(Color.FromArgb(0x7F, 255, 255, 255));
        }

        // Uninitialize uninitializes the UI of the component
        public void Uninitialize(object sender = null, RoutedEventArgs e = null)
        {
            // Set Background to be black and semi-transparent
            this.Background = new SolidColorBrush(Color.FromArgb(0x80, 0, 0, 0));
        }

        // Update group event
        private void UpdateGroup(object sender, RoutedEventArgs e)
        {
            if (this.group == -1) { MessageBox.Show("Tab is uninitialized. Call Setup method."); }
            // Get Main Window
            MainWindow win = (MainWindow)Window.GetWindow(this);
            // Call main_display_panels to update with associate tab id
            win.main_group.Setup(this.group);
        }

        public void SetIconVisibility(Visibility visible)
        {
            this.IconSrc.Visibility = visible;
            this.Id.Visibility = visible;
        }
    }
}
