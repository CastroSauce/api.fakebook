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
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
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
            var MockIConfig = new Mock<IConfiguration>();
            MockIConfig.Setup(conf => conf[It.Is<string>(settings => settings == "JWT:Secret")])
                .Returns(RandomString(32));

            var randomUser = GetNewUser();
            var randomRoles = GetRandomRolesList();

            //Act
            var AuthService = GetAuthService(MockIConfig.Object);
            var resultToken = AuthService.GenerateJwtToken(randomUser, randomRoles);

            var ( username,  Id, roles) = GetNameAndIdAndRole(resultToken);

            //Assert
            username.Should().Match(randomUser.UserName);
            Id.Should().Match(randomUser.Id);
            roles.Should().BeEquivalentTo(randomRoles);
        }


        private AuthService GetAuthService(IConfiguration configuration)
        {
            return new AuthService(configuration);
        }

        private ApplicationUser GetNewUser()
        {
            return new ApplicationUser() {UserName = RandomString(7), Id = Guid.NewGuid().ToString()};
        }

        private (string, string, List<string>) GetNameAndIdAndRole(JwtSecurityToken token)
        {
            var username = token.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
            var Id = token.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            var Roles = token.Claims.Where(claim => claim.Type == ClaimTypes.Role).Select(role => role.Value).ToList();

            return (username, Id, Roles);
        }


        private IList<string> GetRandomRolesList()
        {
            string RandomRole() => RandomString(7);

            return new List<string>() {RandomRole(), RandomRole(), RandomRole()};
        }



        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }
    }
}