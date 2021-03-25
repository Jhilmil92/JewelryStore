using JeweleryStore.Common.Enums;
using JewelryStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace JewelryStore.DataRepository.Contexts
{
    public class JewelryStoreDbContext : DbContext
    {
        public JewelryStoreDbContext(DbContextOptions<JewelryStoreDbContext> options) 
            : base(options)
        {


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasData(
                    new User
                    {
                        Id = 1,
                        Name = "Diamond Dust",
                        UserName = "Diamonds",
                        Password = "",
                        UserCategory = UserType.Regular
                    },
                    new User
                    {
                        Id = 2,
                        Name = "Wolverine Adamantium",
                        UserName = "Wolverine",
                        Password = "",
                        UserCategory = UserType.Privileged
                    }
                );

            modelBuilder.Entity<Item>()
                .HasData(
                    new Item
                    {
                        Id = 1,
                        ItemName = "Gold"
                    },
                    new Item
                    {
                        Id = 2,
                        ItemName = "Silver"
                    }
                );

            modelBuilder.Entity<UserDiscount>()
                .HasData(
                    new UserDiscount
                    {
                        Id = 1,
                        ItemId = 1,
                        UserId = 2,
                        Discount = 2
                    }
                );
        }
    }
}
