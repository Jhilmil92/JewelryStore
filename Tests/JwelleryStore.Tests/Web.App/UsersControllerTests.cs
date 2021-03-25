using JewelryStore.BLL.Interfaces;
using JewelryStore.Common.Utilities;
using JewelryStore.Domain.Models;
using JewelryStoreApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace JwelleryStore.Tests.Web.App
{
    public class UsersControllerTests
    {
        [Fact]
        public void UsersController_Ctor()
        {
            // Arrange
            var mockRepo = new Mock<IUserBLL>();

            // Act
            var controller = new UsersController(mockRepo.Object);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public async Task UsersController_Login_Success()
        {
            // Arrange
            string jwtToken = "yugedigei76376rhr&&@@!!ljn#";
            var userModel = new UserModel
            {
                UserName = "SilverSpade",
                Password = "xxxxxxxx"
            };

            var authResponse = new AuthResponse(userModel, jwtToken);
            var mockRepo = new Mock<IUserBLL>();
            mockRepo.Setup(x => x.AuthenticateUser(It.IsAny<UserModel>()))
                .ReturnsAsync(authResponse);
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = await controller.Login(userModel);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }


        [Fact]
        public async Task UsersController_Login_NotAuthorized()
        {
            // Arrange
            var mockRepo = new Mock<IUserBLL>();
            var userModel = new UserModel
            {
                UserName = "SilverSpade",
                Password = "xxxxxxxx"
            };
            mockRepo.Setup(x => x.AuthenticateUser(It.IsAny<UserModel>()))
                .ReturnsAsync(null as AuthResponse);
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = await controller.Login(userModel);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task UsersController_Login_Exception()
        {
            // Arrange
            var mockRepo = new Mock<IUserBLL>();
            var userModel = new UserModel
            {
                UserName = "SilverSpade",
                Password = "xxxxxxxx"
            };
            mockRepo.Setup(x => x.AuthenticateUser(It.IsAny<UserModel>()))
                .ThrowsAsync(new System.Exception());
            var controller = new UsersController(mockRepo.Object);

            // Act
            // Assert

            Assert.ThrowsAsync<Exception>(() => controller.Login(userModel));

        }
    }
}
