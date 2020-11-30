using ScreenRecorder.FFMPEG;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ScreenRecorder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Recorder screenRecorder = null;
        public MainWindow()
        {
            InitializeComponent();
            screenRecorder = new Recorder();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            screenRecorder.Start();
        }       

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            screenRecorder.Stop();
        }
    }
}
