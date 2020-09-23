using Facts.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Facts.Services.Implementations
{
    public class AnimalFacts : IFacts
    {
        private readonly HttpClient _httpClient;
        private const string ANIMAL_FACTS_URL = "https://cat-fact.herokuapp.com/facts";

        public AnimalFacts(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyList<Fact>> GetFacts(IFactQuery query)
        {
            try
            {
                var qeuryUrl = query.ToQueryUrl();
                var responseMessage = await _httpClient.GetAsync($"{ANIMAL_FACTS_URL}{qeuryUrl}");
                var content = await responseMessage.Content.ReadAsStringAsync();

                if (query.Amount == 1)
                {
                    var fact = JsonConvert.DeserializeObject<Fact>(content);
                    return new List<Fact>() { fact };
                }
                else
                {
                    return JsonConvert.DeserializeObject<List<Fact>>(content);
                }
            }
            catch (Exception ex)
            {
                return default(List<Fact>);
            }
        }

        public async Task<Fact> GetFact(IFactQuery query)
        {
            return (await GetFacts(query))?.FirstOrDefault();
        }
    }
}
