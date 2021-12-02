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