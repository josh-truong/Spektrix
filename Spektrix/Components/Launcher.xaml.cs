using Spektrix.Code;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Spektrix.Components
{
    /// <summary>
    /// Interaction logic for Launcher.xaml
    /// </summary>

    public class LauncherInfo
    {
        public readonly string Background;
        public readonly string Icon;
        public readonly string Name;
        public string Action;

        public LauncherInfo(string[] info)
        {
            Background = info[0];
            Icon = info[1];
            Name = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(info[2] ?? "".ToLower());
            Action = info[3];
        }
    }

    public partial class Launcher : UserControl
    {
        private readonly string BackgroundsResourceUrl = "pack://application:,,,/Resources/Backgrounds/";
        private readonly string IconsResourceUrl = "pack://application:,,,/Resources/AppIcons/";

        public LauncherInfo panelInfo;

        public Launcher()
        {
            InitializeComponent();
        }

        // Setup sets the information to be displayed
        public void Setup(int group, int launcher)
        {
            // Generates ID to access user settings key
            string key = $"Group{group}Launcher{launcher}";
            string background = ApplicationSettingsManager.Get(key + "Background");
            string icon = ApplicationSettingsManager.Get(key + "Icon");
            string name = ApplicationSettingsManager.Get(key + "Name");
            string action = ApplicationSettingsManager.Get(key + "Action");

            panelInfo = new LauncherInfo(new string[4] { background, icon, name, action });

            // Sets information to User Control
            SetIcon(panelInfo.Icon);
            SetName(panelInfo.Name);
        }

        // Initialize initializes the UI of the component
        public void Initialize()
        {
            // Disable mouse events (hovering/clicking) on elements
            this.IsHitTestVisible = false;
        }

        // Uninitialize uninitializes the UI of the component
        public void Uninitialize()
        {
            DefaultBackground();
            SetInfoVisibility(Visibility.Hidden);
        }

        // Animate elements
        public void RunAnimation()
        {
            SetInfoVisibility(Visibility.Visible);

            DoubleAnimation FadeInAnimation = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(1)));
            this.main_meta_grid.BeginAnimation(OpacityProperty, FadeInAnimation);

            ThicknessAnimation ta = new ThicknessAnimation(
                new Thickness(0, -100, 0, 0),
                sub_meta_grid.Margin,
                new Duration(TimeSpan.FromSeconds(1))
            );

            // Skip metadata (name/icon) animation if no data is present.
            if (String.IsNullOrEmpty(panelInfo.Name) && String.IsNullOrEmpty(panelInfo.Icon))
            {
                this.IsHitTestVisible = true;
                return;
            }
            ta.Completed += (s, e) => { this.IsHitTestVisible = true; };
            this.sub_meta_grid.BeginAnimation(MarginProperty, ta);
        }

        private void SetInfoVisibility(Visibility visible)
        {
            // Set visibility of meta data (name/icon)
            this.ApplicationIcon.Visibility = visible;
            this.ApplicationName.Visibility = visible;
        }

        private void ActiveBackground(object sender, RoutedEventArgs e)
        {
            // Set Application Card to a solid background image
            this.panel_bar.Opacity = 1.0;
            SetBackground(panelInfo.Background);
        }

        public void DefaultBackground(object sender = null, RoutedEventArgs e = null)
        {
            // Set Application Card to a solid color semi-transparent background
            this.panel_bar.Opacity = 0.5;
            this.Background = new SolidColorBrush(Color.FromArgb(0x80, 0, 0, 0));
        }

        private void ExecuteLauncher(object sender, RoutedEventArgs e)
        {
            // #######################
            // Security Vulnerability - Command Injection Vulnerability
            // #######################
            // Execute action - should support .exe, url, cmd, etc.
            try
            {
                if (String.IsNullOrEmpty(panelInfo.Action)) return;
                Process.Start(panelInfo.Action);
                try
                {
                    // Only for main window and not settings window
                    MainWindow win = (MainWindow)Window.GetWindow(this);
                    win.Uninitialize();
                }
                catch { }
            }
            catch (Exception err) { MessageBox.Show($"Path to \"{panelInfo.Name}\" executable does not exist.\nSet Path: {panelInfo.Action}\n\n{err}"); }
        }

        public void SetBackground(string filename)
        {
            ImageBrush brush = new ImageBrush();
            try { brush.ImageSource = new BitmapImage(new Uri(BackgroundsResourceUrl + filename)); }
            catch { brush.ImageSource = null; }
            this.Background = brush;
        }

        public void SetIcon(string filename)
        {
            try { this.ApplicationIcon.Source = new BitmapImage(new Uri(IconsResourceUrl + filename)); }
            catch { this.ApplicationIcon.Source = null; }
        }

        public void SetName(string name)
        {
            this.ApplicationName.Text = name;
            this.ApplicationName.FontSize = SystemParameters.PrimaryScreenHeight * 0.025;
        }

        public void SetAction(string action)
        {
            panelInfo.Action = action;
        }
    }
}
