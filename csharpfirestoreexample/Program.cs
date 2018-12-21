using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;

namespace bancocomposicao
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine("Gerando Document Firestore by jsons");
            
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", $"{path}{"mega-rubk-dev-firebase.json"}");
            //Console.WriteLine(Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS"));
            /*
            if (Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS") == null)
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            */
            var db = new Firestore();

            //db.ListDevelopments().Wait();
            //db.Listening1().Wait(); 
            //db.Listening2().Wait();

            string[] files = Directory.GetFiles($"{path}/json");
            foreach (string file in files)
            {
                try
                {
                    string name = Path.GetFileName(file);
                    Console.Write($"{name}  - ");
                    BenchCompositionItemSynthetic obj = JsonConvert.DeserializeObject<BenchCompositionItemSynthetic>(File.ReadAllText(file));
                    Task.Run(() => db.SetCollection("benchCompositionItems", name.Remove(name.IndexOf('.'), 5), obj)).Wait();
                } catch (Exception ex)
                {
                    Console.WriteLine($"erro {ex.Message}");
                }
            }

            Console.WriteLine("fim..."); Console.ReadKey();

        }
    }
}
