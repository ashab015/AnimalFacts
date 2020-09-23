using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace Facts.Services.Implementations
{
    /// <summary>
    /// A thread safe text file writer implementation.
    /// </summary>
    public class TextFileWriter : IFileWriter
    {
        private const string EVENT_HANDLE_NAME = "THREAD_SAFE";
        private readonly EventWaitHandle _waitHandle;
        private readonly string _filePath;
        private bool _inUse;

        public TextFileWriter(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            _filePath = filePath;
            _waitHandle = new EventWaitHandle(true, EventResetMode.AutoReset, EVENT_HANDLE_NAME);

            try
            {
                if (File.Exists(_filePath))
                {
                    File.Delete(_filePath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Attempted to delete existing file. {_filePath}");
            }
        }

        public virtual async Task<bool> CreateOrAppend(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            try
            {
                _inUse = _waitHandle.WaitOne();
                await AppendText(text);
                _inUse = _waitHandle.Set();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (_inUse == true)
                    _waitHandle.Set();
            }
        }

        protected virtual async Task AppendText(string text)
        {
            await File.AppendAllTextAsync(_filePath, text + Environment.NewLine);
        }

        public bool InUse { get => _inUse; }
    }
}
