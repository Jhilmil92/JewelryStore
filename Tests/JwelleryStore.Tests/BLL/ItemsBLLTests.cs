using JewelryStore.BLL.Classes;
using JewelryStore.Common.Interfaces;
using JewelryStore.DataRepository.Interfaces;
using JewelryStore.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JwelleryStore.Tests.BLL
{
    public class ItemsBLLTests
    {
        [Fact]
        public async Task ItemsBLL_GetItemById_IdNotPresent() 
        {
            // Arrange
            var mockRepo = new Mock<IUnitOfWork>();
            var repoMock = new Mock<IRepository<Item>>();
            mockRepo.Setup(x => x.Repository<Item>())
                .Returns(repoMock.Object);

            repoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(null as Item);
            var controller = new ItemsBLL(mockRepo.Object);

            // Act
            var result = await controller.GetItemById(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ItemsBLL_GetItemById_Present()
        {
            // Arrange
            var mockRepo = new Mock<IUnitOfWork>();
            var repoMock = new Mock<IRepository<Item>>();
            var item = new Item() { Id = 1, ItemName = "Name" };
            mockRepo.Setup(x => x.Repository<Item>())
                .Returns(repoMock.Object);

            repoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(item);
            var controller = new ItemsBLL(mockRepo.Object);

            // Act
            var result = await controller.GetItemById(1);

            // Assert
            Assert.Equal(item.Id,result.Id);
            Assert.Equal(item.ItemName, result.ItemName);
        }


        [Fact]
        public async Task ItemsBLL_GetItems_Ok()
        {
            // Arrange
            var mockRepo = new Mock<IUnitOfWork>();
            var repoMock = new Mock<IRepository<Item>>();
            var item = new Item() { Id = 1, ItemName = "Name" };
            var item2 = new Item() { Id = 2, ItemName = "Name2" };
            var itemList = new List<Item>() { item, item2 };
            mockRepo.Setup(x => x.Repository<Item>())
                .Returns(repoMock.Object);
            repoMock.Setup(x => x.Get(null, null, null))
                .ReturnsAsync(itemList);
            var bll = new ItemsBLL(mockRepo.Object);

            // Act
            var result = await bll.GetItems();

            // Assert
            Assert.Equal(itemList.Count, result.ToList().Count);
        }

        [Fact]
        public async Task ItemsBLL_ComputeTotalPriceAsync_PreivilagedUser()
        {
            // Arrange
            var mockRepo = new Mock<IUnitOfWork>();
            var userRepoMock = new Mock<IRepository<User>>();
            var userDiscountRepoMock = new Mock<IRepository<UserDiscount>>();

            mockRepo.Setup(x => x.Repository<User>())
                .Returns(userRepoMock.Object);
            userRepoMock.Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>(),It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),It.IsAny<string[]>()))
                .ReturnsAsync(new List<User>() { new User() { Id = 1, UserCategory = JeweleryStore.Common.Enums.UserType.Privileged } });
            mockRepo.Setup(x => x.Repository<UserDiscount>())
               .Returns(userDiscountRepoMock.Object);

            userDiscountRepoMock.Setup(x => x.Get(It.IsAny<Expression<Func<UserDiscount, bool>>>(), It.IsAny<Func<IQueryable<UserDiscount>, IOrderedQueryable<UserDiscount>>>(), It.IsAny<string[]>()))
                .ReturnsAsync(new List<UserDiscount>() { new UserDiscount() { ItemId = 2, UserId = 1, Discount = 10 } });
            var bll = new ItemsBLL(mockRepo.Object);

            // Act
            var result = await bll.ComputeTotalPriceAsync(20, 5, 2, JeweleryStore.Common.Enums.UserType.Privileged, "name");

            // Assert
            Assert.Equal(90, result);
        }

        [Fact]
        public async Task ItemsBLL_ComputeTotalPriceAsync_RegularUser()
        {
            // Arrange
            var mockRepo = new Mock<IUnitOfWork>();
            var userRepoMock = new Mock<IRepository<User>>();
            var userDiscountRepoMock = new Mock<IRepository<UserDiscount>>();

            mockRepo.Setup(x => x.Repository<User>())
                .Returns(userRepoMock.Object);
            userRepoMock.Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(), It.IsAny<string[]>()))
                .ReturnsAsync(new List<User>() { new User() { Id = 1, UserCategory = JeweleryStore.Common.Enums.UserType.Regular } });
            mockRepo.Setup(x => x.Repository<UserDiscount>())
               .Returns(userDiscountRepoMock.Object);

            userDiscountRepoMock.Setup(x => x.Get(It.IsAny<Expression<Func<UserDiscount, bool>>>(), It.IsAny<Func<IQueryable<UserDiscount>, IOrderedQueryable<UserDiscount>>>(), It.IsAny<string[]>()))
                .ReturnsAsync(new List<UserDiscount>() { new UserDiscount() { ItemId = 2, UserId = 1, Discount = 10 } });
            var bll = new ItemsBLL(mockRepo.Object);

            // Act
            var result = await bll.ComputeTotalPriceAsync(20, 5, 2, JeweleryStore.Common.Enums.UserType.Regular, "name");

            // Assert
            Assert.Equal(100, result);
        }

        [Fact]
        public async Task ItemsBLL_ComputeTotalPriceAsync_PrivilagedUser_ItemNotDisounted()
        {
            // Arrange
            var mockRepo = new Mock<IUnitOfWork>();
            var userRepoMock = new Mock<IRepository<User>>();
            var userDiscountRepoMock = new Mock<IRepository<UserDiscount>>();

            mockRepo.Setup(x => x.Repository<User>())
                .Returns(userRepoMock.Object);
            userRepoMock.Setup(x => x.Get(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(), It.IsAny<string[]>()))
                .ReturnsAsync(new List<User>() { new User() { Id = 1, UserCategory = JeweleryStore.Common.Enums.UserType.Privileged } });
            mockRepo.Setup(x => x.Repository<UserDiscount>())
               .Returns(userDiscountRepoMock.Object);

            userDiscountRepoMock.Setup(x => x.Get(It.IsAny<Expression<Func<UserDiscount, bool>>>(), It.IsAny<Func<IQueryable<UserDiscount>, IOrderedQueryable<UserDiscount>>>(), It.IsAny<string[]>()))
                .ReturnsAsync(new List<UserDiscount>() {  });
            var bll = new ItemsBLL(mockRepo.Object);

            // Act
            var result = await bll.ComputeTotalPriceAsync(20, 5, 2, JeweleryStore.Common.Enums.UserType.Privileged, "name");

            // Assert
            Assert.Equal(100, result);
        }
    }
}
