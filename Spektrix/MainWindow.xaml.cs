using Spektrix.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Spektrix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /**
         * Main public methods for the application
         * Setup(), Initialize(), Uninitialize(), RunAnimation()
         */

        readonly MainApplicationBackground ApplicationBackground;
        public MainWindow()
        {
            ApplicationBackground = new MainApplicationBackground(this);
            InitializeComponent();
            SettingsSetup();
            Setup();
        }

        private void SettingsSetup()
        {
            /**
             * Dynamically create the user settings
             * Create key/value pair for every launcher assigned to group with 4 attributes
             * - Background, Icon, Name, Action
             * To identify a specific launcher the value is Group[n]Launcher[n]Attribute
             */

            ApplicationResources resources = new ApplicationResources();
            List<string> backgrounds = resources.GetResourcesName("resources/backgrounds/");
            List<string> icons = resources.GetResourcesName("resources/appicons/");
            List<string> tabIcons = resources.GetResourcesName("resources/tabicons/");

            Random rand = new Random();
            int groupCount = this.main_tab_bar.tabs.Count();
            int launchersCount = this.main_group.launchers.Count();

            for (int i = 0; i < groupCount; i++)
            {
                string menuIcon = tabIcons[rand.Next(0, tabIcons.Count())];
                if (!ApplicationSettingsManager.ContainsKey($"T{i}"))
                    ApplicationSettingsManager.Set($"T{i}", menuIcon);
            }

            // Set default for launchers
            for (int idx = 0; idx < groupCount * launchersCount; idx++)
            {
                int i = idx / launchersCount;
                int j = idx % launchersCount;

                string key = $"Group{i}Launcher{j}";
                string background = backgrounds[rand.Next(0, backgrounds.Count())];
                string icon = icons[rand.Next(0, icons.Count())];

                if (!ApplicationSettingsManager.ContainsKey(key + "Background"))
                    ApplicationSettingsManager.Set(key + "Background", background);
                if (!ApplicationSettingsManager.ContainsKey(key + "Icon"))
                    ApplicationSettingsManager.Set(key + "Icon", icon);
                if (!ApplicationSettingsManager.ContainsKey(key + "Name"))
                    ApplicationSettingsManager.Set(key + "Name", System.IO.Path.GetFileNameWithoutExtension(icon));
                if (!ApplicationSettingsManager.ContainsKey(key + "Action"))
                    ApplicationSettingsManager.Set(key + "Action", "");
            }
        }

        // Setup sets the information to be displayed
        private void Setup()
        {
            // Setup user controls
            this.main_tab_bar.Setup();
            this.main_group.Setup();
        }

        // Initialize initializes the UI of the component
        public void Initialize(object sender = null, RoutedEventArgs e = null)
        {
            // Enable Blurred Window Background
            ApplicationBackground.SetBackground(AccentState.ACCENT_ENABLE_BLURBEHIND);
            // 0x0C(0.05) is non-zero transparency to avoid click-though windows
            Application.Current.MainWindow.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0x0C, 0, 0, 0));
            // Set visibility to specific user controls
            SetUserControlVisibility(Visibility.Visible);
            // initialize user controls
            this.main_tab_bar.Initialize();
            this.main_group.Initialize();
            // Start user control animation
            RunAnimation();
        }

        // Uninitialize uninitialize(reset) the UI of the component
        public void Uninitialize(object sender = null, RoutedEventArgs e = null)
        {
            // Disable Blurred Window Background
            ApplicationBackground.SetBackground(AccentState.ACCENT_ENABLE_GRADIENT);
            // 0x00 is absolute transparency to allow click-though windows
            Application.Current.MainWindow.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0x00, 0, 0, 0));
            // Set visibility to specific user controls
            SetUserControlVisibility(Visibility.Hidden);
            // Reset user control
            this.main_tab_bar.Uninitialize();
            this.main_group.Uninitialize();
            // Reset to first tab
            this.main_group.Setup();
        }

        // Animate user controls
        private void RunAnimation()
        {
            DoubleAnimation bottom_bar_animation = new DoubleAnimation(0, this.menu_widget_hub.ActualWidth, new Duration(TimeSpan.FromSeconds(1)));
            bottom_bar.BeginAnimation(WidthProperty, bottom_bar_animation);
            this.main_tab_bar.RunAnimation();
            this.main_group.RunAnimation();
        }

        private void Exit(object sender, EventArgs e) { this.Close(); }


        private void SetUserControlVisibility(Visibility visible)
        {
            // Set User Controls Visibility
            this.main_display_clock.Visibility = visible;
            this.main_tab_bar.Visibility = visible;
            this.main_group.Visibility = visible;
            this.menu_widget_hub.Visibility = visible;
            this.startup_bar.Visibility = (visible == Visibility.Visible) ? Visibility.Hidden : Visibility.Visible;
        }
    }
}
