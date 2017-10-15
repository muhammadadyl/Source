using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Journals.Service.Interfaces;
using AutoMapper;
using Telerik.JustMock;
using Journals.Web.Mapper;
using Journals.Web.Controllers;
using System.Web.Mvc;
using Journals.Web.ViewModels;
using Journals.Model;
using System.Web.Security;
using Journals.Web.Helpers;
using System.Web;
using Journals.Core.Common;

namespace Journals.Web.Tests.Controllers
{
    [TestClass]
    public class IssueControllerTest
    {
        private IStaticMembershipService membershipService = Mock.Create<IStaticMembershipService>();
        private IJournalService journalService = Mock.Create<IJournalService>();
        private IIssueService issueService = Mock.Create<IIssueService>();
        private ISubscriptionService subscriptionService = Mock.Create<ISubscriptionService>();
        private IMapper mapper = Mock.Create<IMapper>();

        [TestMethod]
        public void Create_return_issue()
        {
            IssueController controller = new IssueController(issueService, journalService, membershipService, mapper);
            ViewResult actionResult = (ViewResult)controller.Create(1);
            var model = actionResult.Model as IssueViewModel;

            Assert.IsNotNull(model);
        }

        [TestMethod]
        public void Create()
        {

            // Arrange
            Mock.Arrange(() => journalService.GetJournalById(1)).Returns(new Journal { Id = 1, Title = "test", Description = "description of tesrt" });
            var userMock = Mock.Create<MembershipUser>();
            Mock.Arrange(() => userMock.ProviderUserKey).Returns(1);
            Mock.Arrange(() => membershipService.GetUser()).Returns(userMock);
            Issue issue = new Issue { Version = 1, JournalId = 1, Id = 1, FileName = "myfile.pdf" };
            Mock.Arrange(() => issueService.AddIssue(issue)).Returns(new OperationStatus() { Status = true });

            IssueController controller = new IssueController(issueService, journalService, membershipService, mapper);
            ViewResult actionResult = (ViewResult)controller.Create(1);

            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void GetFile_return_file()
        {
            Mock.Arrange(() => issueService.GetIssueById(1)).Returns(new Issue { Id = 1, Version = 1, FileName = "something", JournalId = 1, ContentType = "pdf", Content = new byte[1000] });

            //Act
            IssueController controller = new IssueController(issueService, journalService, membershipService, mapper);
            FileContentResult actionResult = (FileContentResult)controller.GetFile(1);

            //Assert
            Assert.IsNotNull(actionResult);
        }

        [TestMethod()]
        public void Edit_return_journal()
        {
            var selectedIssue = new Issue { Id = 1, Version = 1, FileName = "something", JournalId = 1, ContentType = "pdf", Content = new byte[1000], Journal = new Journal { Id = 1, UserId = 1, Title = "test", Description = "details goes here" } };
            Mock.Arrange(() => issueService.GetIssueById(1)).Returns(selectedIssue);

            var userMock = Mock.Create<MembershipUser>();
            Mock.Arrange(() => userMock.ProviderUserKey).Returns(1);
            Mock.Arrange(() => membershipService.GetUser()).Returns(userMock);

            var newIssue = Mock.Create<IssueViewModel>();
            Mock.Arrange(() => mapper.Map<Issue, IssueViewModel>(selectedIssue)).Returns(newIssue);

            //Act
            IssueController controller = new IssueController(issueService, journalService, membershipService, mapper);
            ViewResult actionResult = (ViewResult)controller.Edit(1);
            var model = actionResult.Model as IssueViewModel;

            //Assert
            Assert.AreEqual(newIssue, model);
        }

        [TestMethod()]
        public void Edit()
        {
            var issue = new Issue { Id = 1, Version = 1, FileName = "something", JournalId = 1, ContentType = "pdf", Content = new byte[1000], Journal = new Journal { Id = 1, UserId = 1, Title = "test", Description = "details goes here" } };
            Mock.Arrange(() => issueService.GetIssueById(1)).Returns(issue);

            var userMock = Mock.Create<MembershipUser>();
            Mock.Arrange(() => userMock.ProviderUserKey).Returns(1);
            Mock.Arrange(() => membershipService.GetUser()).Returns(userMock);
            Mock.Arrange(() => issueService.UpdateIssue(issue)).Returns(new OperationStatus { Status = true });

            //Act
            IssueController controller = new IssueController(issueService, journalService, membershipService, mapper);
            RedirectToRouteResult actionResult = (RedirectToRouteResult)controller.Edit(1, new IssueViewModel { Id = 1, JournalId = 1, Version = 1 });

            //Assert
            Assert.IsNotNull(actionResult);

        }

        [TestMethod()]
        public void Delete_return_issue()
        {
            var selectedIssue = new Issue { Id = 1, Version = 1, FileName = "something", JournalId = 1, ContentType = "pdf", Content = new byte[1000], Journal = new Journal { Id = 1, UserId = 1, Title = "test", Description = "details goes here" } };
            Mock.Arrange(() => issueService.GetIssueById(1)).Returns(selectedIssue);

            var userMock = Mock.Create<MembershipUser>();
            Mock.Arrange(() => userMock.ProviderUserKey).Returns(1);
            Mock.Arrange(() => membershipService.GetUser()).Returns(userMock);

            var newIssue = Mock.Create<IssueViewModel>();
            Mock.Arrange(() => mapper.Map<Issue, IssueViewModel> (selectedIssue)).Returns(newIssue);
            newIssue.Id = 1;

            //Act
            IssueController controller = new IssueController(issueService, journalService, membershipService, mapper);
            ViewResult actionResult = (ViewResult)controller.Delete(1);
            var model = actionResult.Model as IssueViewModel;

            //Assert
            Assert.AreEqual(newIssue, model);
        }

        [TestMethod()]
        public void Delete()
        {
            var issue = Mock.Create<Issue>();
            var journal = Mock.Create<Journal>();
            Mock.Arrange(() => issueService.GetIssueById(1)).Returns(issue);
            issue.JournalId = 1;
            Mock.Arrange(() => journalService.GetJournalById(1)).Returns(journal);
            journal.UserId = 1;

            var userMock = Mock.Create<MembershipUser>();
            Mock.Arrange(() => userMock.ProviderUserKey).Returns(1);
            Mock.Arrange(() => membershipService.GetUser()).Returns(userMock);

            var newIssue = Mock.Create<IssueViewModel>();
            Mock.Arrange(() => mapper.Map<IssueViewModel, Issue>(newIssue)).Returns(issue);

            var opStatusMock = Mock.Create<OperationStatus>();
            opStatusMock.Status = true;
            Mock.Arrange(() => issueService.DeleteIssue(issue)).Returns(opStatusMock);

            //Act
            IssueController controller = new IssueController(issueService, journalService, membershipService, mapper);
            RedirectToRouteResult actionResult = (RedirectToRouteResult)controller.Delete(1, newIssue);

            //Assert
            Assert.IsNotNull(actionResult);
        }
    }
}
