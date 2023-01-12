using Spektrix.Code;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Windows.Storage.Streams;
using static Spektrix.Code.MediaManager;
using Color = System.Windows.Media.Color;
using UserControl = System.Windows.Controls.UserControl;

namespace Spektrix.Widgets
{
    /// <summary>
    /// Interaction logic for MultiMedia.xaml
    /// </summary>
    public partial class MultiMedia : UserControl
    {
        private readonly string MultiMediaResourceUrl = "pack://application:,,,/Resources/MultiMedia/";
        private const int WM_APPCOMMAND = 0x319;
        private const int APPCOMMAND_MEDIA_PLAY_PAUSE = 0xE0000;
        private const int APPCOMMAND_MEDIA_PREVIOUSTRACK = 0xB0000;
        private const int APPCOMMAND_MEDIA_NEXTTRACK = 0xC0000;
        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;

        private static readonly MediaManager mediaManager = new MediaManager();


        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        public MultiMedia()
        {
            InitializeComponent();
            Setup();
        }

        // Setup sets the information to be displayed
        private void Setup()
        {
            this.root_border.Background = new SolidColorBrush(Color.FromArgb(0x80, 0, 0, 0));
            this.previous.Source = new BitmapImage(new Uri(MultiMediaResourceUrl + "Previous.png"));
            this.play_pause.Source = new BitmapImage(new Uri(MultiMediaResourceUrl + "Play.png"));
            this.next.Source = new BitmapImage(new Uri(MultiMediaResourceUrl + "Next.png"));
            this.volume_mute.Source = new BitmapImage(new Uri(MultiMediaResourceUrl + "Volume.png"));

            this.songTitle.FontSize = SystemParameters.PrimaryScreenHeight * 0.015;
            this.songAuthor.FontSize = SystemParameters.PrimaryScreenHeight * 0.010;

            mediaManager.OnAnySessionOpened += Initialize;
            //mediaManager.OnAnySessionOpened += MediaManager_OnAnySessionOpened;
            //mediaManager.OnAnySessionClosed += MediaManager_OnAnySessionClosed;
            //mediaManager.OnFocusedSessionChanged += MediaManager_OnFocusedSessionChanged;
            mediaManager.Start();
        }

        // Initialize initializes the UI of the component
        private void Initialize(MediaSession mediaSession)
        {
            var songInfo = mediaSession.ControlSession.TryGetMediaPropertiesAsync().GetAwaiter().GetResult();
            if (songInfo != null)
            {
                this.songTitle.Text = songInfo.Title.ToUpper();
                this.songAuthor.Text = songInfo.Artist;
                this.songImage.Source = GetThumbnail(songInfo.Thumbnail);
            }
        }

        // Uninitialize uninitializes the UI of the component
        private void Uninitialize()
        {

        }



        private void PlayPause(object sender, EventArgs e)
        {
            Window win = Window.GetWindow(this);
            IntPtr Handle = new WindowInteropHelper(win).Handle;
            SendMessageW(Handle, WM_APPCOMMAND, Handle, (IntPtr)APPCOMMAND_MEDIA_PLAY_PAUSE);
        }

        private void Previous(object sender, EventArgs e)
        {
            Window win = Window.GetWindow(this);
            IntPtr Handle = new WindowInteropHelper(win).Handle;
            SendMessageW(Handle, WM_APPCOMMAND, Handle, (IntPtr)APPCOMMAND_MEDIA_PREVIOUSTRACK);
        }

        private void Next(object sender, EventArgs e)
        {
            Window win = Window.GetWindow(this);
            IntPtr Handle = new WindowInteropHelper(win).Handle;
            SendMessageW(Handle, WM_APPCOMMAND, Handle, (IntPtr)APPCOMMAND_MEDIA_NEXTTRACK);
        }

        private void VolumeMute(object sender, EventArgs e)
        {
            Window win = Window.GetWindow(this);
            IntPtr Handle = new WindowInteropHelper(win).Handle;
            SendMessageW(Handle, WM_APPCOMMAND, Handle, (IntPtr)APPCOMMAND_VOLUME_MUTE);
        }

        private static BitmapImage GetThumbnail(IRandomAccessStreamReference Thumbnail)
        {
            if (Thumbnail == null) return null;

            var imageStream = Thumbnail.OpenReadAsync().GetAwaiter().GetResult();
            byte[] fileBytes = new byte[imageStream.Size];
            using (DataReader reader = new DataReader(imageStream))
            {
                reader.LoadAsync((uint)imageStream.Size).GetAwaiter().GetResult();
                reader.ReadBytes(fileBytes);
            }

            var image = new BitmapImage();
            using (var ms = new System.IO.MemoryStream(fileBytes))
            {
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
            }
            return image;
        }
    }
}
