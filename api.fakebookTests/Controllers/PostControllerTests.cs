﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
using api.fakebook.Models.PostModels;
using Microsoft.IdentityModel.Claims;
using ClaimsPrincipal = System.Security.Claims.ClaimsPrincipal;

namespace api.fakebook.Controllers.Tests
{
    [TestClass()]
    public class PostControllerTests
    {

        
        [TestMethod()]
        [DataRow(false, typeof(OkObjectResult))]
        [DataRow(true, typeof(NoContentResult))]
        public async Task GetPostsTest(bool returnEmptyList, Type expectedResult)
        {
            //Arrange
            var mockPostService = GetMockedPostService();

            mockPostService.Setup(service => service.GetPostsByUserIdAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(GetPostList(returnEmptyList));

            //Act
            var controller = GetController(mockPostService.Object);
            var result = await controller.GetPosts("aq290312dasd");
            //Assert
            result.Should().BeOfType(expectedResult);
        }


        [TestMethod()]
        public async Task CreatePost_ValidPost_ReturnCreatedItem()
        {
            //Arrange
            var mockPostService = GetMockedPostService();

            var PostToCreate = new createPostDto() { text = RandomString(200) };
            //Act
            var controller = GetController(mockPostService.Object);
            var result = await controller.CreatePost(PostToCreate) as CreatedAtActionResult;
            //Assert
            var createdPost = result.Value as BasePostDto;

            PostToCreate.Should().BeEquivalentTo(
                createdPost,
                options => options.ComparingByMembers<BasePostDto>().ExcludingMissingMembers()
                );
        }

        
        [TestMethod]
        [DataRow(false, typeof(OkObjectResult))]
        [DataRow(true, typeof(NoContentResult))]
        public async Task GetPostById_ExistingUserAndNonExistingUser(bool returnNullPost, Type expectedResult)
        {
            //Arrange
            var mockPostService = GetMockedPostService();
            mockPostService.Setup(service => service.GetPostById(It.IsAny<string>())).ReturnsAsync(getPost(returnNullPost));

            //Act
            var controller = GetController(mockPostService.Object);
            var result = await controller.GetPostById(Guid.NewGuid().ToString());
            //Assert
            result.Should().BeOfType(expectedResult);
        }

        [TestMethod]
        [DataRow(false, typeof(OkObjectResult))]
        [DataRow(true, typeof(NoContentResult))]
        public async Task GetWallTest(bool returnEmptyList, Type expectedResult)
        {
            //Arrange
            var mockPostService = GetMockedPostService();
            
            mockPostService.Setup(service => service.GetWall(It.IsAny<ClaimsPrincipal>(),It.IsAny<int>())).ReturnsAsync(GetPostList(returnEmptyList));

            //Act
            var controller = GetController(mockPostService.Object);
            var result = await controller.GetWall();
            //Assert
            result.Should().BeOfType(expectedResult);
        }



        private ResponsePostDto getPost(bool returnNull = false)
        {
            if (returnNull)
            {
                return null;
            }

            return new ResponsePostDto()
            {
                postDate = DateTime.Now,
                username = RandomString(8),
                userId = RandomString(25),
                Id = Guid.NewGuid(),
                text = RandomString(22)
            };
        }

        private List<ResponsePostDto> GetPostList(bool empty = false)
        {
            if (empty) return new List<ResponsePostDto>();

            return new List<ResponsePostDto>() {getPost(), getPost(), getPost()};
        }


        private Mock<IPostService> GetMockedPostService()
        {
            return new Mock<IPostService>();
        }

        private PostController GetController(IPostService postService)
        {
            return new PostController(postService);
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }

    }
}