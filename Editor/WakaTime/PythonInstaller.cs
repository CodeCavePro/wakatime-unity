using UnityEditor;
using System;
using UnityEngine;
using System.Diagnostics;

using System.IO;

namespace WakaTime
{
    public static class PythonInstaller
    {
        static WWW www;

        static Process installProcess;

        static string GetFileFolder()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }

        static string GetFilePath()
        {
            return GetFileFolder() + PythonManager.GetPythonFileName();
        }

        static bool IsDownloaded()
        {
            return File.Exists(GetFilePath());
        }

        static public void DownloadAndInstall()
        {
            if (!PythonManager.IsPythonInstalled())
            {
                if (!IsDownloaded())
                {
                    Download();
                }
                else {
                    Install();
                }
            }
        }

        static public bool IsInstalling()
        {
            return IsDownloading() || installProcess != null;
        }

        static public void Download()
        {
            string url = PythonManager.GetPythonDownloadUrl();

            www = new WWW(url);
            EditorApplication.update = WhileDownloading;
        }

        public static bool IsDownloading()
        {
            return www != null;
        }

        static void WhileDownloading()
        {
            EditorUtility.DisplayProgressBar("Downloading Python", "Python is being downloaded", www.progress);

            if (www.isDone)
            {
                EditorApplication.update = null;
                DownloadCompleted();
            }
        }

        static void DownloadCompleted()
        {
            EditorUtility.ClearProgressBar();

            if (Main.IsDebug)
            {
                UnityEngine.Debug.Log("Python downloaded: " + www.size);
            }
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string localFile = dir + PythonManager.GetPythonFileName();


            try
            {
                using (var stream = new FileStream(localFile, FileMode.Create, FileAccess.Write))
                {
                    stream.Write(www.bytes, 0, www.bytes.Length);

                    // close file stream
                    stream.Close();
                }

                www = null;
            }
            catch (Exception ex)
            {
                if (Main.IsDebug)
                {
                    UnityEngine.Debug.LogError("Python download failed: " + ex.Message);
                }
            }

            Install();
        }

        static void Install()
        {
            string arguments = "/i \"" + GetFilePath() + "\"";
            arguments = arguments + " /norestart /qb!";

            try
            {
                var procInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    FileName = "msiexec",
                    CreateNoWindow = true,
                    Arguments = arguments
                };

                installProcess = Process.Start(procInfo);
                installProcess.WaitForExit();
                installProcess.Close();

                installProcess = null;
            }
            catch (Exception ex)
            {
                if (Main.IsDebug)
                {
                    UnityEngine.Debug.LogError("Python installation failed: " + ex.Message);
                }
            }
        }
    }
}