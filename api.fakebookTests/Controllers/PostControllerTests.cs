using Microsoft.VisualStudio.TestTools.UnitTesting;
using api.fakebook.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using api.fakebook.Services.UserService;
using Moq;
using FluentAssertions;
using api.fakebook.Services.PostService;
using api.fakebook.Dto;

namespace api.fakebook.Controllers.Tests
{
    [TestClass()]
    public class PostControllerTests
    {
        [TestMethod()]
        public async Task GetPosts_PostsAvailable_ReturnPosts()
        {
            //Arrange
            var mockPostService = new Mock<IPostService>();

            var mockedReturnValue = new List<PostDto>() { createPost(), createPost(), createPost(), };

            mockPostService.Setup(service => service.GetPostsByUserId(It.IsAny<string>())).ReturnsAsync(mockedReturnValue);

            //Act
            var controller = new PostController(mockPostService.Object);
            var result = await controller.GetPosts("aq290312dasd");
            //Assert
            result.Should().BeEquivalentTo(mockedReturnValue, options => options.ComparingByMembers<PostDto>());
        }


        private PostDto createPost()
        {
            return new PostDto()
            {
                postDate = DateTime.Now,
                postedBy = RandomString(6),
                publicId = Guid.NewGuid(),
                text = RandomString(22)
            };
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }

    }
}