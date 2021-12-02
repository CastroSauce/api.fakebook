using Microsoft.VisualStudio.TestTools.UnitTesting;
using api.fakebook.Services.AuthService;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using api.fakebook.Models;
using api.fakebook.Models.Authentication;
using api.fakebook.Services.UserService;
using api.fakebookTests.helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Claims;
using Moq;

namespace api.fakebook.Services.AuthService.Tests
{
    [TestClass()]
    public class AuthServiceTests
    {
        [TestMethod()]
        public void GenerateJwtTokenTest()
        {
            //Arrange
            var (mockIConfig, mockUserService) = GetMockedDepedencies();

            mockIConfig.Setup(conf => conf[It.Is<string>(settings => settings == "JWT:Secret")])
                .Returns(Helper.RandomString(32));

            var randomUser = GetNewUser();
            var randomRoles = Helper.GetRandomRolesList();

            //Act
            var service = GetAuthService(mockIConfig.Object, mockUserService.Object);
            var resultToken = service.GenerateJwtToken(randomUser, randomRoles);

            var (username, Id, roles) = GetNameAndIdAndRole(resultToken);

            //Assert
            username.Should().Match(randomUser.UserName);
            Id.Should().Match(randomUser.Id);
            roles.Should().BeEquivalentTo(randomRoles);
        }

        [TestMethod()]
        [DataRow("testname", "password123")]
        public async Task Register_UserAlreadyExist_ReturnBad(string username, string password)
        {
            //arrange
            var (mockIConfig, mockUserService) = GetMockedDepedencies();
            Helper.SetupCheckIfUserExistsByUsername(mockUserService);
            //act
            var service = GetAuthService(mockIConfig.Object, mockUserService.Object);
            var result = await service.Register(GetRandomRegister());
            //assert
            result.Should().BeOfType(typeof(RegisterResponse));
            result.status.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task Register_NoUserExists_ReturnOk()
        {
            //arrange
            var (mockIConfig, mockUserService) = GetMockedDepedencies();
            Helper.SetupCheckIfUserExistsByUsername(mockUserService, false);
            mockUserService.Setup(service => service.CreateUserAsync(It.IsAny<RegisterModel>())).ReturnsAsync(IdentityResult.Success);
            //act
            var service = GetAuthService(mockIConfig.Object, mockUserService.Object);
            var result = await service.Register(GetRandomRegister());
            //assert
            result.Should().BeOfType(typeof(RegisterResponse));
            result.status.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task Authenticate_WrongUsername_ReturnNull()
        {
            //Arrange
            var (mockIConfig, mockUserService) = GetMockedDepedencies();
            Helper.SetupFindUserByUsername(mockUserService, false);
            //Act
            var service = GetAuthService(mockIConfig.Object, mockUserService.Object);
            var result = await service.Authenticate(Helper.GetRandomLogin());
            //Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public async Task Authenticate_WrongPassword_ReturnNull()
        {
            //Arrange
            var (mockIConfig, mockUserService) = GetMockedDepedencies();
            Helper.SetupFindUserByUsername(mockUserService);
            Helper.SetupCheckPassword(mockUserService, false);
            //Act
            var service = GetAuthService(mockIConfig.Object, mockUserService.Object);
            var result = await service.Authenticate(Helper.GetRandomLogin());
            //Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public async Task Authenticate_correctCredentials_LoginResponse()
        {
            //Arrange
            var (mockIConfig, mockUserService) = GetMockedDepedencies();

            Helper.SetupFindUserByUsername(mockUserService);
            Helper.SetupCheckPassword(mockUserService, true);

            mockIConfig.Setup(conf => conf[It.Is<string>(settings => settings == "JWT:Secret")])
                .Returns(Helper.RandomString(32));

            mockUserService.Setup(service => service.GetUserRoles(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(Helper.GetRandomRolesList());
            //Act
            var service = GetAuthService(mockIConfig.Object, mockUserService.Object);
            var result = await service.Authenticate(Helper.GetRandomLogin());
            //Assert
            result.Should().BeOfType(typeof(LoginResponse));
        }


        private AuthService GetAuthService(IConfiguration configuration, IUserService userService)
        {
            return new AuthService(configuration, userService);
        }

        private Mock<IUserService> GetMockUserService()
        {
            return new Mock<IUserService>();
        }

        private Mock<IConfiguration> GetMockConfig()
        {
            return new Mock<IConfiguration>();
        }

        private (Mock<IConfiguration>, Mock<IUserService>) GetMockedDepedencies()
        {
            return (GetMockConfig(), GetMockUserService());
        }


        private RegisterModel GetRandomRegister()
        {
            return new RegisterModel() { Username = Helper.RandomString(8), Password = Helper.RandomString(8), Email = Helper.RandomString(8) };
        }

        private ApplicationUser GetNewUser()
        {
            return new ApplicationUser() { UserName = Helper.RandomString(7), Id = Guid.NewGuid().ToString() };
        }

        private (string, string, List<string>) GetNameAndIdAndRole(JwtSecurityToken token)
        {
            var username = token.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
            var Id = token.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            var Roles = token.Claims.Where(claim => claim.Type == ClaimTypes.Role).Select(role => role.Value).ToList();

            return (username, Id, Roles);
        }





    }
}