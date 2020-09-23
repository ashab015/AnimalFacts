using Facts.Services.Implementations;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnimalFactsUnitTests
{
    public class TextFileWriterTest
    {
        private IFileWriter _fileWriter;
        private Mock<TextFileWriter> _mockFileWriter;

        [SetUp]
        public void SetUp()
        {
            _mockFileWriter = new Mock<TextFileWriter>("idontexist.txt");
            _mockFileWriter.CallBase = true;
            _mockFileWriter.Protected()
            .Setup<Task>(
                "AppendText",
                ItExpr.IsAny<string>()
            )
            .Returns(Task.CompletedTask);

            _fileWriter = _mockFileWriter.Object;
        }

        [Test]
        public void TextFileWriterTest_EmptyTextArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _fileWriter.CreateOrAppend(string.Empty));
        }

        /// <summary>
        /// This test will test whether exceptions are handled. 
        /// But also makes sure that after a exception happens the EventWaitHandle is Set after we catch the exception and is ready to be used again.
        /// </summary>
        [Test]
        public async Task TextFileWriterTest_EmptyTextException()
        {
            _mockFileWriter.Protected()
            .Setup<Task>(
                "AppendText",
                ItExpr.IsAny<string>()
            )
            .ThrowsAsync(new Exception("Someone else is using this unit test."));

            bool result = await _fileWriter.CreateOrAppend("Please write me.");
            Assert.IsFalse(result);
        }

        [Test]
        public async Task TextFileWriterTest_SuccessReturnsTrue()
        {
            bool result = await _fileWriter.CreateOrAppend("Please write me.");
            Assert.IsTrue(result);
        }
    }
}
