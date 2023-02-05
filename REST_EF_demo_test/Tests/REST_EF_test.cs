using Microsoft.EntityFrameworkCore;
using REST_EF_demo.Data;
using REST_EF_demo.Models;
using REST_EF_demo.Services;
using FluentAssertions;

namespace REST_EF_demo_test.Repository
{
    public class REST_EF_test
    {
        private async Task<AppDbContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new AppDbContext(options);
            databaseContext.Database.EnsureCreated();               
            return databaseContext;
        }
        [Fact]
        public async void LibraryService_GetAuthor_ReturnsAuthor()
        {
            // Arrange
            var name = "J. R. R. Tolkien";
            var dbContext = await GetDatabaseContext();
            var LibraryService = new LibraryService(dbContext);

            // Act
            dbContext.Authors.Add(
                    new Author
                    {
                        Name = "J. R. R. Tolkien",
                        DateOfBirth = new DateTime(1892, 1, 3)
                    });
            await dbContext.SaveChangesAsync();
            var result = LibraryService.GetAuthorAsync(name, false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Task<Author>>();
        }
    }
}
