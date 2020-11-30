using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenRecorder.FFMPEG
{
    public enum RecordingType
    {
        FullScreen,
        Region
    }
    public sealed class Recorder
    {
        private string ffpmegPath;
        public RecordingType recordingType { get; set; }
        private Process recordingProcess = null;
        private StreamWriter myStreamWriter = null;
        private string OutPutDirectory;
        private string FileName;
        public Recorder()
        {
            recordingType = RecordingType.FullScreen;
            this.ffpmegPath= AppDomain.CurrentDomain.BaseDirectory + @"FFMPEG\ffmpeg-4.3.1\bin\ffmpeg.exe";
        }
        public void Start()
        {
            try
            {

                string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                OutPutDirectory = Path.Combine(myDocuments, "ScreenRecordings");
                if (!Directory.Exists(OutPutDirectory))
                    Directory.CreateDirectory(OutPutDirectory);
                recordingProcess = new Process();
                /*
                recordingProcess.StartInfo.FileName = ffpmegPath;
                recordingProcess.StartInfo.Arguments = "";
                recordingProcess.EnableRaisingEvents = true;
                recordingProcess.StartInfo.UseShellExecute = false;
                recordingProcess.StartInfo.RedirectStandardInput = true;
                */
                FileName = "ScreenRecording_" + DateTime.Now.Ticks + ".mp4";
                recordingProcess.StartInfo.FileName = ffpmegPath; // Change the directory where ffmpeg.exe is.  
                //recordingProcess.StartInfo.WorkingDirectory = OutPutDirectory; // The output directory  
                //Flags                                                     //process.StartInfo.Arguments = @"-f gdigrab -framerate " + Framerate + " -i desktop -preset ultrafast -pix_fmt yuv420p " + FileName;
                recordingProcess.StartInfo.CreateNoWindow = true;
                recordingProcess.StartInfo.UseShellExecute = false;
                recordingProcess.StartInfo.RedirectStandardError = true;
                recordingProcess.StartInfo.RedirectStandardOutput = true;
                recordingProcess.StartInfo.RedirectStandardInput = true;
                recordingProcess.EnableRaisingEvents = true;
                recordingProcess.ErrorDataReceived += RecordingProcess_ErrorDataReceived; ;
                recordingProcess.OutputDataReceived += RecordingProcess_OutputDataReceived;
                if(recordingType == RecordingType.FullScreen)
                    recordingProcess.StartInfo.Arguments = @" -rtbufsize 1500M -f gdigrab -framerate 25 -draw_mouse 0 -i desktop -pix_fmt yuv420p -c:v libx264  -preset ultrafast " + FileName;
                else
                {

                }
                Task t = new Task(() => {
                    recordingProcess.Start();
                    myStreamWriter = recordingProcess.StandardInput;
                    recordingProcess.BeginOutputReadLine();
                    recordingProcess.BeginErrorReadLine();
                    //Thread.Sleep(5000);
                    myStreamWriter = recordingProcess.StandardInput;
                    recordingProcess.WaitForExit();
                    if (recordingProcess.ExitCode == 0)
                    {
                       //Means Recording is Finished
                    }
                });
                t.Start();
            }
            catch(Exception ex)
            {

            }
        }

        private void RecordingProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Debug.WriteLine(e.Data);
        }

        private void RecordingProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Debug.WriteLine(e.Data);
        }

        public void Stop()
        {
            try
            {
                if (recordingProcess != null)
                {
                    myStreamWriter.WriteLine("q");
                    recordingProcess.WaitForExit();
                    recordingProcess.Close();
                    myStreamWriter.Close();
                    myStreamWriter = null;
                    recordingProcess = null;                    
                }
            }
            catch (Exception ex)
            {
                //_log.Info("Exception in Stop:->" + ex.Message);
            }
            finally
            {
                if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName)))
                {
                    File.Move(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName),Path.Combine(OutPutDirectory,FileName));
                }
            }
        }
    }
}
