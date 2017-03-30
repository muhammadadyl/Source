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

namespace Journals.Web.Tests.Controllers
{
    [TestClass]
    public class PublisherControllerTest
    {
        [TestMethod]
        public void Index_Returns_All_Journals()
        {

            //Arrange
            var membershipRepository = Mock.Create<IStaticMembershipService>();
            var userMock = Mock.Create<MembershipUser>();
            Mock.Arrange(() => userMock.ProviderUserKey).Returns(1);
            Mock.Arrange(() => membershipRepository.GetUser()).Returns(userMock);

            var journalRepository = Mock.Create<IJournalRepository>();
            Mock.Arrange(() => journalRepository.GetAllJournals((int)userMock.ProviderUserKey)).Returns(new List<Journal>(){
                    new Journal{ Id=1, Description="TestDesc", FileName="TestFilename.pdf", Title="Tester", UserId=1, ModifiedDate= DateTime.Now},
                    new Journal{ Id=1, Description="TestDesc2", FileName="TestFilename2.pdf", Title="Tester2", UserId=1, ModifiedDate = DateTime.Now}
            }).MustBeCalled();

            //Mapper instance for Injection
            var mapper = MappingProfile.InitializeAutoMapper().CreateMapper();

            //Act
            PublisherController controller = new PublisherController(journalRepository, membershipRepository, mapper);
            ViewResult actionResult = (ViewResult)controller.Index();
            var model = actionResult.Model as IEnumerable<JournalViewModel>;

            //Assert
            Assert.AreEqual(2, model.Count());
        }
    }
}