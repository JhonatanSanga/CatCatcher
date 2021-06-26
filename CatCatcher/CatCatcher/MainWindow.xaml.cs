using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Vision.Motion;

namespace CatCatcher
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Public properties

        public ObservableCollection<FilterInfo> VideoDevices { get; set; }

        public FilterInfo CurrentDevice
        {
            get { return _currentDevice; }
            set { _currentDevice = value; this.OnPropertyChanged("CurrentDevice"); }
        }
        private FilterInfo _currentDevice;


        #endregion

        #region Private fields

        private VideoCaptureDevice _videoSource;//IVideoSource

        #endregion
        #region variables Arduino
        System.IO.Ports.SerialPort arduino;
        bool detect = true;
        #endregion
        public ObservableCollection<VideoCapabilities> VideoCapabilities { get; set; }

        public MainWindow()
        {
            System.Threading.Thread.Sleep(4444);//que buenos tiempos no? :3
            InitializeComponent();
            this.DataContext = this;
            GetVideoDevices();
            #region Arduino
            arduino = new System.IO.Ports.SerialPort();
            arduino.PortName = "COM5";
            arduino.BaudRate = 9600;
            arduino.Open();
            #endregion

            this.Closing += MainWindow_Closing;
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StopCamera();
            
            Application.Current.Shutdown();
        }


        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            StartCamera();
        }
        byte count = 0;//contador para que haga una busqueda cada 5 frames
        private void video_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            try
            {
                BitmapImage bi;
                using (var bitmap = (Bitmap)eventArgs.Frame.Clone())
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {

                        GlyphRecognition.GlyphRecognizer glyphRecognizer;
                        glyphRecognizer = new GlyphRecognition.GlyphRecognizer(new GlyphRecognition.GlyphDatabase(5));
                        glyphRecognizer.GlyphDatabase.Add(new GlyphRecognition.Glyph("nuevo", new byte[,] {
                            { 0, 0, 0, 0, 0},
                            { 0, 1, 1, 0, 0},
                            { 0, 0, 1, 1, 0},
                            { 0, 1, 1, 0, 0},
                            { 0, 0, 0, 0, 0}}));

                        count++;
                        if (count >= 5)
                        {
                            count = 0;///////////////////////////////////////
                            if (glyphRecognizer.FindGlyphs(bitmap).Count > 1)
                            {
                                lblFind.Foreground = System.Windows.Media.Brushes.LightGreen;
                                lblFind.Content = "Encontrado";
                                if (detect==true)
                                {
                                    arduino.Write("E");
                                    detect = false;
                                }
                            }
                            else
                            {
                                lblFind.Foreground = System.Windows.Media.Brushes.Red;
                                lblFind.Content = "No encontrado";
                                detect = true;
                            }
                        }
                        //Dispatcher.BeginInvoke(new ThreadStart(delegate { imageCapture.Source = bi; }));
                    }));
                    bi = bitmap.ToBitmapImage();////
                    bi.Freeze();
                }


                bi.Freeze(); // avoid cross thread operations and prevents leaks
                Dispatcher.BeginInvoke(new ThreadStart(delegate { videoPlayer.Source = bi; }));

            }
            catch (Exception exc)
            {
                MessageBox.Show("Error on _videoSource_NewFrame:\n" + exc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                StopCamera();
            }
        }


        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            StopCamera();
            videoPlayer.Source = new BitmapImage(new Uri("Media/splashScreen.jpg", UriKind.Relative));
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //cmbCapabilities.Items.Clear();
            capturedevice = new VideoCaptureDevice(CurrentDevice.MonikerString);
            /*if (_videoSource != null)
            {
                foreach (var item in _videoSource.VideoCapabilities)
                {
                    cmbCapabilities.Items.Add(item.FrameSize);
                }
            }*/
        }

        private void BtnChangeGlypho_Click(object sender, RoutedEventArgs e)
        {
            Glypho g = new Glypho();
            g.Show();
        }
        

        private void GetVideoDevices()
        {
            VideoDevices = new ObservableCollection<FilterInfo>();
            foreach (FilterInfo filterInfo in new FilterInfoCollection(FilterCategory.VideoInputDevice))
            {
                VideoDevices.Add(filterInfo);
            }
            if (VideoDevices.Any())
            {
                CurrentDevice = VideoDevices[0];
            }
            else
            {
                MessageBox.Show("No video sources found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        VideoCaptureDevice capturedevice = new VideoCaptureDevice();
        private void StartCamera()
        {

            if (CurrentDevice != null)
            {
                _videoSource = new VideoCaptureDevice(CurrentDevice.MonikerString);


                capturedevice = new VideoCaptureDevice(CurrentDevice.MonikerString);
               //if (cmbCapabilities.Text != "")//NO CAMBIA POR QUE MI CAMARA NO QUIERE :(
                    //EN TODOS LOS CODIGOS QUE HE ENCONTRADO ESTA ASI :(
                    //_videoSource.VideoResolution = _videoSource.VideoCapabilities[cmbCapabilities.SelectedIndex];

                //_videoSource = capturedevice;
                _videoSource.NewFrame += video_NewFrame;
                _videoSource.Start();

            }
        }
        private void StopCamera()
        {
            if (_videoSource != null && _videoSource.IsRunning)
            {
                _videoSource.SignalToStop();
                _videoSource.NewFrame -= new NewFrameEventHandler(video_NewFrame);
            }
        }

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (arduino.IsOpen)
            {
                arduino.Close();
            }
        }
    }
}
