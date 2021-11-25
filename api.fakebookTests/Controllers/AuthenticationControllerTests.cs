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
            (var MockAuthService, var MockUserService) = GetMockedClasses();
            MockUserService.Setup(service => service.FindUserByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            //act
            var controller = new AuthenticationController(MockAuthService.Object, MockUserService.Object);
            var result = await controller.Register(new RegisterModel{Username = username, Password = password });
            //assert
            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }

        [TestMethod]
        [DataRow("testname", "password123")]
        public async Task Register_NoUserExists_ReturnOk(string username, string password)
        {
            //arrange
            (var MockAuthService, var MockUserService) = GetMockedClasses();
            MockUserService.Setup(service => service.FindUserByNameAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);
            MockUserService.Setup(service => service.CreateUserAsync(It.IsAny<ApplicationUser>() ,It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            //act
            var controller = GetController(MockAuthService, MockUserService);
            var result = await controller.Register(new RegisterModel { Username = username, Password = password });
            //assert
            result.Should().BeOfType(typeof(OkObjectResult));
        }


        [TestMethod]
        [DataRow("testest")]
        public async Task Login_WrongUsername_ReturnUnauthorized(string username)
        {
            //Arrange
            (var MockAuthService, var MockUserService) = GetMockedClasses();
            MockUserService.Setup(service => service.FindUserByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            //Act
            var controller = GetController(MockAuthService, MockUserService);
            var result = await controller.Login(new LoginModel { Username = username, Password = "" });
            //Assert
            result.Should().BeOfType(typeof(UnauthorizedResult));
        }

        [TestMethod]
        [DataRow("testest", "test123")]
        public async Task Login_WrongPassword_ReturnUnauthorized(string username, string password)
        {
            //Arrange
            (var MockAuthService, var MockUserService) = GetMockedClasses();
            MockUserService.Setup(service => service.FindUserByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            MockUserService.Setup(service => service.CheckUserPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(false);
            //Act
            var controller = GetController(MockAuthService, MockUserService);
            var result = await controller.Login(new LoginModel { Username = username, Password = "" });
            //Assert
            result.Should().BeOfType(typeof(UnauthorizedResult));
        }

        [TestMethod]
        [DataRow("testest", "test123")]
        public async Task Login_correctCredentials_ReturnOk(string username, string password)
        {
            //Arrange
            (var MockAuthService, var MockUserService) = GetMockedClasses();
            MockUserService.Setup(service => service.FindUserByNameAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            MockUserService.Setup(service => service.CheckUserPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(true);
            MockAuthService.Setup(service => service.GenerateJwtToken(It.IsAny<ApplicationUser>(), It.IsAny<IList<string>>())).Returns(new System.IdentityModel.Tokens.Jwt.JwtSecurityToken());
            //Act
            var controller = GetController(MockAuthService, MockUserService);
            var result = await controller.Login(new LoginModel { Username = username, Password = password });
            //Assert
            result.Should().BeOfType(typeof(OkObjectResult));
        }



        private (Mock<IAuthService>, Mock<IUserService>) GetMockedClasses()
        {
            return (new Mock<IAuthService>(), new Mock<IUserService>());
        }

        private AuthenticationController GetController(Mock<IAuthService> authService, Mock<IUserService> userService)
        {
            return new AuthenticationController(authService.Object, userService.Object);
        }
    }
}