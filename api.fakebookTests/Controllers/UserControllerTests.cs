using Microsoft.VisualStudio.TestTools.UnitTesting;
using api.fakebook.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.fakebook.Dto.User;
using api.fakebook.Services.UserService;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace api.fakebook.Controllers.Tests
{
    [TestClass()]
    public class UserControllerTests
    {
        [TestMethod]
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


        [TestMethod]
        [DataRow(true, typeof(OkResult))]
        [DataRow(false, typeof(BadRequestResult))]
        public async Task SendDirectMessageTest(bool userExists, Type expectedResult)
        {
            //Arrange
            var mockUserService = GetMockedUserService();
            mockUserService
                .Setup(service => service.SendDirectMessage(It.IsAny<ClaimsPrincipal>(), It.IsAny<DirectMessageDto>()))
                .ReturnsAsync(userExists);

            //Act
            var controller = GetController(mockUserService.Object);
            var result = await controller.SendDirectMessage(new DirectMessageDto());

            //Assert
            result.Should().BeOfType(expectedResult);
        }

        [TestMethod]
        [DataRow(false, typeof(OkObjectResult))]
        [DataRow(true, typeof(NoContentResult))]
        public async Task GetDirectMessage(bool returnEmptyList, Type expectedResult)
        {
            //Arrange
            var mockUserService = GetMockedUserService();
            mockUserService
                .Setup(service => service.GetDirectMessages(It.IsAny<ClaimsPrincipal>(), It.IsAny<string>()))
                .ReturnsAsync(GetDirectMessageList(returnEmptyList));

            //Act
            var controller = GetController(mockUserService.Object);
            var result = await controller.GetDirectMessages(Guid.NewGuid().ToString());

            //Assert
            result.Should().BeOfType(expectedResult);
        }





        private DirectMessageResponseDto getPost(bool returnNull = false)
        {
            if (returnNull)
            {
                return null;
            }

            return new DirectMessageResponseDto() { };
        }

        private List<DirectMessageResponseDto> GetDirectMessageList(bool empty = false)
        {
            if (empty) return new List<DirectMessageResponseDto>();

            return new List<DirectMessageResponseDto>() { getPost(), getPost(), getPost() };
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