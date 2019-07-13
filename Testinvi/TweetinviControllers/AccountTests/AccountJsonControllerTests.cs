﻿using System;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Account;
using Tweetinvi.Core.Web;

namespace Testinvi.TweetinviControllers.AccountTests
{
    [TestClass]
    public class AccountJsonControllerTests
    {
        private FakeClassBuilder<AccountJsonController> _fakeBuilder;
        private Fake<ITwitterAccessor> _fakeTwitterAccessor;
        private Fake<IAccountQueryGenerator> _fakeAccountQueryGenerator;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<AccountJsonController>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();
            _fakeAccountQueryGenerator = _fakeBuilder.GetFake<IAccountQueryGenerator>();
        }

        [TestMethod]
        public async Task GetAuthenticatedUserSettingsJson_ReturnsAccessorJsonResult()
        {
            string query = Guid.NewGuid().ToString();
            string jsonResult = Guid.NewGuid().ToString();

            // Arrange
            var controller = CreateAccountJsonController();

            ArrangeGetAuthenticatedUserAccountSettingsQuery(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonGETQuery(query, jsonResult);

            // Act
            var result = await controller.GetAuthenticatedUserSettingsJson();

            // Assert
            Assert.AreEqual(result, jsonResult);
        }

        private void ArrangeGetAuthenticatedUserAccountSettingsQuery(string query)
        {
            _fakeAccountQueryGenerator
                .CallsTo(x => x.GetAuthenticatedUserAccountSettingsQuery())
                .Returns(query);
        }

        private AccountJsonController CreateAccountJsonController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}