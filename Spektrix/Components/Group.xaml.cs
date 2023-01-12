using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Spektrix.Components
{
    /// <summary>
    /// Interaction logic for Group.xaml
    /// </summary>
    public partial class Group : UserControl
    {
        public Launcher[] launchers = new Launcher[6];

        public Group()
        {
            InitializeComponent();

            // Add named user control elements from xaml file to list
            launchers[0] = launcher_0;
            launchers[1] = launcher_1;
            launchers[2] = launcher_2;
            launchers[3] = launcher_3;
            launchers[4] = launcher_4;
            launchers[5] = launcher_5;
        }

        // Setup sets the information to be displayed
        public void Setup(int group = 0)
        {
            // Iterate through "ApplicationCard[] panels" and run ApplicationCard "Init" method
            for (int i = 0; i < launchers.Length; i++)
                launchers[i].Setup(group, i);
        }

        // Initialize initializes the UI of the component
        public void Initialize()
        {
            // Runs User Control Animation
            foreach (Launcher panel in launchers)
                panel.Initialize();
        }

        // Uninitialize uninitialize(reset) the UI of the component
        public void Uninitialize()
        {
            foreach (Launcher panel in launchers)
                panel.Uninitialize();
        }

        // Animate User Controls
        public void RunAnimation()
        {
            // Animate the white panel bar
            DoubleAnimation panel_bar_animation = new DoubleAnimation(0, this.ActualWidth, new Duration(TimeSpan.FromSeconds(1.5)));
            panel_bar_animation.Completed += (s, e) =>
            {
                // Animate the panel height
                DoubleAnimation panel_animation = new DoubleAnimation(0, this.ActualHeight, new Duration(TimeSpan.FromSeconds(1)));
                panel_animation.Completed += (s1, e1) =>
                {
                    foreach (Launcher panel in launchers)
                        panel.RunAnimation();
                };
                foreach (Launcher panel in launchers)
                    panel.BeginAnimation(HeightProperty, panel_animation);
            };

            foreach (Launcher panel in launchers)
            {
                DoubleAnimation init_animation = new DoubleAnimation(panel.panel_bar.ActualHeight, panel.panel_bar.ActualHeight, new Duration(TimeSpan.FromSeconds(0)));
                panel.BeginAnimation(HeightProperty, init_animation);
                panel.panel_bar.BeginAnimation(WidthProperty, panel_bar_animation);
            }
        }
    }
}
