using Application.Interfaces.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Seeds
{
    public static class DefaultCategory
    {
        public static async Task SeedAsync(ICategoryRepositoryAsync categoryRepository)
        {
            //Seed category
            await categoryRepository.AddAsync(
                new Category{ Id = 1, CategoryName = "Quần áo"});
            await categoryRepository.AddAsync(
                new Category { Id = 2, CategoryName = "Đồ gia dụng" });
            await categoryRepository.AddAsync(
                new Category { Id = 3, CategoryName = "Học tập" });
            await categoryRepository.AddAsync(
                new Category { Id = 4, CategoryName = "Thể thao" });
            await categoryRepository.AddAsync(
                new Category { Id = 5, CategoryName = "Điện tử" });
            await categoryRepository.AddAsync(
                new Category { Id = 6, CategoryName = "Nội thất" });
            await categoryRepository.AddAsync(
                new Category { Id = 7, CategoryName = "Khác" });
        }
    }
}
