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
using Microsoft.AspNetCore.Mvc;
using api.fakebook.Dto.Post;

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

            var mockedReturnValue = new List<ResponsePostDto>() { createPost(), createPost(), createPost(), };

            mockPostService.Setup(service => service.GetPostsByUserIdAsync(It.IsAny<string>())).ReturnsAsync(mockedReturnValue);

            //Act
            var controller = new PostController(mockPostService.Object);
            var result = await controller.GetPosts("aq290312dasd") as OkObjectResult;
            //Assert
            result.Value.Should().BeEquivalentTo(
                mockedReturnValue, 
                options => options.ComparingByMembers<ResponsePostDto>()
                );
        }

        [TestMethod()]
        public async Task GetPosts_NoContent_ReturnNoContent()
        {
            //Arrange
            var mockPostService = new Mock<IPostService>();

            mockPostService.Setup(service => service.GetPostsByUserIdAsync(It.IsAny<string>())).ReturnsAsync(new List<ResponsePostDto>());
            //Act
            var controller = new PostController(mockPostService.Object);
            var result = await controller.GetPosts("aq290312dasd");
            //Assert
            result.Should().BeOfType(typeof(NoContentResult));      
        }

        [TestMethod()]
        public async Task CreatePost_ValidPost_ReturnCreatedItem()
        {
            //Arrange
            var mockPostService = new Mock<IPostService>();

            var PostToCreate = new createPostDto() { text = RandomString(200) };
            //Act
            var controller = new PostController(mockPostService.Object);
            var result = await controller.CreatePost(PostToCreate) as CreatedAtActionResult;
            //Assert
            var createdPost = result.Value as BasePostDto;

            PostToCreate.Should().BeEquivalentTo(
                createdPost,
                options => options.ComparingByMembers<BasePostDto>().ExcludingMissingMembers()
                );
        }




        private ResponsePostDto createPost()
        {
            return new ResponsePostDto()
            {
                postDate = DateTime.Now,
                username = RandomString(8),
                userId = RandomString(25),
                Id = Guid.NewGuid(),
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