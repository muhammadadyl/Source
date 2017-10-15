using AutoMapper;
using Journals.Model;
using Journals.Repository;
using Journals.Web.Controllers;
using Journals.Web.ViewModels;
using Journals.Web.Mapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Telerik.JustMock;
using Journals.Service.Interfaces;
using Journals.Core.Common;

namespace Journals.Web.Tests.Controllers
{
    [TestClass]
    public class PublisherControllerTest
    {
        private IStaticMembershipService membershipService = Mock.Create<IStaticMembershipService>();
        private IJournalService journalService = Mock.Create<IJournalService>();
        private IMapper mapper = Mock.Create<IMapper>();

        [TestMethod]
        public void Index_Returns_All_Journals()
        {

            //Arrange
            var journals = new List<Journal>(){
                    new Journal{ Id=1, Description="TestDesc", Title="Tester", UserId=1, ModifiedDate= DateTime.Now},
                    new Journal{ Id=1, Description="TestDesc2", Title="Tester2", UserId=1, ModifiedDate = DateTime.Now}
            };

            var userMock = Mock.Create<MembershipUser>();
            Mock.Arrange(() => userMock.ProviderUserKey).Returns(1);
            Mock.Arrange(() => membershipService.GetUser()).Returns(userMock);

            Mock.Arrange(() => journalService.GetAllJournals((int)userMock.ProviderUserKey)).Returns(journals).MustBeCalled();

            var journalsViewModels = Mock.Create<List<JournalViewModel>>();
            Mock.Arrange(() => mapper.Map<List<Journal>, List<JournalViewModel>>(journals)).Returns(journalsViewModels);
            //Act
            PublisherController controller = new PublisherController(journalService, membershipService, mapper);
            ViewResult actionResult = (ViewResult)controller.Index();
            var model = actionResult.Model as IEnumerable<JournalViewModel>;

            //Assert
            Assert.AreEqual(journalsViewModels, model);
        }

        [TestMethod()]
        public void Details_return_journal()
        {
            var id = 0;
            var selectedJournal = Mock.Create<Journal>();
            Mock.Arrange(() => journalService.GetJournalById(id)).Returns(selectedJournal).MustBeCalled();
            var selectedJournalVm = Mock.Create<JournalViewModel>();
            Mock.Arrange(() => mapper.Map<Journal, JournalViewModel>(selectedJournal)).Returns(selectedJournalVm);


            //Act
            PublisherController controller = new PublisherController(journalService, membershipService, mapper);
            ViewResult actionResult = (ViewResult)controller.Details(id);
            var model = actionResult.Model as JournalViewModel;

            //Assert
            Assert.AreEqual(selectedJournalVm, model);

        }

        [TestMethod()]
        public void Create_return_journal()
        {
            var journalNew = Mock.Create<JournalViewModel>();
            var journal = Mock.Create<Journal>();
            Mock.Arrange(() => mapper.Map<JournalViewModel, Journal>(journalNew)).Returns(journal);

            var userMock = Mock.Create<MembershipUser>();
            Mock.Arrange(() => userMock.ProviderUserKey).Returns(1);
            Mock.Arrange(() => membershipService.GetUser()).Returns(userMock);

            var opStatusMock = Mock.Create<OperationStatus>();
            opStatusMock.Status = true;

            Mock.Arrange(() => journalService.AddJournal(journal)).Returns(opStatusMock);

            //Act
            PublisherController controller = new PublisherController(journalService, membershipService, mapper);
            RedirectToRouteResult actionResult = (RedirectToRouteResult)controller.Create(journalNew);


            //Assert
            Assert.IsNotNull(actionResult);

        }

        [TestMethod()]
        public void Delete_return_journal()
        {
            var id = 1;
            var journal = Mock.Create<Journal>();
            journal.UserId = 1;

            Mock.Arrange(() => journalService.GetJournalById(id)).Returns(journal);

            var userMock = Mock.Create<MembershipUser>();
            Mock.Arrange(() => userMock.ProviderUserKey).Returns(1);
            Mock.Arrange(() => membershipService.GetUser()).Returns(userMock);

            
            var journalNew = Mock.Create<JournalViewModel>();

            Mock.Arrange(() => mapper.Map<Journal, JournalViewModel>(journal)).Returns(journalNew);

            //Act
            PublisherController controller = new PublisherController(journalService, membershipService, mapper);
            ViewResult actionResult = (ViewResult)controller.Delete(id);
            var model = actionResult.Model as JournalViewModel;

            //Assert
            Assert.AreEqual(journalNew, model);

        }

        [TestMethod()]
        public void Delete_journal()
        {

            var userMock = Mock.Create<MembershipUser>();
            Mock.Arrange(() => userMock.ProviderUserKey).Returns(1);
            Mock.Arrange(() => membershipService.GetUser()).Returns(userMock);

            var journal = Mock.Create<JournalViewModel>();
            var journalNew = Mock.Create<Journal>();
            Mock.Arrange(()=> mapper.Map<JournalViewModel, Journal>(journal)).Returns(journalNew);

            var opStatusMock = Mock.Create<OperationStatus>();
            opStatusMock.Status = true;
            Mock.Arrange(() => journalService.DeleteJournal(journalNew)).Returns(opStatusMock);


            //Act
            PublisherController controller = new PublisherController(journalService, membershipService, mapper);
            RedirectToRouteResult actionResult = (RedirectToRouteResult)controller.Delete(journal);
            
            //Assert
            Assert.IsNotNull(actionResult);

        }

        [TestMethod()]
        public void Edit_return_journal()
        {
            var id = 1;
            var selectedJournal = new Journal { Id = 1, Description = "TestDesc", Title = "Tester", UserId = 1, ModifiedDate = DateTime.Now };
            Mock.Arrange(() => journalService.GetJournalById(id)).Returns(selectedJournal);

            var userMock = Mock.Create<MembershipUser>();
            Mock.Arrange(() => userMock.ProviderUserKey).Returns(1);
            Mock.Arrange(() => membershipService.GetUser()).Returns(userMock);

            var journalEdit = Mock.Create<JournalUpdateViewModel>();
            Mock.Arrange(() => mapper.Map<Journal, JournalUpdateViewModel>(selectedJournal)).Returns(journalEdit);

            //Act
            PublisherController controller = new PublisherController(journalService, membershipService, mapper);
            ViewResult actionResult = (ViewResult)controller.Edit(id);
            var model = actionResult.Model as JournalUpdateViewModel;

            //Assert
            Assert.AreEqual(journalEdit, model);

        }

        [TestMethod()]
        public void Edit_journal()
        {
            int id = 1;
             
            var userMock = Mock.Create<MembershipUser>();
            Mock.Arrange(() => userMock.ProviderUserKey).Returns(1);
            Mock.Arrange(() => membershipService.GetUser()).Returns(userMock);

            var journal = Mock.Create<JournalUpdateViewModel>();
            var journalEdit = Mock.Create<Journal>();
            Mock.Arrange(() => mapper.Map<JournalUpdateViewModel, Journal>(journal)).Returns(journalEdit);

            var opStatusMock = Mock.Create<OperationStatus>();
            opStatusMock.Status = true;
            Mock.Arrange(() => journalService.UpdateJournal(journalEdit)).Returns(opStatusMock);


            //Act
            PublisherController controller = new PublisherController(journalService, membershipService, mapper);
            RedirectToRouteResult actionResult = (RedirectToRouteResult)controller.Edit(id);

            //Assert
            Assert.IsNotNull(actionResult);

        }
    }
}