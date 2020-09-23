using Facts.Models;
using Facts.Services.Implementations;
using System;
using System.Net.Http;
using System.Threading.Tasks;
#pragma warning disable 4014

namespace ConsoleApp2
{
    class Program
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        static async Task Main(string[] args)
        {
            Console.WriteLine($"Writing cat facts to console.");

            Task.Run(() => WriteFacts(FactSubject.Cat, 3, ConsoleColor.Red));
            Task.Run(() => WriteFacts(FactSubject.Dog, 5, ConsoleColor.Blue));
            Task.Run(() => WriteFacts(FactSubject.Horse, 10, ConsoleColor.Green));

            // Close after any key entered.
            Console.Read();
        }

        public static async Task WriteFacts(FactSubject subject, int repeatSeconds, ConsoleColor consoleColor)
        {
            AnimalFacts facts = new AnimalFacts(HttpClient);

            while (true)
            {
                var fact = await facts.GetFact(new AnimalFactQuery(subject));
                Console.ForegroundColor = consoleColor;
                Console.WriteLine(fact.ToString(), consoleColor);
                await Task.Delay(TimeSpan.FromSeconds(repeatSeconds));
            }
        }
    }
}
