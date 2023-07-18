using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw1_oop_systex
{
    internal class MonitorFolder
    {
        FileSystemWatcher fileSystemWatcher;
        public MonitorFolder(string path)
        {
            fileSystemWatcher = new FileSystemWatcher(path)
            {
                NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size
            };
        }
        public void MonitorFolder_Start() 
        {

            fileSystemWatcher.Changed += OnChanged;
            fileSystemWatcher.Created += OnCreated;
            fileSystemWatcher.Deleted += OnDeleted;
            fileSystemWatcher.Renamed += OnRenamed;
            fileSystemWatcher.Error += OnError;


            fileSystemWatcher.Filter = "T30.txt";
            fileSystemWatcher.IncludeSubdirectories = true;
            fileSystemWatcher.EnableRaisingEvents = true;

        }
        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            Console.WriteLine($"Changed: {e.FullPath}");
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            MessageBox.Show($"Created: {e.FullPath}");
        }

        private static void OnDeleted(object sender, FileSystemEventArgs e) =>
            MessageBox.Show($"Deleted: {e.FullPath}");

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            MessageBox.Show($"Renamed:\n    " +
                            $"Old: {e.OldFullPath}\n" +
                            $"    New: {e.FullPath}");
        }

        private static void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private static void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }
    }
}
