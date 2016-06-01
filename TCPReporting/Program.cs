using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using TCPReporting.Properties;

namespace TCPReporting
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // initialize the source and target folders
            InitFolders();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoadReports());
        }
        
        /// <summary>
        /// Initialize document folders
        /// </summary>
        private static void InitFolders()
        {
            CreateFolderIfNotExist(Settings.Default.SourceFolder);
            CreateFolderIfNotExist(Settings.Default.TargetFolder);
        }

        /// <summary>
        /// Create a folder if it doesn't already exist
        /// </summary>
        /// <param name="folderPath">The folder path to create</param>
        private static void CreateFolderIfNotExist(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                return;
            }

            Directory.CreateDirectory(folderPath);
        }
    }
}
