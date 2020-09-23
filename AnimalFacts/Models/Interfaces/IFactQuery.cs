using System;
using System.Collections.Generic;
using System.Text;

namespace Facts.Models
{
    public interface IFactQuery
    {
        public FactSubject Subject { get; }
        public bool Random { get; }
        public long Amount { get; }
        public string ToQueryUrl();
    }
}
