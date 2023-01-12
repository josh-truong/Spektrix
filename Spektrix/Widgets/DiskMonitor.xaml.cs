using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Spektrix.Widgets
{
    /// <summary>
    /// Interaction logic for DiskMonitor.xaml
    /// </summary>
    public partial class DiskMonitor : UserControl
    {
        private readonly DispatcherTimer Frequency = new DispatcherTimer();

        public DiskMonitor()
        {
            InitializeComponent();
            Setup();
            Tick(null, null);
        }

        private void Setup()
        {
            this.root_border.Background = new SolidColorBrush(Color.FromArgb(0x80, 0, 0, 0));
            // DispatcherTimer ticks on the UI Thread
            Frequency.Tick += new EventHandler(Tick);
            Frequency.Interval = new TimeSpan(0, 0, 5, 0, 0);
            Frequency.Start();
        }

        // Update Widget
        private void Tick(Object sender, EventArgs e)
        {
            int maxDrives = 2; int numDrives = 0;

            this.drive0_label.Text = "";
            this.drive0_col0.Text = $"Free: ---";
            this.drive0_col1.Text = $"Total: ---";

            this.drive1_label.Text = "";
            this.drive1_col0.Text = $"Free: ---";
            this.drive1_col1.Text = $"Total: ---";

            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (numDrives >= maxDrives) break;
                if (drive.IsReady && numDrives == 0)
                {
                    numDrives++;
                    this.drive0_label.Text = $"({drive.Name})";
                    this.drive0_col0.Text = $"Free: {drive.AvailableFreeSpace * 9.31e-10:F2} GB";
                    this.drive0_col1.Text = $"Total: {drive.TotalSize * 9.31e-10:F2} GB";
                }
                else if (drive.IsReady && numDrives == 1)
                {
                    numDrives++;
                    this.drive1_label.Text = $"({drive.Name})";
                    this.drive1_col0.Text = $"Free: {drive.AvailableFreeSpace * 9.31e-10:F2} GB";
                    this.drive1_col1.Text = $"Total: {drive.TotalSize * 9.31e-10:F2} GB";
                }
            }
        }
    }
}
