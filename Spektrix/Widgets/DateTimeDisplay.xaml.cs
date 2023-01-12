using System;
using System.Windows.Controls;

namespace Spektrix.Components
{
    /// <summary>
    /// Interaction logic for DateTimeDisplay.xaml
    /// </summary>
    public partial class DateTimeDisplay : UserControl
    {
        private readonly System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();

        public DateTimeDisplay()
        {
            InitializeComponent();
            Setup();
        }

        // Setup sets the information to be displayed
        private void Setup()
        {
            // Initialize Timer
            Timer.Tick += new EventHandler(Initialize);
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
        }

        // Initialize initializes the UI of the component
        private void Initialize(object sender, EventArgs e)
        {
            // Update Time
            DateTime datetime = DateTime.Now;
            clock.Content = datetime.ToString("hh:mm:ss tt");
            date.Content = datetime.ToString("MMM dd yyyy");
            day.Content = datetime.ToString("dddd");
        }
    }
}
