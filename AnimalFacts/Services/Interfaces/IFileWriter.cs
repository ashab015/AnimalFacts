using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Facts.Services.Implementations
{
    public interface IFileWriter
    {
        public Task<bool> CreateOrAppend(string text);
        public bool InUse { get; }
    }
}
