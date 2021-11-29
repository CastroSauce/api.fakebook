using Microsoft.VisualStudio.TestTools.UnitTesting;
using api.fakebook.Services.UserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using api.fakebook.Dto.User;
using api.fakebook.Models;
using api.fakebook.Models.Authentication;
using api.fakebook.Models.UserModels;
using api.fakebookTests.helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;

namespace api.fakebook.Services.UserService.Tests
{
    [TestClass()]
    public class UserServiceTests
    {

        //TODO create wrap dbcontext in repository so services are testable

        //[TestMethod()]
        //public async Task SendDirectMessage_targetDoesntExist_returnFalse()
        //{
        //    //Arrange
        //    var mockApplicationDbContext = GetMockApplicationDbContext();
        //    var mockUserManager = GetMockUserManager();
        //    setupFindUserById(mockUserManager, false);
        //    var message = new DirectMessageDto() {targetUserId = Guid.NewGuid().ToString()};
        //    //Act
        //    var service = GetService(mockUserManager.Object, mockApplicationDbContext.Object);
        //    var result = await service.SendDirectMessage(Helper.GetRandomUser(), message);
        //    //Assert
        //    result.Should().BeFalse();
        //}



        //private UserService GetService(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        //{
        //    return new UserService(userManager, dbContext);
        //}

        //private static Mock<UserManager<ApplicationUser>> setupFindUserById(Mock<UserManager<ApplicationUser>> MockUserManager, bool returnUser = true)
        //{
        //    MockUserManager.Setup(mock => mock.FindByIdAsync(It.IsAny<string>()))
        //        .ReturnsAsync(returnUser ? new ApplicationUser() : (ApplicationUser)null);

        //    return MockUserManager;
        //}

        //private Mock<UserManager<ApplicationUser>> GetMockUserManager()
        //{
        //    var store = new Mock<IUserStore<ApplicationUser>>();
        //    return new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
        //}

        //private ApplicationDbContext GetMockApplicationDbContext()
        //{

        //    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        //        .UseInMemoryDatabse;

        //    return new ApplicationDbContext(options);
        //}
    }
}