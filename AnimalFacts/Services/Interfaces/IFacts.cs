using Facts.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Facts
{
    public interface IFacts
    {
        public Task<IReadOnlyList<Fact>> GetFacts(IFactQuery query);
        public Task<Fact> GetFact(IFactQuery query);
    }
}
