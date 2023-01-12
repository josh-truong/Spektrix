using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DispatcherTimer = System.Windows.Threading.DispatcherTimer;

namespace Spektrix.Widgets
{
    /// <summary>
    /// Interaction logic for CpuRamMonitor.xaml
    /// </summary>
    public partial class CpuRamMonitor : UserControl
    {
        private PerformanceCounter cpuCounter;
        private PerformanceCounter ramCounter;
        private readonly DispatcherTimer Frequency = new DispatcherTimer();
        private readonly double TotalRam = (double)new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory * 9.31e-10;

        public CpuRamMonitor()
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
            Frequency.Interval = new TimeSpan(0, 0, 0, 1, 0);
            Frequency.Start();

            // Setup Performance Counter for cpu/ram
            cpuCounter = new PerformanceCounter("Processor Information", "% Processor Utility", "_Total");
            ramCounter = new PerformanceCounter("Memory", "Available Bytes");

            // FontSize Setup
            this.title.FontSize = SystemParameters.PrimaryScreenHeight * 0.015;
            this.cpu_col0.FontSize = SystemParameters.PrimaryScreenHeight * 0.015;
            this.ram_col0.FontSize = SystemParameters.PrimaryScreenHeight * 0.015;

            this.cpu_col1.FontSize = SystemParameters.PrimaryScreenHeight * 0.015;
            this.ram_col1.FontSize = SystemParameters.PrimaryScreenHeight * 0.015;

            this.cpu_col2.FontSize = SystemParameters.PrimaryScreenHeight * 0.015;
            this.ram_col2.FontSize = SystemParameters.PrimaryScreenHeight * 0.015;
        }

        // Update Widget
        private void Tick(Object sender, EventArgs e)
        {
            GetCurrentCpuUsage();
            GetAvailableRAM();
        }

        // Cpu Usage
        private void GetCurrentCpuUsage()
        {
            this.cpu_col1.Text = cpuCounter.NextValue().ToString("0.##") + "%";
        }

        // Ram usage
        private void GetAvailableRAM()
        {
            double availableRamGb = TotalRam - (ramCounter.NextValue() * 9.31e-10);
            this.ram_col1.Text = $"{availableRamGb:F2}GB / {TotalRam:F2}GB";
            this.ram_col2.Text = $"{availableRamGb / TotalRam:P2}";
        }
    }
}
