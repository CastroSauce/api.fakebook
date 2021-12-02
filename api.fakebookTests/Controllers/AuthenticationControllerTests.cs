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
using api.fakebook.Services.AuthService;
using api.fakebook.Services.UserService;
using api.fakebookTests.helpers;

namespace api.fakebook.Controllers.Tests
{
    [TestClass()]
    public class AuthenticationControllerTests
    {
        [TestMethod()]
        [DataRow("testname", "password123")]
        public async Task Register_UserAlreadyExist_ReturnBad(string username, string password)
        {
            //arrange
            var ( mockAuthService,  mockUserService) = GetMockedClasses();
            Helper.SetupFindUserByUsername(mockUserService);
            //act
            var controller = new AuthenticationController(mockAuthService.Object, mockUserService.Object);
            var result = await controller.Register(GetRandomRegister());
            //assert
            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Register_NoUserExists_ReturnOk()
        {
            //arrange
            var ( mockAuthService,  mockUserService) = GetMockedClasses();
            Helper.SetupFindUserByUsername(mockUserService, false);
            mockUserService.Setup(service => service.CreateUserAsync(It.IsAny<RegisterModel>())).ReturnsAsync(IdentityResult.Success);
            //act
            var controller = GetController(mockAuthService, mockUserService);
            var result = await controller.Register(GetRandomRegister());
            //assert
            result.Should().BeOfType(typeof(OkObjectResult));
        }




        private (Mock<IAuthService>, Mock<IUserService>) GetMockedClasses()
        {
            return (new Mock<IAuthService>(), new Mock<IUserService>());
        }


        private RegisterModel GetRandomRegister()
        {
            return new RegisterModel() { Username = Helper.RandomString(8), Password = Helper.RandomString(8), Email = Helper.RandomString(8)};
        }

        private AuthenticationController GetController(Mock<IAuthService> authService, Mock<IUserService> userService)
        {
            return new AuthenticationController(authService.Object, userService.Object);
        }
    }
}