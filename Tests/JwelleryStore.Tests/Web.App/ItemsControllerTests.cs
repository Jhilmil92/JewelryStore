using JeweleryStore.Common.Enums;
using JewelryStore.BLL.Interfaces;
using JewelryStore.Common.Exceptions;
using JewelryStore.Domain.Models;
using JewelryStoreApp.Controllers;
using JewelryStoreApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace JwelleryStore.Tests.Web.App
{
    public class ItemsControllerTests
    {
        [Fact]
        public void ItemsController_Ctor()
        {
            // Arrange
            var mockRepo = new Mock<IItemsBLL>();
            var httpServiceMock = new Mock<IHttpServices>();
            // Act
            var controller = new ItemsController(mockRepo.Object, httpServiceMock.Object);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public async Task ItemsController_Get_ItemPresent()
        {
            // Arrange
            var item = new ItemModel() { Id = 1, ItemName = "name" };
            var mockRepo = new Mock<IItemsBLL>();
            var httpServiceMock = new Mock<IHttpServices>();

            mockRepo.Setup(x => x.GetItemById(It.IsAny<int>()))
                    .Returns(Task.FromResult(item));
            var controller = new ItemsController(mockRepo.Object, httpServiceMock.Object);

            // Act
            var result = await controller.Get(1);

            // Assert
            Assert.Equal(item,result.Value);
        }

        [Fact]
        public async Task ItemsController_Get_ItemNotPresent()
        {
            // Arrange
            var mockRepo = new Mock<IItemsBLL>();
            var httpServiceMock = new Mock<IHttpServices>();

            mockRepo.Setup(x => x.GetItemById(It.IsAny<int>()))
                    .ReturnsAsync(null as ItemModel);
            var controller = new ItemsController(mockRepo.Object, httpServiceMock.Object);

            // Act
            var result = await controller.Get(1);

            // Assert
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task ItemsController_GetAllItems()
        {
            // Arrange
            var items = new List<ItemModel>() { new ItemModel { Id = 1, ItemName = "name" } ,
                new ItemModel { Id = 2, ItemName = "fame" } };
            var mockRepo = new Mock<IItemsBLL>();
            var httpServiceMock = new Mock<IHttpServices>();

            mockRepo.Setup(x => x.GetItems())
                    .ReturnsAsync(items);
            var controller = new ItemsController(mockRepo.Object, httpServiceMock.Object);

            // Act
            var result = await controller.GetAll();

            // Assert
            Assert.Equal(items, result.Value);
        }

        [Fact]
        public async Task ItemsController_ComputeTotalPrice()
        {
            // Arrange
            double value = 20;
            var mockRepo = new Mock<IItemsBLL>();
            var httpServiceMock = new Mock<IHttpServices>();

            httpServiceMock.Setup(x => x.GetClaimsIdentity(It.IsAny<HttpContext>()))
                            .Returns(new UserClaim());
            mockRepo.Setup(x => x.ComputeTotalPriceAsync(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<int>(), It.IsAny<UserType>(), It.IsAny<string>()))
                    .ReturnsAsync(value);
            var controller = new ItemsController(mockRepo.Object, httpServiceMock.Object);

            // Act
            var result = await controller.ComputePrice(10,2,1);

            // Assert
            Assert.Equal(value, result);
        }



        [Fact]
        public async Task ItemsController_CreateItem()
        {
            // Arrange
            var item = new ItemModel() { Id = 1, ItemName = "name" };
            var mockRepo = new Mock<IItemsBLL>();
            var httpServiceMock = new Mock<IHttpServices>();

            mockRepo.Setup(x => x.GetItemById(It.IsAny<int>()))
                    .ReturnsAsync(item);
            var controller = new ItemsController(mockRepo.Object, httpServiceMock.Object);

            // Act
            var result = await controller.Create(item);

            // Assert
            Assert.Equal(400, ((StatusCodeResult) result.Result).StatusCode);
        }

        [Fact]
        public async Task ItemsController_CreateNoItem()
        {
            // Arrange
            ItemModel item = null;
            var mockRepo = new Mock<IItemsBLL>();
            var httpServiceMock = new Mock<IHttpServices>();

            mockRepo.Setup(x => x.CreateItem(It.IsAny<ItemModel>()))
                    .ReturnsAsync(false);
            var controller = new ItemsController(mockRepo.Object, httpServiceMock.Object);

            // Act
            var result = await controller.Create(item);

            // Assert
            Assert.Equal(item, result.Value);
        }

        [Fact]
        public async Task ItemsController_UpdateItem_Success()
        {
            // Arrange
            int id = 1;
            var item = new ItemModel() { Id = 1, ItemName = "name" };
            var mockRepo = new Mock<IItemsBLL>();
            var httpServiceMock = new Mock<IHttpServices>();

            mockRepo.Setup(x => x.GetItemById(It.IsAny<int>()))
                   .ReturnsAsync(item);
            mockRepo.Setup(x => x.UpdateItem(It.IsAny<ItemModel>()))
                    .ReturnsAsync(true);
            var controller = new ItemsController(mockRepo.Object, httpServiceMock.Object);

            // Act
            var result = await controller.Update(id,item);

            // Assert
            Assert.Equal(204, ((StatusCodeResult)result.Result).StatusCode);
        }

        [Fact]
        public async Task ItemsController_UpdateItem_IdMismatch()
        {
            // Arrange
            int id = 2;
            var item = new ItemModel() { Id = 1, ItemName = "name" };
            var mockRepo = new Mock<IItemsBLL>();
            var httpServiceMock = new Mock<IHttpServices>();

            mockRepo.Setup(x => x.UpdateItem(It.IsAny<ItemModel>()))
                    .ReturnsAsync(false);
            var controller = new ItemsController(mockRepo.Object, httpServiceMock.Object);

            // Act
            var result = await controller.Update(id, item);

            // Assert
            Assert.Equal(400, ((StatusCodeResult)result.Result).StatusCode);
        }

        [Fact]
        public async Task ItemsController_UpdateItem_ItemNotFound()
        {
            // Arrange
            int id = 1;
            var item = new ItemModel() { Id = 1, ItemName = "name" };
            var mockRepo = new Mock<IItemsBLL>();
            var httpServiceMock = new Mock<IHttpServices>();

            mockRepo.Setup(x => x.UpdateItem(It.IsAny<ItemModel>()))
                    .Throws(new NotFoundException());
            var controller = new ItemsController(mockRepo.Object, httpServiceMock.Object);
            
            // Act
            var result = await controller.Update(id, item);

            // Assert
            Assert.Equal(404, ((StatusCodeResult)result.Result).StatusCode);
        }

        [Fact]

        public async Task ItemsController_UpdateItem_UnhandledException()
        {
            // Arrange
            int id = 1;
            var item = new ItemModel() { Id = 1, ItemName = "name" };
            var mockRepo = new Mock<IItemsBLL>();
            var httpServiceMock = new Mock<IHttpServices>();

            mockRepo.Setup(x => x.UpdateItem(It.IsAny<ItemModel>()))
                    .Throws(new Exception());
            var controller = new ItemsController(mockRepo.Object, httpServiceMock.Object);

            // Assert
            Assert.ThrowsAsync<Exception>(() =>  controller.Update(id, item));

        }

        [Fact]
        public async Task ItemsController_DeleteItem_Success()
        {
            // Arrange
            int id = 1;
            var item = new ItemModel() { Id = 1, ItemName = "name" };
            var mockRepo = new Mock<IItemsBLL>();
            var httpServiceMock = new Mock<IHttpServices>();

            mockRepo.Setup(x => x.GetItemById(It.IsAny<int>()))
                   .ReturnsAsync(item);
            mockRepo.Setup(x => x.DeleteItem(It.IsAny<ItemModel>()))
                    .ReturnsAsync(true);
            var controller = new ItemsController(mockRepo.Object, httpServiceMock.Object);

            // Act
            var result = await controller.Delete(id);

            // Assert
            Assert.Equal(200, ((StatusCodeResult)result).StatusCode);
        }

        [Fact]
        public async Task ItemsController_DeleteItem_ItemDoesntExist()
        {
            // Arrange
            int id = 2;
            var item = new ItemModel() { Id = 1, ItemName = "name" };
            var mockRepo = new Mock<IItemsBLL>();
            var httpServiceMock = new Mock<IHttpServices>();

            mockRepo.Setup(x => x.GetItemById(It.IsAny<int>()))
                    .ReturnsAsync(null as ItemModel);
            var controller = new ItemsController(mockRepo.Object, httpServiceMock.Object);

            // Act
            var result = await controller.Delete(id);

            // Assert
            Assert.Equal(404, ((StatusCodeResult)result).StatusCode);
        }

        [Fact]
        public async Task ItemsController_DeleteItem_ConcurrencyItemNotFound()
        {
            // Arrange
            int id = 2;
            var item = new ItemModel() { Id = 1, ItemName = "name" };
            var mockRepo = new Mock<IItemsBLL>();
            var httpServiceMock = new Mock<IHttpServices>();

            mockRepo.Setup(x => x.GetItemById(It.IsAny<int>()))
                  .ReturnsAsync(item);
            mockRepo.Setup(x => x.DeleteItem(It.IsAny<ItemModel>()))
                    .ThrowsAsync(new Exception());
            var controller = new ItemsController(mockRepo.Object, httpServiceMock.Object);

            // Act
            var result = await controller.Delete(id);

            // Assert
            Assert.Equal(204, ((StatusCodeResult)result).StatusCode);
        }


        [Fact]
        public async Task ItemsController_DeleteItem_DeleteFailed()
        {
            // Arrange
            int id = 2;
            var item = new ItemModel() { Id = 1, ItemName = "name" };
            var mockRepo = new Mock<IItemsBLL>();
            var httpServiceMock = new Mock<IHttpServices>();

            mockRepo.Setup(x => x.GetItemById(It.IsAny<int>()))
                  .ReturnsAsync(item);
            mockRepo.Setup(x => x.DeleteItem(It.IsAny<ItemModel>()))
                .ReturnsAsync(false);
            var controller = new ItemsController(mockRepo.Object, httpServiceMock.Object);

            // Act
            var result = await controller.Delete(id);

            // Assert
            Assert.Equal(404, ((StatusCodeResult)result).StatusCode);
        }


    }
}
