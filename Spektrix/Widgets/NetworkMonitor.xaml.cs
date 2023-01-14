using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Spektrix.Widgets
{
    /// <summary>
    /// Interaction logic for NetworkMonitor.xaml
    /// </summary>
    public partial class NetworkMonitor : UserControl
    {
        private PerformanceCounter bytesSent;
        private PerformanceCounter bytesReceived;
        private readonly DispatcherTimer Frequency = new DispatcherTimer();

        private float peakSent = 0;
        private float peakReceived = 0;

        private float sumSent = 0;
        private float sumReceived = 0;

        public NetworkMonitor()
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

            // Setup Performance Counter for network
            PerformanceCounterCategory performanceCounterCategory = new PerformanceCounterCategory("Network Interface");
            string instance = performanceCounterCategory.GetInstanceNames()[0]; // Get network card
            bytesSent = new PerformanceCounter("Network Interface", "Bytes Sent/sec", instance);
            bytesReceived = new PerformanceCounter("Network Interface", "Bytes Received/sec", instance);

            this.wifi_ipv4_col1.Visibility = Visibility.Hidden;
            this.ethernet_ipv4_col1.Visibility = Visibility.Hidden;

            // Find and set ipv4 address to wifi and ethernet if applicable
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            if (ni.Name.IndexOf("WI-FI", StringComparison.OrdinalIgnoreCase) >= 0)
                                this.wifi_ipv4_col1.Text = ip.Address.ToString();
                            if (ni.Name.IndexOf("Ethernet", StringComparison.OrdinalIgnoreCase) >= 0)
                                this.ethernet_ipv4_col1.Text = ip.Address.ToString();
                        }
                    }
                }
            }
        }

        private void Tick(Object sender, EventArgs e)
        {
            // Check if you're connected to the internet
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) return;

            float sent = bytesSent.NextValue();
            float received = bytesReceived.NextValue();

            peakSent = (sent <= peakSent) ? peakSent : sent;
            peakReceived = (received <= peakReceived) ? peakReceived : received;

            sumSent += sent;
            sumReceived += received;

            this.speed_col0.Text = $"{sent:F1} B/s";
            this.speed_col2.Text = $"{received:F1} B/s";

            this.peak_col0.Text = $"{peakSent * 9.537e-7:F1} MB/s";
            this.peak_col2.Text = $"{peakReceived * 7.8e-3:F1} kB/s";

            this.sum_col0.Text = $"{sumSent * 9.31e-10:F1} GB";
            this.sum_col2.Text = $"{sumReceived * 9.31e-10:F1} GB";
        }

        private void Initialize(Object sender, EventArgs e)
        {
            Grid section = (Grid)sender;
            section.Background = new SolidColorBrush(Color.FromArgb(0x80, 0xFF, 0xFF, 0xFF));
        }

        private void Uninitialize(Object sender, EventArgs e)
        {
            Grid section = (Grid)sender;
            section.Background = new SolidColorBrush(Color.FromArgb(0x00, 0x00, 0x00, 0x00));
        }

        private void SetWifiVisibility(Object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left || e.ClickCount != 2) return;
            this.wifi_ipv4_col1.Visibility = (this.wifi_ipv4_col1.Visibility == Visibility.Visible) ? Visibility.Hidden : Visibility.Visible;
        }

        private void SetEthernetVisibility(Object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left || e.ClickCount != 2) return;
            this.ethernet_ipv4_col1.Visibility = (this.ethernet_ipv4_col1.Visibility == Visibility.Visible) ? Visibility.Hidden : Visibility.Visible;
        }
    }
}
