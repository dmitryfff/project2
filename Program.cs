using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;

namespace Application1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Application1 start");
            Run();

        }
        //The name of the directory to change 
        static string path_directory = "";
        private static void Run()
        {
            //The name of the configuration file
            string path_config = "config.ini";
            try
            {
                using (StreamReader sr = File.OpenText(path_config))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        path_directory = s;
                        Console.WriteLine("File directory: " + path_directory);
                    }
                }
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.Message);
            }
            
            
            FileSystemWatcher watcher = new FileSystemWatcher(path_directory);
            // Watch for changes in LastAccess and LastWrite times, and
            // the renaming of files or directories.
            watcher.NotifyFilter = NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.FileName
                                 | NotifyFilters.DirectoryName;

            // Only watch text files.
            watcher.Filter = "";

            // Add event handlers.
            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;
            watcher.Deleted += OnChanged;
            watcher.Renamed += OnRenamed;

            // Begin watching.
            watcher.EnableRaisingEvents = true;

            // Wait for the user to quit the program.
            Console.WriteLine("Press 'q' to quit from programm.");
            while (Console.Read() != 'q') ;

        }
        // Define the event handlers.
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            //path to the modified file
            string file_path = e.FullPath;
            //string file_name = Path.GetFileName(e.FullPath);
            Console.WriteLine("File: " + file_path + " " + e.ChangeType);
                      
            //Open file for save path modified file          
            using (StreamWriter sw = File.CreateText("file.dat"))
            {
                sw.WriteLine(file_path);
                
            }
            Console.WriteLine("Path modified file: " + file_path);
            //Start application2
            try
            {
                Process myProcess = new Process();
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.FileName = "ConsoleApp2.exe";
                myProcess.Start();
               
            }
            catch (Exception e2)
            {
                Console.WriteLine(e2.Message);
            }

        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            Console.WriteLine($"File: {e.OldFullPath} renamed to {e.FullPath}");
        }
    }
}