using System.ComponentModel;
using System.Net;
using UnityEditor;

namespace WakaTime
{
    public class DownloadProgressWindow : IDownloadProgressReporter
    {
        public int Progress { get; private set; }

        public void Close(AsyncCompletedEventArgs e)
        {
            EditorUtility.ClearProgressBar();
        }

        public void Dispose()
        {
            // TODO release managed resources, if any
        }

        public void Report(DownloadProgressChangedEventArgs value)
        {
            Progress = value.ProgressPercentage;
        }

        public void Show(string message = "")
        {
            EditorUtility.DisplayProgressBar("Downloading Python", "Python is being downloaded", Progress);
        }
    }
}
