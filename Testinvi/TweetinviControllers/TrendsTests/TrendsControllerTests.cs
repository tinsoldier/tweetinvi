﻿using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.Trends;
using Tweetinvi.Models;

namespace Testinvi.TweetinviControllers.TrendsTests
{
    [TestClass]
    public class TrendsControllerTests
    {
        private FakeClassBuilder<TrendsController> _fakeBuilder;
        private Fake<ITrendsQueryExecutor> _fakeTrendsQueryExecutor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<TrendsController>();
            _fakeTrendsQueryExecutor = _fakeBuilder.GetFake<ITrendsQueryExecutor>();
        }

        [TestMethod]
        public async Task  GetPlaceTrendsAt_WithLocationId_ReturnsQueryExecutor()
        {
            // Arrange
            var controller = CreateTrendsController();
            var locationId = TestHelper.GenerateRandomLong();
            var expectedResult = A.Fake<IPlaceTrends>();

            _fakeTrendsQueryExecutor.CallsTo(x => x.GetPlaceTrendsAt(locationId)).Returns(expectedResult);

            // Act
            var result = await controller.GetPlaceTrendsAt(locationId);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public async Task GetPlaceTrendsAt_WithWoeIdLocation_ReturnsQueryExecutor()
        {
            // Arrange
            var controller = CreateTrendsController();
            var woeIdLocation = A.Fake<IWoeIdLocation>();
            var expectedResult = A.Fake<IPlaceTrends>();

            _fakeTrendsQueryExecutor.CallsTo(x => x.GetPlaceTrendsAt(woeIdLocation)).Returns(expectedResult);

            // Act
            var result = await controller.GetPlaceTrendsAt(woeIdLocation);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private TrendsController CreateTrendsController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}