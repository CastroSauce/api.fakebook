using Microsoft.VisualStudio.TestTools.UnitTesting;
using api.fakebook.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.fakebook.Services.UserService;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace api.fakebook.Controllers.Tests
{
    [TestClass()]
    public class UserControllerTests
    {
        [TestMethod()]
        [DataRow(true, typeof(OkObjectResult))]
        [DataRow(false, typeof(BadRequestObjectResult))]
        public async Task followUser_tagetDoesntExit_ReturnBadRequest(bool followSuccess, Type expectedType)
        {
            //Arrange
            var mockUserService = GetMockedUserService();
            mockUserService.Setup(service => service.FollowUser(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>()))
                .ReturnsAsync(followSuccess);

            //Act
            var controller = GetController(mockUserService.Object);
            var result = await controller.FollowUser(Guid.NewGuid().ToString());

            //Assert
            result.Should().BeOfType(expectedType);
        }





        

        private UserController GetController(IUserService userService)
        {
            return new UserController(userService);
        }

        private Mock<IUserService> GetMockedUserService()
        {
            return new Mock<IUserService>();
        }
    }
}