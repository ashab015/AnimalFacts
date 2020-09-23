using System;
using System.Collections.Generic;
using System.Text;

namespace Facts.Models
{
    public class AnimalFactQuery : IFactQuery
    {
        public AnimalFactQuery(FactSubject subject, int amount = 1)
        {
            Subject = subject;
            Amount = amount;
        }

        public FactSubject Subject { get; }

        /// <summary>
        /// Returns true. All animals facts will be random.
        /// </summary>
        public bool Random { get => true; }
        public long Amount { get; }

        public string ToQueryUrl()
        {
            return $"/random?animal_type={Subject.ToString()}&amount={Amount}".ToLower();
        }
    }
}
