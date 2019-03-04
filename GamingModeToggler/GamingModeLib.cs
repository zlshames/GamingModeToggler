using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GamingModeToggler
{
    class GamingModeLib
    {
        private static string roaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static string local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private static string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        private static string programFiles86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
  
        // Game Mode On Lists
        private ArrayList gmOnStartLst = new ArrayList();
        private ArrayList gmOnKillLst = new ArrayList();

        // Game Mode Off Lists
        private ArrayList gmOffStartLst = new ArrayList();
        private ArrayList gmOffKillLst = new ArrayList();

        // Default On Lists
        private string[] defaultGmOnKillLst = {
            Path.Combine(roaming, "Spotify", "Spotify.exe"),
            Path.Combine(programFiles, "Rainmeter", "Rainmeter.exe"),
            Path.Combine(local, "slack", "slack.exe"),
            Path.Combine(programFiles, "ueli", "ueli.exe"),
            Path.Combine(local, "Mailspring", "mailspring.exe"),
            Path.Combine(programFiles, "Greenshot", "Greenshot.exe"),
            Path.Combine(programFiles86, "TranslucentTB", "TranslucentTB.exe")
        };
        private string[] defaultGmOnStartLst = {
            // None yet
        };

        // Default Off Lists
        private string[] defaultGmOffKillLst = {
            // None Yet
        };
        private string[] defaultGmOffStartLst = {
            Path.Combine(programFiles, "Rainmeter", "Rainmeter.exe"),
            Path.Combine(local, "slack", "slack.exe"),
            Path.Combine(programFiles, "ueli", "ueli.exe"),
            Path.Combine(local, "Mailspring", "mailspring.exe"),
            Path.Combine(programFiles, "Greenshot", "Greenshot.exe"),
            Path.Combine(programFiles86, "TranslucentTB", "TranslucentTB.exe")
        };

        // List compilation
        private ArrayList[] lists = new ArrayList[4];
        private string[] settings = { "gmOnStartLst", "gmOnKillLst", "gmOffStartLst", "gmOffKillLst" };

        public GamingModeLib()
        {
            lists[0] = gmOnStartLst;
            lists[1] = gmOnKillLst;
            lists[2] = gmOffStartLst;
            lists[3] = gmOffKillLst;

            loadDefaultSettings();

            // Load saved list
            if (Properties.Settings.Default.hasSavedSettings)
                loadSavedSettings();
        }

        public ArrayList gamingModeOn()
        {
            return toggleProcesses(gmOnKillLst, gmOnStartLst);
        }

        public ArrayList gamingModeOff()
        {
            return toggleProcesses(gmOffKillLst, gmOffStartLst);
        }

        private ArrayList toggleProcesses(ArrayList killLst, ArrayList startLst)
        {
            ArrayList output = new ArrayList();
      
            // Kill all processes from list
            var processes = Process.GetProcesses();
            foreach (Process theprocess in processes) {
                string procName = string.Format("{0}.exe", theprocess.ProcessName);

                // Find matches and kill
                foreach (string path in killLst) {
                    string appName = path.Split('\\').Last();
                    if (appName.Equals(procName)) {
                        output.Add(string.Format("Killing {0}", procName));

                        try {
                            theprocess.Kill();
                        } catch (Exception ex) {
                            output.Add(string.Format("Failed to kill {0}", procName));
                            output.Add(string.Format("Error: {0}", ex.Message));
                        }
                    }
                }
            }

            // Start processes
            foreach (string path in startLst) {
                string appName = path.Split('\\').Last();

                try {
                    ArrayList res = startIfNotRunning(path);
                    foreach (string item in res)
                        output.Add(item);
                } catch (Exception ex) {
                    output.Add(string.Format("Failed to start {0}", appName));
                    output.Add(string.Format("Error: {0}", ex.Message));
                }
            }

            return output;
        }

        private ArrayList startIfNotRunning(string procPath)
        {
            ArrayList output = new ArrayList();
            var processes = Process.GetProcesses();

            // Check if process is already running
            bool alreadyRunning = false;
            string appName = procPath.Split('\\').Last();
            foreach (Process proc in processes) {
                string procName = string.Format("{0}.exe", proc.ProcessName);
                if (procName.Equals(appName)) {
                    alreadyRunning = true;
                }
            }

            try {
                // Restart the process if not already running
                if (alreadyRunning == false) {
                    output.Add(string.Format("Starting {0}", appName));
                    Process.Start(procPath);
                } else {
                    output.Add(string.Format("{0} is already running", appName));
                }
            } catch (Exception) {
                // Don't do anything
            }

            return output;
        }

        private void loadSavedSettings()
        {
            for (int i = 0; i < lists.Length; i++) {
                lists[i].Clear();

                string config = Properties.Settings.Default[settings[i]].ToString();
                foreach (string lstItem in config.Split(',')) {
                    if (!string.IsNullOrEmpty(lstItem))
                        lists[i].Add(lstItem);
                }
            }
        }

        public void saveSettings()
        {
            Properties.Settings.Default.gmOnKillLst = string.Join(",", (string[])gmOnKillLst.ToArray(Type.GetType("System.String")));
            Console.WriteLine(string.Join(",", (string[])gmOnKillLst.ToArray(Type.GetType("System.String"))));
            Properties.Settings.Default.gmOnStartLst = string.Join(",", (string[])gmOnStartLst.ToArray(Type.GetType("System.String")));
            Properties.Settings.Default.gmOffKillLst = string.Join(",", (string[])gmOffKillLst.ToArray(Type.GetType("System.String")));
            Properties.Settings.Default.gmOffStartLst = string.Join(",", (string[])gmOffStartLst.ToArray(Type.GetType("System.String")));
            Properties.Settings.Default.hasSavedSettings = true;
            Properties.Settings.Default.Save();
        }

        private void loadDefaultSettings()
        {
            // Load default game mode on kill list
            foreach (string path in defaultGmOnKillLst) {
                if (File.Exists(path))
                    gmOnKillLst.Add(path);
            }

            // Load default game mode on start list
            foreach (string path in defaultGmOnStartLst) {
                if (File.Exists(path))
                    gmOnStartLst.Add(path);
            }

            // Load default game mode off kill list
            foreach (string path in defaultGmOffKillLst) {
                if (File.Exists(path))
                    gmOffKillLst.Add(path);
            }

            // Load default game mode off start list
            foreach (string path in defaultGmOffStartLst) {
                if (File.Exists(path))
                    gmOffStartLst.Add(path);
            }
        }

        public ArrayList getGmOnKillList()
        {
            return gmOnKillLst;
        }

        public ArrayList getGmOnStartList()
        {
            return gmOnStartLst;
        }

        public ArrayList getGmOffKillList()
        {
            return gmOffKillLst;
        }

        public ArrayList getGmOffStartList()
        {
            return gmOffStartLst;
        }
    }
}
