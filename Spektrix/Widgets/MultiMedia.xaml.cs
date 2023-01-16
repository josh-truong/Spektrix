using Spektrix.Code;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Windows.Media.Control;
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
        private MediaSession previousSession = null;
        static readonly object _writeLock = new object();


        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        public MultiMedia()
        {
            InitializeComponent();
            Setup();
            MouseEvent();
        }

        // Setup sets the information to be displayed
        private void Setup()
        {
            this.root_border.Background = new SolidColorBrush(Color.FromArgb(0x80, 0, 0, 0));
            this.previous.Source = new BitmapImage(new Uri(MultiMediaResourceUrl + "Previous.png"));
            this.play_pause.Source = new BitmapImage(new Uri(MultiMediaResourceUrl + "Play.png"));
            this.next.Source = new BitmapImage(new Uri(MultiMediaResourceUrl + "Next.png"));
            this.volume_mute.Source = new BitmapImage(new Uri(MultiMediaResourceUrl + "Mute.png"));

            // Subscribe to event handlers to WindowsMediaController
            mediaManager.OnAnySessionOpened += MediaManager_OnAnySessionOpened;
            mediaManager.OnAnySessionClosed += MediaManager_OnAnySessionClosed;
            mediaManager.OnFocusedSessionChanged += MediaManager_OnFocusedSessionChanged;
            mediaManager.OnAnyPlaybackStateChanged += MediaManager_OnAnyPlaybackStateChanged;
            mediaManager.OnAnyMediaPropertyChanged += MediaManager_OnAnyMediaPropertyChanged;

            mediaManager.Start();
        }

        private void MouseEvent()
        {
            this.previous_container.MouseEnter += (s, e) => { this.previous_container.Background = new SolidColorBrush(Color.FromArgb(0x80, 0xFF, 0xFF, 0xFF)); };
            this.previous_container.MouseLeave += (s, e) => { this.previous_container.Background = new SolidColorBrush(Color.FromArgb(0x00, 0x00, 0x00, 0x00)); };

            this.play_pause_container.MouseEnter += (s, e) => { this.play_pause_container.Background = new SolidColorBrush(Color.FromArgb(0x80, 0xFF, 0xFF, 0xFF)); };
            this.play_pause_container.MouseLeave += (s, e) => { this.play_pause_container.Background = new SolidColorBrush(Color.FromArgb(0x00, 0x00, 0x00, 0x00)); };

            this.next_container.MouseEnter += (s, e) => { this.next_container.Background = new SolidColorBrush(Color.FromArgb(0x80, 0xFF, 0xFF, 0xFF)); };
            this.next_container.MouseLeave += (s, e) => { this.next_container.Background = new SolidColorBrush(Color.FromArgb(0x00, 0x00, 0x00, 0x00)); };

            this.volume_mute_container.MouseEnter += (s, e) => { this.volume_mute_container.Background = new SolidColorBrush(Color.FromArgb(0x80, 0xFF, 0xFF, 0xFF)); };
            this.volume_mute_container.MouseLeave += (s, e) => { this.volume_mute_container.Background = new SolidColorBrush(Color.FromArgb(0x00, 0x00, 0x00, 0x00)); };

        }

        // Add media source to StackPanel
        private void MediaManager_OnAnySessionOpened(MediaManager.MediaSession session)
        {
            void AddSession()
            {
                try
                {
                    // Get media properties from current media session
                    var songInfo = session.ControlSession.TryGetMediaPropertiesAsync().GetAwaiter().GetResult();
                    // Get song thumbnail
                    BitmapImage session_thumbnail;
                    try { session_thumbnail = new BitmapImage(new Uri(MultiMediaResourceUrl + "Music.png")); }
                    catch { session_thumbnail = null; }

                    // Create a container
                    Border container = new Border()
                    {
                        Tag = session.Id,
                        BorderBrush = Brushes.White,
                        BorderThickness = new Thickness(2),
                        CornerRadius = new CornerRadius(10),
                        Margin = new Thickness(5, 1, 5, 1),
                        Child = new Viewbox()
                        {
                            // Create a viewbox with an image child
                            Child = new Image
                            {
                                ToolTip = session.Id,
                                Source = session_thumbnail,
                                Visibility = Visibility.Visible
                            }
                        }
                    };
                    container.MouseEnter += (s, e) =>
                    {
                        container.Background = new SolidColorBrush(Color.FromArgb(0x80, 0xFF, 0xFF, 0xFF));
                    };
                    container.MouseLeave += (s, e) =>
                    {
                        container.Background = new SolidColorBrush(Color.FromArgb(0x00, 0x00, 0x00, 0x00));
                    };
                    container.MouseDown += (s, e) =>
                    {
                        Console.WriteLine($"Now playing on {session.Id}.");
                    };

                    // Add to stackpanel
                    this.media_session_menu.Children.Add(container);
                    // Update stackpanel layout
                    this.media_session_menu.UpdateLayout();
                }
                catch { }
            }

            WriteLineColor("-- New Source: " + session.Id, ConsoleColor.Green);
            Application.Current.Dispatcher.Invoke(AddSession);
        }

        // Remove media source to StackPanel
        private void MediaManager_OnAnySessionClosed(MediaManager.MediaSession session)
        {
            WriteLineColor("-- Removed Source: " + session.Id, ConsoleColor.Red);
            void RemoveSession()
            {
                try
                {
                    Border container = GetContainer(session);
                    if (container == null) return;

                    // Remove viewbox from stackpanel
                    this.media_session_menu.Children.Remove(container);
                    // Update stackpanel
                    this.media_session_menu.UpdateLayout();
                }
                catch { }
            }

            Application.Current.Dispatcher.Invoke(RemoveSession);
            Application.Current.Dispatcher.Invoke(() => { UpdateMediaDisplay(previousSession); });
        }

        private void MediaManager_OnFocusedSessionChanged(MediaManager.MediaSession mediaSession)
        {
            void ChangeSession()
            {
                try
                {
                    // Reset previous session background
                    if (previousSession != null)
                        GetContainer(previousSession).Background = new SolidColorBrush(Color.FromArgb(0x00, 0x00, 0x00, 0x00));

                    // Highlight session being used
                    Border container = GetContainer(mediaSession);
                    if (container == null) return;
                    container.Background = new SolidColorBrush(Color.FromArgb(0x80, 0xFF, 0xFF, 0xFF));

                    // Set current to previous
                    previousSession = mediaSession;
                }
                catch { }
            }
            WriteLineColor("== Session Focus Changed: " + mediaSession?.ControlSession?.SourceAppUserModelId, ConsoleColor.Gray);
            Application.Current.Dispatcher.Invoke(ChangeSession);
            Application.Current.Dispatcher.Invoke(() => { UpdateMediaDisplay(mediaSession); });
        }

        private void MediaManager_OnAnyPlaybackStateChanged(MediaManager.MediaSession sender, GlobalSystemMediaTransportControlsSessionPlaybackInfo args)
        {
            void PlaybackState()
            {
                try
                {
                    if (args.PlaybackStatus.ToString() == "Playing")
                        this.play_pause.Source = new BitmapImage(new Uri(MultiMediaResourceUrl + "Pause.png"));
                    else if (args.PlaybackStatus.ToString() == "Paused")
                        this.play_pause.Source = new BitmapImage(new Uri(MultiMediaResourceUrl + "Play.png"));
                }
                catch { }
            }
            WriteLineColor($"{sender.Id} is now {args.PlaybackStatus}", ConsoleColor.Yellow);

            Application.Current.Dispatcher.Invoke(PlaybackState);
            Application.Current.Dispatcher.Invoke(() => { UpdateMediaDisplay(sender); });
        }

        private void MediaManager_OnAnyMediaPropertyChanged(MediaManager.MediaSession sender, GlobalSystemMediaTransportControlsSessionMediaProperties args)
        {
            WriteLineColor($"{sender.Id} is now playing {args.Title} {(String.IsNullOrEmpty(args.Artist) ? "" : $"by {args.Artist}")}", ConsoleColor.Cyan);
            Application.Current.Dispatcher.Invoke(() => { UpdateMediaDisplay(sender); });
        }

        public static void WriteLineColor(object toprint, ConsoleColor color = ConsoleColor.White)
        {
            lock (_writeLock)
            {
                Console.ForegroundColor = color;
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss.fff") + "] " + toprint);
            }
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

        private Border GetContainer(MediaManager.MediaSession session)
        {
            // Check if there are any children in stackpanel
            if (this.media_session_menu.Children.Count == 0) return null;
            // Iterate through stackpanel Children
            foreach (Border child in this.media_session_menu.Children)
                // Check if viewbox tag matches session id
                if (child.Tag.ToString() == session.Id) return child;
            return null;
        }

        void UpdateMediaDisplay(MediaManager.MediaSession session)
        {
            if (session == null) return;
            var songInfo = session.ControlSession.TryGetMediaPropertiesAsync().GetAwaiter().GetResult();
            if (songInfo == null) return;

            // Update song image, title, author
            BitmapImage session_thumbnail = (songInfo != null) ? GetThumbnail(songInfo.Thumbnail) : null;

            this.songTitle.Text = songInfo.Title.ToUpper();
            this.songAuthor.Text = songInfo.Artist;
            this.songImage.Source = GetThumbnail(songInfo.Thumbnail);
        }
    }
}
