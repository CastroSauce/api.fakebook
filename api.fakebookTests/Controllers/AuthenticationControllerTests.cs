using Microsoft.VisualStudio.TestTools.UnitTesting;
using api.fakebook.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.AspNetCore.Identity;
using api.fakebook.Models.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using System.Threading;

namespace api.fakebook.Controllers.Tests
{
    [TestClass()]
    public class AuthenticationControllerTests
    {
        [TestMethod()]
        [DataRow("testname", "password123")]
        public async Task Register_UserAlreadyExist_ReturnOk(string username, string password)
        {
            //arrange
            var configurationMock = new Mock<IConfiguration>();
            var roleManagerMock = getMockedRoleManager();
            var userManagerMock = getMockedUserManager();

            userManagerMock.Setup(mock => mock.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());

            //act
            var controller = new AuthenticationController(userManagerMock.Object, roleManagerMock.Object, configurationMock.Object);
            var result = await controller.Register(new RegisterModel{Username = username, Password = password });

            //assert
            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }

        [TestMethod]
        [DataRow("testname", "password123")]
        public async Task Register_NoUserExists_ReturnOk(string username, string password)
        {
            //arrange
            var configurationMock = new Mock<IConfiguration>();
            var roleManagerMock = getMockedRoleManager();
            var userManagerMock = getMockedUserManager();

            userManagerMock.Setup(mock => mock.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);

            userManagerMock.Setup(mock => mock.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            //act
            var controller = new AuthenticationController(userManagerMock.Object, roleManagerMock.Object, configurationMock.Object);
            var result = await controller.Register(new RegisterModel { Username = username, Password = password });

            //assert
            result.Should().BeOfType(typeof(OkObjectResult));
        }


        private Mock<UserManager<ApplicationUser>> getMockedUserManager()
        {
            var userStore = new Mock<IUserStore<ApplicationUser>>();

            var identityManagerMock = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);

            return identityManagerMock;
        }

        private Mock<RoleManager<IdentityRole>> getMockedRoleManager()
        {
            var roleStoreMock = new Mock<IRoleStore<IdentityRole>>();
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(roleStoreMock.Object, null, null, null, null);

            return roleManagerMock;
        }
    }
}