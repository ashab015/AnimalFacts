using Facts;
using Facts.Models;
using Facts.Services.Implementations;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AnimalFactsUnitTests
{
    [TestFixture]
    public class AnimalFactsTests
    {
        private IFacts _facts;
        private List<Fact> _fakeFacts;

        [SetUp]
        public void SetUp()
        {
            _fakeFacts = new List<Fact>()
            {
                new Fact() { Id = Guid.NewGuid().ToString() },
                new Fact() { Id = Guid.NewGuid().ToString() }
            };

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(_fakeFacts))
            });

            _facts = new AnimalFacts(new HttpClient(mockMessageHandler.Object));
        }

        [TestCase(FactSubject.Cat)]
        [TestCase(FactSubject.Dog)]
        [TestCase(FactSubject.Horse)]
        [Test]
        public async Task AnimalFactsTests_HandlesExceptionReturnsNull(FactSubject factSubject)
        {
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ThrowsAsync(new Exception("No cat facts for you."));

            _facts = new AnimalFacts(new HttpClient(mockMessageHandler.Object));

            var oneFact = await _facts.GetFact(new AnimalFactQuery(factSubject, 10));

            Assert.IsTrue(oneFact is null);
        }

        [TestCase(FactSubject.Cat)]
        [TestCase(FactSubject.Dog)]
        [TestCase(FactSubject.Horse)]
        [Test]
        public async Task AnimalFactsTests_FactsReturnsOne(FactSubject factSubject)
        {
            var oneFact = await _facts.GetFact(new AnimalFactQuery(factSubject, 10));

            Assert.IsTrue(oneFact is Fact);
            Assert.IsNotNull(oneFact);
            Assert.IsTrue(_fakeFacts.Any(x => x.Id == oneFact.Id));
        }

        [TestCase(FactSubject.Cat)]
        [TestCase(FactSubject.Dog)]
        [TestCase(FactSubject.Horse)]
        [Test]
        public async Task AnimalFactsTests_FactsReturnsMultipleCorrectCount(FactSubject factSubject)
        {
            var multipleFacts = await _facts.GetFacts(new AnimalFactQuery(factSubject, 10));

            Assert.IsNotNull(multipleFacts is IReadOnlyList<Fact>);
            Assert.IsTrue(multipleFacts.Count() == _fakeFacts.Count);
        }
    }
}