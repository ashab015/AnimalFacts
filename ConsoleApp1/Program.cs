using Facts.Models;
using Facts.Services.Implementations;
using System;
using System.Net.Http;
using System.Threading.Tasks;
#pragma warning disable 4014

namespace ConsoleApp1
{
    class Program
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private static readonly string WRITEFILE = "catfacts.txt";

        static async Task Main(string[] args)
        {
            Console.WriteLine($"Writing cat facts to {WRITEFILE}");

            Task.Run(() => WriteFacts());

            // Close after any key entered.
            Console.Read();
        }

        public static async Task WriteFacts()
        {
            AnimalFacts facts = new AnimalFacts(HttpClient);
            TextFileWriter writer = new TextFileWriter(WRITEFILE);

            while (true)
            {
                var fact = await facts.GetFact(new AnimalFactQuery(FactSubject.Cat));
                bool successfullyWritten = await writer.CreateOrAppend(fact.ToString());
                Console.WriteLine($"Fact {fact.Id} written to file. Successful: {successfullyWritten}");
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }
    }
}
