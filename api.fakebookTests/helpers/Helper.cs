using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.fakebook.Models.Authentication;
using api.fakebook.Services.UserService;
using Microsoft.Extensions.Configuration;
using Moq;

namespace api.fakebookTests.helpers
{
    static class Helper
    {

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }

        public static ClaimsPrincipal GetRandomUser(string name = null, string id = null)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, name ?? Helper.RandomString(7)),
                new Claim(ClaimTypes.NameIdentifier, id ?? Guid.NewGuid().ToString()),
            }, "mock"));
        }

        public static Mock<IUserService> SetupFindUserByUsername(Mock<IUserService> mockUserService, bool returnUser = true)
        {
            mockUserService.Setup(service => service.FindByUsernameAsync(It.IsAny<string>()))
                .ReturnsAsync(returnUser ? new ApplicationUser(){UserName = RandomString(8), Id = RandomString(18)} : (ApplicationUser)null);

            return mockUserService;
        }

        public static Mock<IUserService> SetupCheckIfUserExistsByUsername(Mock<IUserService> mockUserService, bool checkResult = true)
        {
            mockUserService.Setup(service => service.CheckIfUserExistsByUsername(It.IsAny<string>()))
                .ReturnsAsync(checkResult);

            return mockUserService;
        }

        public static LoginModel GetRandomLogin()
        {
            return new LoginModel() { Username = RandomString(8), Password = RandomString(8) };
        }

        public static Mock<IUserService> SetupCheckPassword(Mock<IUserService> mockUserService, bool returnSuccessfull)
        {
            mockUserService.Setup(service => service.CheckUserPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(returnSuccessfull);

            return mockUserService;
        }

        public static IList<string> GetRandomRolesList()
        {
            string RandomRole() => Helper.RandomString(7);

            return new List<string>() { RandomRole(), RandomRole(), RandomRole() };
        }


    }
}
