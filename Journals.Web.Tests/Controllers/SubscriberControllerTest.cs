using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Journals.Service.Interfaces;
using AutoMapper;
using Journals.Web.Mapper;
using Telerik.JustMock;
using System.Collections.Generic;
using Journals.Model;
using System.Web.Security;
using Journals.Web.Controllers;
using System.Web.Mvc;
using Journals.Web.ViewModels;
using System.Linq;
using Journals.Core.Common;

namespace Journals.Web.Tests.Controllers
{
    [TestClass]
    public class SubscriberControllerTest
    {
        private IStaticMembershipService membershipService = Mock.Create<IStaticMembershipService>();
        private IJournalService journalService = Mock.Create<IJournalService>();
        private IIssueService issueService = Mock.Create<IIssueService>();
        private ISubscriptionService subscriptionService = Mock.Create<ISubscriptionService>();
        private IMapper mapper = Mock.Create<IMapper>();

        [TestMethod]
        public void Index_Returns_All_Subscriber()
        {
            // Arrange
            var userMock = Mock.Create<MembershipUser>();
            Mock.Arrange(() => userMock.ProviderUserKey).Returns(1);
            Mock.Arrange(() => membershipService.GetUser()).Returns(userMock);

            Mock.Arrange(() => subscriptionService.GetAllJournals()).Returns(new List<Journal>(){
                    new Journal{ Id=1, Description="TestDesc", Title="Tester", UserId=1, ModifiedDate= DateTime.Now},
                    new Journal{ Id=2, Description="TestDesc2", Title="Tester2", UserId=1, ModifiedDate = DateTime.Now}
            });

            //Act
            SubscriberController controller = new SubscriberController(journalService, membershipService, issueService, subscriptionService, mapper);
            ViewResult actionResult = (ViewResult)controller.Index();
            var model = actionResult.Model as IEnumerable<SubscriptionViewModel>;

            //Assert
            Assert.AreEqual(2, model.Count());
        }

        [TestMethod]
        public void Subscribe()
        {
            var userMock = Mock.Create<MembershipUser>();
            Mock.Arrange(() => userMock.ProviderUserKey).Returns(1);
            Mock.Arrange(() => membershipService.GetUser()).Returns(userMock);

            var opStatusMock = Mock.Create<OperationStatus>();
            opStatusMock.Status = true;

            Mock.Arrange(() => subscriptionService.AddSubscription(1, 1)).Returns(opStatusMock);

            //Act
            SubscriberController controller = new SubscriberController(journalService, membershipService, issueService, subscriptionService, mapper);
            RedirectToRouteResult actionResult = (RedirectToRouteResult)controller.Subscribe(1);

            //Assert
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void UnSubscribe()
        {
            var userMock = Mock.Create<MembershipUser>();
            Mock.Arrange(() => userMock.ProviderUserKey).Returns(1);
            Mock.Arrange(() => membershipService.GetUser()).Returns(userMock);

            var opStatusMock = Mock.Create<OperationStatus>();
            opStatusMock.Status = true;

            Mock.Arrange(() => subscriptionService.UnSubscribe(1, 1)).Returns(opStatusMock);

            //Act
            SubscriberController controller = new SubscriberController(journalService, membershipService, issueService, subscriptionService, mapper);
            RedirectToRouteResult actionResult = (RedirectToRouteResult)controller.UnSubscribe(1);

            //Assert
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void GetJournal_return_file()
        {
            var userMock = Mock.Create<MembershipUser>();
            Mock.Arrange(() => userMock.ProviderUserKey).Returns(1);
            Mock.Arrange(() => membershipService.GetUser()).Returns(userMock);

            Mock.Arrange(() => subscriptionService.GetJournalsForSubscriber(1)).Returns(new List<Subscription> {
                new Subscription { Id = 1, JournalId=1, UserId=1 },
                new Subscription { Id = 2, JournalId=2, UserId=1 }
            });

            Mock.Arrange(() => issueService.GetIssueById(1)).Returns(new Issue { Id = 1, Version = 1, FileName = "something", JournalId = 1, ContentType = "pdf", Content = new byte[1000] });

            //Act
            SubscriberController controller = new SubscriberController(journalService, membershipService, issueService, subscriptionService, mapper);
            FileContentResult actionResult = (FileContentResult)controller.GetJournal(1);

            //Assert
            Assert.IsNotNull(actionResult);
        }
    }
}
