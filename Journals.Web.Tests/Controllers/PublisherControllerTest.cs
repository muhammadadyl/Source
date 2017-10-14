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
        IStaticMembershipService membershipService = Mock.Create<IStaticMembershipService>();
        IJournalService journalService = Mock.Create<IJournalService>();
        IMapper mapper = MappingProfile.InitializeAutoMapper().CreateMapper();

        [TestMethod]
        public void Index_Returns_All_Journals()
        {

            //Arrange
            var userMock = Mock.Create<MembershipUser>();
            Mock.Arrange(() => userMock.ProviderUserKey).Returns(1);
            Mock.Arrange(() => membershipService.GetUser()).Returns(userMock);

            Mock.Arrange(() => journalService.GetAllJournals((int)userMock.ProviderUserKey)).Returns(new List<Journal>(){
                    new Journal{ Id=1, Description="TestDesc", Title="Tester", UserId=1, ModifiedDate= DateTime.Now},
                    new Journal{ Id=1, Description="TestDesc2", Title="Tester2", UserId=1, ModifiedDate = DateTime.Now}
            }).MustBeCalled();


            //Act
            PublisherController controller = new PublisherController(journalService, membershipService, mapper);
            ViewResult actionResult = (ViewResult)controller.Index();
            var model = actionResult.Model as IEnumerable<JournalViewModel>;

            //Assert
            Assert.AreEqual(2, model.Count());
        }

        [TestMethod()]
        public void Details_return_journal()
        {
            int id = 1;
            Mock.Arrange(() => journalService.GetJournalById(id)).Returns(new Journal { Id = 1, Description = "TestDesc", Title = "Tester", UserId = 1, ModifiedDate = DateTime.Now }).MustBeCalled();

            //Act
            PublisherController controller = new PublisherController(journalService, membershipService, mapper);
            ViewResult actionResult = (ViewResult)controller.Details(id);
            var model = actionResult.Model as JournalViewModel;

            //Assert
            Assert.AreEqual(id, model.Id);

        }

        [TestMethod()]
        public void Create_return_journal()
        {

            var journalVM = new JournalViewModel { Title = "test", Description="test app" };
            var journal = mapper.Map<JournalViewModel, Journal>(journalVM);
            journal.ModifiedDate = DateTime.Now;

            var userMock = Mock.Create<MembershipUser>();
            Mock.Arrange(() => userMock.ProviderUserKey).Returns(1);
            Mock.Arrange(() => membershipService.GetUser()).Returns(userMock);


            var opStatusMock = Mock.Create<OperationStatus>();
            opStatusMock.Status = true;

            Mock.Arrange(() => journalService.AddJournal(journal)).Returns(opStatusMock).MustBeCalled();



            //Act
            PublisherController controller = new PublisherController(journalService, membershipService, mapper);
            ViewResult actionResult = (ViewResult)controller.Create(journalVM);
            var model = actionResult.Model as JournalViewModel;

            //Assert
            Assert.AreEqual(journalVM.Title, model.Title);

        }

        [TestMethod()]
        public void Delete_return_journal()
        {
            int id = 1;
            Mock.Arrange(() => journalService.GetJournalById(id)).Returns(new Journal { Id = 1, Description = "TestDesc", Title = "Tester", UserId = 1, ModifiedDate = DateTime.Now }).MustBeCalled();

            var userMock = Mock.Create<MembershipUser>();
            Mock.Arrange(() => userMock.ProviderUserKey).Returns(1);
            Mock.Arrange(() => membershipService.GetUser()).Returns(userMock);


            //Act
            PublisherController controller = new PublisherController(journalService, membershipService, mapper);
            ViewResult actionResult = (ViewResult)controller.Delete(id);
            var model = actionResult.Model as JournalViewModel;

            //Assert
            Assert.AreEqual(id, model.Id);

        }

        [TestMethod()]
        public void Delete_journal()
        {

            var userMock = Mock.Create<MembershipUser>();
            Mock.Arrange(() => userMock.ProviderUserKey).Returns(1);
            Mock.Arrange(() => membershipService.GetUser()).Returns(userMock);

            var journal = Mock.Create<JournalViewModel>();
            var journalNew = mapper.Map<JournalViewModel, Journal>(journal);

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
            int id = 1;
            Mock.Arrange(() => journalService.GetJournalById(id)).Returns(new Journal { Id = 1, Description = "TestDesc", Title = "Tester", UserId = 1, ModifiedDate = DateTime.Now }).MustBeCalled();

            var userMock = Mock.Create<MembershipUser>();
            Mock.Arrange(() => userMock.ProviderUserKey).Returns(1);
            Mock.Arrange(() => membershipService.GetUser()).Returns(userMock);


            //Act
            PublisherController controller = new PublisherController(journalService, membershipService, mapper);
            ViewResult actionResult = (ViewResult)controller.Edit(id);
            var model = actionResult.Model as JournalUpdateViewModel;

            //Assert
            Assert.AreEqual(id, model.Id);

        }

        [TestMethod()]
        public void Edit_journal()
        {
            int id = 1;
             
            var userMock = Mock.Create<MembershipUser>();
            Mock.Arrange(() => userMock.ProviderUserKey).Returns(1);
            Mock.Arrange(() => membershipService.GetUser()).Returns(userMock);

            var journal = Mock.Create<JournalUpdateViewModel>();
            var journalEdit = mapper.Map<JournalUpdateViewModel, Journal>(journal);

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