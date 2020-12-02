using ScreenRecorder.FFMPEG;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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
using System.Windows.Threading;

namespace ScreenRecorder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Recorder screenRecorder = null;
        private string _RecordingDuration = "";
        private DateTime? StartTime = null;
        public string RecordingDuration
        {
            get
            {
                return _RecordingDuration;
            }
            set
            {
                _RecordingDuration = value;
                OnPropertyChanged();
            }
        }

        DispatcherTimer timer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            screenRecorder = new Recorder();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            StartTime = DateTime.Now;
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += Timer_Tick;
            timer.Start();
            screenRecorder.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                DateTime currentTime = DateTime.Now;
                var duration = currentTime - StartTime;
                string formatted =duration.Value.ToString(@"hh\:mm\:ss");
                RecordingDuration = formatted;
            }catch(Exception ex)
            {

            }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            StartTime = null;
            screenRecorder.Stop();
        }
    }
}
