﻿using Newtonsoft.Json;
using SubitoHelper_ConsoleApp.Model;
using SubitoNotifier.Controllers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SubitoHelper_ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                SubitoSettings settings;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "settings.txt");
                if (File.Exists(path))
                    settings = loadSettings(path);
                else
                    settings = createSettings(path);

                await JobSelection(settings);
            }).GetAwaiter().GetResult();
        }

        private static SubitoSettings loadSettings(string path)
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<SubitoSettings>(json);

        }

        private static SubitoSettings createSettings(string path)
        {
            File.Create(path).Dispose();
            return editSettings(path);
        }

        private static SubitoSettings editSettings(string path)
        {
            using (var tw = new StreamWriter(path))
            {
                SubitoSettings settings = new SubitoSettings();
                Console.WriteLine("insert your subito username");
                settings.username = Console.ReadLine();
                Console.WriteLine("insert your subito password");
                settings.password = Console.ReadLine();
                Console.WriteLine("insert your subito chatToken");
                settings.chatToken = Console.ReadLine();
                Console.WriteLine("insert your subito botToken");
                settings.botToken = Console.ReadLine();
                Console.WriteLine("insert your sqlConnectionString ");
                settings.SqlConnectionString = Console.ReadLine();
                Console.WriteLine("insert your pastebin json file ID");
                settings.idPastebin = Console.ReadLine();
                string json = JsonConvert.SerializeObject(settings);

                tw.WriteLine(json);
                tw.Close();
                return settings;
            }
        }

        private static async Task JobSelection(SubitoSettings settings)
        {
            SubitoController subitoController = new SubitoController();
            int choice = 0;
            string result = string.Empty;
            while (choice != 9)
            {
                Console.WriteLine("1. Delete all Insertions");
                Console.WriteLine("2. Reinsert all Insertions saved on the bitbucket File ");
                Console.WriteLine("4. edit the preferences");
                Console.WriteLine("9. close");
                choice = 0;
                while (choice == 0)
                    int.TryParse(Console.ReadLine(), out choice);
                if (confirmselection()) { 
                    Console.WriteLine();
                    switch (choice)
                    {
                        case 1:
                            result = await subitoController.GetDeleteAll(settings.username, settings.password);
                            Console.WriteLine(result);
                            break;

                        case 2:
                            result = await subitoController.GetReinsertAll(settings.username, settings.password, settings.idPastebin);
                            Console.WriteLine(result);
                            break;

                        case 4:
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "settings.txt");
                            settings = editSettings(path);
                            break;

                        case 9:
                            break;
                    }
                    Console.WriteLine();
                }
            }
        }

        private static bool confirmselection()
        {
            Console.WriteLine();
            Console.WriteLine("are you sure? y/n");
            while (true)
            {
                string result = Console.ReadLine();
                if (result == "y")
                    return true;
                else if (result == "n")
                    return false;
            }
        }
    }
}
