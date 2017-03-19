using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ips2htaccess
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Podaj ścieżkę do logów HTTP");
            string path = Console.ReadLine();
            while (!File.Exists(path))
            {
                Console.WriteLine("Plik nie istnieje !");
                path = Console.ReadLine();
            }
            Console.WriteLine("Gdzie zapisać nowy plik htaccess ?");
            string dir = Console.ReadLine();
            while (!Directory.Exists(dir))
            {
                Console.WriteLine("Ścieżka nie istnieje!");
                dir = Console.ReadLine();
            }

            Console.WriteLine("Wczytywanie pliku...");
            string[] file = File.ReadAllLines(path);
            Console.WriteLine("Plik wczytany!");
            Console.WriteLine("Parsowanie pliku...");

            List<string> ips = new List<string>();
            StringBuilder builder = new StringBuilder();

            foreach (var line in file)
            {
                if (line.Contains("GET / HTTP/1.0") || line.Contains("GET / HTTP/1.1"))
                {
                    builder.Clear();
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i] == ' ') break; // odczytuj do pierwszej spacji, czyli sam adres IP
                        builder.Append(line[i]);
                    }
                    if (!ips.Contains(builder.ToString())) ips.Add(builder.ToString());
                }
            }
            Console.WriteLine("Plik sparsowany !");
            Console.WriteLine("Przepisywanie do nowego pliku...");

            StreamWriter writer = new StreamWriter(dir + "htaccess.txt");
            foreach (var line in ips) writer.WriteLine("Deny from " + line);
            writer.Close();
            Console.WriteLine("Plik przepisany!");
            Console.Beep();
            Console.ReadKey();
        }
    }
}
