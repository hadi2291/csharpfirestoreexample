using System;

namespace bancocomposicao
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine(path);

            if (Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS") == null)
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            var db = new Firestore();
            //db.ListDevelopments().Wait();
            //db.Listening1().Wait(); 
            db.Listening2().Wait();

            Console.WriteLine("fim..."); Console.ReadKey();

        }
    }
}
