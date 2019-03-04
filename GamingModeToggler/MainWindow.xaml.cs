using System.Windows;
using System.Collections;
using System.ComponentModel;
using Microsoft.Win32;
using System;

namespace GamingModeToggler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GamingModeLib gmLib;
        private ArrayList[] lists = new ArrayList[4];

        public MainWindow()
        {
            InitializeComponent();
            gmLib = new GamingModeLib();
            updateLists();
        }

        private void updateLists()
        {
            foreach (string path in gmLib.getGmOnKillList()) {
                gmOnKillLstBox.Items.Add(path);
            }

            foreach (string path in gmLib.getGmOnStartList()) {
                gmOnStartLstBox.Items.Add(path);
            }

            foreach (string path in gmLib.getGmOffKillList()) {
                gmOffKillLstBox.Items.Add(path);
            }

            foreach (string path in gmLib.getGmOffStartList()) {
                gmOffStartLstBox.Items.Add(path);
            }
        }

        private void OnBtn_Click(object sender, RoutedEventArgs e)
        {
            OutputLog.Clear();
            writeLog("Activating Gaming Mode...");

            ArrayList output = gmLib.gamingModeOn();
            foreach (string log in output)
                writeLog(log);

            writeLog("Gaming Mode Activated...");
        }

        private void OffBtn_Click(object sender, RoutedEventArgs e)
        {
            OutputLog.Clear();
            writeLog("Deactivating Gaming Mode...");

            ArrayList output = gmLib.gamingModeOff();
            foreach (string log in output)
                writeLog(log);

            writeLog("Gaming Mode Dectivated...");
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            writeLog("Saving Settings...");
            gmLib.saveSettings();
            writeLog("Settings Saved...");
        }

        private void writeLog(string log)
        {
            OutputLog.AppendText(string.Format("{0}\n", log));
        }

        private void AddOnKillBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog addDialog = new OpenFileDialog();
            addDialog.Title = "Select a file";
            if (addDialog.ShowDialog() == true) {
                gmOnKillLstBox.Items.Add(addDialog.FileName);
                gmLib.getGmOnKillList().Add(addDialog.FileName);
            }
        }

        private void RemoveOnKillBtn_Click(object sender, RoutedEventArgs e)
        {
            try {
                gmLib.getGmOnKillList().Remove(gmOnKillLstBox.SelectedItem.ToString());
                gmOnKillLstBox.Items.Remove(gmOnKillLstBox.SelectedItem);
            } catch (Exception) {
                // Do nothing
            }
        }

        private void AddOnStartBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog addDialog = new OpenFileDialog();
            addDialog.Title = "Select a file";
            if (addDialog.ShowDialog() == true) {
                gmOnStartLstBox.Items.Add(addDialog.FileName);
                gmLib.getGmOnStartList().Add(addDialog.FileName);
            }
        }

        private void RemoveOnStartBtn_Click(object sender, RoutedEventArgs e)
        {
            try {
                gmLib.getGmOnStartList().Remove(gmOnStartLstBox.SelectedItem.ToString());
                gmOnStartLstBox.Items.Remove(gmOnStartLstBox.SelectedItem);
            } catch (Exception) {
                // Do nothing
            }
        }

        private void AddOffStartBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog addDialog = new OpenFileDialog();
            addDialog.Title = "Select a file";
            if (addDialog.ShowDialog() == true) {
                gmOffStartLstBox.Items.Add(addDialog.FileName);
                gmLib.getGmOffStartList().Add(addDialog.FileName);
            }
        }

        private void RemoveOffStartBtn_Click(object sender, RoutedEventArgs e)
        {
            try {
                gmLib.getGmOffStartList().Remove(gmOffStartLstBox.SelectedItem.ToString());
                gmOffStartLstBox.Items.Remove(gmOffStartLstBox.SelectedItem);
            } catch (Exception) {
                // Do nothing
            }
        }

        private void AddOffKillBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog addDialog = new OpenFileDialog();
            addDialog.Title = "Select a file";
            if (addDialog.ShowDialog() == true) {
                gmOffKillLstBox.Items.Add(addDialog.FileName);
                gmLib.getGmOffKillList().Add(addDialog.FileName);
            }
        }

        private void RemoveOffKillBtn_Click(object sender, RoutedEventArgs e)
        {
            try {
                gmLib.getGmOffKillList().Remove(gmOffKillLstBox.SelectedItem.ToString());
                gmOffKillLstBox.Items.Remove(gmOffKillLstBox.SelectedItem);
            } catch (Exception) {
                // Do nothing
            }
        }
    }
}
