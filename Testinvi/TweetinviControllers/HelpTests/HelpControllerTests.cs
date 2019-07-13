﻿using System;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.Help;
using Tweetinvi.Models;

namespace Testinvi.TweetinviControllers.HelpTests
{
    [TestClass]
    public class HelpControllerTests
    {
        private FakeClassBuilder<HelpController> _fakeBuilder;
        private Fake<IHelpQueryExecutor> _fakeHelpQueryExecutor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<HelpController>();
            _fakeHelpQueryExecutor = _fakeBuilder.GetFake<IHelpQueryExecutor>();
        }

        [TestMethod]
        public async Task  GetTokenRateLimits_ReturnsQueryExecutor()
        {
            var expectedResult = A.Fake<ICredentialsRateLimits>();

            // Arrange
            var helpController = CreateHelpControllerTests();
            ArrangeQueryExecutorGetTokenRateLimits(expectedResult);

            // Act
            var result = await helpController.GetCurrentCredentialsRateLimits();

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private void ArrangeQueryExecutorGetTokenRateLimits(ICredentialsRateLimits result)
        {
            _fakeHelpQueryExecutor
                .CallsTo(x => x.GetCurrentCredentialsRateLimits())
                .Returns(result);
        }

        [TestMethod]
        public async Task GetTwitterPrivacyPolicy_ReturnsQueryExecutor()
        {
            var expectedResult = Guid.NewGuid().ToString();

            // Arrange
            var helpController = CreateHelpControllerTests();
            ArrangeQueryExecutorGetTwitterPrivacyPolicy(expectedResult);

            // Act
            var result = await helpController.GetTwitterPrivacyPolicy();

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private void ArrangeQueryExecutorGetTwitterPrivacyPolicy(string result)
        {
            _fakeHelpQueryExecutor
                .CallsTo(x => x.GetTwitterPrivacyPolicy())
                .Returns(result);
        }

        private HelpController CreateHelpControllerTests()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}