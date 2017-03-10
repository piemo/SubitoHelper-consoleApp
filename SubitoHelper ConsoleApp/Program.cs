using Newtonsoft.Json;
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
            Console.WriteLine("1. Delete all Insertions");
            Console.WriteLine("2. Reinsert all Insertions saved on the bitbucket File ");
            int choice = 0;
            while ( choice == 0)
            {
                int.TryParse(Console.ReadLine(), out choice);
            }
            string result = string.Empty;
            switch (choice)
            {
                case 1:
                    result = await subitoController.GetDeleteAll(settings.username,settings.password);
                    break;

                case 2:
                    result = await subitoController.GetReinsertAll(settings.username, settings.password,settings.idPastebin);
                    break;
            }
            Console.WriteLine(result);
            Console.ReadLine();
            
        }
    }
}
