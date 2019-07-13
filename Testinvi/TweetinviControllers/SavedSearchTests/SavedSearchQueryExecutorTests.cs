﻿using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.SavedSearch;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Testinvi.TweetinviControllers.SavedSearchTests
{
    [TestClass]
    public class SavedSearchQueryExecutorTests
    {
        private FakeClassBuilder<SavedSearchQueryExecutor> _fakeBuilder;
        private Fake<ISavedSearchQueryGenerator> _fakeSavedSearchQueryGenerator;
        private Fake<ITwitterAccessor> _fakeTwitterAccessor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<SavedSearchQueryExecutor>();
            _fakeSavedSearchQueryGenerator = _fakeBuilder.GetFake<ISavedSearchQueryGenerator>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();
        }

        [TestMethod]
        public async Task GetSavedSearches_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var controller = CreateSavedSearchQueryExecutor();
            string query = TestHelper.GenerateString();
            IEnumerable<ISavedSearchDTO> expectedResult = new List<ISavedSearchDTO>();

            _fakeSavedSearchQueryGenerator.CallsTo(x => x.GetSavedSearchesQuery()).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteGETQuery(query, expectedResult);

            // Act
            var result = await controller.GetSavedSearches();

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public async Task DestroySavedSearch_WithSavedSearchObject_ReturnsTwitterAccessorResult()
        {
            // Arrange - Act
            var result1 = await DestroySavedSearch_WithSavedSearchObject(true);
            var result2 = await DestroySavedSearch_WithSavedSearchObject(false);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }

        private async Task<bool> DestroySavedSearch_WithSavedSearchObject(bool expectedResult)
        {
            // Arrange
            var controller = CreateSavedSearchQueryExecutor();
            var query = TestHelper.GenerateString();
            var savedSearch = A.Fake<ISavedSearch>();

            _fakeSavedSearchQueryGenerator.CallsTo(x => x.GetDestroySavedSearchQuery(savedSearch)).Returns(query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, expectedResult);

            // Act
            return await controller.DestroySavedSearch(savedSearch);
        }

        [TestMethod]
        public async Task DestroySavedSearch_WithSavedSearchId_ReturnsTwitterAccessorResult()
        {
            // Arrange - Act
            var result1 = await DestroySavedSearch_WithSavedSearchId(true);
            var result2 = await DestroySavedSearch_WithSavedSearchId(false);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }

        private async Task<bool> DestroySavedSearch_WithSavedSearchId(bool expectedResult)
        {
            // Arrange
            var controller = CreateSavedSearchQueryExecutor();
            var query = TestHelper.GenerateString();
            var searchId = TestHelper.GenerateRandomLong();

            _fakeSavedSearchQueryGenerator.CallsTo(x => x.GetDestroySavedSearchQuery(searchId)).Returns(query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, expectedResult);

            // Act
            return await controller.DestroySavedSearch(searchId);
        }

        private SavedSearchQueryExecutor CreateSavedSearchQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}